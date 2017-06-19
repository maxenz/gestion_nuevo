using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Paramedic.Gestion.Model
{
	[Table("Noticias")]
	public class Noticia : AuditableEntity<int>
	{
		#region Properties

		[Required]
		[Display(Name ="Descripción")]
		public string Descripcion { get; set; }

		[Required]
		[Display(Name ="Título")]
		public string Titulo { get; set; }

		[Required]
		[Display(Name ="Fecha de vencimiento")]
		public DateTime FechaVencimiento { get; set; }

		#endregion
	}
}
