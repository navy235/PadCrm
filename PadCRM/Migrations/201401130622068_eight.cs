namespace PadCRM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class eight : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CustomerShare",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        MemberID = c.Int(nullable: false),
                        AddUser = c.Int(nullable: false),
                        AddTime = c.DateTime(nullable: false),
                        CompanyID = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.CustomerCompany", t => t.CompanyID)
                .ForeignKey("dbo.Member", t => t.MemberID)
                .Index(t => t.CompanyID)
                .Index(t => t.MemberID);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.CustomerShare", new[] { "MemberID" });
            DropIndex("dbo.CustomerShare", new[] { "CompanyID" });
            DropForeignKey("dbo.CustomerShare", "MemberID", "dbo.Member");
            DropForeignKey("dbo.CustomerShare", "CompanyID", "dbo.CustomerCompany");
            DropTable("dbo.CustomerShare");
        }
    }
}
