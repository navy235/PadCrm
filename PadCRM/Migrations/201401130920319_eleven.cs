namespace PadCRM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class eleven : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LunarCalenderContrastTable",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Calender = c.String(),
                        Lunar = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.LunarCalenderContrastTable");
        }
    }
}
