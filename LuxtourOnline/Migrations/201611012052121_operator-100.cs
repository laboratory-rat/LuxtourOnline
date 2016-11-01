namespace LuxtourOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class operator100 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TelGrubModels", "Operator_Id", "dbo.TelGrubOperators");
            DropIndex("dbo.TelGrubModels", new[] { "Operator_Id" });
            AddColumn("dbo.AspNetUsers", "AllowTelGrub", c => c.Boolean(nullable: false));
            AddColumn("dbo.TelGrubModels", "TelGrubOperators_Id", c => c.Int());
            AlterColumn("dbo.TelGrubModels", "Operator_Id", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.TelGrubModels", "Operator_Id");
            CreateIndex("dbo.TelGrubModels", "TelGrubOperators_Id");
            AddForeignKey("dbo.TelGrubModels", "TelGrubOperators_Id", "dbo.TelGrubOperators", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TelGrubModels", "TelGrubOperators_Id", "dbo.TelGrubOperators");
            DropIndex("dbo.TelGrubModels", new[] { "TelGrubOperators_Id" });
            DropIndex("dbo.TelGrubModels", new[] { "Operator_Id" });
            AlterColumn("dbo.TelGrubModels", "Operator_Id", c => c.Int(nullable: false));
            DropColumn("dbo.TelGrubModels", "TelGrubOperators_Id");
            DropColumn("dbo.AspNetUsers", "AllowTelGrub");
            CreateIndex("dbo.TelGrubModels", "Operator_Id");
            AddForeignKey("dbo.TelGrubModels", "Operator_Id", "dbo.TelGrubOperators", "Id", cascadeDelete: true);
        }
    }
}
