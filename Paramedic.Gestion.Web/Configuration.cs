
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
                roles.CreateRole("Cliente");
                roles.CreateRole("Cliente ticket");
                roles.AddUsersToRoles(new[] { "mpoggio" }, new[] { "Administrador" });
            }

            // --> Creacion de paises

            IPaisRepository paisRepo = new PaisRepository(context);
            Pais pais = new Pais() { Codigo = "ARG", Descripcion = "Argentina" };
            Provincia provincia = new Provincia() { Codigo = "BUE", Descripcion = "Buenos Aires" };
            Localidad localidad = new Localidad() { Codigo = "VER", Descripcion = "Versalles" };
            provincia.Localidades.Add(localidad);
            pais.Provincias.Add(provincia);
            paisRepo.Add(pais);
            paisRepo.Save();

            ILicenciaRepository licenciaRepo = new LicenciaRepository(context);
            Licencia licencia = new Licencia() { Serial = "5677899234" };
            licenciaRepo.Add(licencia);
            licenciaRepo.Save();

            IEstadoRepository estadoRepo = new EstadoRepository(context);
            Estado estado = new Estado() { Descripcion = "Desarrollo" };
            estadoRepo.Add(estado);
            estadoRepo.Save();

            ITipoTerminalRepository terminalRepo = new TipoTerminalRepository(context);
            TipoTerminal tipoTerminal = new TipoTerminal() { Descripcion = "VNC" };
            terminalRepo.Add(tipoTerminal);
            terminalRepo.Save();

            IMedioDifusionRepository medioDifusionRepo = new MedioDifusionRepository(context);
            MedioDifusion medioDifusion = new MedioDifusion() { Descripcion = "Redes Sociales" };
            medioDifusionRepo.Add(medioDifusion);
            medioDifusionRepo.Save();

            IClienteRepository clienteRepository = new ClienteRepository(context);
            Cliente cliente = new Cliente()
            {
                Altura = "6141",
                Calle = "Santo Tome",
                FechaIngreso = new System.DateTime(),
                Localidad = localidad,
                MedioDifusion = medioDifusion,
                RazonSocial = "MAXO SA",
                Referencia = "REF"
            };

            ClientesContacto contacto = new ClientesContacto("Roberto", "rober@gmail.com", "4564345464", 1);
            cliente.ClientesContactos.Add(contacto);
            clienteRepository.Add(cliente);
            clienteRepository.Save();
        }
    }
}
