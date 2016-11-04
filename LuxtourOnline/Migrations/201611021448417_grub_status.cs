namespace LuxtourOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class grub_status : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TelGrubModels", "TelGrubOperators_Id", "dbo.TelGrubOperators");
            DropIndex("dbo.TelGrubModels", new[] { "TelGrubOperators_Id" });
            AddColumn("dbo.TelGrubModels", "Language", c => c.String());
            AddColumn("dbo.TelGrubModels", "Ip", c => c.String());
            AddColumn("dbo.TelGrubModels", "Comment", c => c.String());
            DropColumn("dbo.TelGrubModels", "TelGrubOperators_Id");
            DropTable("dbo.TelGrubOperators");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.TelGrubOperators",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OperatorName = c.String(),
                        OperatorEmail = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.TelGrubModels", "TelGrubOperators_Id", c => c.Int());
            DropColumn("dbo.TelGrubModels", "Comment");
            DropColumn("dbo.TelGrubModels", "Ip");
            DropColumn("dbo.TelGrubModels", "Language");
            CreateIndex("dbo.TelGrubModels", "TelGrubOperators_Id");
            AddForeignKey("dbo.TelGrubModels", "TelGrubOperators_Id", "dbo.TelGrubOperators", "Id");
        }
    }
}
