namespace PadCRM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changetask : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Punish", "Score", c => c.Int(nullable: false));
            AddColumn("dbo.Task", "Start", c => c.DateTime(nullable: false));
            AddColumn("dbo.Task", "End", c => c.DateTime(nullable: false));
            DropColumn("dbo.Task", "StartTime");
            DropColumn("dbo.Task", "EndTime");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Task", "EndTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.Task", "StartTime", c => c.DateTime(nullable: false));
            DropColumn("dbo.Task", "End");
            DropColumn("dbo.Task", "Start");
            DropColumn("dbo.Punish", "Score");
        }
    }
}
