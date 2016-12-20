using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Gestion.Models
{
    [Table("ClientesLicenciasProductosModulos")]
    public class ClientesLicenciasProductosModulo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [ForeignKey("ClientesLicenciasProducto")]
        public int ClientesLicenciasProductoID { get; set; }

        [Required]
        //[ForeignKey("ProductosModulo")]
        public int ProductosModuloID { get; set; }

        public virtual ClientesLicenciasProducto ClientesLicenciasProducto { get; set; }
       // public virtual ProductosModulo ProductosModulo { get; set; }


    }
}