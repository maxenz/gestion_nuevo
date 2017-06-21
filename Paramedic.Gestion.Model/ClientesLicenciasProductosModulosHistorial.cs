using Paramedic.Gestion.Model.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Paramedic.Gestion.Model
{
	[Table("ClientesLicenciasProductosModulosHistoriales")]
	public class ClientesLicenciasProductosModulosHistorial : AuditableEntity<int>
	{
		#region Properties

		public int ClientesLicenciasProductosModuloId { get; set; }

		[ForeignKey("ClientesLicenciasProductosModuloId")]
		public virtual ClientesLicenciasProductosModulo ClientesLicenciasProductosModulo { get; set; }

		[Display(Name = "Fecha de vencimiento")]
		[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
		public DateTime FechaVencimiento { get; set; }

		public int ProductosModulosIntentoId { get; set; }

		[ForeignKey("ProductosModulosIntentoId")]
		public virtual ProductosModulosIntento ProductosModulosIntento { get; set; }

		[Display(Name = "Estado")]
		public virtual TrialStateType Estado
		{
			get
			{
				return FechaVencimiento < DateTime.Now ? TrialStateType.Expired : TrialStateType.Active;
			}
		}

		[Display(Name = "Días restantes")]
		public virtual double DiasRestantes
		{
			get
			{
				if (Estado == TrialStateType.Expired) return 0;

				return (FechaVencimiento.Date - DateTime.Now.Date).TotalDays;
			}
		}

		#endregion

		#region Constructors

		public ClientesLicenciasProductosModulosHistorial() { } //EF

		public ClientesLicenciasProductosModulosHistorial
			(
				ClientesLicenciasProductosModulo cliLicProdMod,
				ProductosModulosIntento intento
			)
		{
			this.ProductosModulosIntento = intento;
			this.ProductosModulosIntentoId = intento.Id;
			this.ClientesLicenciasProductosModulo = cliLicProdMod;
			this.ClientesLicenciasProductosModuloId = cliLicProdMod.Id;
			this.FechaVencimiento = DateTime.Now.AddDays(intento.Dias);
		}

		#endregion
	}
}
