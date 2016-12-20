using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace Gestion.Models
{
    public class GestionDb : DbContext
    {
        public GestionDb()
            : base("name = DefaultConnection")
        {
            Database.SetInitializer<GestionDb>(new CreateDatabaseIfNotExists<GestionDb>());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Licencia>()
                .HasMany(pr => pr.Productos)
                .WithMany(lic => lic.Licencias)
                .Map(mc =>
                {
                    mc.ToTable("Licencias_Productos");
                    mc.MapLeftKey("LicenciaID");
                    mc.MapRightKey("ProductoID");
                }
            );

            base.OnModelCreating(modelBuilder);

       
        }

        public DbSet<UserProfile> UserProfiles { get; set; }

        public DbSet<TipoTerminal> TipoTerminales { get; set; }

        public DbSet<Pais> Paises { get; set; }

        public DbSet<Provincia> Provincias { get; set; }

        public DbSet<Localidad> Localidades { get; set; }

        public DbSet<Estado> Estados { get; set; }

        public DbSet<Producto> Productos { get; set; }

        public DbSet<ProductosModulo> ProductosModulos { get; set; }

        public DbSet<Licencia> Licencias { get; set; }

        public DbSet<Cliente> Clientes { get; set; }

        public DbSet<Revendedor> Revendedores { get; set; }

        public DbSet<ClientesContacto> ClientesContactos { get; set; }

        public DbSet<ClientesGestion> ClientesGestiones { get; set; }

        public DbSet<ClientesTerminal> ClientesTerminales { get; set; }

        public DbSet<ClientesUsuario> ClientesUsuarios { get; set; }

        public DbSet<ClientesLicencia> ClientesLicencias { get; set; }

        public DbSet<ClientesLicenciasProducto> ClientesLicenciasProductos { get; set; }

        public DbSet<ClientesLicenciasProductosModulo> ClientesLicenciasProductosModulos { get; set; }

        public DbSet<Usuario> Usuarios { get; set; }

        public DbSet<LicenciasLog> LicenciasLogs { get; set; }

        public DbSet<Ticket> Tickets { get; set; }

        public DbSet<TicketEstado> TicketEstados { get; set; }

        public DbSet<TicketTipoEvento> TicketTipoEventos { get; set; }

        public DbSet<TicketEvento> TicketEventos { get; set; }

        public DbSet<Video> Videos { get; set; }

        public DbSet<VideosCliente> VideosClientes { get; set; }

        public DbSet<MedioDifusion> MediosDifusion { get; set; }

        public DbSet<LogRegistroSistema> LogsRegistroSistema { get; set; }

        public DbSet<Sitio> Sitios { get; set; }

    }
}