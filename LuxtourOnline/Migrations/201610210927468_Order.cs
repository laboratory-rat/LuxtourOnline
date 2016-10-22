namespace LuxtourOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Order : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.HotelFeatures", "Order", c => c.Int(nullable: false));
            AddColumn("dbo.HotelElements", "Order", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.HotelElements", "Order");
            DropColumn("dbo.HotelFeatures", "Order");
        }
    }
}
