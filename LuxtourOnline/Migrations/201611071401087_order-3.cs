namespace LuxtourOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class order3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "FlyOutCity", c => c.String());
            AddColumn("dbo.Orders", "Ip", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "Ip");
            DropColumn("dbo.Orders", "FlyOutCity");
        }
    }
}
