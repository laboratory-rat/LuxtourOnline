namespace LuxtourOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class orders2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "OrderDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Orders", "Price", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.CustomerDatas", "Birthday", c => c.DateTime());
            AlterColumn("dbo.CustomerDatas", "PassportUntil", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.CustomerDatas", "PassportUntil", c => c.DateTime(nullable: false));
            AlterColumn("dbo.CustomerDatas", "Birthday", c => c.DateTime(nullable: false));
            DropColumn("dbo.Orders", "Price");
            DropColumn("dbo.Orders", "OrderDate");
        }
    }
}
