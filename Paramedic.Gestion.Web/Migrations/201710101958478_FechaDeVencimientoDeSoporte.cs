namespace Paramedic.Gestion.Web
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FechaDeVencimientoDeSoporte : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ClientesLicencias", "FechaVencimientoSoporte", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            CreateIndex("dbo.ClientesLicenciasProductosModulos", "ProductosModuloId");
            AddForeignKey("dbo.ClientesLicenciasProductosModulos", "ProductosModuloId", "dbo.ProductosModulos", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ClientesLicenciasProductosModulos", "ProductosModuloId", "dbo.ProductosModulos");
            DropIndex("dbo.ClientesLicenciasProductosModulos", new[] { "ProductosModuloId" });
            DropColumn("dbo.ClientesLicencias", "FechaVencimientoSoporte");
        }
    }
}
