namespace LuxtourOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Aparments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Lang = c.String(nullable: false),
                        Title = c.String(nullable: false),
                        Description = c.String(nullable: false),
                        Adult = c.Int(),
                        Child = c.Int(),
                        Enabled = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SiteImages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 100),
                        Alt = c.String(),
                        Description = c.String(),
                        Url = c.String(),
                        Path = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Hotels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                        Avaliable = c.Boolean(nullable: false),
                        Rate = c.Int(nullable: false),
                        CreatedTime = c.DateTime(nullable: false),
                        ModifyDate = c.DateTime(),
                        ModifyUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ModifyUser_Id)
                .Index(t => t.ModifyUser_Id);
            
            CreateTable(
                "dbo.HotelDescriptions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Lang = c.String(),
                        Description = c.String(),
                        Hotel_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Hotels", t => t.Hotel_Id)
                .Index(t => t.Hotel_Id);
            
            CreateTable(
                "dbo.HotelFeatures",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Description = c.String(),
                        Glyph = c.String(),
                        HotelDescription_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.HotelDescriptions", t => t.HotelDescription_Id)
                .Index(t => t.HotelDescription_Id);
            
            CreateTable(
                "dbo.HotelElements",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Glyph = c.String(),
                        Feature_Id = c.Int(),
                        HotelFeature_Id = c.Int(),
                        HotelFeature_Id1 = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.HotelFeatures", t => t.Feature_Id)
                .ForeignKey("dbo.HotelFeatures", t => t.HotelFeature_Id)
                .ForeignKey("dbo.HotelFeatures", t => t.HotelFeature_Id1)
                .Index(t => t.Feature_Id)
                .Index(t => t.HotelFeature_Id)
                .Index(t => t.HotelFeature_Id1);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FullName = c.String(),
                        RegIp = c.String(),
                        Active = c.Boolean(nullable: false),
                        RegDate = c.String(),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Reviews",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Tours",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Enable = c.Boolean(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DaysCount = c.Int(),
                        Adults = c.Int(),
                        Child = c.Int(),
                        CreateTime = c.DateTime(nullable: false),
                        ModifyDate = c.DateTime(),
                        Image_Id = c.Int(nullable: false),
                        ModifiedBy_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SiteImages", t => t.Image_Id, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.ModifiedBy_Id, cascadeDelete: true)
                .Index(t => t.Image_Id)
                .Index(t => t.ModifiedBy_Id);
            
            CreateTable(
                "dbo.TourDescriptions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Lang = c.String(nullable: false, maxLength: 2),
                        Title = c.String(),
                        Description = c.String(),
                        ConnectedTour_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Tours", t => t.ConnectedTour_Id)
                .Index(t => t.ConnectedTour_Id);
            
            CreateTable(
                "dbo.Tags",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                        Description = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.SiteImageAparments",
                c => new
                    {
                        SiteImage_Id = c.Int(nullable: false),
                        Aparment_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.SiteImage_Id, t.Aparment_Id })
                .ForeignKey("dbo.SiteImages", t => t.SiteImage_Id, cascadeDelete: true)
                .ForeignKey("dbo.Aparments", t => t.Aparment_Id, cascadeDelete: true)
                .Index(t => t.SiteImage_Id)
                .Index(t => t.Aparment_Id);
            
            CreateTable(
                "dbo.HotelAparments",
                c => new
                    {
                        Hotel_Id = c.Int(nullable: false),
                        Aparment_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Hotel_Id, t.Aparment_Id })
                .ForeignKey("dbo.Hotels", t => t.Hotel_Id, cascadeDelete: true)
                .ForeignKey("dbo.Aparments", t => t.Aparment_Id, cascadeDelete: true)
                .Index(t => t.Hotel_Id)
                .Index(t => t.Aparment_Id);
            
            CreateTable(
                "dbo.HotelSiteImages",
                c => new
                    {
                        Hotel_Id = c.Int(nullable: false),
                        SiteImage_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Hotel_Id, t.SiteImage_Id })
                .ForeignKey("dbo.Hotels", t => t.Hotel_Id, cascadeDelete: true)
                .ForeignKey("dbo.SiteImages", t => t.SiteImage_Id, cascadeDelete: true)
                .Index(t => t.Hotel_Id)
                .Index(t => t.SiteImage_Id);
            
            CreateTable(
                "dbo.ReviewHotels",
                c => new
                    {
                        Review_Id = c.Int(nullable: false),
                        Hotel_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Review_Id, t.Hotel_Id })
                .ForeignKey("dbo.Reviews", t => t.Review_Id, cascadeDelete: true)
                .ForeignKey("dbo.Hotels", t => t.Hotel_Id, cascadeDelete: true)
                .Index(t => t.Review_Id)
                .Index(t => t.Hotel_Id);
            
            CreateTable(
                "dbo.TourReviews",
                c => new
                    {
                        Tour_Id = c.Int(nullable: false),
                        Review_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Tour_Id, t.Review_Id })
                .ForeignKey("dbo.Tours", t => t.Tour_Id, cascadeDelete: true)
                .ForeignKey("dbo.Reviews", t => t.Review_Id, cascadeDelete: true)
                .Index(t => t.Tour_Id)
                .Index(t => t.Review_Id);
            
            CreateTable(
                "dbo.TagAparments",
                c => new
                    {
                        Tag_Id = c.Int(nullable: false),
                        Aparment_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Tag_Id, t.Aparment_Id })
                .ForeignKey("dbo.Tags", t => t.Tag_Id, cascadeDelete: true)
                .ForeignKey("dbo.Aparments", t => t.Aparment_Id, cascadeDelete: true)
                .Index(t => t.Tag_Id)
                .Index(t => t.Aparment_Id);
            
            CreateTable(
                "dbo.TagHotels",
                c => new
                    {
                        Tag_Id = c.Int(nullable: false),
                        Hotel_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Tag_Id, t.Hotel_Id })
                .ForeignKey("dbo.Tags", t => t.Tag_Id, cascadeDelete: true)
                .ForeignKey("dbo.Hotels", t => t.Hotel_Id, cascadeDelete: true)
                .Index(t => t.Tag_Id)
                .Index(t => t.Hotel_Id);
            
            CreateTable(
                "dbo.TagTours",
                c => new
                    {
                        Tag_Id = c.Int(nullable: false),
                        Tour_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Tag_Id, t.Tour_Id })
                .ForeignKey("dbo.Tags", t => t.Tag_Id, cascadeDelete: true)
                .ForeignKey("dbo.Tours", t => t.Tour_Id, cascadeDelete: true)
                .Index(t => t.Tag_Id)
                .Index(t => t.Tour_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.TagTours", "Tour_Id", "dbo.Tours");
            DropForeignKey("dbo.TagTours", "Tag_Id", "dbo.Tags");
            DropForeignKey("dbo.TagHotels", "Hotel_Id", "dbo.Hotels");
            DropForeignKey("dbo.TagHotels", "Tag_Id", "dbo.Tags");
            DropForeignKey("dbo.TagAparments", "Aparment_Id", "dbo.Aparments");
            DropForeignKey("dbo.TagAparments", "Tag_Id", "dbo.Tags");
            DropForeignKey("dbo.TourReviews", "Review_Id", "dbo.Reviews");
            DropForeignKey("dbo.TourReviews", "Tour_Id", "dbo.Tours");
            DropForeignKey("dbo.Tours", "ModifiedBy_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Tours", "Image_Id", "dbo.SiteImages");
            DropForeignKey("dbo.TourDescriptions", "ConnectedTour_Id", "dbo.Tours");
            DropForeignKey("dbo.ReviewHotels", "Hotel_Id", "dbo.Hotels");
            DropForeignKey("dbo.ReviewHotels", "Review_Id", "dbo.Reviews");
            DropForeignKey("dbo.Hotels", "ModifyUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.HotelSiteImages", "SiteImage_Id", "dbo.SiteImages");
            DropForeignKey("dbo.HotelSiteImages", "Hotel_Id", "dbo.Hotels");
            DropForeignKey("dbo.HotelDescriptions", "Hotel_Id", "dbo.Hotels");
            DropForeignKey("dbo.HotelElements", "HotelFeature_Id1", "dbo.HotelFeatures");
            DropForeignKey("dbo.HotelFeatures", "HotelDescription_Id", "dbo.HotelDescriptions");
            DropForeignKey("dbo.HotelElements", "HotelFeature_Id", "dbo.HotelFeatures");
            DropForeignKey("dbo.HotelElements", "Feature_Id", "dbo.HotelFeatures");
            DropForeignKey("dbo.HotelAparments", "Aparment_Id", "dbo.Aparments");
            DropForeignKey("dbo.HotelAparments", "Hotel_Id", "dbo.Hotels");
            DropForeignKey("dbo.SiteImageAparments", "Aparment_Id", "dbo.Aparments");
            DropForeignKey("dbo.SiteImageAparments", "SiteImage_Id", "dbo.SiteImages");
            DropIndex("dbo.TagTours", new[] { "Tour_Id" });
            DropIndex("dbo.TagTours", new[] { "Tag_Id" });
            DropIndex("dbo.TagHotels", new[] { "Hotel_Id" });
            DropIndex("dbo.TagHotels", new[] { "Tag_Id" });
            DropIndex("dbo.TagAparments", new[] { "Aparment_Id" });
            DropIndex("dbo.TagAparments", new[] { "Tag_Id" });
            DropIndex("dbo.TourReviews", new[] { "Review_Id" });
            DropIndex("dbo.TourReviews", new[] { "Tour_Id" });
            DropIndex("dbo.ReviewHotels", new[] { "Hotel_Id" });
            DropIndex("dbo.ReviewHotels", new[] { "Review_Id" });
            DropIndex("dbo.HotelSiteImages", new[] { "SiteImage_Id" });
            DropIndex("dbo.HotelSiteImages", new[] { "Hotel_Id" });
            DropIndex("dbo.HotelAparments", new[] { "Aparment_Id" });
            DropIndex("dbo.HotelAparments", new[] { "Hotel_Id" });
            DropIndex("dbo.SiteImageAparments", new[] { "Aparment_Id" });
            DropIndex("dbo.SiteImageAparments", new[] { "SiteImage_Id" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.TourDescriptions", new[] { "ConnectedTour_Id" });
            DropIndex("dbo.Tours", new[] { "ModifiedBy_Id" });
            DropIndex("dbo.Tours", new[] { "Image_Id" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.HotelElements", new[] { "HotelFeature_Id1" });
            DropIndex("dbo.HotelElements", new[] { "HotelFeature_Id" });
            DropIndex("dbo.HotelElements", new[] { "Feature_Id" });
            DropIndex("dbo.HotelFeatures", new[] { "HotelDescription_Id" });
            DropIndex("dbo.HotelDescriptions", new[] { "Hotel_Id" });
            DropIndex("dbo.Hotels", new[] { "ModifyUser_Id" });
            DropTable("dbo.TagTours");
            DropTable("dbo.TagHotels");
            DropTable("dbo.TagAparments");
            DropTable("dbo.TourReviews");
            DropTable("dbo.ReviewHotels");
            DropTable("dbo.HotelSiteImages");
            DropTable("dbo.HotelAparments");
            DropTable("dbo.SiteImageAparments");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Tags");
            DropTable("dbo.TourDescriptions");
            DropTable("dbo.Tours");
            DropTable("dbo.Reviews");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.HotelElements");
            DropTable("dbo.HotelFeatures");
            DropTable("dbo.HotelDescriptions");
            DropTable("dbo.Hotels");
            DropTable("dbo.SiteImages");
            DropTable("dbo.Aparments");
        }
    }
}
