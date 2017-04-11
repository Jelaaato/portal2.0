namespace Portal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IdentityDB : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PortalRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.PortalUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                        IdentityUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.PortalRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.PortalUsers", t => t.IdentityUser_Id)
                .Index(t => t.RoleId)
                .Index(t => t.IdentityUser_Id);
            
            CreateTable(
                "dbo.PortalUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PortalUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                        IdentityUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PortalUsers", t => t.IdentityUser_Id)
                .Index(t => t.IdentityUser_Id);
            
            CreateTable(
                "dbo.PortalUserLogin",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                        IdentityUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.PortalUsers", t => t.IdentityUser_Id)
                .Index(t => t.IdentityUser_Id);
           
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PortalUserRoles", "IdentityUser_Id", "dbo.PortalUsers");
            DropForeignKey("dbo.PortalUserLogin", "IdentityUser_Id", "dbo.PortalUsers");
            DropForeignKey("dbo.PortalUserClaims", "IdentityUser_Id", "dbo.PortalUsers");
            DropForeignKey("dbo.PortalUserRoles", "RoleId", "dbo.PortalRoles");
            DropIndex("dbo.AspNetUsers", new[] { "Id" });
            DropIndex("dbo.PortalUserLogin", new[] { "IdentityUser_Id" });
            DropIndex("dbo.PortalUserClaims", new[] { "IdentityUser_Id" });
            DropIndex("dbo.PortalUserRoles", new[] { "IdentityUser_Id" });
            DropIndex("dbo.PortalUserRoles", new[] { "RoleId" });
            DropIndex("dbo.PortalRoles", "RoleNameIndex");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.PortalUserLogin");
            DropTable("dbo.PortalUserClaims");
            DropTable("dbo.PortalUsers");
            DropTable("dbo.PortalUserRoles");
            DropTable("dbo.PortalRoles");
        }
    }
}
