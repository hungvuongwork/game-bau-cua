using GameBauCua.Web.Client.Data;
using GameBauCua.Web.Client.Data.Entities;
using GameBauCua.Web.Client.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GameBauCua.Web.Client.Hubs
{
    public class GameHub : Hub
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<GameHub> _logger;

        public GameHub(ApplicationDbContext context, UserManager<ApplicationUser> userManager, ILogger<GameHub> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var roomId = Context.GetHttpContext().Request.Cookies["RoomId"];
            var user = await _userManager.FindByNameAsync(Context.User.Identity.Name);

            var roomDetailModel = await _context.RoomDetails.FindAsync(roomId, user.Id);

            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                if (roomDetailModel.IsHost)
                {
                    var currentRoom = await _context.Rooms.FindAsync(roomId, user.Id);
                    currentRoom.IsClosed = true;

                    await _context.SaveChangesAsync();

                    await Clients.Group(roomId).SendAsync("ExitedRoomFromHost");
                }
                else
                {
                    _context.RoomDetails.Remove(roomDetailModel);

                    await _context.SaveChangesAsync();

                    await Clients.Group(roomId).SendAsync("ExitedRoomFromPlayer", user.FullName);
                    await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId);
                }
            }

            await base.OnDisconnectedAsync(exception);
        }

        #region Room Methods
        public async Task ExitRoom(string roomId)
        {
            try
            {
                var player = await _userManager.FindByNameAsync(Context.User.Identity.Name);
                var currentPlayerInRoom = await _context.RoomDetails.SingleOrDefaultAsync(s => s.RoomId == roomId && s.PlayerId == player.Id);

                if (currentPlayerInRoom.IsHost)
                {
                    var currentRoom = await _context.Rooms.FindAsync(roomId);

                    currentRoom.IsClosed = true;

                    var result = await _context.SaveChangesAsync();
                    if (result > 0)
                        await Clients.Group(roomId).SendAsync("ExitedRoomFromHost");
                }
                else
                {
                    _context.RoomDetails.Remove(currentPlayerInRoom);

                    var result = await _context.SaveChangesAsync();
                    if (result > 0)
                    {
                        await Clients.Group(roomId).SendAsync("ExitedRoomFromPlayer", player);
                        await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }

        public async Task GetPlayersByRoomId(string roomId, bool addToGroup = false)
        {
            var roomDetails = await (from rd in _context.RoomDetails
                                     where rd.RoomId == roomId
                                     select new PlayerListViewModel
                                     {
                                         Id = rd.Player.Id,
                                         FullName = rd.Player.FullName,
                                         UserName = rd.Player.UserName,
                                         YourCapital = rd.Player.YourCapital,
                                         IsHost = rd.IsHost,
                                         Color = rd.Color
                                     }).ToListAsync();

            if (addToGroup)
                await Groups.AddToGroupAsync(Context.ConnectionId, roomId);

            await Clients.Group(roomId).SendAsync("LoadedPlayersInRoom", roomDetails);
        }

        public async Task CreateNewRound(string jsonViewModel)
        {
            try
            {
                var viewModel = JsonConvert.DeserializeObject<RoundPlay>(jsonViewModel);
                
                var host = await _userManager.FindByNameAsync(Context.User.Identity.Name);
                if (host.YourCapital < 0)
                {
                    await Clients.Caller.SendAsync("AlertMessage", "warning", "Bạn không đủ tiền để tạo vòng mới!");
                    return;
                }

                var model = new RoundPlay
                {
                    MaximumBet = viewModel.MaximumBet,
                    MinimumBet = viewModel.MinimumBet,
                    RoundNumber = viewModel.RoundNumber,
                    RoomId = viewModel.RoomId
                };

                var anyRoundPlayFinished = await _context.RoundPlays.Where(w => w.RoomId == viewModel.RoomId && string.IsNullOrEmpty(w.WinMascots) && w.IsFinished == false).AnyAsync();
                if (anyRoundPlayFinished)
                {
                    await Clients.Caller.SendAsync("AlertMessage", "warning", "Vòng chơi chưa hoàn tất!");
                    return;
                }

                await _context.RoundPlays.AddAsync(model);

                var result = await _context.SaveChangesAsync();
                if (result > 0)
                {
                    await Clients.Group(viewModel.RoomId).SendAsync("StartedNewRound", model);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }

        public async Task BetByPlayer(string jsonViewModel)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(Context.User.Identity.Name);
                var viewModel = JsonConvert.DeserializeObject<RoundPlayDetail>(jsonViewModel);

                if (user.YourCapital < 0)
                {
                    await Clients.Caller.SendAsync("AlertMessage", "warning", "Bạn không đủ tiền để đặt cược!");
                    return;
                }

                var anyRoundPlayExists = await _context.RoundPlayDetails.Where(w => w.RoundPlayId == viewModel.RoundPlayId && w.PlayerId == viewModel.PlayerId).AnyAsync();
                if (anyRoundPlayExists)
                {
                    await Clients.Caller.SendAsync("AlertMessage", "warning", "Bạn đã đặt cược rồi!");
                    return;
                }

                var roundPlay = await _context.RoundPlays.SingleOrDefaultAsync(w => w.Id == viewModel.RoundPlayId);
                if (viewModel.YourBet < roundPlay.MinimumBet || viewModel.YourBet > roundPlay.MaximumBet)
                {
                    await Clients.Caller.SendAsync("AlertMessage", "warning", $"Mức cược từ {roundPlay.MinimumBet.ToString("{0:n0}")} đến {roundPlay.MaximumBet.ToString("{0:n0}")}");
                    return;
                }

                var model = new RoundPlayDetail
                {
                    RoundPlayId = viewModel.RoundPlayId,
                    PlayerId = user.Id,
                    YourBet = viewModel.YourBet,
                    MascotBet = viewModel.MascotBet
                };

                await _context.RoundPlayDetails.AddAsync(model);

                var result = await _context.SaveChangesAsync();
                if (result > 0)
                {
                    var playerBetList = await (from rpd in _context.RoundPlayDetails
                                               join p in _context.Users on rpd.PlayerId equals p.Id
                                               where rpd.RoundPlayId == roundPlay.Id
                                               select new RoundPlayDetailViewModel
                                               {
                                                   MascotBet = rpd.MascotBet,
                                                   YourBet = rpd.YourBet,
                                                   RoundPlayId = rpd.RoundPlayId,
                                                   PlayerId = p.Id,
                                                   PlayerFullName = p.FullName
                                               }).ToListAsync();

                    await Clients.Group(roundPlay.RoomId).SendAsync("GetPlayerBetList", playerBetList);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }

        public async Task SaveDiceResult(int roundPlayId, int[] results, BetResultViewModel[] betResults)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var roundPlay = await _context.RoundPlays.FindAsync(roundPlayId);
                    var hostPlayer = await _context.RoomDetails.SingleOrDefaultAsync(w => w.RoomId == roundPlay.RoomId && w.IsHost);

                    roundPlay.WinMascots = string.Join(",", results.Select(s => s.ToString()).ToArray());
                    roundPlay.IsFinished = true;

                    _context.RoundPlays.Update(roundPlay);

                    foreach (var item in betResults)
                    {
                        var player = await _context.Users.FindAsync(item.PlayerId);
                        player.YourCapital += item.ResultBet;
                        _context.Users.Update(player);

                        var host = await _context.Users.FindAsync(hostPlayer.PlayerId);
                        host.YourCapital -= item.ResultBet;
                        _context.Users.Update(player);
                    }

                    var result = await _context.SaveChangesAsync();
                    if (result > 0)
                    {
                        await transaction.CommitAsync();
                        await Clients.Group(roundPlay.RoomId).SendAsync("GetFinalRoundPlayResult", betResults, results);
                    }
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    _logger.LogError(ex.Message);
                }
            }
        }
        #endregion Room Methods
    }
}
