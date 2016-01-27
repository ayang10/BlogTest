namespace BlogTest.Migrations
{
    using Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<BlogTest.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(BlogTest.Models.ApplicationDbContext context)
        {
            
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
                     //lamda expression, check to see if context, roles, look for any roles in the table name is "Admin"
                     //if(!roleManager.RoleExists("Admin")) this if functions does the same thing
            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                roleManager.Create(new IdentityRole { Name = "Admin" });
            }
            if (!context.Roles.Any(r => r.Name == "Moderator"))
            {
                roleManager.Create(new IdentityRole { Name = "Moderator" });
            }

            //assign me to the Admin role, if not already in it
            var uStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(uStore);

            if (userManager.FindByEmail("ayang014@gmail.com") == null)
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "ayang014@gmail.com",
                    Email = "ayang014@gmail.com",
                    FirstName = "A",
                    LastName = "Yang"
                }, "Password-1");
            }
            //assign me to the Admin role, if not already in it
            var userId = userManager.FindByEmail("ayang014@gmail.com").Id;
            if (!userManager.IsInRole(userId, "Admin"))
            {
                userManager.AddToRole(userId, "Admin");
            }

            // add bobby to the database
            if (userManager.FindByEmail("bdavis@coderfoundry") == null)
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "bdavis@coderfoundry",
                    Email = "bdavis@coderfoundry.com",
                    FirstName = "Bobby",
                    LastName = "Davis"
                }, "Password-1");
            }
            //assign me to the Admin role, if not already in it
            userId = userManager.FindByEmail("bdavis@coderfoundry.com").Id;
            if (!userManager.IsInRole(userId, "Moderator"))
            {
                userManager.AddToRole(userId, "Moderator");
            }

            
            // add Andrew to the database
            if (userManager.FindByEmail("ajensen@coderfoundry.com") == null)
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "ajensen@coderfoundry.com",
                    Email = "ajensen@coderfoundry.com",
                    FirstName = "Andrew",
                    LastName = "Jensen"
                }, "Password-1");
            }

            // assign Andrew to the Moderator role, if not already in it
            userId = userManager.FindByEmail("ajensen@coderfoundry.com").Id;
            if (!userManager.IsInRole(userId, "Moderator"))
            {
                userManager.AddToRole(userId, "Moderator");
            }


            // add Rai to the database
            if (userManager.FindByEmail("ramanglani@coderfoundry.com") == null)
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "ramanglani@coderfoundry.com",
                    Email = "ramanglani@coderfoundry.com",
                    FirstName = "Ria",
                    LastName = "Manglani"
                }, "Password-1");
            }

            // assign Ria to the Moderator role, if not already in it
            userId = userManager.FindByEmail("ramanglani@coderfoundry.com").Id;
            if (!userManager.IsInRole(userId, "Moderator"))
            {
                userManager.AddToRole(userId, "Moderator");
            }

            
            // add TJ to the database
            if (userManager.FindByEmail("tjones@coderfoundry.com") == null)
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "tjones@coderfoundry.com",
                    Email = "tjones@coderfoundry.com",
                    FirstName = "TJ",
                    LastName = "Jones"
                }, "Password-1");
            }


            // assign TJ to the Moderator role, if not already in it
            userId = userManager.FindByEmail("tjones@coderfoundry.com").Id;
            if (!userManager.IsInRole(userId, "Moderator"))
            {
                userManager.AddToRole(userId, "Moderator");
            }


        }
    }
}
