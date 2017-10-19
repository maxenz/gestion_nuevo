namespace Paramedic.Gestion.Web
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProyectosTareas : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Proyectos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Descripcion = c.String(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(nullable: false),
                        UpdatedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Tareas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Descripcion = c.String(nullable: false),
                        ProyectoId = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(nullable: false),
                        UpdatedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Proyectos", t => t.ProyectoId)
                .Index(t => t.ProyectoId);
            
            CreateTable(
                "dbo.TareasGestiones",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TareaId = c.Int(nullable: false),
                        Fecha = c.DateTime(nullable: false),
                        Horas = c.Double(nullable: false),
                        ClienteId = c.Int(nullable: false),
                        Observaciones = c.String(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(nullable: false),
                        UpdatedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Clientes", t => t.ClienteId)
                .ForeignKey("dbo.Tareas", t => t.TareaId)
                .Index(t => t.TareaId)
                .Index(t => t.ClienteId);
            
            AddColumn("dbo.ClientesLicencias", "FechaVencimientoSoporte", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            CreateIndex("dbo.ClientesLicenciasProductosModulos", "ProductosModuloId");
            AddForeignKey("dbo.ClientesLicenciasProductosModulos", "ProductosModuloId", "dbo.ProductosModulos", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TareasGestiones", "TareaId", "dbo.Tareas");
            DropForeignKey("dbo.TareasGestiones", "ClienteId", "dbo.Clientes");
            DropForeignKey("dbo.Tareas", "ProyectoId", "dbo.Proyectos");
            DropForeignKey("dbo.ClientesLicenciasProductosModulos", "ProductosModuloId", "dbo.ProductosModulos");
            DropIndex("dbo.TareasGestiones", new[] { "ClienteId" });
            DropIndex("dbo.TareasGestiones", new[] { "TareaId" });
            DropIndex("dbo.Tareas", new[] { "ProyectoId" });
            DropIndex("dbo.ClientesLicenciasProductosModulos", new[] { "ProductosModuloId" });
            DropColumn("dbo.ClientesLicencias", "FechaVencimientoSoporte");
            DropTable("dbo.TareasGestiones");
            DropTable("dbo.Tareas");
            DropTable("dbo.Proyectos");
        }
    }
}
