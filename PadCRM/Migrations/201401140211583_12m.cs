namespace PadCRM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _12m : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customer", "BirthDay1", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Customer", "BirthDay1");
        }
    }
}
