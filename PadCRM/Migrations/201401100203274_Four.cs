namespace PadCRM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Four : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Member", "DepartmentID", c => c.Int(nullable: false));
            AddColumn("dbo.Member", "IsLeader", c => c.Boolean(nullable: false));
            AddForeignKey("dbo.Member", "DepartmentID", "dbo.Department", "ID");
            CreateIndex("dbo.Member", "DepartmentID");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Member", new[] { "DepartmentID" });
            DropForeignKey("dbo.Member", "DepartmentID", "dbo.Department");
            DropColumn("dbo.Member", "IsLeader");
            DropColumn("dbo.Member", "DepartmentID");
        }
    }
}
