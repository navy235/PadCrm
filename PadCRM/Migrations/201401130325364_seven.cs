namespace PadCRM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class seven : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Department", "LeaderID", c => c.Int(nullable: false));
            DropColumn("dbo.Department", "Leader");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Department", "Leader", c => c.String(maxLength: 50));
            DropColumn("dbo.Department", "LeaderID");
        }
    }
}
