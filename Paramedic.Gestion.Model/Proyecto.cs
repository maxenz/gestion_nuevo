using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Paramedic.Gestion.Model
{
	[Table("Proyectos")]
	public class Proyecto : AuditableEntity<int>
	{
		#region Properties

		[Required]
		public string Descripcion { get; set; }

		[Required]
		[Display(Name = "Clasificacion")]
		public int ClasificacionId { get; set; }

		[ForeignKey("ClasificacionId")]
		public virtual ClasificacionesProyecto Clasificacion { get; set; }

		public virtual ICollection<Tarea> Tareas { get; set; }

		#endregion

		#region Constructors

		public Proyecto()
		{
			this.Tareas = new List<Tarea>();
		}

		#endregion
	}
}
