namespace SingleSignOn.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IdentityDb : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.WebAPPRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.WebAPPUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                        IdentityUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.WebAPPRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.WebAPPUsers", t => t.IdentityUser_Id)
                .Index(t => t.RoleId)
                .Index(t => t.IdentityUser_Id);
            
            CreateTable(
                "dbo.WebAPPUsers",
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
                "dbo.WebAPPUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                        IdentityUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.WebAPPUsers", t => t.IdentityUser_Id)
                .Index(t => t.IdentityUser_Id);
            
            CreateTable(
                "dbo.WebAPPUserLogin",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                        IdentityUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.WebAPPUsers", t => t.IdentityUser_Id)
                .Index(t => t.IdentityUser_Id);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.WebAPPUsers", t => t.Id)
                .Index(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "Id", "dbo.WebAPPUsers");
            DropForeignKey("dbo.WebAPPUserRoles", "IdentityUser_Id", "dbo.WebAPPUsers");
            DropForeignKey("dbo.WebAPPUserLogin", "IdentityUser_Id", "dbo.WebAPPUsers");
            DropForeignKey("dbo.WebAPPUserClaims", "IdentityUser_Id", "dbo.WebAPPUsers");
            DropForeignKey("dbo.WebAPPUserRoles", "RoleId", "dbo.WebAPPRoles");
            DropIndex("dbo.AspNetUsers", new[] { "Id" });
            DropIndex("dbo.WebAPPUserLogin", new[] { "IdentityUser_Id" });
            DropIndex("dbo.WebAPPUserClaims", new[] { "IdentityUser_Id" });
            DropIndex("dbo.WebAPPUserRoles", new[] { "IdentityUser_Id" });
            DropIndex("dbo.WebAPPUserRoles", new[] { "RoleId" });
            DropIndex("dbo.WebAPPRoles", "RoleNameIndex");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.WebAPPUserLogin");
            DropTable("dbo.WebAPPUserClaims");
            DropTable("dbo.WebAPPUsers");
            DropTable("dbo.WebAPPUserRoles");
            DropTable("dbo.WebAPPRoles");
        }
    }
}
