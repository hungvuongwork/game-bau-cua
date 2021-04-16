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

        }

        #region Room Methods
        public async Task ExitRoom(string roomId)
        {
            var player = await _userManager.FindByNameAsync(Context.User.Identity.Name);
            var currentPlayerInRoom = await _context.RoomDetails.SingleOrDefaultAsync(s => s.RoomId == roomId && s.PlayerId == player.Id);

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    if (currentPlayerInRoom.IsHost)
                    {
                        var deletingRoom = await _context.Rooms.FindAsync(roomId);
                        var deletingRoomDetails = await _context.RoomDetails.Where(w => w.RoomId == roomId).ToListAsync();

                        _context.RoomDetails.RemoveRange(deletingRoomDetails);
                        _context.Rooms.Remove(deletingRoom);

                        var result = await _context.SaveChangesAsync();
                        if (result > 0)
                        {
                            await transaction.CommitAsync();
                            await Clients.Group(roomId).SendAsync("ExitedRoomFromHost");
                        }
                    }
                    else
                    {
                        _context.RoomDetails.Remove(currentPlayerInRoom);

                        // chuyen trang thai phong thanh open neu full
                        var room = await _context.Rooms.SingleOrDefaultAsync(s => s.Id == roomId);
                        if (room.IsFull)
                        {
                            var currentRoom = await _context.Rooms.FindAsync(roomId);
                            currentRoom.IsFull = false;

                            _context.Rooms.Update(currentRoom);
                        }

                        var result = await _context.SaveChangesAsync();
                        if (result > 0)
                        {
                            await transaction.CommitAsync();
                            await Clients.User(Context.UserIdentifier).SendAsync("ExitedRoomFromPlayer", player.FullName);
                            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    await transaction.RollbackAsync();
                }
            }
        }

        public async Task GetPlayersByRoomId(string roomId)
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

            await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
            await Clients.Group(roomId).SendAsync("LoadedPlayersInRoom", roomDetails);
        }

        public async Task CreateNewRound(string jsonViewModel)
        {
            try
            {
                var viewModel = JsonConvert.DeserializeObject<RoundPlay>(jsonViewModel);

                var model = new RoundPlay
                {
                    MaximumBet = viewModel.MaximumBet,
                    MinimumBet = viewModel.MinimumBet,
                    RoundNumber = viewModel.RoundNumber,
                    RoomId = viewModel.RoomId
                };

                await _context.RoundPlays.AddAsync(model);

                var result = await _context.SaveChangesAsync();
                if (result > 0)
                    await Clients.Group(viewModel.RoomId).SendAsync("StartedNewRound", model);
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
                    var roundPlay = await _context.RoundPlays.FindAsync(model.RoundPlayId);

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

        public async Task ChooseMascotByPlayer(int mascotId)
        {

        }
        #endregion Room Methods

        private async Task<bool> CheckAllPlayersReady(string roomId)
        {
            var numberOfPlayersInRoom = await _context.RoomDetails.Where(w => w.RoomId == roomId).CountAsync();

            var numberOfReadyPlayersInRoom = await (from rpd in _context.RoundPlayDetails
                                                    join rp in _context.RoundPlays on rpd.RoundPlayId equals rp.Id
                                                    where rp.RoomId == roomId
                                                    select rpd).CountAsync();

            if (numberOfPlayersInRoom == numberOfReadyPlayersInRoom)
                return true;

            return false;
        }
    }
}
