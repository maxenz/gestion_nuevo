using Paramedic.Gestion.Model.Enums;
using System;

namespace Paramedic.Gestion.Web.ViewModels
{
	public class Vencimiento
	{
		#region Properties

		public DateTime FechaVencimiento { get; set; }

		public VencimientoType TipoDeVencimiento { get; set; }

		public string Descripcion
		{
			get
			{
				return VencimientoTypes.GetVencimientoTypeName(this.TipoDeVencimiento);
			}
		}

		public string NroLicencia { get; set; }

		public string ClienteDescripcion { get; set; }

		public bool ProximoAVencer
		{
			get
			{
				return this.FechaVencimiento > DateTime.Now.AddDays(-20).Date;
			}
		}

		#endregion

		#region Constructors

		public Vencimiento() { }

		public Vencimiento(
			DateTime fechaVencimiento,
			string nroLicencia,
			string clienteDescripcion,
			VencimientoType tipoDeVencimiento
			)
		{
			this.FechaVencimiento = fechaVencimiento;
			this.NroLicencia = nroLicencia;
			this.ClienteDescripcion = clienteDescripcion;
			this.TipoDeVencimiento = tipoDeVencimiento;
		}

		#endregion

	}
}