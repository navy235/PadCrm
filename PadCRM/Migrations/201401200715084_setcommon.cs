namespace PadCRM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class setcommon : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CustomerCompany", "IsCommon", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CustomerCompany", "IsCommon");
        }
    }
}
