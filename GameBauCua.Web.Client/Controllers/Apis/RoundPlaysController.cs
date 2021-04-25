using GameBauCua.Web.Client.Data;
using GameBauCua.Web.Client.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameBauCua.Web.Client.Controllers.Apis
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoundPlaysController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<RoundPlaysController> _logger;

        public RoundPlaysController(ApplicationDbContext context, ILogger<RoundPlaysController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet("{roomId}")]
        public async Task<IActionResult> GetAllByRoomId(string roomId)
        {
            return Ok(await _context.RoundPlays.Where(w => w.RoomId == roomId && w.IsFinished == true).Select(s => new RoundPlayViewModel
            {
                Id = s.Id,
                WinMascots = s.WinMascots,
                MinimumBet = s.MinimumBet,
                MaximumBet = s.MaximumBet
            }).ToListAsync());
        }

        [HttpGet("playing-round/{roomId}")]
        public async Task<IActionResult> GetPlayingRound(string roomId)
        {
            var room = await (from rp in _context.RoundPlays
                              where rp.RoomId == roomId && rp.IsFinished == false
                              select new RoundPlayViewModel
                              {
                                  Id = rp.Id,
                                  IsFinished = rp.IsFinished,
                                  MaximumBet = rp.MaximumBet,
                                  MinimumBet = rp.MinimumBet,
                                  RoomId = rp.RoomId,
                                  RoundNumber = rp.RoundNumber,
                                  WinMascots = rp.WinMascots,
                                  RoundPlayDetails = rp.RoundPlayDetails.Select(s => new RoundPlayDetailViewModel
                                  {
                                      MascotBet = s.MascotBet,
                                      YourBet= s.YourBet,
                                      PlayerFullName = s.Player.FullName,
                                      PlayerId = s.PlayerId,
                                      RoundPlayId = s.RoundPlayId
                                  })
                              }).FirstOrDefaultAsync();

            if (room != null)
                return Ok(new { Room = room });

            return NotFound();
        }
    }
}
