using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Paramedic.Gestion.Model
{
    [Table("Paises")]
    public class Pais : AuditableEntity<int>
    {
        #region Properties

        [Required]
        [MaxLength(3)]
        public string Codigo { get; set; }

        [Required]
        public string Descripcion { get; set; }

        public virtual ICollection<Provincia> Provincias { get; set; }

        #endregion
    }
}