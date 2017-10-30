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
		[Range(1,int.MaxValue, ErrorMessage = "You must select a task")]
		[Display(Name = "Tarea")]
		public int TareaId { get; set; }

		[ForeignKey("TareaId")]
		public virtual Tarea Tarea { get; set; }

		[Required]
		public DateTime Fecha { get; set; }

		[Required]
		public double Horas { get; set; }

		[Display(Name = "Cliente")]
		public int? ClienteId { get; set; }

		[ForeignKey("ClienteId")]
		public virtual Cliente Cliente { get; set; }

		[Display(Name ="Usuario")]
		[Required]
		public int UsuarioId { get; set; }

		[ForeignKey("UsuarioId")]
		public virtual UserProfile Usuario { get; set; }

		public string Observaciones { get; set; }

		#endregion
	}
}
