namespace Dashboard.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Adddevice : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Devices",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationUser_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Devices", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Devices", new[] { "ApplicationUser_Id" });
            DropTable("dbo.Devices");
        }
    }
}
