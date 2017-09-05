namespace Paramedic.Gestion.Web
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LicenseModuleStatusType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ClientesLicenciasProductosModulos", "LicenseModuleStatusType", c => c.Int(nullable: false));
            DropColumn("dbo.ClientesLicenciasProductosModulos", "LicenseModuleType");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ClientesLicenciasProductosModulos", "LicenseModuleType", c => c.Int(nullable: false));
            DropColumn("dbo.ClientesLicenciasProductosModulos", "LicenseModuleStatusType");
        }
    }
}
