using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gestion.ViewModels
{
    public class ProductosAsignados
    {
        public int ProductoID { get; set; }
        public String Descripcion { get; set; }
        public bool Asignado { get; set; }
    }
}