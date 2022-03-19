using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MVC.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly EventMSContext _context;
        private readonly IHubContext<SignalrServer> _signalRHub;
        public HomeController(ILogger<HomeController> logger, EventMSContext context, IHubContext<SignalrServer> signalRHub)
        {
            _logger = logger;
            _context = context;
            _signalRHub = signalRHub;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            if (HttpContext.Session.GetInt32("id") != null) return RedirectToAction(nameof(Index));
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Register()
        {
            if (HttpContext.Session.GetInt32("id") != null) return RedirectToAction(nameof(Index));
            return View();
        }

        public async Task<IActionResult> Event()
        {
            var eventMSContext = _context.Events.Include(o => o.CreationUser).Include(o => o.Status);
            return View(await eventMSContext.ToListAsync());
        }

        public async Task<IActionResult> EventDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var oevent = await _context.Events
                .Include(o => o.CreationUser)
                .Include(o => o.Status)
                .Include(o => o.Comments)
                .Include(o => o.EventTickets)

                .FirstOrDefaultAsync(m => m.Id == id);
            if (oevent == null)
            {
                return NotFound();
            }

            return View(oevent);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateComment([Bind("Id,EventId,CreationUserId,Text,CreationDate")] Comment comment)
        {
            if (HttpContext.Session.GetInt32("id") == null) return RedirectToAction(nameof(Login));

            if (ModelState.IsValid)
            {
                comment.CreationDate = DateTime.Now;
                comment.CreationUserId = HttpContext.Session.GetInt32("id");

                _context.Add(comment);
                await _context.SaveChangesAsync();
                return Redirect($"/Home/EventDetails/{comment.EventId}");
            }
            return View(comment);

        }

        public IActionResult Profile()
        {
            if (HttpContext.Session.GetInt32("id") == null) return RedirectToAction(nameof(Login));

            var id = HttpContext.Session.GetInt32("id");

            if (id == null) return RedirectToAction(nameof(Login));

            User user = _context.Users.SingleOrDefault(u => u.Id == id);

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateProfile(User user)
        {
            // Update Profile here
            return RedirectToAction("Profile");
        }

        public IActionResult SearchEvent(string searchText)
        {
            IEnumerable<Event> _event = _context.Events
                .Include(o => o.CreationUser)
                .Include(o => o.Status)
                .Include(o => o.Comments)
                .Include(o => o.EventTickets)
                .ToList().FindAll(e => e.Name.ToLower().Contains(searchText.ToLower()));

            if (_event.Count() == 0)
            {
                ViewBag.Message = "No event found";
            }

            return View("Event", _event);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public Models.UserRole Role { get; set; }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("FirstName,LastName,Password,Email")] User User)
        {
            if (HttpContext.Session.GetInt32("id") != null) return Redirect("/Home");

            if (ModelState.IsValid)
            {
                if (AccountExists(User.Email))
                { /*Msg = "Email has been used, please choose another!";*/
                    return View(User);
                }
                if (User.Password != User.Bio)
                { /*Msg = "Email has been used, please choose another!";*/
                    return View(User);
                }
                User.Bio = User.Email;
                User.IsActive = true;
                _context.Add(User);
                await _context.SaveChangesAsync();
                await _signalRHub.Clients.All.SendAsync("LoadUsers");
                var i = _context.Users.FirstOrDefault(o => o.Email.ToLower().Contains(User.Email.ToLower())).Id;
                Role = new Models.UserRole();
                Role.RoleId = (int?)3;
                Role.UserId = (int?)i;
                _context.UserRoles.Add(Role);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Login));
            }
            return View(User);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind("Password,Email")] User User)
        {
            if (HttpContext.Session.GetInt32("id") != null) return Redirect("/Home");

            if (ModelState.IsValid)
            {
                var check = _context.Users.FirstOrDefault(o => o.Email.ToLower().Equals(User.Email.ToLower()) && o.Password.Equals(User.Password));
                if (check != null)
                {
                    HttpContext.Session.SetString("name", check.Email);
                    HttpContext.Session.SetInt32("id", check.Id);
                    foreach (var i in _context.UserRoles.ToList())
                    {
                        if (i.UserId == check.Id && i.RoleId == 1)
                        {
                            HttpContext.Session.SetString("role", "admin");
                        }
                        else if (i.UserId == check.Id && i.RoleId == 2)
                        {
                            HttpContext.Session.SetString("role1", "host");

                        }
                        else if (i.UserId == check.Id && i.RoleId == 3)
                        {
                            HttpContext.Session.SetString("role2", "member");
                        }
                    }
                    if (HttpContext.Session.GetString("role") == "admin") return Redirect("/Users");
                    if (HttpContext.Session.GetString("role1") == "host") return Redirect("/events");
                    if (HttpContext.Session.GetString("role2") == "member") return RedirectToAction("Index");
                    return RedirectToAction("Index");
                }
            }
            return View(User);
        }
        private bool AccountExists(string id)
        {
            return _context.Users.Any(e => e.Email == id);
        }
    }
}