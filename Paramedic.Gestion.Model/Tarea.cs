using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Script.Serialization;

namespace Paramedic.Gestion.Model
{
	[Table("Tareas")]
	public class Tarea : AuditableEntity<int>
	{
		#region Properties

		[Required]
		public string Descripcion { get; set; }

		[Required]
		[Display(Name = "Proyecto")]
		public int ProyectoId { get; set; }

		[ForeignKey("ProyectoId")]
		public virtual Proyecto Proyecto { get; set; }

		public virtual ICollection<TareasGestion> TareasGestiones { get; set; }

		#endregion

		#region Constructors

		public Tarea()
		{
			this.TareasGestiones = new List<TareasGestion>();
		}

		#endregion
	}
}
