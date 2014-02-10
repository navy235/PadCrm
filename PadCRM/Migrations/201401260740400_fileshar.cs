namespace PadCRM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fileshar : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FileShare",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 50),
                        Description = c.String(),
                        AddUser = c.Int(nullable: false),
                        AddTime = c.DateTime(nullable: false),
                        FilePath = c.String(maxLength: 500),
                        FileCateID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.FileCate", t => t.FileCateID)
                .ForeignKey("dbo.Member", t => t.AddUser)
                .Index(t => t.FileCateID)
                .Index(t => t.AddUser);
            
            CreateTable(
                "dbo.FileCate",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CateName = c.String(maxLength: 50),
                        PID = c.Int(),
                        Code = c.Int(nullable: false),
                        Level = c.Int(nullable: false),
                        OrderIndex = c.Int(nullable: false),
                        PCate_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.FileCate", t => t.PCate_ID)
                .Index(t => t.PCate_ID);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.FileCate", new[] { "PCate_ID" });
            DropIndex("dbo.FileShare", new[] { "AddUser" });
            DropIndex("dbo.FileShare", new[] { "FileCateID" });
            DropForeignKey("dbo.FileCate", "PCate_ID", "dbo.FileCate");
            DropForeignKey("dbo.FileShare", "AddUser", "dbo.Member");
            DropForeignKey("dbo.FileShare", "FileCateID", "dbo.FileCate");
            DropTable("dbo.FileCate");
            DropTable("dbo.FileShare");
        }
    }
}
