namespace LuxtourOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class image_problem4 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TourDescriptions", "ConnectedTour_Id", "dbo.Tours");
            DropIndex("dbo.TourDescriptions", new[] { "ConnectedTour_Id" });
            AlterColumn("dbo.TourDescriptions", "ConnectedTour_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.TourDescriptions", "ConnectedTour_Id");
            AddForeignKey("dbo.TourDescriptions", "ConnectedTour_Id", "dbo.Tours", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TourDescriptions", "ConnectedTour_Id", "dbo.Tours");
            DropIndex("dbo.TourDescriptions", new[] { "ConnectedTour_Id" });
            AlterColumn("dbo.TourDescriptions", "ConnectedTour_Id", c => c.Int());
            CreateIndex("dbo.TourDescriptions", "ConnectedTour_Id");
            AddForeignKey("dbo.TourDescriptions", "ConnectedTour_Id", "dbo.Tours", "Id");
        }
    }
}
