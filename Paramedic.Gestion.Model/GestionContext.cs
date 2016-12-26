using System;
using System.Data.Entity;
using System.Linq;
using System.Threading;

namespace Paramedic.Gestion.Model
{

    public class GestionContext : DbContext
    {

        #region DbSets

        public DbSet<Pais> Paises { get; set; }
        public DbSet<Provincia> Provincias { get; set; }
        public DbSet<Localidad> Localidades { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<TicketEstado> TicketEstado { get; set; }
        public DbSet<TicketEvento> TicketEventos { get; set; }
        public DbSet<TicketTipoEvento> TicketTipoEventos { get; set; }

        #endregion

        #region Constructors

        public GestionContext() : base("Name=GestionContext")
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<GestionContext>());
        }

        #endregion

        #region Public Methods

        public override int SaveChanges()
        {
            var modifiedEntries = ChangeTracker.Entries()
                .Where(x => x.Entity is IAuditableEntity
                    && (x.State == System.Data.Entity.EntityState.Added || x.State == System.Data.Entity.EntityState.Modified));

            foreach (var entry in modifiedEntries)
            {
                IAuditableEntity entity = entry.Entity as IAuditableEntity;
                if (entity != null)
                {
                    string identityName = Thread.CurrentPrincipal.Identity.Name;
                    DateTime now = DateTime.UtcNow;

                    if (entry.State == System.Data.Entity.EntityState.Added)
                    {
                        entity.CreatedBy = identityName;
                        entity.CreatedDate = now;
                    }
                    else {
                        base.Entry(entity).Property(x => x.CreatedBy).IsModified = false;
                        base.Entry(entity).Property(x => x.CreatedDate).IsModified = false;
                    }

                    entity.UpdatedBy = identityName;
                    entity.UpdatedDate = now;
                }
            }

            return base.SaveChanges();
        }

        #endregion

    }
}
