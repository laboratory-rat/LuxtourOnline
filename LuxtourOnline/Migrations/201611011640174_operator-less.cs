namespace LuxtourOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class operatorless : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TelGrubOperators", "Operator_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.TelGrubModels", "Operator_Id", "dbo.AspNetUsers");
            DropIndex("dbo.TelGrubOperators", new[] { "Operator_Id" });
            DropIndex("dbo.TelGrubModels", new[] { "Operator_Id" });
            AddColumn("dbo.TelGrubOperators", "OperatorName", c => c.String());
            AddColumn("dbo.TelGrubOperators", "OperatorEmail", c => c.String());
            AlterColumn("dbo.TelGrubModels", "Operator_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.TelGrubModels", "Operator_Id");
            AddForeignKey("dbo.TelGrubModels", "Operator_Id", "dbo.TelGrubOperators", "Id", cascadeDelete: true);
            DropColumn("dbo.TelGrubOperators", "Operator_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TelGrubOperators", "Operator_Id", c => c.String(nullable: false, maxLength: 128));
            DropForeignKey("dbo.TelGrubModels", "Operator_Id", "dbo.TelGrubOperators");
            DropIndex("dbo.TelGrubModels", new[] { "Operator_Id" });
            AlterColumn("dbo.TelGrubModels", "Operator_Id", c => c.String(maxLength: 128));
            DropColumn("dbo.TelGrubOperators", "OperatorEmail");
            DropColumn("dbo.TelGrubOperators", "OperatorName");
            CreateIndex("dbo.TelGrubModels", "Operator_Id");
            CreateIndex("dbo.TelGrubOperators", "Operator_Id");
            AddForeignKey("dbo.TelGrubModels", "Operator_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.TelGrubOperators", "Operator_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
