namespace LuxtourOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class operators2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TelGrubOperators", "Operator_Id", "dbo.AspNetUsers");
            DropIndex("dbo.TelGrubOperators", new[] { "Operator_Id" });
            AlterColumn("dbo.TelGrubOperators", "Operator_Id", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.TelGrubOperators", "Operator_Id");
            AddForeignKey("dbo.TelGrubOperators", "Operator_Id", "dbo.AspNetUsers", "Id", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TelGrubOperators", "Operator_Id", "dbo.AspNetUsers");
            DropIndex("dbo.TelGrubOperators", new[] { "Operator_Id" });
            AlterColumn("dbo.TelGrubOperators", "Operator_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.TelGrubOperators", "Operator_Id");
            AddForeignKey("dbo.TelGrubOperators", "Operator_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
