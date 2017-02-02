
using Paramedic.Gestion.Model;
using Paramedic.Gestion.Repository;
using Paramedic.Gestion.Service;
using System.Data.Entity.Migrations;
using System.Web.Security;
using WebMatrix.WebData;

namespace Paramedic.Gestion.Web
{
    internal sealed class Configuration : DbMigrationsConfiguration<GestionContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(GestionContext context)
        {
            WebSecurity.InitializeDatabaseConnection("GestionContext",
               "UserProfile", "Id", "UserName", autoCreateTables: true);
        }
    }
}
