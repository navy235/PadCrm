namespace PadCRM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class First : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customer", "Name", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Customer", "Name");
        }
    }
}
