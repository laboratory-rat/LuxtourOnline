namespace LuxtourOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Aparments", "Deleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.Hotels", "Deleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.Tours", "Deleted", c => c.Boolean(nullable: false));
            DropColumn("dbo.Aparments", "Lang");
            DropColumn("dbo.Aparments", "Description");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Aparments", "Description", c => c.String(nullable: false));
            AddColumn("dbo.Aparments", "Lang", c => c.String(nullable: false));
            DropColumn("dbo.Tours", "Deleted");
            DropColumn("dbo.Hotels", "Deleted");
            DropColumn("dbo.Aparments", "Deleted");
        }
    }
}
