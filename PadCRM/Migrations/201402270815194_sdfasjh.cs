namespace PadCRM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sdfasjh : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Department_TcNotice",
                c => new
                    {
                        DepartmentID = c.Int(nullable: false),
                        TcNoticeID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.DepartmentID, t.TcNoticeID })
                .ForeignKey("dbo.Department", t => t.DepartmentID, cascadeDelete: true)
                .ForeignKey("dbo.TcNotice", t => t.TcNoticeID, cascadeDelete: true)
                .Index(t => t.DepartmentID)
                .Index(t => t.TcNoticeID);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Department_TcNotice", new[] { "TcNoticeID" });
            DropIndex("dbo.Department_TcNotice", new[] { "DepartmentID" });
            DropForeignKey("dbo.Department_TcNotice", "TcNoticeID", "dbo.TcNotice");
            DropForeignKey("dbo.Department_TcNotice", "DepartmentID", "dbo.Department");
            DropTable("dbo.Department_TcNotice");
        }
    }
}
