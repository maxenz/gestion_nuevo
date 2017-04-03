using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Threading;

namespace Paramedic.Gestion.Model
{

    public class GestionContext : DbContext
    {

        #region DbSets

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<ClientesContacto> ClientesContactos { get; set; }
        public DbSet<ClientesGestion> ClientesGestiones { get; set; }
        public DbSet<ClientesLicencia> ClientesLicencias { get; set; }
        public DbSet<ClientesLicenciasProducto> ClientesLicenciasProductos { get; set; }
        public DbSet<ClientesLicenciasProductosModulo> ClientesLicenciasProductosModulos { get; set; }
        public DbSet<ClientesTerminal> ClientesTerminales { get; set; }
        public DbSet<ClientesUsuario> ClientesUsuarios { get; set; }
        public DbSet<Estado> Estados { get; set; }
        public DbSet<Licencia> Licencias { get; set; }
        public DbSet<LicenciasLog> LicenciasLogs { get; set; }
        public DbSet<Localidad> Localidades { get; set; }
        public DbSet<LogRegistroSistema> LogRegistroSistema { get; set; }
        public DbSet<MedioDifusion> MediosDifusion { get; set; }
        public DbSet<Pais> Paises { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<ProductosModulo> ProductosModulos { get; set; }
        public DbSet<Provincia> Provincias { get; set; }
        public DbSet<Revendedor> Revendedores { get; set; }
        public DbSet<Sitio> Sitios { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<TicketEvento> TicketEventos { get; set; }
        public DbSet<TipoTerminal> TipoTerminales { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Video> Videos { get; set; }
        public DbSet<VideosCliente> VideosClientes { get; set; }
        public DbSet<TicketsClasificacion> TicketClasificaciones { get; set; }
        public DbSet<TicketsClasificacionUsuario> TicketClasificacionUsuarios { get; set; }

        #endregion

        #region Constructors

        public GestionContext() : base("Name=GestionContext")
        {
            //Database.SetInitializer(new CreateDatabaseIfNotExists<GestionContext>());
            Database.SetInitializer<GestionContext>(null);
        }

        #endregion

        #region Public Methods

        public override int SaveChanges()
        {
            var modifiedEntries = ChangeTracker.Entries()
                .Where(x => x.Entity is IAuditableEntity
                    && (x.State == EntityState.Added || x.State == EntityState.Modified));

            foreach (var entry in modifiedEntries)
            {
                IAuditableEntity entity = entry.Entity as IAuditableEntity;
                if (entity != null)
                {
                    string identityName = Thread.CurrentPrincipal.Identity.Name;
                    DateTime now = DateTime.Now;

                    if (entry.State == EntityState.Added)
                    {
                        entity.CreatedBy = identityName;
                        entity.CreatedDate = now;
                    }
                    else
                    {
                        base.Entry(entity).Property(x => x.CreatedBy).IsModified = false;
                        base.Entry(entity).Property(x => x.CreatedDate).IsModified = false;
                    }

                    entity.UpdatedBy = identityName;
                    entity.UpdatedDate = now;
                }
            }

            return base.SaveChanges();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            modelBuilder.Entity<Licencia>()
                .HasMany(pr => pr.Productos)
                .WithMany(lic => lic.Licencias)
                .Map(mc =>
                {
                    mc.ToTable("Licencias_Productos");
                    mc.MapLeftKey("LicenciaId");
                    mc.MapRightKey("ProductoId");
                }
            );

            modelBuilder.Entity<VideosCliente>()
            .HasKey(c => new { c.Id, c.VideoId });

            modelBuilder.Entity<VideosCliente>()
                        .Property(c => c.Id)
                        .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

        }



        #endregion
    }
}
