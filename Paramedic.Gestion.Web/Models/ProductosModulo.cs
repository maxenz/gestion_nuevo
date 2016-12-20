using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Gestion.Models
{
    [Table("ProductosModulos")]
    public class ProductosModulo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [ForeignKey("Producto")]
        public int ProductoID { get; set; }

        public String Codigo { get; set; }

        [Required]
        public String Descripcion { get; set; }

        public virtual Producto Producto { get; set; }

    }
}