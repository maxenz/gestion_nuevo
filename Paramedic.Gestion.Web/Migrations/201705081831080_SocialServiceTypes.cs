namespace Paramedic.Gestion.Web
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SocialServiceTypes : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SocialServices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Enabled = c.Boolean(nullable: false),
                        Description = c.String(nullable: false),
                        Configuration = c.String(nullable: false),
                        SocialServiceTypeId = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(nullable: false),
                        UpdatedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SocialServiceTypes", t => t.SocialServiceTypeId)
                .Index(t => t.SocialServiceTypeId);
            
            CreateTable(
                "dbo.SocialServiceTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Enabled = c.Boolean(nullable: false),
                        Description = c.String(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(nullable: false),
                        UpdatedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SocialServices", "SocialServiceTypeId", "dbo.SocialServiceTypes");
            DropIndex("dbo.SocialServices", new[] { "SocialServiceTypeId" });
            DropTable("dbo.SocialServiceTypes");
            DropTable("dbo.SocialServices");
        }
    }
}
