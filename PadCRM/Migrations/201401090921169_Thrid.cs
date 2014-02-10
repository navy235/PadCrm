namespace PadCRM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Thrid : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Department", "PID", c => c.Int());
            AddColumn("dbo.Department", "Code", c => c.Int(nullable: false));
            AddColumn("dbo.Department", "Level", c => c.Int(nullable: false));
            AddForeignKey("dbo.Department", "PID", "dbo.Department", "ID");
            CreateIndex("dbo.Department", "PID");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Department", new[] { "PID" });
            DropForeignKey("dbo.Department", "PID", "dbo.Department");
            DropColumn("dbo.Department", "Level");
            DropColumn("dbo.Department", "Code");
            DropColumn("dbo.Department", "PID");
        }
    }
}
