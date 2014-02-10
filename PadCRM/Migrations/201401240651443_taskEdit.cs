namespace PadCRM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class taskEdit : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Task", "AddUser", c => c.Int(nullable: false));
            AddColumn("dbo.Task", "AddTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.Task", "IsOtherAdd", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Task", "IsOtherAdd");
            DropColumn("dbo.Task", "AddTime");
            DropColumn("dbo.Task", "AddUser");
        }
    }
}
