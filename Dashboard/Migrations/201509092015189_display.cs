namespace Dashboard.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class display : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Devices", "Display", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Devices", "Display");
        }
    }
}
