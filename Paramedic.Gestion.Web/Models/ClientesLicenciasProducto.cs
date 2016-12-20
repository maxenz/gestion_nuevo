using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Gestion.Models
{
    [Table("ClientesLicenciasProductos")]
    public class ClientesLicenciasProducto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [ForeignKey("ClientesLicencia")]
        public int ClientesLicenciaID { get; set; }

        [Required]
        [ForeignKey("Producto")]
        public int ProductoID { get; set; }

        public virtual ClientesLicencia ClientesLicencia { get; set; }
        public virtual Producto Producto { get; set; }

        public virtual IList<ClientesLicenciasProductosModulo> ClientesLicenciasProductosModulos { get; set; }


    }
}