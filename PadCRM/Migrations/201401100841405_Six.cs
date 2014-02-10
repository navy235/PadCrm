namespace PadCRM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Six : DbMigration
    {
        public override void Up()
        {
            AddForeignKey("dbo.CustomerCompany", "AddUser", "dbo.Member", "MemberID");
            AddForeignKey("dbo.PlanLog", "AddUser", "dbo.Member", "MemberID");
            AddForeignKey("dbo.TraceLog", "AddUser", "dbo.Member", "MemberID");
            AddForeignKey("dbo.Customer", "AddUser", "dbo.Member", "MemberID");
            CreateIndex("dbo.CustomerCompany", "AddUser");
            CreateIndex("dbo.PlanLog", "AddUser");
            CreateIndex("dbo.TraceLog", "AddUser");
            CreateIndex("dbo.Customer", "AddUser");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Customer", new[] { "AddUser" });
            DropIndex("dbo.TraceLog", new[] { "AddUser" });
            DropIndex("dbo.PlanLog", new[] { "AddUser" });
            DropIndex("dbo.CustomerCompany", new[] { "AddUser" });
            DropForeignKey("dbo.Customer", "AddUser", "dbo.Member");
            DropForeignKey("dbo.TraceLog", "AddUser", "dbo.Member");
            DropForeignKey("dbo.PlanLog", "AddUser", "dbo.Member");
            DropForeignKey("dbo.CustomerCompany", "AddUser", "dbo.Member");
        }
    }
}
