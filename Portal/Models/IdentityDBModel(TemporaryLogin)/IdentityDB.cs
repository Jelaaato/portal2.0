using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Portal.Models.IdentityDBModel_TemporaryLogin_
{
    public class IdentityDB: IdentityDbContext<Users>
    {
        public IdentityDB()
            : base("IdentityDB") { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<IdentityUser>().ToTable("PortalUsers");
            modelBuilder.Entity<IdentityRole>().ToTable("PortalRoles");
            modelBuilder.Entity<IdentityUserRole>().ToTable("PortalUserRoles");
            modelBuilder.Entity<IdentityUserClaim>().ToTable("PortalUserClaims");
            modelBuilder.Entity<IdentityUserLogin>().ToTable("PortalUserLogin");
        }

        static IdentityDB()
        {
            Database.SetInitializer<IdentityDB>(new IdentityDbSeed());
        }

        public static IdentityDB Create()
        {
            return new IdentityDB();
        }
    }

    public class IdentityDbSeed : DropCreateDatabaseIfModelChanges<IdentityDB>
    {
        protected override void Seed(IdentityDB context)
        {
            PerformInitialSetup(context);
            base.Seed(context);
        }

        public void PerformInitialSetup(IdentityDB Context)
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