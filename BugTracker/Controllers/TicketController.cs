using BugTracker.Data;
using BugTracker.Models.Domain;
using BugTracker.Models.Helpers;
using BugTracker.Models.ViewModels;
using BugTracker.Models.ViewModels.Project;
using BugTracker.Models.ViewModels.Ticket;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BugTracker.Controllers
{
    [Authorize]
    public class TicketController : Controller
    {

        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ProjectHelper ProjectHelper;
        private readonly TicketHelper TicketHelper;

        public TicketController(ApplicationDbContext db, UserManager<IdentityUser> userManager)
        {
            _db = db;
            _userManager = userManager;
            ProjectHelper = new ProjectHelper(_db);
            TicketHelper = new TicketHelper(_db);
        }

        //Create Ticket GET
        [HttpGet]
        [Authorize(Roles = "Submitter")]
        public IActionResult Createticket()
        {
            var model = new CreateTicketViewModel();
            //var userId = User.FindFirstOrDValue(ClaimTypes.NameIdentifier);
            var userId = _userManager.GetUserId(User);

            model.ProjectIdNameList = ProjectHelper.GetAllProject().Select(p => new ProjectIdName { Id = p.Id, Name = p.Name }).ToList();
            model.TicketPriorityNameList = TicketHelper.GetTicketPriorityNames();
            return View(model);
        }

        //Create Ticket POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Submitter")]
        public ActionResult CreateTicket(CreateTicketViewModel formData)
        {
            var userId = _userManager.GetUserId(User);
            
            if (!ModelState.IsValid)
            {
                var model = new CreateTicketViewModel();

                model.ProjectIdNameList = ProjectHelper.GetAllProject().Select(p => new ProjectIdName { Id = p.Id, Name = p.Name }).ToList();
                model.TicketPriorityNameList = TicketHelper.GetTicketPriorityNames();

                return View(model);
            }

            var ticket = new Ticket();

            ticket.Title = formData.Title;
            ticket.Description = formData.Description;
            ticket.Created = DateTime.Now;
            ticket.CreatedById = userId;
            ticket.ProjectId = formData.ProjectId;
            ticket.TicketPriorityId = formData.TicketPriorityId;

            ticket.TicketStatusId = 1;

            _db.Tickets.Add(ticket);
            _db.SaveChanges();

            return RedirectToAction(nameof(TicketController.OwnedTicketsList));
        }


        //Display All ticket List for admin and project manager 
        [Authorize(Roles = "Admin, ProjectManager, Developer")]
        public async Task<ActionResult> TicketsList()
        {
            ViewBag.Title = "All Tickets";
            var tickets = _db.Tickets
                .Include(c => c.TicketPriority)
                .Include(c => c.TicketStatus)
                .Include(c => c.Project)
                .Include(c => c.CreatedBy)
                .AsNoTracking();
            return View(await tickets.ToListAsync());
        }

        //Display Owned Ticket List 
        [Authorize(Roles = "Submitter")]
        public async Task<ActionResult> OwnedTicketsList()
        {
            ViewBag.Title = "Owned Tickets";
            var userId = _userManager.GetUserId(User);
            var tickets = _db.Tickets
                .Include(c => c.TicketPriority)
                .Include(c => c.TicketStatus)
                .Include(c => c.Project)
                .Include(c => c.CreatedBy)
                .AsNoTracking()
                .Where(p => p.CreatedById == userId);
            return View("TicketsList", await tickets.ToListAsync());
        }

        //Edit Ticket GET
        [HttpGet]
        public ActionResult EditTicket(int? tId)
        {
            var userId = _userManager.GetUserId(User);

            if (tId == null)
            {
                return RedirectToAction(nameof(TicketController.OwnedTicketsList));
            }

            //user permission check
            var userHasTicket = false;
            userHasTicket = CheckUserHasTicket(tId, userId);
            if (userHasTicket != true)
            {
                return RedirectToAction(nameof(TicketController.OwnedTicketsList));
            }

            var result = TicketHelper.GetFirstTicketByTid(tId);
            //var DeveloperRoleId = _db.Roles.FirstOrDefault(p => p.Name == "Developer").Id;

            if (result == null)
            {
                return RedirectToAction(nameof(TicketController.TicketsList));
            }

            var model = new EditTicketViewModel();

            model.Id = result.Id;
            model.Title = result.Title;
            model.Description = result.Description;
            model.ProjectId = result.ProjectId;
            //model.TicketTypeId = result.TicketTypeId;
            model.TicketPriorityId = result.TicketPriorityId;
            model.TicketStatusId = result.TicketStatusId;

            if (User.IsInRole("Admin") || User.IsInRole("Project Manager"))
            {
                model.ProjectNameList = ProjectHelper.GetAllProject().Select(p => new ProjectIdName { Id = p.Id, Name = p.Name }).ToList();
            }
            else
            {
                model.ProjectNameList = ProjectHelper.GetMyProjectListByUserId(userId).Select(p => new ProjectIdName { Id = p.Id, Name = p.Name }).ToList();
            }

            //model.TicketTypeNameList = TicketHelper.GetTicketTypeNames();
            model.TicketPriorityNameList = TicketHelper.GetTicketPriorityNames();
            model.TicketStatusNameList = TicketHelper.GetTicketStatusNames();
            //model.DeveloperNameList = TicketHelper.GetDeveloperNamesByRoleId(DeveloperRoleId);

            return View(model);
        }

        //Edit Ticket POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditTicket(EditTicketViewModel formData, int tId)
        {
            var userId = _userManager.GetUserId(User);
            var result = TicketHelper.GetFirstTicketByTid(tId);

            if (!ModelState.IsValid)
            {
                //var DeveloperRoleId = _db.Roles.FirstOrDefault(p => p.Name == "Developer").Id;
                var model = new EditTicketViewModel();
                model.Id = result.Id;
                model.Title = result.Title;
                model.Description = result.Description;
                model.ProjectId = result.ProjectId;
                //model.TicketTypeId = result.TicketTypeId;
                model.TicketPriorityId = result.TicketPriorityId;
                model.TicketStatusId = result.TicketStatusId;
                //model.AssignedToId = result.AssignedToId;
                var project = ProjectHelper.GetAllProject().ToList();
                foreach(var item in project)
                {
                    List<ProjectIdName> ids = new List<ProjectIdName>();
                    var projectIdName = new ProjectIdName(); 
                    projectIdName.Id = item.Id;
                    projectIdName.Name = item.Name;
                    ids.Add(projectIdName);
                    model.ProjectNameList = ids;
                }
              
                //model.TicketTypeNameList = TicketHelper.GetTicketTypeNames();
                model.TicketPriorityNameList = TicketHelper.GetTicketPriorityNames();
                model.TicketStatusNameList = TicketHelper.GetTicketStatusNames();
                //model.DeveloperNameList = TicketHelper.GetDeveloperNamesByRoleId(DeveloperRoleId);

                return View(model);
            }

            //User permission Check 
            var userHasTicket = false;
            userHasTicket = CheckUserHasTicket(tId, userId);
            if (userHasTicket != true)
            {
                return RedirectToAction(nameof(TicketController.OwnedTicketsList));
            }

            result.Title = formData.Title;
            result.Description = formData.Description;
            result.ProjectId = formData.ProjectId;
            //result.TicketTypeId = formData.TicketTypeId;
            result.TicketPriorityId = formData.TicketPriorityId;
            result.TicketStatusId = formData.TicketStatusId;
            //result.AssignedToId = formData.AssignedToId;
            result.Updated = DateTime.Now;

            _db.Tickets.Update(result);
            _db.SaveChanges();

            return RedirectToAction(nameof(TicketController.TicketsList));
        }

        //Display Detail of Ticket
        [HttpGet]
        public async Task<ActionResult> DetailTicketAsync(int? tId)

        {
            var userId = _userManager.GetUserId(User);
            ViewBag.UserId = userId;

            if (tId == null)
            {
                return RedirectToAction(nameof(TicketController.OwnedTicketsList));
            }

            //User permission Check  
            var UserHasTicket = false;
            UserHasTicket = CheckUserHasTicket(tId, userId);
            if (UserHasTicket != true)
            {
                return RedirectToAction(nameof(TicketController.OwnedTicketsList));
            }

            var ticketsDetail = _db.Tickets.Where(p => p.Id == tId)
                .Include(c => c.TicketPriority)
                .Include(c => c.TicketStatus)
                .Include(c => c.Project)
                .Include(c => c.CreatedBy)
                .AsNoTracking();

            return View(await ticketsDetail.ToListAsync());
        }

        //Drop Down List for project
        private void PopulateProjectsDropDownList(object selectedProject = null)
        {
            var projectsQuery = from d in _db.Projects orderby d.Name select d;
            ViewBag.ProjectId = new SelectList(projectsQuery.AsNoTracking(), "Id", "Name", selectedProject);
        }
        //Drop Down List for status
        private void PopulateTicketStatusDropDownList(object selectedStatus = null)
        {
            var ticketStatusQuery = from d in _db.TicketStatuses orderby d.Name select d;
            ViewBag.StatusId = new SelectList(ticketStatusQuery.AsNoTracking(), "Id", "Name", ticketStatusQuery);
        }
        //Drop Down List for Priority
        private void PopulateTicketPrioritiesDropDownList(object selectedPriority = null)
        {
            var ticketPriorityQuery = from d in _db.TicketPriorities orderby d.Name select d;
            ViewBag.ticketPriorityId = new SelectList(ticketPriorityQuery.AsNoTracking(), "Id", "Name", ticketPriorityQuery);
        }

        //Check valid ticketId that user has it
        public bool CheckUserHasTicket(int? tId, string userId)
        {
            var result = false;
            var Ticket = TicketHelper.GetFirstTicketByTid(tId);

            if (Ticket == null)
            {
                return false;
            }

            //if (User.IsInRole("Developer"))
            //{
            //    if (Ticket.AssignedToId == userId)
            //    {
            //        result = true;
            //    }
            //    else
            //    {
            //        result = false;
            //    }
            //}

            if (User.IsInRole("Submitter"))
            {
                if (Ticket.CreatedById == userId)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            else if (User.IsInRole("Admin") || User.IsInRole("ProjectManager"))
            {
                result = true;
            }
            return result;
        }
    }
}
