namespace Paramedic.Gestion.Web
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Clientes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RazonSocial = c.String(nullable: false),
                        Calle = c.String(),
                        Altura = c.String(),
                        Piso = c.String(),
                        Departamento = c.String(),
                        Domicilio = c.String(),
                        SitioWeb = c.String(),
                        Referencia = c.String(),
                        Latitud = c.String(),
                        Longitud = c.String(),
                        FechaIngreso = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        LocalidadId = c.Int(nullable: false),
                        RevendedorId = c.Int(),
                        CuentaCorrienteId = c.Int(),
                        MedioDifusionId = c.Int(),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(nullable: false),
                        UpdatedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Localidades", t => t.LocalidadId)
                .ForeignKey("dbo.MediosDifusion", t => t.MedioDifusionId)
                .Index(t => t.LocalidadId)
                .Index(t => t.MedioDifusionId);
            
            CreateTable(
                "dbo.ClientesContactos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Email = c.String(),
                        Otros = c.String(),
                        flgPrincipal = c.Int(nullable: false),
                        esInstitucional = c.Boolean(nullable: false),
                        Nombre = c.String(nullable: false),
                        Telefono = c.String(),
                        ClienteId = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(nullable: false),
                        UpdatedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Clientes", t => t.ClienteId)
                .Index(t => t.ClienteId);
            
            CreateTable(
                "dbo.ClientesGestiones",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EstadoId = c.Int(nullable: false),
                        ClienteId = c.Int(nullable: false),
                        Observaciones = c.String(nullable: false),
                        PdfGestion = c.Binary(),
                        Fecha = c.DateTime(nullable: false),
                        FechaRecontacto = c.DateTime(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(nullable: false),
                        UpdatedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Clientes", t => t.ClienteId)
                .ForeignKey("dbo.Estados", t => t.EstadoId)
                .Index(t => t.EstadoId)
                .Index(t => t.ClienteId);
            
            CreateTable(
                "dbo.Estados",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Numero = c.Int(nullable: false),
                        Descripcion = c.String(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(nullable: false),
                        UpdatedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ClientesLicencias",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LicenciaId = c.Int(nullable: false),
                        ClienteId = c.Int(nullable: false),
                        CnnDataSource = c.String(nullable: false),
                        CnnCatalog = c.String(nullable: false),
                        CnnUser = c.String(nullable: false),
                        CnnPassword = c.String(nullable: false),
                        FechaDeVencimiento = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ConexionServidor = c.String(),
                        SitioId = c.Int(),
                        SitioPuerto = c.Int(),
                        Alias = c.String(),
                        AndroidPassword = c.String(),
                        AndroidUrl = c.String(),
                        FtpAndroidDir = c.String(),
                        FtpAndroidUser = c.String(),
                        FtpAndroidPassword = c.String(),
                        SitioSubDominio = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(nullable: false),
                        UpdatedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Clientes", t => t.ClienteId)
                .ForeignKey("dbo.Licencias", t => t.LicenciaId)
                .ForeignKey("dbo.Sitios", t => t.SitioId)
                .Index(t => t.LicenciaId)
                .Index(t => t.ClienteId)
                .Index(t => t.SitioId);
            
            CreateTable(
                "dbo.ClientesLicenciasProductos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ClientesLicenciaId = c.Int(nullable: false),
                        ProductoId = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(nullable: false),
                        UpdatedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ClientesLicencias", t => t.ClientesLicenciaId)
                .ForeignKey("dbo.Productos", t => t.ProductoId)
                .Index(t => t.ClientesLicenciaId)
                .Index(t => t.ProductoId);
            
            CreateTable(
                "dbo.ClientesLicenciasProductosModulos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ClientesLicenciasProductoId = c.Int(nullable: false),
                        ProductosModuloId = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(nullable: false),
                        UpdatedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ClientesLicenciasProductos", t => t.ClientesLicenciasProductoId)
                .Index(t => t.ClientesLicenciasProductoId);
            
            CreateTable(
                "dbo.Productos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Numero = c.Int(nullable: false),
                        Descripcion = c.String(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(nullable: false),
                        UpdatedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Licencias",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Serial = c.String(nullable: false),
                        NumeroDeLlave = c.String(),
                        Estado = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(nullable: false),
                        UpdatedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ProductosModulos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProductoId = c.Int(nullable: false),
                        Codigo = c.String(),
                        Descripcion = c.String(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(nullable: false),
                        UpdatedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Productos", t => t.ProductoId)
                .Index(t => t.ProductoId);
            
            CreateTable(
                "dbo.Sitios",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Descripcion = c.String(nullable: false),
                        Url = c.String(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(nullable: false),
                        UpdatedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ClientesTerminales",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TipoTerminalId = c.Int(nullable: false),
                        ClienteId = c.Int(nullable: false),
                        Valor1 = c.String(nullable: false),
                        Valor2 = c.String(),
                        Valor3 = c.String(),
                        Valor4 = c.String(),
                        Referencia = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(nullable: false),
                        UpdatedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Clientes", t => t.ClienteId)
                .ForeignKey("dbo.TipoTerminales", t => t.TipoTerminalId)
                .Index(t => t.TipoTerminalId)
                .Index(t => t.ClienteId);
            
            CreateTable(
                "dbo.TipoTerminales",
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
                "dbo.ClientesUsuarios",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UsuarioId = c.Int(nullable: false),
                        ClienteId = c.Int(nullable: false),
                        ShamanFullId = c.Int(),
                        ShamanExpressId = c.Int(),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(nullable: false),
                        UpdatedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Clientes", t => t.ClienteId)
                .ForeignKey("dbo.UserProfile", t => t.UsuarioId)
                .Index(t => t.UsuarioId)
                .Index(t => t.ClienteId);
            
            CreateTable(
                "dbo.UserProfile",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                        Email = c.String(),
                        Nombre = c.String(),
                        Apellido = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.VideosClientes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        VideoId = c.Int(nullable: false),
                        ClienteId = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(nullable: false),
                        UpdatedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => new { t.Id, t.VideoId })
                .ForeignKey("dbo.Clientes", t => t.ClienteId)
                .ForeignKey("dbo.Videos", t => t.VideoId)
                .Index(t => t.VideoId)
                .Index(t => t.ClienteId);
            
            CreateTable(
                "dbo.Videos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Descripcion = c.String(nullable: false),
                        Alias = c.String(nullable: false),
                        EsPublico = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(nullable: false),
                        UpdatedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Localidades",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Codigo = c.String(nullable: false, maxLength: 3),
                        Descripcion = c.String(nullable: false),
                        ProvinciaId = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(nullable: false),
                        UpdatedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Provincias", t => t.ProvinciaId)
                .Index(t => t.ProvinciaId);
            
            CreateTable(
                "dbo.Provincias",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Codigo = c.String(nullable: false, maxLength: 3),
                        Descripcion = c.String(nullable: false),
                        PaisId = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(nullable: false),
                        UpdatedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Paises", t => t.PaisId)
                .Index(t => t.PaisId);
            
            CreateTable(
                "dbo.Paises",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Codigo = c.String(nullable: false, maxLength: 3),
                        Descripcion = c.String(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(nullable: false),
                        UpdatedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MediosDifusion",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MapaVisible = c.Boolean(nullable: false),
                        Descripcion = c.String(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(nullable: false),
                        UpdatedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.LicenciasLogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LicenciaId = c.Int(nullable: false),
                        IP = c.String(nullable: false),
                        Type = c.Int(nullable: false),
                        GenericDescription = c.String(),
                        Referencias = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(nullable: false),
                        UpdatedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Licencias", t => t.LicenciaId)
                .Index(t => t.LicenciaId);
            
            CreateTable(
                "dbo.LogsRegistrosSistema",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserProfileId = c.Int(nullable: false),
                        DescripcionAccion = c.String(nullable: false),
                        ObservacionesAccion = c.String(),
                        Fecha = c.DateTime(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(nullable: false),
                        UpdatedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserProfile", t => t.UserProfileId)
                .Index(t => t.UserProfileId);
            
            CreateTable(
                "dbo.Revendedores",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false),
                        Comision = c.Double(nullable: false),
                        BajoContrato = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(nullable: false),
                        UpdatedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TicketEventos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Descripcion = c.String(nullable: false),
                        TicketId = c.Int(nullable: false),
                        UserProfileId = c.Int(nullable: false),
                        ImageData = c.Binary(),
                        ImageMimeType = c.String(),
                        TicketTipoEventoType = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(nullable: false),
                        UpdatedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Tickets", t => t.TicketId)
                .ForeignKey("dbo.UserProfile", t => t.UserProfileId)
                .Index(t => t.TicketId)
                .Index(t => t.UserProfileId);
            
            CreateTable(
                "dbo.Tickets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Asunto = c.String(nullable: false),
                        UserProfileId = c.Int(nullable: false),
                        TicketEstadoType = c.Int(nullable: false),
                        FuturaMejora = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(nullable: false),
                        UpdatedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserProfile", t => t.UserProfileId)
                .Index(t => t.UserProfileId);
            
            CreateTable(
                "dbo.Licencias_Productos",
                c => new
                    {
                        LicenciaId = c.Int(nullable: false),
                        ProductoId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.LicenciaId, t.ProductoId })
                .ForeignKey("dbo.Licencias", t => t.LicenciaId, cascadeDelete: true)
                .ForeignKey("dbo.Productos", t => t.ProductoId, cascadeDelete: true)
                .Index(t => t.LicenciaId)
                .Index(t => t.ProductoId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TicketEventos", "UserProfileId", "dbo.UserProfile");
            DropForeignKey("dbo.Tickets", "UserProfileId", "dbo.UserProfile");
            DropForeignKey("dbo.TicketEventos", "TicketId", "dbo.Tickets");
            DropForeignKey("dbo.LogsRegistrosSistema", "UserProfileId", "dbo.UserProfile");
            DropForeignKey("dbo.LicenciasLogs", "LicenciaId", "dbo.Licencias");
            DropForeignKey("dbo.Clientes", "MedioDifusionId", "dbo.MediosDifusion");
            DropForeignKey("dbo.Clientes", "LocalidadId", "dbo.Localidades");
            DropForeignKey("dbo.Provincias", "PaisId", "dbo.Paises");
            DropForeignKey("dbo.Localidades", "ProvinciaId", "dbo.Provincias");
            DropForeignKey("dbo.VideosClientes", "VideoId", "dbo.Videos");
            DropForeignKey("dbo.VideosClientes", "ClienteId", "dbo.Clientes");
            DropForeignKey("dbo.ClientesUsuarios", "UsuarioId", "dbo.UserProfile");
            DropForeignKey("dbo.ClientesUsuarios", "ClienteId", "dbo.Clientes");
            DropForeignKey("dbo.ClientesTerminales", "TipoTerminalId", "dbo.TipoTerminales");
            DropForeignKey("dbo.ClientesTerminales", "ClienteId", "dbo.Clientes");
            DropForeignKey("dbo.ClientesLicencias", "SitioId", "dbo.Sitios");
            DropForeignKey("dbo.ClientesLicencias", "LicenciaId", "dbo.Licencias");
            DropForeignKey("dbo.ClientesLicenciasProductos", "ProductoId", "dbo.Productos");
            DropForeignKey("dbo.ProductosModulos", "ProductoId", "dbo.Productos");
            DropForeignKey("dbo.Licencias_Productos", "ProductoId", "dbo.Productos");
            DropForeignKey("dbo.Licencias_Productos", "LicenciaId", "dbo.Licencias");
            DropForeignKey("dbo.ClientesLicenciasProductosModulos", "ClientesLicenciasProductoId", "dbo.ClientesLicenciasProductos");
            DropForeignKey("dbo.ClientesLicenciasProductos", "ClientesLicenciaId", "dbo.ClientesLicencias");
            DropForeignKey("dbo.ClientesLicencias", "ClienteId", "dbo.Clientes");
            DropForeignKey("dbo.ClientesGestiones", "EstadoId", "dbo.Estados");
            DropForeignKey("dbo.ClientesGestiones", "ClienteId", "dbo.Clientes");
            DropForeignKey("dbo.ClientesContactos", "ClienteId", "dbo.Clientes");
            DropIndex("dbo.Licencias_Productos", new[] { "ProductoId" });
            DropIndex("dbo.Licencias_Productos", new[] { "LicenciaId" });
            DropIndex("dbo.Tickets", new[] { "UserProfileId" });
            DropIndex("dbo.TicketEventos", new[] { "UserProfileId" });
            DropIndex("dbo.TicketEventos", new[] { "TicketId" });
            DropIndex("dbo.LogsRegistrosSistema", new[] { "UserProfileId" });
            DropIndex("dbo.LicenciasLogs", new[] { "LicenciaId" });
            DropIndex("dbo.Provincias", new[] { "PaisId" });
            DropIndex("dbo.Localidades", new[] { "ProvinciaId" });
            DropIndex("dbo.VideosClientes", new[] { "ClienteId" });
            DropIndex("dbo.VideosClientes", new[] { "VideoId" });
            DropIndex("dbo.ClientesUsuarios", new[] { "ClienteId" });
            DropIndex("dbo.ClientesUsuarios", new[] { "UsuarioId" });
            DropIndex("dbo.ClientesTerminales", new[] { "ClienteId" });
            DropIndex("dbo.ClientesTerminales", new[] { "TipoTerminalId" });
            DropIndex("dbo.ProductosModulos", new[] { "ProductoId" });
            DropIndex("dbo.ClientesLicenciasProductosModulos", new[] { "ClientesLicenciasProductoId" });
            DropIndex("dbo.ClientesLicenciasProductos", new[] { "ProductoId" });
            DropIndex("dbo.ClientesLicenciasProductos", new[] { "ClientesLicenciaId" });
            DropIndex("dbo.ClientesLicencias", new[] { "SitioId" });
            DropIndex("dbo.ClientesLicencias", new[] { "ClienteId" });
            DropIndex("dbo.ClientesLicencias", new[] { "LicenciaId" });
            DropIndex("dbo.ClientesGestiones", new[] { "ClienteId" });
            DropIndex("dbo.ClientesGestiones", new[] { "EstadoId" });
            DropIndex("dbo.ClientesContactos", new[] { "ClienteId" });
            DropIndex("dbo.Clientes", new[] { "MedioDifusionId" });
            DropIndex("dbo.Clientes", new[] { "LocalidadId" });
            DropTable("dbo.Licencias_Productos");
            DropTable("dbo.Tickets");
            DropTable("dbo.TicketEventos");
            DropTable("dbo.Revendedores");
            DropTable("dbo.LogsRegistrosSistema");
            DropTable("dbo.LicenciasLogs");
            DropTable("dbo.MediosDifusion");
            DropTable("dbo.Paises");
            DropTable("dbo.Provincias");
            DropTable("dbo.Localidades");
            DropTable("dbo.Videos");
            DropTable("dbo.VideosClientes");
            DropTable("dbo.UserProfile");
            DropTable("dbo.ClientesUsuarios");
            DropTable("dbo.TipoTerminales");
            DropTable("dbo.ClientesTerminales");
            DropTable("dbo.Sitios");
            DropTable("dbo.ProductosModulos");
            DropTable("dbo.Licencias");
            DropTable("dbo.Productos");
            DropTable("dbo.ClientesLicenciasProductosModulos");
            DropTable("dbo.ClientesLicenciasProductos");
            DropTable("dbo.ClientesLicencias");
            DropTable("dbo.Estados");
            DropTable("dbo.ClientesGestiones");
            DropTable("dbo.ClientesContactos");
            DropTable("dbo.Clientes");
        }
    }
}
