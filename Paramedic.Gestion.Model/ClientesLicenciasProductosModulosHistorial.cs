using Paramedic.Gestion.Model.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Script.Serialization;

namespace Paramedic.Gestion.Model
{
	[Table("ClientesLicenciasProductosModulosHistoriales")]
	public class ClientesLicenciasProductosModulosHistorial : AuditableEntity<int>
	{
		#region Properties

		public int ClientesLicenciasProductosModuloId { get; set; }

		[ScriptIgnore(ApplyToOverrides = true)]
		[ForeignKey("ClientesLicenciasProductosModuloId")]
		public virtual ClientesLicenciasProductosModulo ClientesLicenciasProductosModulo { get; set; }

		[Display(Name = "Fecha de vencimiento")]
		[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
		public DateTime FechaVencimiento { get; set; }

		public int ProductosModulosIntentoId { get; set; }

		[ScriptIgnore(ApplyToOverrides = true)]
		[ForeignKey("ProductosModulosIntentoId")]
		public virtual ProductosModulosIntento ProductosModulosIntento { get; set; }

		[ScriptIgnore(ApplyToOverrides = true)]
		[Display(Name = "Estado")]
		public virtual TrialStateType Estado
		{
			get
			{
				return FechaVencimiento < DateTime.Now ? TrialStateType.Expired : TrialStateType.Active;
			}
		}

		[ScriptIgnore(ApplyToOverrides = true)]
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
