namespace Paramedic.Gestion.Web
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WebServiceCache : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Sitios", "IX_UrlSite");
            AddColumn("dbo.ClientesLicencias", "WebServiceCacheId", c => c.Int());
            CreateIndex("dbo.ClientesLicencias", "WebServiceCacheId");
            AddForeignKey("dbo.ClientesLicencias", "WebServiceCacheId", "dbo.Sitios", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ClientesLicencias", "WebServiceCacheId", "dbo.Sitios");
            DropIndex("dbo.ClientesLicencias", new[] { "WebServiceCacheId" });
            DropColumn("dbo.ClientesLicencias", "WebServiceCacheId");
            CreateIndex("dbo.Sitios", "Url", unique: true, name: "IX_UrlSite");
        }
    }
}
