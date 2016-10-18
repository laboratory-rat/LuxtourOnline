namespace LuxtourOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Apartments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                        Adult = c.Int(),
                        Child = c.Int(),
                        Enabled = c.Boolean(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                        Tag_Id = c.Int(),
                        Hotel_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Tags", t => t.Tag_Id)
                .ForeignKey("dbo.Hotels", t => t.Hotel_Id, cascadeDelete: true)
                .Index(t => t.Tag_Id)
                .Index(t => t.Hotel_Id);
            
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
                        Deleted = c.Boolean(nullable: false),
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
                        Hotel_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Hotels", t => t.Hotel_Id, cascadeDelete: true)
                .Index(t => t.Hotel_Id);
            
            CreateTable(
                "dbo.HotelFeatures",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Description = c.String(),
                        Glyph = c.String(),
                        HotelDescription_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.HotelDescriptions", t => t.HotelDescription_Id, cascadeDelete: true)
                .Index(t => t.HotelDescription_Id);
            
            CreateTable(
                "dbo.HotelElements",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Glyph = c.String(),
                        Feature_Id = c.Int(nullable: false),
                        HotelFeature_Id = c.Int(),
                        HotelFeature_Id1 = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.HotelFeatures", t => t.Feature_Id, cascadeDelete: true)
                .ForeignKey("dbo.HotelFeatures", t => t.HotelFeature_Id)
                .ForeignKey("dbo.HotelFeatures", t => t.HotelFeature_Id1)
                .Index(t => t.Feature_Id)
                .Index(t => t.HotelFeature_Id)
                .Index(t => t.HotelFeature_Id1);
            
            CreateTable(
                "dbo.SiteImages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Alt = c.String(),
                        Description = c.String(),
                        Url = c.String(),
                        Path = c.String(),
                        Order = c.Int(nullable: false),
                        Apartment_Id = c.Int(),
                        Hotel_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Apartments", t => t.Apartment_Id)
                .ForeignKey("dbo.Hotels", t => t.Hotel_Id)
                .Index(t => t.Apartment_Id)
                .Index(t => t.Hotel_Id);
            
            CreateTable(
                "dbo.Tours",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Enable = c.Boolean(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DaysCount = c.Int(),
                        Adults = c.Int(),
                        Child = c.Int(),
                        CreateTime = c.DateTime(nullable: false),
                        ModifyDate = c.DateTime(),
                        Deleted = c.Boolean(nullable: false),
                        ModifiedBy_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SiteImages", t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ModifiedBy_Id, cascadeDelete: true)
                .Index(t => t.Id)
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
                "dbo.Orders",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Date = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                        City = c.String(nullable: false),
                        Phone = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        Comments = c.String(),
                        Apartment_Id = c.Int(nullable: false),
                        Hotel_Id = c.Int(nullable: false),
                        Tour_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Apartments", t => t.Apartment_Id, cascadeDelete: false)
                .ForeignKey("dbo.Hotels", t => t.Hotel_Id, cascadeDelete: false)
                .ForeignKey("dbo.Tours", t => t.Tour_Id, cascadeDelete: false)
                .Index(t => t.Apartment_Id)
                .Index(t => t.Hotel_Id)
                .Index(t => t.Tour_Id);
            
            CreateTable(
                "dbo.CustomerDatas",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FullName = c.String(nullable: false),
                        IsChild = c.Boolean(nullable: false),
                        Birthday = c.DateTime(nullable: false),
                        CountryFrom = c.String(),
                        CountryLive = c.String(),
                        PassportData = c.String(),
                        PassportNumber = c.String(),
                        PassportFrom = c.String(),
                        PassportUntil = c.DateTime(nullable: false),
                        Email = c.String(),
                        Phone = c.String(),
                        City = c.String(),
                        LoadPassportImages = c.Boolean(nullable: false),
                        Order_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Orders", t => t.Order_Id, cascadeDelete: false)
                .Index(t => t.Order_Id);
            
            CreateTable(
                "dbo.PassportImages",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Data = c.Binary(nullable: false),
                        CustomerData_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CustomerDatas", t => t.CustomerData_Id, cascadeDelete: true)
                .Index(t => t.CustomerData_Id);
            
            CreateTable(
                "dbo.Reviews",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Hotel_Id = c.Int(),
                        Tour_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Hotels", t => t.Hotel_Id)
                .ForeignKey("dbo.Tours", t => t.Tour_Id)
                .Index(t => t.Hotel_Id)
                .Index(t => t.Tour_Id);
            
            CreateTable(
                "dbo.Tags",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Logs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EventDateTime = c.DateTime(nullable: false),
                        EventLevel = c.String(nullable: false, maxLength: 100),
                        UserName = c.String(nullable: false, maxLength: 100),
                        MachineName = c.String(nullable: false, maxLength: 100),
                        EventMessage = c.String(nullable: false),
                        ErrorSource = c.String(maxLength: 100),
                        ErrorClass = c.String(maxLength: 500),
                        ErrorMethod = c.String(),
                        ErrorMessage = c.String(),
                        InnerErrorMessage = c.String(),
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
            DropForeignKey("dbo.Apartments", "Hotel_Id", "dbo.Hotels");
            DropForeignKey("dbo.Hotels", "ModifyUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.TagTours", "Tour_Id", "dbo.Tours");
            DropForeignKey("dbo.TagTours", "Tag_Id", "dbo.Tags");
            DropForeignKey("dbo.TagHotels", "Hotel_Id", "dbo.Hotels");
            DropForeignKey("dbo.TagHotels", "Tag_Id", "dbo.Tags");
            DropForeignKey("dbo.Apartments", "Tag_Id", "dbo.Tags");
            DropForeignKey("dbo.Reviews", "Tour_Id", "dbo.Tours");
            DropForeignKey("dbo.Reviews", "Hotel_Id", "dbo.Hotels");
            DropForeignKey("dbo.Orders", "Tour_Id", "dbo.Tours");
            DropForeignKey("dbo.Orders", "Hotel_Id", "dbo.Hotels");
            DropForeignKey("dbo.PassportImages", "CustomerData_Id", "dbo.CustomerDatas");
            DropForeignKey("dbo.CustomerDatas", "Order_Id", "dbo.Orders");
            DropForeignKey("dbo.Orders", "Apartment_Id", "dbo.Apartments");
            DropForeignKey("dbo.Tours", "ModifiedBy_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Tours", "Id", "dbo.SiteImages");
            DropForeignKey("dbo.TourDescriptions", "ConnectedTour_Id", "dbo.Tours");
            DropForeignKey("dbo.SiteImages", "Hotel_Id", "dbo.Hotels");
            DropForeignKey("dbo.SiteImages", "Apartment_Id", "dbo.Apartments");
            DropForeignKey("dbo.HotelDescriptions", "Hotel_Id", "dbo.Hotels");
            DropForeignKey("dbo.HotelElements", "HotelFeature_Id1", "dbo.HotelFeatures");
            DropForeignKey("dbo.HotelFeatures", "HotelDescription_Id", "dbo.HotelDescriptions");
            DropForeignKey("dbo.HotelElements", "HotelFeature_Id", "dbo.HotelFeatures");
            DropForeignKey("dbo.HotelElements", "Feature_Id", "dbo.HotelFeatures");
            DropIndex("dbo.TagTours", new[] { "Tour_Id" });
            DropIndex("dbo.TagTours", new[] { "Tag_Id" });
            DropIndex("dbo.TagHotels", new[] { "Hotel_Id" });
            DropIndex("dbo.TagHotels", new[] { "Tag_Id" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Reviews", new[] { "Tour_Id" });
            DropIndex("dbo.Reviews", new[] { "Hotel_Id" });
            DropIndex("dbo.PassportImages", new[] { "CustomerData_Id" });
            DropIndex("dbo.CustomerDatas", new[] { "Order_Id" });
            DropIndex("dbo.Orders", new[] { "Tour_Id" });
            DropIndex("dbo.Orders", new[] { "Hotel_Id" });
            DropIndex("dbo.Orders", new[] { "Apartment_Id" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.TourDescriptions", new[] { "ConnectedTour_Id" });
            DropIndex("dbo.Tours", new[] { "ModifiedBy_Id" });
            DropIndex("dbo.Tours", new[] { "Id" });
            DropIndex("dbo.SiteImages", new[] { "Hotel_Id" });
            DropIndex("dbo.SiteImages", new[] { "Apartment_Id" });
            DropIndex("dbo.HotelElements", new[] { "HotelFeature_Id1" });
            DropIndex("dbo.HotelElements", new[] { "HotelFeature_Id" });
            DropIndex("dbo.HotelElements", new[] { "Feature_Id" });
            DropIndex("dbo.HotelFeatures", new[] { "HotelDescription_Id" });
            DropIndex("dbo.HotelDescriptions", new[] { "Hotel_Id" });
            DropIndex("dbo.Hotels", new[] { "ModifyUser_Id" });
            DropIndex("dbo.Apartments", new[] { "Hotel_Id" });
            DropIndex("dbo.Apartments", new[] { "Tag_Id" });
            DropTable("dbo.TagTours");
            DropTable("dbo.TagHotels");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Logs");
            DropTable("dbo.Tags");
            DropTable("dbo.Reviews");
            DropTable("dbo.PassportImages");
            DropTable("dbo.CustomerDatas");
            DropTable("dbo.Orders");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.TourDescriptions");
            DropTable("dbo.Tours");
            DropTable("dbo.SiteImages");
            DropTable("dbo.HotelElements");
            DropTable("dbo.HotelFeatures");
            DropTable("dbo.HotelDescriptions");
            DropTable("dbo.Hotels");
            DropTable("dbo.Apartments");
        }
    }
}
