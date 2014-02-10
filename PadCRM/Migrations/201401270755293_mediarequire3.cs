namespace PadCRM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mediarequire3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MediaRequire", "PID", c => c.Int(nullable: false));
            AddColumn("dbo.ContactRequire", "PID", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ContactRequire", "PID");
            DropColumn("dbo.MediaRequire", "PID");
        }
    }
}
