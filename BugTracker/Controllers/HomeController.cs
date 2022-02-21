using BugTracker.Data;
using BugTracker.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BugTracker.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _db = db;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult Dashboard()
        {
            ViewBag.Title = "Dash board";

            var userId = _userManager.GetUserId(User);
            var model = new DashboardViewModel();
            if (User.IsInRole("Admin") || User.IsInRole("Project Manager"))
            {
                model.NumberOfProjects = _db.Projects.Where(p => p.Archive != true).Count();
                model.NumberOfTickets = _db.Tickets.Where(p => p.Project.Archive != true).Count();
                model.NumberOfTicketsOpen = _db.Tickets.Where(u => u.TicketStatusId == 1).Where(u => u.Project.Archive != true).Count();
                model.NumberOfTicketsResolved = _db.Tickets.Where(u => u.TicketStatusId == 2).Where(u => u.Project.Archive != true).Count();
                model.NumberOfTicketsRejected = _db.Tickets.Where(u => u.TicketStatusId == 3).Where(u => u.Project.Archive != true).Count();
            }
            else
            {
                model.NumberOfProjects = _db.Projects.Where(p => p.Archive != true).Where(p => p.Users.Any(u => u.Id
                == userId)).Count();

                if (User.IsInRole("Submitter"))
                {
                    model.NumberOfTicketsCreated = _db.Tickets.Where(p => p.Project.Archive != true).Where(p => p.CreatedById == userId).Count();
                }

                //if (User.IsInRole("Developer"))
                //{
                //    model.NumberOfTicketsAssigned = _db.Tickets.Where(p => p.Project.Archive != true).Where(p => p.AssignedToId == userId).Count();
                //}

            }
            return View(model);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}