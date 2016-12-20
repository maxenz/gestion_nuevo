using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Gestion.Models
{
    [Table("ClientesUsuarios")]
    public class ClientesUsuario
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [ForeignKey("Usuario")]
        public int UsuarioID { get; set; }

        [Required]
        [ForeignKey("Cliente")]
        public int ClienteID { get; set; }

        [Display(Name="Shaman Full ID")]
        public int? ShamanFullID { get; set; }

        [Display(Name = "Shaman Express ID")]
        public int? ShamanExpressID { get; set; }

        public virtual Cliente Cliente { get; set; }
        public virtual UserProfile Usuario { get; set; }

    }
}