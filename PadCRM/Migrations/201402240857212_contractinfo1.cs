namespace PadCRM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class contractinfo1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ContractInfo", "NextTime", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ContractInfo", "NextTime");
        }
    }
}
