namespace LuxtourOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Cascade2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.HotelDescriptions", "Hotel_Id", "dbo.Hotels");
            DropForeignKey("dbo.HotelFeatures", "HotelDescription_Id", "dbo.HotelDescriptions");
            DropForeignKey("dbo.HotelElements", "Feature_Id", "dbo.HotelFeatures");
            DropIndex("dbo.HotelDescriptions", new[] { "Hotel_Id" });
            DropIndex("dbo.HotelFeatures", new[] { "HotelDescription_Id" });
            DropIndex("dbo.HotelElements", new[] { "Feature_Id" });
            AlterColumn("dbo.HotelDescriptions", "Hotel_Id", c => c.Int(nullable: false));
            AlterColumn("dbo.HotelFeatures", "HotelDescription_Id", c => c.Int(nullable: false));
            AlterColumn("dbo.HotelElements", "Feature_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.HotelDescriptions", "Hotel_Id");
            CreateIndex("dbo.HotelFeatures", "HotelDescription_Id");
            CreateIndex("dbo.HotelElements", "Feature_Id");
            AddForeignKey("dbo.HotelDescriptions", "Hotel_Id", "dbo.Hotels", "Id", cascadeDelete: true);
            AddForeignKey("dbo.HotelFeatures", "HotelDescription_Id", "dbo.HotelDescriptions", "Id", cascadeDelete: true);
            AddForeignKey("dbo.HotelElements", "Feature_Id", "dbo.HotelFeatures", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.HotelElements", "Feature_Id", "dbo.HotelFeatures");
            DropForeignKey("dbo.HotelFeatures", "HotelDescription_Id", "dbo.HotelDescriptions");
            DropForeignKey("dbo.HotelDescriptions", "Hotel_Id", "dbo.Hotels");
            DropIndex("dbo.HotelElements", new[] { "Feature_Id" });
            DropIndex("dbo.HotelFeatures", new[] { "HotelDescription_Id" });
            DropIndex("dbo.HotelDescriptions", new[] { "Hotel_Id" });
            AlterColumn("dbo.HotelElements", "Feature_Id", c => c.Int());
            AlterColumn("dbo.HotelFeatures", "HotelDescription_Id", c => c.Int());
            AlterColumn("dbo.HotelDescriptions", "Hotel_Id", c => c.Int());
            CreateIndex("dbo.HotelElements", "Feature_Id");
            CreateIndex("dbo.HotelFeatures", "HotelDescription_Id");
            CreateIndex("dbo.HotelDescriptions", "Hotel_Id");
            AddForeignKey("dbo.HotelElements", "Feature_Id", "dbo.HotelFeatures", "Id");
            AddForeignKey("dbo.HotelFeatures", "HotelDescription_Id", "dbo.HotelDescriptions", "Id");
            AddForeignKey("dbo.HotelDescriptions", "Hotel_Id", "dbo.Hotels", "Id");
        }
    }
}
