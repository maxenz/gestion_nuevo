using Paramedic.Gestion.Model;
using System.Linq;

namespace Paramedic.Gestion.Web.ViewModels
{
    public class MapaViewModel
    {
        #region Properties

        public int Id { get; set; }

        public string Cliente { get; set; }

        public string Latitud { get; set; }

        public string Longitud { get; set; }

        public string Localidad { get; set; }

        public string EmailPrincipal { get; set; }

        public string Telefono { get; set; }

        public string SitioWeb { get; set; }

        public int? MedioDifusionId { get; set; }

        public string VersionShaman { get; set; }

        #endregion

        #region Constructors

        public MapaViewModel(Cliente cliente)
        {
            this.Id = cliente.Id;
            this.Latitud = cliente.Latitud;
            this.Longitud = cliente.Longitud;
            this.Cliente = cliente.RazonSocial;
            this.Localidad = cliente.Localidad.Descripcion;
            this.EmailPrincipal = cliente.ClientesContactos
                .Where(x => x.esInstitucional)
                .Select(x => x.Email)
                .FirstOrDefault();
            this.Telefono = cliente.ClientesContactos
                .Where(x => x.esInstitucional)
                .Select(x => x.Telefono)
                .FirstOrDefault();
            this.SitioWeb = cliente.SitioWeb;
            this.MedioDifusionId = cliente.MedioDifusionId;
            setVersionShaman(cliente);
        }

        #endregion

        #region Private Methods

        private void setVersionShaman(Cliente cliente)
        {

            if (cliente.ClientesLicencias.Count > 0)
            {
                var prods = cliente.ClientesLicencias.FirstOrDefault().Licencia.Productos;
                try
                {
                    this.VersionShaman = prods.Where(x => (x.Numero == 1 || x.Numero == 500)).FirstOrDefault().Descripcion;
                }
                catch
                {
                    this.VersionShaman = "";
                }
            }
            else
            {
                this.VersionShaman = "";
            }
        }

        #endregion
    }
}