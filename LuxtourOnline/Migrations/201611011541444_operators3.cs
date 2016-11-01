namespace LuxtourOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class operators3 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TelGrubOperators", "Operator_Id", "dbo.AspNetUsers");
            AddForeignKey("dbo.TelGrubOperators", "Operator_Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TelGrubOperators", "Operator_Id", "dbo.AspNetUsers");
            AddForeignKey("dbo.TelGrubOperators", "Operator_Id", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
    }
}
