namespace LuxtourOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class top_tour_2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TopTours", "SetDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TopTours", "SetDate", c => c.DateTime(nullable: false));
        }
    }
}
