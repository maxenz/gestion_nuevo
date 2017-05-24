namespace Paramedic.Gestion.Web
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TodoNuevoAddons : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ClientesLicenciasProductosModulosHistoriales",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ClientesLicenciasProductosModuloId = c.Int(nullable: false),
                        FechaVencimiento = c.DateTime(nullable: false),
                        ProductosModulosIntentoId = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(nullable: false),
                        UpdatedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ClientesLicenciasProductosModulos", t => t.ClientesLicenciasProductosModuloId)
                .ForeignKey("dbo.ProductosModulosIntentos", t => t.ProductosModulosIntentoId)
                .Index(t => t.ClientesLicenciasProductosModuloId)
                .Index(t => t.ProductosModulosIntentoId);
            
            CreateTable(
                "dbo.ProductosModulosIntentos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProductosModuloId = c.Int(nullable: false),
                        Dias = c.Int(nullable: false),
                        Orden = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(nullable: false),
                        UpdatedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ProductosModulos", t => t.ProductosModuloId)
                .Index(t => t.ProductosModuloId);
            
            DropColumn("dbo.ClientesLicenciasProductosModulos", "FechaDeVencimiento");
            DropColumn("dbo.ClientesLicenciasProductosModulos", "IntentosRealizados");
            DropColumn("dbo.ProductosModulos", "Intentos");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ProductosModulos", "Intentos", c => c.Int(nullable: false));
            AddColumn("dbo.ClientesLicenciasProductosModulos", "IntentosRealizados", c => c.Int(nullable: false));
            AddColumn("dbo.ClientesLicenciasProductosModulos", "FechaDeVencimiento", c => c.DateTime());
            DropForeignKey("dbo.ClientesLicenciasProductosModulosHistoriales", "ProductosModulosIntentoId", "dbo.ProductosModulosIntentos");
            DropForeignKey("dbo.ProductosModulosIntentos", "ProductosModuloId", "dbo.ProductosModulos");
            DropForeignKey("dbo.ClientesLicenciasProductosModulosHistoriales", "ClientesLicenciasProductosModuloId", "dbo.ClientesLicenciasProductosModulos");
            DropIndex("dbo.ProductosModulosIntentos", new[] { "ProductosModuloId" });
            DropIndex("dbo.ClientesLicenciasProductosModulosHistoriales", new[] { "ProductosModulosIntentoId" });
            DropIndex("dbo.ClientesLicenciasProductosModulosHistoriales", new[] { "ClientesLicenciasProductosModuloId" });
            DropTable("dbo.ProductosModulosIntentos");
            DropTable("dbo.ClientesLicenciasProductosModulosHistoriales");
        }
    }
}
