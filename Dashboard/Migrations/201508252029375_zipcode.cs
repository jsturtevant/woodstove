namespace Dashboard.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class zipcode : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Devices", "ZipCode", c => c.String());
            AlterColumn("dbo.Devices", "Name", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Devices", "Name", c => c.String());
            DropColumn("dbo.Devices", "ZipCode");
        }
    }
}
