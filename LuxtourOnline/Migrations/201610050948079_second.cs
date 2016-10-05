namespace LuxtourOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class second : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Logs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EventDateTime = c.DateTime(nullable: false),
                        EventLevel = c.String(nullable: false, maxLength: 100),
                        UserName = c.String(nullable: false, maxLength: 100),
                        MachineName = c.String(nullable: false, maxLength: 100),
                        EventMessage = c.String(nullable: false),
                        ErrorSource = c.String(maxLength: 100),
                        ErrorClass = c.String(maxLength: 500),
                        ErrorMethod = c.String(),
                        ErrorMessage = c.String(),
                        InnerErrorMessage = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Logs");
        }
    }
}
