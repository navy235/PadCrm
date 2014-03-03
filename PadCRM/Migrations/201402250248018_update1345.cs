namespace PadCRM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update1345 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.ContractInfo", "SignerID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ContractInfo", "SignerID", c => c.Int(nullable: false));
        }
    }
}
