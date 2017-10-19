using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Paramedic.Gestion.Model
{
	[Table("TareasGestiones")]
	public class TareasGestion : AuditableEntity<int>
	{
		#region Properties

		[Required]
		[Display(Name = "Tarea")]
		public int TareaId { get; set; }

		[ForeignKey("TareaId")]
		public virtual Tarea Tarea { get; set; }

		[Required]
		public DateTime Fecha { get; set; }

		[Required]
		public double Horas { get; set; }

		[Display(Name = "Cliente")]
		public int ClienteId { get; set; }

		[ForeignKey("ClienteId")]
		public virtual Cliente Cliente { get; set; }

		[Required]
		public string Observaciones { get; set; }

		#endregion
	}
}
