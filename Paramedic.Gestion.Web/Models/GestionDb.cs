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

            //modelBuilder.Entity<Licencia>()
            //    .HasMany(pr => pr.Productos)
            //    .WithMany(lic => lic.Licencias)
            //    .Map(mc =>
            //    {
            //        mc.ToTable("Licencias_Productos");
            //        mc.MapLeftKey("LicenciaID");
            //        mc.MapRightKey("ProductoID");
            //    }
            //);

            base.OnModelCreating(modelBuilder);

       
        }


    }
}