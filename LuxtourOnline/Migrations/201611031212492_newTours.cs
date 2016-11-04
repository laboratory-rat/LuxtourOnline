namespace LuxtourOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newTours : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SiteImages", "Name", c => c.String());
            AddColumn("dbo.SiteImages", "Extension", c => c.String());
            AddColumn("dbo.SiteImages", "IsTmp", c => c.Boolean(nullable: false));
            AddColumn("dbo.Tours", "TravelAndFood", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tours", "TravelAndFood");
            DropColumn("dbo.SiteImages", "IsTmp");
            DropColumn("dbo.SiteImages", "Extension");
            DropColumn("dbo.SiteImages", "Name");
        }
    }
}
