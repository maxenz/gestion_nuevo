using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paramedic.Gestion.TrialsExpirationManager
{
	public class HistorialDTO
	{
		public int ClientesLicenciasProductosModuloId { get; set; }
		public DateTime FechaVencimiento { get; set; }
		public int ProductosModulosIntentoId { get; set; }
		public DateTime CreatedDate { get; set; }
		public string CreatedBy { get; set; }
		public DateTime UpdatedDate { get; set; }
		public string UpdatedBy { get; set; }
		public int Id { get; set; }
	}
}
