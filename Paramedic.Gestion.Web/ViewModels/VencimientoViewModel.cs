using Paramedic.Gestion.Model.Enums;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Paramedic.Gestion.Web.ViewModels
{
	public class VencimientoViewModel
	{
		#region Properties

		[DisplayName("Fecha de vencimiento")]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
		public DateTime FechaVencimiento { get; set; }

		public VencimientoType TipoDeVencimiento { get; set; }

		public string TipoVencimientoClassDescription
		{
			get
			{
				return this.TipoDeVencimiento == VencimientoType.Licencia ? "color-vencimiento-licencia" : "color-vencimiento-soporte";
			}
		}

		[DisplayName("Tipo de vencimiento")]
		public string Descripcion
		{
			get
			{
				return VencimientoTypes.GetVencimientoTypeName(this.TipoDeVencimiento);
			}
		}

		public string NroLicencia { get; set; }

		[DisplayName("Cliente")]
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

		public VencimientoViewModel() { }

		public VencimientoViewModel(
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