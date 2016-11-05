namespace LuxtourOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class aprtmetnsOrder : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Apartments", "Order", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Apartments", "Order");
        }
    }
}
