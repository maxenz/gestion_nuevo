using System;
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
        public String Codigo { get; set; }

        [Required]
        public String Descripcion { get; set; }

        public virtual ICollection<Provincia> Provincias { get; set; }

        #endregion
    }
}