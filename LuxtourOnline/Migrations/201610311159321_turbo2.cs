namespace LuxtourOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class turbo2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Subscribes", "FullName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Subscribes", "FullName");
        }
    }
}
