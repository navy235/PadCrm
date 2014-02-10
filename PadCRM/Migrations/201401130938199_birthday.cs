namespace PadCRM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class birthday : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Member", "BirthDay1", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Member", "BirthDay1");
        }
    }
}
