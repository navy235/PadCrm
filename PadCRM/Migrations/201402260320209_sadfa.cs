namespace PadCRM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sadfa : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TcNotice",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 50),
                        Content = c.String(),
                        AttachmentPath = c.String(maxLength: 500),
                        AddUser = c.Int(nullable: false),
                        AddTime = c.DateTime(nullable: false),
                        LastUser = c.Int(nullable: false),
                        LastTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.TcNotice");
        }
    }
}
