namespace PadCRM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class feedback : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.JobTitleCate",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CateName = c.String(maxLength: 50),
                        PID = c.Int(),
                        Code = c.Int(nullable: false),
                        Level = c.Int(nullable: false),
                        OrderIndex = c.Int(nullable: false),
                        PCate_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.JobTitleCate", t => t.PCate_ID)
                .Index(t => t.PCate_ID);
            
            AddColumn("dbo.CustomerCompany", "Finance", c => c.String(maxLength: 50));
            AddColumn("dbo.CustomerCompany", "FinancePhone", c => c.String(maxLength: 50));
            AddColumn("dbo.CustomerCompany", "ProxyName", c => c.String(maxLength: 50));
            AddColumn("dbo.CustomerCompany", "ProxyAddress", c => c.String(maxLength: 50));
            AddColumn("dbo.CustomerCompany", "ProxyPhone", c => c.String(maxLength: 50));
            AddColumn("dbo.Member", "JobTitleID", c => c.Int(nullable: false));
            AddColumn("dbo.Member", "JobTitleCate_ID", c => c.Int());
            AddColumn("dbo.Customer", "Mobile1", c => c.String(maxLength: 50));
            AlterColumn("dbo.CustomerCompany", "Name", c => c.String(maxLength: 50));
            AlterColumn("dbo.CustomerCompany", "BrandName", c => c.String(maxLength: 50));
            AlterColumn("dbo.CustomerCompany", "CityValue", c => c.String(maxLength: 50));
            AlterColumn("dbo.CustomerCompany", "IndustryValue", c => c.String(maxLength: 50));
            AlterColumn("dbo.CustomerCompany", "Fax", c => c.String(maxLength: 50));
            AlterColumn("dbo.CustomerCompany", "Phone", c => c.String(maxLength: 50));
            AlterColumn("dbo.CustomerCompany", "Address", c => c.String(maxLength: 50));
            AlterColumn("dbo.Customer", "Name", c => c.String(maxLength: 50));
            AlterColumn("dbo.Customer", "Mobile", c => c.String(maxLength: 50));
            AlterColumn("dbo.Customer", "Phone", c => c.String(maxLength: 50));
            AlterColumn("dbo.Customer", "QQ", c => c.String(maxLength: 50));
            AlterColumn("dbo.Customer", "Jobs", c => c.String(maxLength: 50));
            AlterColumn("dbo.Customer", "Email", c => c.String(maxLength: 50));
            AlterColumn("dbo.Customer", "Favorite", c => c.String(maxLength: 50));
            AlterColumn("dbo.Customer", "Address", c => c.String(maxLength: 50));
            AlterColumn("dbo.Customer", "ReMark", c => c.String(maxLength: 50));
            AddForeignKey("dbo.Member", "JobTitleCate_ID", "dbo.JobTitleCate", "ID");
            CreateIndex("dbo.Member", "JobTitleCate_ID");
        }
        
        public override void Down()
        {
            DropIndex("dbo.JobTitleCate", new[] { "PCate_ID" });
            DropIndex("dbo.Member", new[] { "JobTitleCate_ID" });
            DropForeignKey("dbo.JobTitleCate", "PCate_ID", "dbo.JobTitleCate");
            DropForeignKey("dbo.Member", "JobTitleCate_ID", "dbo.JobTitleCate");
            AlterColumn("dbo.Customer", "ReMark", c => c.String());
            AlterColumn("dbo.Customer", "Address", c => c.String());
            AlterColumn("dbo.Customer", "Favorite", c => c.String());
            AlterColumn("dbo.Customer", "Email", c => c.String());
            AlterColumn("dbo.Customer", "Jobs", c => c.String());
            AlterColumn("dbo.Customer", "QQ", c => c.String());
            AlterColumn("dbo.Customer", "Phone", c => c.String());
            AlterColumn("dbo.Customer", "Mobile", c => c.String());
            AlterColumn("dbo.Customer", "Name", c => c.String());
            AlterColumn("dbo.CustomerCompany", "Address", c => c.String());
            AlterColumn("dbo.CustomerCompany", "Phone", c => c.String());
            AlterColumn("dbo.CustomerCompany", "Fax", c => c.String());
            AlterColumn("dbo.CustomerCompany", "IndustryValue", c => c.String());
            AlterColumn("dbo.CustomerCompany", "CityValue", c => c.String());
            AlterColumn("dbo.CustomerCompany", "BrandName", c => c.String());
            AlterColumn("dbo.CustomerCompany", "Name", c => c.String());
            DropColumn("dbo.Customer", "Mobile1");
            DropColumn("dbo.Member", "JobTitleCate_ID");
            DropColumn("dbo.Member", "JobTitleID");
            DropColumn("dbo.CustomerCompany", "ProxyPhone");
            DropColumn("dbo.CustomerCompany", "ProxyAddress");
            DropColumn("dbo.CustomerCompany", "ProxyName");
            DropColumn("dbo.CustomerCompany", "FinancePhone");
            DropColumn("dbo.CustomerCompany", "Finance");
            DropTable("dbo.JobTitleCate");
        }
    }
}
