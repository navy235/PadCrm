namespace PadCRM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ten : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Member", "BirthDay", c => c.DateTime(nullable: false));
            AddColumn("dbo.Customer", "BirthDay", c => c.DateTime(nullable: false));
            DropColumn("dbo.Member", "BirthDay");
            DropColumn("dbo.Customer", "BirthDay");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Customer", "BirthDay", c => c.DateTime(nullable: false));
            AddColumn("dbo.Member", "BirthDay", c => c.DateTime(nullable: false));
            DropColumn("dbo.Customer", "BirthDay");
            DropColumn("dbo.Member", "BirthDay");
        }
    }
}
