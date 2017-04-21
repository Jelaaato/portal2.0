namespace Portal.Migrations
{
    using Microsoft.AspNet.Identity;
    using Portal.Models.IdentityDBModel_TemporaryLogin_;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Portal.Models.IdentityDBModel_TemporaryLogin_.IdentityDB>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Portal.Models.IdentityDBModel_TemporaryLogin_.IdentityDB context)
        {
            var hashPassword = new PasswordHasher();
            string pass = hashPassword.HashPassword("@Pp$d3v");
            context.Users.AddOrUpdate(u => u.UserName, new Users
            {
                UserName = "@dministrator",
                PasswordHash = pass
            });
        }
    }
}
