namespace LuxtourOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class turbo : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TopTours",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Factor = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SetDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Tours", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.TopHotels",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Factor = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SetDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Hotels", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.Subscribes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Email = c.String(),
                        Tel = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        SubscribeDate = c.DateTime(nullable: false),
                        Language = c.String(),
                        RemoveSubscribeString = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TopHotels", "Id", "dbo.Hotels");
            DropForeignKey("dbo.TopTours", "Id", "dbo.Tours");
            DropIndex("dbo.TopHotels", new[] { "Id" });
            DropIndex("dbo.TopTours", new[] { "Id" });
            DropTable("dbo.Subscribes");
            DropTable("dbo.TopHotels");
            DropTable("dbo.TopTours");
        }
    }
}
