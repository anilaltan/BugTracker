using BugTracker.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BugTracker.Controllers
{
    public class UserController : Controller
    {
        private SignInManager<IdentityUser> signInManager;
        private UserManager<IdentityUser> userManager;
        private ApplicationDbContext _db;

        public UserController(UserManager<IdentityUser> userMgr, SignInManager<IdentityUser> signInMgr, ApplicationDbContext db)
        {
            userManager = userMgr;
            signInManager = signInMgr;
            _db = db;
        }

        [AllowAnonymous]
        public async Task<ActionResult> Demo(string roleName)
        {
            var demoUser = new IdentityUser();

            if (roleName == "Admin")
            {
                demoUser = _db.Users.Where(p => p.Email == "admindemo@gmail.com").FirstOrDefault();
            }
            else if (roleName == "ProjectManager")
            {
                demoUser = _db.Users.Where(p => p.Email == "projectmanagerdemo@gmail.com").FirstOrDefault();
            }
            else if (roleName == "Developer")
            {
                demoUser = _db.Users.Where(p => p.Email == "developerdemo@gmail.com").FirstOrDefault();
            }
            else if (roleName == "Submitter")
            {
                demoUser = _db.Users.Where(p => p.Email == "submitterdemo@gmail.com").FirstOrDefault();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }

            //SignInManager<IdentityUser>.SignIn(demoUser, false, false);
            await signInManager.SignInAsync(demoUser, false);


            return RedirectToAction("Dashboard", "Home");
        }

        //public ActionResult Register()
        //{
        //    return RedirectToAction(Areas.Identity.Pages.Account.);
        //}
    }
}
