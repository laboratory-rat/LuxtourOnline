namespace LuxtourOnline.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<LuxtourOnline.Models.SiteDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;


        }

        protected override void Seed(LuxtourOnline.Models.SiteDbContext context)
        {
            var userManger = new AppUserManager(new UserStore<AppUser>(context));
            var roleManager = new RoleManager<AppUserRole>(new RoleStore<AppUserRole>(context));

            // создаем две роли
            var role1 = new AppUserRole { Name = "admin" };
            var role2 = new AppUserRole { Name = "manager" };
            var role5 = new AppUserRole { Name = "content_manager" };
            var role3 = new AppUserRole { Name = "agent" };
            var role4 = new AppUserRole { Name = "user" };

            // добавляем роли в бд
            roleManager.Create(role1);
            roleManager.Create(role2);
            roleManager.Create(role5);
            roleManager.Create(role3);
            roleManager.Create(role4);

            var admin = userManger.FindByEmail("oleg.timofeev20@gmail.com") ?? null;
            if (admin == null)
            {
                admin = new AppUser("oleg.timofeev20@gmail.com");
                admin.FullName = "Oleg Timofeev";
                admin.PhoneNumber = "0508837161";
                admin.RegIp = "0.0.0.0";
                admin.RegDate = DateTime.Now.Date.ToShortDateString();

                userManger.Create(admin, "tf27324");

                userManger.AddToRole(admin.Id, role1.Name);
            }
        }
    }
}