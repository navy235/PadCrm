namespace PadCRM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changemulti : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Notice",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 50),
                        Content = c.String(),
                        DepartmentID = c.Int(nullable: false),
                        AddUser = c.Int(nullable: false),
                        AddTime = c.DateTime(nullable: false),
                        LastUser = c.Int(nullable: false),
                        LastTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Department", t => t.DepartmentID)
                .Index(t => t.DepartmentID);
            
            CreateTable(
                "dbo.Punish",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Description = c.String(maxLength: 150),
                        MemberID = c.Int(nullable: false),
                        RuleID = c.Int(nullable: false),
                        AddUser = c.Int(nullable: false),
                        AddTime = c.DateTime(nullable: false),
                        LastUser = c.Int(nullable: false),
                        LastTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Member", t => t.MemberID)
                .ForeignKey("dbo.RuleCate", t => t.RuleID)
                .Index(t => t.MemberID)
                .Index(t => t.RuleID);
            
            CreateTable(
                "dbo.RuleCate",
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
                .ForeignKey("dbo.RuleCate", t => t.PCate_ID)
                .Index(t => t.PCate_ID);
            
            CreateTable(
                "dbo.Task",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        StartTime = c.DateTime(nullable: false),
                        EndTime = c.DateTime(nullable: false),
                        Title = c.String(maxLength: 50),
                        Description = c.String(maxLength: 250),
                        MemberID = c.Int(nullable: false),
                        IsAllDay = c.Boolean(nullable: false),
                        RecurrenceRule = c.String(),
                        RecurrenceID = c.Int(),
                        RecurrenceException = c.String(),
                        StartTimeZone = c.String(),
                        EndTimeZone = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Member", t => t.MemberID)
                .Index(t => t.MemberID);
            
            AddColumn("dbo.Group", "Content", c => c.String());
        }
        
        public override void Down()
        {
            DropIndex("dbo.Task", new[] { "MemberID" });
            DropIndex("dbo.RuleCate", new[] { "PCate_ID" });
            DropIndex("dbo.Punish", new[] { "RuleID" });
            DropIndex("dbo.Punish", new[] { "MemberID" });
            DropIndex("dbo.Notice", new[] { "DepartmentID" });
            DropForeignKey("dbo.Task", "MemberID", "dbo.Member");
            DropForeignKey("dbo.RuleCate", "PCate_ID", "dbo.RuleCate");
            DropForeignKey("dbo.Punish", "RuleID", "dbo.RuleCate");
            DropForeignKey("dbo.Punish", "MemberID", "dbo.Member");
            DropForeignKey("dbo.Notice", "DepartmentID", "dbo.Department");
            DropColumn("dbo.Group", "Content");
            DropTable("dbo.Task");
            DropTable("dbo.RuleCate");
            DropTable("dbo.Punish");
            DropTable("dbo.Notice");
        }
    }
}
