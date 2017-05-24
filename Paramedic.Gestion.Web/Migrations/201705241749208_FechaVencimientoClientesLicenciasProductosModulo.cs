namespace Paramedic.Gestion.Web
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FechaVencimientoClientesLicenciasProductosModulo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ClientesLicenciasProductosModulos", "FechaDeVencimiento", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ClientesLicenciasProductosModulos", "FechaDeVencimiento");
        }
    }
}
