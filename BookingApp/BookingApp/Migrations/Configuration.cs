using BookingApp.Models;

namespace BookingApp.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<BookingApp.Models.BAContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(BookingApp.Models.BAContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole { Name = "Admin" };

                manager.Create(role);
            }

            if (!context.Roles.Any(r => r.Name == "Manager"))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole { Name = "Manager" };

                manager.Create(role);
            }

            if (!context.Roles.Any(r => r.Name == "AppUser"))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole { Name = "AppUser" };

                manager.Create(role);
            }


            context.AppUsers.AddOrUpdate(
                  p => p.UserName,
                  new AppUser() { UserName = "admin" }
            );
            context.AppUsers.AddOrUpdate(
                p => p.UserName,
                new AppUser() { UserName = "AppUser AppUserovic" }
            );
            context.SaveChanges();

            var userStore = new UserStore<BAIdentityUser>(context);
            var userManager = new UserManager<BAIdentityUser>(userStore);
            if (!context.Users.Any(u => u.UserName == "admin"))
            {
                var _appUser = context.AppUsers.FirstOrDefault(a => a.UserName == "Admin Adminovic");
                var user = new BAIdentityUser() { Id = "admin", UserName = "admin", Email = "admin@yahoo.com", PasswordHash = BAIdentityUser.HashPassword("admin"), appUser = _appUser};
                userManager.Create(user);
                userManager.AddToRole(user.Id, "Admin");
            }

            if (!context.Users.Any(u => u.UserName == "appu"))
            {
                var _appUser = context.AppUsers.FirstOrDefault(a => a.UserName == "AppUser AppUserovic");
                var user = new BAIdentityUser() { Id = "appu", UserName = "appu", Email = "appu@yahoo.com", PasswordHash = BAIdentityUser.HashPassword("appu"), appUser = _appUser };
                userManager.Create(user);
                userManager.AddToRole(user.Id, "AppUser");
            }

            context.Countries.AddOrUpdate(
                  p => p.Name,
                  new Country { Name = "Srbija" },
                  new Country { Name = "BiH" },
                  new Country { Name = "Crna Gora" }
                );
        }
    }
}
