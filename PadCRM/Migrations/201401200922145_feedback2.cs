namespace PadCRM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class feedback2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Member", "JobTitleCate_ID", "dbo.JobTitleCate");
            DropIndex("dbo.Member", new[] { "JobTitleCate_ID" });
            AddForeignKey("dbo.Member", "JobTitleID", "dbo.JobTitleCate", "ID");
            CreateIndex("dbo.Member", "JobTitleID");
            DropColumn("dbo.Member", "JobTitleCate_ID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Member", "JobTitleCate_ID", c => c.Int());
            DropIndex("dbo.Member", new[] { "JobTitleID" });
            DropForeignKey("dbo.Member", "JobTitleID", "dbo.JobTitleCate");
            CreateIndex("dbo.Member", "JobTitleCate_ID");
            AddForeignKey("dbo.Member", "JobTitleCate_ID", "dbo.JobTitleCate", "ID");
        }
    }
}
