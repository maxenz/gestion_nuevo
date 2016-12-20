using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Gestion.Models
{
    [Table("ClientesTerminales")]
    public class ClientesTerminal
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [ForeignKey("TipoTerminal")]
        public int TipoTerminalID { get; set; }

        [Required]
        [ForeignKey("Cliente")]
        public int ClienteID { get; set; }

        [Required]
        [Display(Name = "Valor 1")]
        public String Valor1 { get; set; }

        [Display(Name = "Valor 2")]
        public String Valor2 { get; set; }

        [Display(Name = "Valor 3")]
        public String Valor3 { get; set; }

        [Display(Name = "Valor 4")]
        public String Valor4 { get; set; }

        public String Referencia { get; set; }

        public virtual Cliente Cliente { get; set; }
        public virtual TipoTerminal TipoTerminal { get; set; }

    }
}