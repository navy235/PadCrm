namespace PadCRM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class idnumber : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Member", "IDNumber", c => c.String());
            AddColumn("dbo.Member", "JobExp", c => c.String());
            AddColumn("dbo.Member", "StudyExp", c => c.String());
            AddColumn("dbo.Member", "FamilySituation", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Member", "FamilySituation");
            DropColumn("dbo.Member", "StudyExp");
            DropColumn("dbo.Member", "JobExp");
            DropColumn("dbo.Member", "IDNumber");
        }
    }
}
