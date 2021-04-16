using GameBauCua.Web.Client.Data;
using GameBauCua.Web.Client.Data.Entities;
using GameBauCua.Web.Client.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameBauCua.Web.Client.Controllers
{
    public class RoomController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public RoomController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(string roomId)
        {
            if (string.IsNullOrEmpty(roomId))
                return RedirectToAction("Index", "Home");

            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var roomDetail = await _context.RoomDetails.Where(w => w.RoomId == roomId && w.PlayerId == user.Id).FirstOrDefaultAsync();

            return View(await _context.RoomDetails.Where(w => w.RoomId == roomId && w.PlayerId == user.Id)
                .Include(i => i.Player)
                .Include(i => i.Room)
                .FirstOrDefaultAsync());
        }
    }
}
