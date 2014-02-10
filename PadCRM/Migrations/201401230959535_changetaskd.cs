namespace PadCRM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changetaskd : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Task", "IsAllDay");
            DropColumn("dbo.Task", "RecurrenceRule");
            DropColumn("dbo.Task", "RecurrenceID");
            DropColumn("dbo.Task", "RecurrenceException");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Task", "RecurrenceException", c => c.String());
            AddColumn("dbo.Task", "RecurrenceID", c => c.Int());
            AddColumn("dbo.Task", "RecurrenceRule", c => c.String());
            AddColumn("dbo.Task", "IsAllDay", c => c.Boolean(nullable: false));
        }
    }
}
