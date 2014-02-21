namespace PadCRM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addfileshare : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FileShare", "DepartmentID", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.FileShare", "DepartmentID");
        }
    }
}
