using GameBauCua.Web.Client.Data;
using GameBauCua.Web.Client.Data.Entities;
using GameBauCua.Web.Client.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace GameBauCua.Web.Client.Controllers.Apis
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RoomsController> _logger;
        private Random _random;

        public RoomsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, ILogger<RoomsController> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
            _random = new Random();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var viewModels = await (from r in _context.Rooms
                                    join rd in _context.RoomDetails on r.Id equals rd.RoomId
                                    where rd.IsHost == true && r.IsClosed == false
                                    select new RoomListViewModel
                                    {
                                        Id = r.Id,
                                        Name = r.Name,
                                        HostFullName = rd.Player.FullName,
                                        NumberOfPlayers = r.NumberOfPlayers,
                                        NumberOfPlayersInRoom = r.RoomDetails.Where(w => w.RoomId == r.Id).Count(),
                                        MinimumBet = r.MinimumBet,
                                        ExpectedMaximumBet = r.ExpectedMaximumBet,
                                    }).ToListAsync();

            if (viewModels.Count() == 0)
                return NotFound();

            return Ok(viewModels);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RoomRequestModel requestModel)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var host = await _userManager.FindByNameAsync(User.Identity.Name);

                    if (host.YourCapital < 0)
                        return BadRequest(new ResponseViewModel { Type = "warning", Message = "Bạn không đủ tiền để tạo phòng" });

                    var roomId = Guid.NewGuid().ToString();

                    _context.Rooms.Add(new Room
                    {
                        Id = roomId,
                        Name = requestModel.Name,
                        NumberOfPlayers = requestModel.NumberOfPlayers,
                        MinimumBet = requestModel.MinimumBet,
                        ExpectedMaximumBet = requestModel.ExpectedMaximumBet,
                    });

                    _context.RoomDetails.Add(new RoomDetail
                    {
                        IsHost = true,
                        PlayerId = host.Id,
                        RoomId = roomId
                    });

                    var result = await _context.SaveChangesAsync();
                    if (result > 0)
                    {
                        HttpContext.Response.Cookies.Append("RoomId", roomId);
                        await transaction.CommitAsync();

                        return CreatedAtAction("Index", "Room", new { roomId = roomId });
                    }

                    await transaction.RollbackAsync();

                    return BadRequest(new ResponseViewModel { Type = "warning", Message = "Không tạo được phòng. Vui lòng thử lại sau!" });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    await transaction.RollbackAsync();

                    return BadRequest(new ResponseViewModel { Type = "warning", Message = "Có lỗi xảy ra!" });
                }
            }
        }

        [HttpPost("join/{roomId}")]
        public async Task<IActionResult> Join(string roomId)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var currentPlayer = await _userManager.FindByNameAsync(User.Identity.Name);

                    if (currentPlayer.YourCapital < 0)
                        return BadRequest(new ResponseViewModel { Type = "warning", Message = "Bạn không đủ tiền để tham gia phòng" });

                    var room = await _context.Rooms.FindAsync(roomId);

                    if (await CheckFullRoom(room.NumberOfPlayers, room.Id))
                        return BadRequest(new ResponseViewModel { Message = "Phòng đã đầy!" });

                    await _context.RoomDetails.AddAsync(new RoomDetail
                    {
                        IsHost = false,
                        PlayerId = currentPlayer.Id,
                        RoomId = roomId,
                        Color = $"rgb({_random.Next(256)},{_random.Next(256)},{_random.Next(256)})"
                    });

                    var joinedPlayerResult = await _context.SaveChangesAsync();
                    if (joinedPlayerResult > 0)
                    {
                        HttpContext.Response.Cookies.Append("RoomId", roomId);
                        await transaction.CommitAsync();

                        return CreatedAtAction("Index", "Room", new { roomId = roomId });
                    }

                    await transaction.RollbackAsync();

                    return BadRequest(new ResponseViewModel { Type = "warning", Message = "Không tham gia vào phòng. Vui lòng thử lại sau!" });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    await transaction.RollbackAsync();

                    return BadRequest(new ResponseViewModel { Type = "warning", Message = "Có lỗi xảy ra!" });
                }
            }
        }

        [HttpGet("get-player-list/{roomId}")]
        public async Task<IActionResult> GetPlayerListById(string roomId)
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

            return Ok(roomDetails);
        }

        private async Task<bool> CheckFullRoom(int numberOfPlayers, string roomId)
        {
            var numberOfPlayersInRoom = await _context.RoomDetails.Where(w => w.RoomId == roomId).CountAsync();

            if (numberOfPlayers == numberOfPlayersInRoom)
                return true;

            return false;
        }
    }
}
