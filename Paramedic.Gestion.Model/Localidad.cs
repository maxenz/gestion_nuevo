using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Paramedic.Gestion.Model
{
    [Table("Localidades")]
    public class Localidad : AuditableEntity<int>
    {
        #region Properties

        [Required]
        [MaxLength(3)]
        public string Codigo { get; set; }

        [Required]
        public string Descripcion { get; set; }

        [Required]
        [Display(Name = "Provincia")]
        public int ProvinciaId { get; set; }

        [ForeignKey("ProvinciaId")]
        public virtual Provincia Provincia { get; set; }

        #endregion
    }
}