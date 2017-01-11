using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Paramedic.Gestion.Model
{
    [Table("Licencias")]
    public class Licencia : AuditableEntity<int>
    {

        #region Properties

        [Required]
        public string Serial { get; set; }

        [Display(Name = "Nro. HardkeyNet")]
        public string NumeroDeLlave { get; set; }

        public virtual ICollection<Producto> Productos { get; set; }

        [ForeignKey("ClientesLicenciaId")]
        public virtual ClientesLicencia ClientesLicencia { get; set; }

        public virtual string FormattedProducts
        {
            get
            {
                string strProductos = "";

                foreach (var prod in this.Productos)
                {
                    if (string.IsNullOrEmpty(strProductos))
                    {
                        strProductos = prod.Numero.ToString();
                    }
                    else
                    {
                        strProductos = string.Format("{0} / {1}", strProductos, prod.Numero.ToString());
                    }
                }

                return strProductos;
            }
        }

        public virtual string Estado
        {
            get
            {
                if (this.ClientesLicencia != null)
                {
                    return this.ClientesLicencia.Cliente.RazonSocial;
                }

                return null;

            }
        }


        #endregion

        #region Constructors

        public Licencia()
        {
            Productos = new List<Producto>();
        }

        #endregion

    }
}