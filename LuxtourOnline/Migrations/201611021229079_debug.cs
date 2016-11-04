namespace LuxtourOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class debug : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TelGrubModels", "Operator_Id", "dbo.AspNetUsers");
            DropIndex("dbo.TelGrubModels", new[] { "Operator_Id" });
            AlterColumn("dbo.TelGrubModels", "Operator_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.TelGrubModels", "Operator_Id");
            AddForeignKey("dbo.TelGrubModels", "Operator_Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TelGrubModels", "Operator_Id", "dbo.AspNetUsers");
            DropIndex("dbo.TelGrubModels", new[] { "Operator_Id" });
            AlterColumn("dbo.TelGrubModels", "Operator_Id", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.TelGrubModels", "Operator_Id");
            AddForeignKey("dbo.TelGrubModels", "Operator_Id", "dbo.AspNetUsers", "Id", cascadeDelete: false);
        }
    }
}
