namespace LuxtourOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class operators : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TelGrubModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CreatedTime = c.DateTime(nullable: false),
                        FullName = c.String(),
                        TelNumber = c.String(),
                        Status = c.Int(nullable: false),
                        GrubKey = c.String(),
                        GrubTime = c.DateTime(),
                        Operator_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Operator_Id)
                .Index(t => t.Operator_Id);
            
            CreateTable(
                "dbo.TelGrubOperators",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Operator_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Operator_Id)
                .Index(t => t.Operator_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TelGrubOperators", "Operator_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.TelGrubModels", "Operator_Id", "dbo.AspNetUsers");
            DropIndex("dbo.TelGrubOperators", new[] { "Operator_Id" });
            DropIndex("dbo.TelGrubModels", new[] { "Operator_Id" });
            DropTable("dbo.TelGrubOperators");
            DropTable("dbo.TelGrubModels");
        }
    }
}
