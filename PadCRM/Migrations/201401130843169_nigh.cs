namespace PadCRM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class nigh : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SolarData",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        yearid = c.Int(nullable: false),
                        data = c.String(),
                        dataint = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.SolarData");
        }
    }
}
