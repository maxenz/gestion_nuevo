using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Paramedic.Gestion.Model
{
    [Table("Provincias")]
    public class Provincia : AuditableEntity<int>
    {
        #region Properties

        [Required]
        [MaxLength(3)]
        public String Codigo { get; set; }

        [Required]
        public String Descripcion { get; set; }

        [Required]
        [Display(Name = "Pais")]
        public int PaisId { get; set; }

        [ForeignKey("PaisId")]
        public virtual Pais Pais { get; set; }

        public virtual IEnumerable<Localidad> Localidades { get; set; }

        #endregion
    }
}