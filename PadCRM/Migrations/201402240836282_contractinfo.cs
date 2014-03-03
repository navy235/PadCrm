namespace PadCRM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class contractinfo : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ContractCate",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CateName = c.String(maxLength: 50),
                        PID = c.Int(),
                        Code = c.Int(nullable: false),
                        Level = c.Int(nullable: false),
                        OrderIndex = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.ContractCate", t => t.PID)
                .Index(t => t.PID);
            
            CreateTable(
                "dbo.ContractInfo",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Key = c.String(maxLength: 50),
                        ContractCateID = c.Int(nullable: false),
                        CompanyID = c.Int(nullable: false),
                        SenderID = c.Int(nullable: false),
                        AttachmentPath = c.String(maxLength: 500),
                        SigningTime = c.DateTime(nullable: false),
                        PlayTime = c.DateTime(nullable: false),
                        ExpiryTime = c.DateTime(nullable: false),
                        SubscribeTime = c.DateTime(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SignerID = c.Int(nullable: false),
                        FinancePhone = c.String(maxLength: 50),
                        FinanceFax = c.String(maxLength: 50),
                        Payment = c.String(maxLength: 150),
                        AddTime = c.DateTime(nullable: false),
                        AddUser = c.Int(nullable: false),
                        LastTime = c.DateTime(nullable: false),
                        LastUser = c.Int(nullable: false),
                        Signer_MemberID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Member", t => t.Signer_MemberID)
                .ForeignKey("dbo.ContractCate", t => t.ContractCateID)
                .Index(t => t.Signer_MemberID)
                .Index(t => t.ContractCateID);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.ContractInfo", new[] { "ContractCateID" });
            DropIndex("dbo.ContractInfo", new[] { "Signer_MemberID" });
            DropIndex("dbo.ContractCate", new[] { "PID" });
            DropForeignKey("dbo.ContractInfo", "ContractCateID", "dbo.ContractCate");
            DropForeignKey("dbo.ContractInfo", "Signer_MemberID", "dbo.Member");
            DropForeignKey("dbo.ContractCate", "PID", "dbo.ContractCate");
            DropTable("dbo.ContractInfo");
            DropTable("dbo.ContractCate");
        }
    }
}
