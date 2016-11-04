namespace LuxtourOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class top_tour_3 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SiteImages", "Apartment_Id", "dbo.Apartments");
            DropIndex("dbo.SiteImages", new[] { "Apartment_Id" });
            DropColumn("dbo.SiteImages", "Apartment_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SiteImages", "Apartment_Id", c => c.Int());
            CreateIndex("dbo.SiteImages", "Apartment_Id");
            AddForeignKey("dbo.SiteImages", "Apartment_Id", "dbo.Apartments", "Id");
        }
    }
}
