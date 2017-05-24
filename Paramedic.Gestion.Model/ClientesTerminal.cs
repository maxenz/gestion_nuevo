using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Paramedic.Gestion.Model
{
	[Table("ClientesTerminales")]
	public class ClientesTerminal : AuditableEntity<int>
	{
		#region Properties

		[Required]
		[Display(Name = "Tipo de terminal")]
		public int TipoTerminalId { get; set; }

		[Required]
		[Display(Name = "Cliente")]
		public int ClienteId { get; set; }

		[Required]
		[Display(Name = "Valor 1")]
		public string Valor1 { get; set; }

		[Display(Name = "Valor 2")]
		public string Valor2 { get; set; }

		[Display(Name = "Valor 3")]
		public string Valor3 { get; set; }

		[Display(Name = "Valor 4")]
		public string Valor4 { get; set; }

		public string Referencia { get; set; }

		[ForeignKey("ClienteId")]
		public virtual Cliente Cliente { get; set; }
		[ForeignKey("TipoTerminalId")]
		public virtual TipoTerminal TipoTerminal { get; set; }

		#endregion
	}
}