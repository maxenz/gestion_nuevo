namespace Gestion.Migrations
{
    using System.Data.Entity.Migrations;
    using System.Web.Security;
    using WebMatrix.WebData;
    using Paramedic.Gestion.Model;
    using Paramedic.Gestion.Repository;
    using System.Collections.Generic;

    internal sealed class Configuration : DbMigrationsConfiguration<GestionContext>
    {
        public Configuration()
        {
            // --> OJO con estos parametros cuando pase esto a produccion.
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;

        }

        protected override void Seed(GestionContext context)
        {
            WebSecurity.InitializeDatabaseConnection("GestionContext",
               "UserProfile", "Id", "UserName", autoCreateTables: true);
            if (!WebSecurity.UserExists("mpoggio"))
            {
                var roles = (SimpleRoleProvider)Roles.Provider;
                var membership = (SimpleMembershipProvider)Membership.Provider;
                membership.CreateUserAndAccount("mpoggio", "elmaxo");
                roles.CreateRole("Administrador");
                roles.AddUsersToRoles(new[] { "mpoggio" }, new[] { "Administrador" });
            }          
        }
    }
}
