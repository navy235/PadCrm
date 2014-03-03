namespace PadCRM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update134545 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.ContractInfo", name: "Signer_MemberID", newName: "SignerID");
        }
        
        public override void Down()
        {
            RenameColumn(table: "dbo.ContractInfo", name: "SignerID", newName: "Signer_MemberID");
        }
    }
}
