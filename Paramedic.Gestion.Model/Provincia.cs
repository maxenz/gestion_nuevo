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
        public string Codigo { get; set; }

        [Required]
        public string Descripcion { get; set; }

        [Required]
        [Display(Name = "Pais")]
        public int PaisId { get; set; }

        [ForeignKey("PaisId")]
        public virtual Pais Pais { get; set; }

        public virtual ICollection<Localidad> Localidades { get; set; }

        #endregion

        #region Constructors

        public Provincia()
        {
            this.Localidades = new List<Localidad>();
        }

        #endregion
    }
}