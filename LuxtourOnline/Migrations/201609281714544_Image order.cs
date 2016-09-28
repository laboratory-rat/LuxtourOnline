namespace LuxtourOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Imageorder : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SiteImages", "Order", c => c.Int(nullable: false));
            AlterColumn("dbo.SiteImages", "Title", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SiteImages", "Title", c => c.String(nullable: false, maxLength: 100));
            DropColumn("dbo.SiteImages", "Order");
        }
    }
}
