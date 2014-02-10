namespace PadCRM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mediarequire : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MediaRequire",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CompanyID = c.Int(nullable: false),
                        Name = c.String(maxLength: 50),
                        Description = c.String(),
                        AttachmentPath = c.String(maxLength: 500),
                        DepartmentID = c.Int(nullable: false),
                        SenderID = c.Int(nullable: false),
                        ResolveID = c.Int(nullable: false),
                        AddUser = c.Int(nullable: false),
                        AddTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Department", t => t.DepartmentID)
                .Index(t => t.DepartmentID);
            
            CreateTable(
                "dbo.ContactRequire",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CompanyID = c.Int(nullable: false),
                        Name = c.String(maxLength: 50),
                        Description = c.String(),
                        SenderID = c.Int(nullable: false),
                        ResolveID = c.Int(nullable: false),
                        AttachmentPath = c.String(maxLength: 500),
                        DepartmentID = c.Int(nullable: false),
                        AddUser = c.Int(nullable: false),
                        AddTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Department", t => t.DepartmentID)
                .Index(t => t.DepartmentID);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.ContactRequire", new[] { "DepartmentID" });
            DropIndex("dbo.MediaRequire", new[] { "DepartmentID" });
            DropForeignKey("dbo.ContactRequire", "DepartmentID", "dbo.Department");
            DropForeignKey("dbo.MediaRequire", "DepartmentID", "dbo.Department");
            DropTable("dbo.ContactRequire");
            DropTable("dbo.MediaRequire");
        }
    }
}
