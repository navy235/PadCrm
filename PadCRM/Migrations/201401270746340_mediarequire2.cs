namespace PadCRM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mediarequire2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MediaRequire", "IsRoot", c => c.Int(nullable: false));
            AddColumn("dbo.MediaRequire", "Status", c => c.Int(nullable: false));
            AddColumn("dbo.ContactRequire", "IsRoot", c => c.Int(nullable: false));
            AddColumn("dbo.ContactRequire", "Status", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ContactRequire", "Status");
            DropColumn("dbo.ContactRequire", "IsRoot");
            DropColumn("dbo.MediaRequire", "Status");
            DropColumn("dbo.MediaRequire", "IsRoot");
        }
    }
}
