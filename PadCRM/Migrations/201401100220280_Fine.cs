namespace PadCRM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Fine : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Member", "IsLeap", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Member", "IsLeap");
        }
    }
}
