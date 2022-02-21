using BugTracker.Data;
using BugTracker.Models.Domain;
using Microsoft.AspNetCore.Identity;

namespace BugTracker.Models.Helpers
{
    public class ProjectHelper
    {
        private ApplicationDbContext _db;

        public ProjectHelper(ApplicationDbContext db)
        {
            _db = db;
        }

        public List<Project> GetAllProject()
        {
            return _db.Projects.Where(p => p.Archive != true).ToList();
        }

        public List<Project> GetMyProjectListByUserId(string UserId)
        {
            return _db.Projects.Where(p => p.Archive != true).Where(p => p.Users.Any(u => u.Id == UserId)).ToList();
        }

        public Project GetProjectDetailByProjectId(int? PId)
        {
            return _db.Projects.Where(p => p.Archive != true).FirstOrDefault(p => p.Id == PId);
        }

        //public List<IdentityUser> GetUserIncludedProject(int? PId)
        //{
        //    return _db.Users.Where(p => p.Projects.Where(u => u.Archive != true).Any(u => u.Id == PId)).ToList();
        //}


        //public List<IdentityUser> GetUsersExcludedProject(int? PId)
        //{
        //    return _db.Users.Where(p => !p.Projects.Where(u => u.Archive != true).Any(u => u.Id == PId)).ToList();
        //}

        public void UpdateProjectMember(string UserId, int? PId, string Require)
        {

            var User = _db.Users.FirstOrDefault(p => p.Id == UserId);

            var Project = _db.Projects.Where(p => p.Archive != true).FirstOrDefault(u => u.Id == PId);

            if (Require == "AddUser")
            {
                Project.Users.Add(User);
            }
            else if (Require == "DeleteUser")
            {
                Project.Users.Remove(User);
            }

            _db.SaveChanges();
        }
    }
}
