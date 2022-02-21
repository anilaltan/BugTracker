using BugTracker.Data;
using BugTracker.Models.Domain;
using BugTracker.Models.Helpers;
using BugTracker.Models.ViewModels.Project;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BugTracker.Controllers
{
    [Authorize]
    public class ProjectController : Controller
    {
        private readonly ApplicationDbContext _db;
        private ProjectHelper ProjectHelper;

        public ProjectController(ApplicationDbContext db)
        {
            _db = db;
            ProjectHelper = new ProjectHelper(_db);
        }

        [HttpGet]
        [Authorize(Roles = "Admin, ProjectManager")]
        public ActionResult CreateProject()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin, ProjectManager")]
        public ActionResult CreateProject(CreateProjectViewModel formData)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var project = new Project();

            project.Name = formData.Name;
            project.Description = formData.Description;
            project.CreateTime = DateTime.Now;

            _db.Projects.Add(project);
            _db.SaveChanges();

            return RedirectToAction("ProjectsList");
        }

        //display all project
        [Authorize(Roles = "Admin, ProjectManager")]
        public ActionResult ProjectsList()
        {
            ViewBag.Title = "All Projects";

            var model = ProjectHelper.GetAllProject().Select(p => new ProjectsListViewModel
            {

                Id = p.Id,
                Name = p.Name,
                //NumberOfMembers = ProjectHelper.GetUserIncludedProject(p.Id).Count(),
                NumberOfTickets = _db.Tickets.Where(u => u.ProjectId == p.Id).ToList().Count(),
                CreateDate = p.CreateTime,

            }).ToList();

            return View("ProjectsList", model);
        }

        //Display Detail Project
        [Authorize(Roles = "Admin, ProjectManager")]
        public async Task<IActionResult> ProjectDetail(int? Id)
        {
            if(Id == null) { return NotFound(); }
            var project = _db.Projects
                .Include(c => c.Ticket)
                .AsNoTracking()
                .FirstOrDefault(p => p.Id == Id);
            if(project == null) { return NotFound(); }
            var numberOfTickets = _db.Tickets.Where(u => u.ProjectId == Id).ToList().Count;
            ViewData["numberOfTickets"] = numberOfTickets;
            PopulateTicketsDropDownList(project.Id);

            return View(project);
        }

        [HttpGet]
        [Authorize(Roles = "Admin, ProjectManager")]
        public async Task<IActionResult> EditProject(int pId)
        {
            var result = ProjectHelper.GetProjectDetailByProjectId(pId);

            if (result == null)
            {
                return RedirectToAction(nameof(ProjectController.ProjectsList));
            }

            var model = new CreateProjectViewModel();

            model.Id = result.Id;
            model.Name = result.Name;
            model.Description = result.Description;
            model.Archive = result.Archive;

            return View(model);
        }

        [HttpPost, ActionName("EditProject")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, ProjectManager")]
        public async Task<IActionResult> ProjectEditAsync(CreateProjectViewModel formData)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var result = ProjectHelper.GetProjectDetailByProjectId(formData.Id);

            if (result == null)
            {
                return View();
            }

            result.Name = formData.Name;
            result.Description = formData.Description;
            result.Archive = formData.Archive;

            _db.SaveChanges();

            return RedirectToAction("ProjectsList", "Project");
        }

        private void PopulateTicketsDropDownList(int? Id)
        {
            //var ticketsQuery = from d in _db.Tickets where d.ProjectId == Id select d;
            var ticketsQuery = _db.Tickets
                .Include(c => c.TicketPriority)
                .Include(c => c.TicketStatus)
                .Include(c => c.CreatedBy)
                .AsNoTracking()
                .Where(d => d.ProjectId == Id);
            var viewModel = new List<Ticket>();
            foreach(var ticket in ticketsQuery)
            {
                viewModel.Add(ticket);
            }
            ViewData["Tickets"] = viewModel;
        }
    }
}
