using Microsoft.AspNet.Identity.EntityFramework;
using SingleSignOn.Models.IdentityDbModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SingleSignOn.Models
{
    public class IdentityDb : IdentityDbContext<Users>
    {
        public IdentityDb()
            : base("IdentityDb") { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<IdentityUser>().ToTable("WebAPPUsers");
            modelBuilder.Entity<IdentityRole>().ToTable("WebAPPRoles");
            modelBuilder.Entity<IdentityUserRole>().ToTable("WebAPPUserRoles");
            modelBuilder.Entity<IdentityUserClaim>().ToTable("WebAPPUserClaims");
            modelBuilder.Entity<IdentityUserLogin>().ToTable("WebAPPUserLogin");
        }

        static IdentityDb()
        {
            Database.SetInitializer<IdentityDb>(new IdentityDbSeed());
        }

        public static IdentityDb Create()
        {
            return new IdentityDb();
        }
    }

    public class IdentityDbSeed : DropCreateDatabaseIfModelChanges<IdentityDb>
    {
        protected override void Seed(IdentityDb context)
        {
            PerformInitialSetup(context);
            base.Seed(context);
        }

        public void PerformInitialSetup(IdentityDb Context)
        {
            //UsersManager UserManager = new UsersManager(new UserStore<Users>(Context));
            //RolesManager RoleManager = new RolesManager(new RoleStore<roles>(Context));

            //string Name = "Administrator";
            //string UserName = "appAdmin";
            //string Password = "P0rt4l@dm!n";
            //string email = "elabetoria@asianhospital.com";

            
        }
    }
}