namespace PadCRM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class workitem : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.WorkItem",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        JobName = c.String(maxLength: 64),
                        WorkerId = c.String(maxLength: 64),
                        Started = c.DateTime(nullable: false),
                        Completed = c.DateTime(),
                        ExceptionInfo = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.WorkItem");
        }
    }
}
