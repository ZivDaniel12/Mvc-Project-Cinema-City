using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.Security;
using CimenaCityProject.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;


namespace CimenaCityProject.Logic
{
    internal class ActionRole
    {
        internal void AddUserAndRole()
        {
            // Access application context
            ApplicationDbContext context =
                new ApplicationDbContext();

            // member variable of type IdentityResult
            IdentityResult identityRoleResult;
            IdentityResult identityUserResult;

            // create RoleStore object contain the use of 
            // ApplicationDbContext
            var roleStore = new RoleStore<IdentityRole>(context);

            // create RoleManager object 
            var roleManager = new RoleManager<IdentityRole>(roleStore);

            // next step create custom role user
            if (roleManager.RoleExists("WebSiteAdmin"))
            {
                identityRoleResult =
                    roleManager.Create
                    (new IdentityRole { Name = "WebSiteAdmin" });
            }

            // create user manager
            var usrManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            // create application user 
            var appUser = new ApplicationUser
            {
                UserName = "WebSiteAdmin@test.com",
                Id = "WebSiteAdmin"
            };

            identityRoleResult = usrManager.Create(appUser, "!P@ssw0rd");

            if (!usrManager.IsInRole("WebSiteAdmin", "WebSiteAdmin"))
            {
                identityUserResult = usrManager.AddToRole("WebSiteAdmin", "WebSiteAdmin");
            }
        }
    }
}