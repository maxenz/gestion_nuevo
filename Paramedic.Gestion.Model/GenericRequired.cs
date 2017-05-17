using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paramedic.Gestion.Model
{
    public class GenericRequired : RequiredAttribute
    {
        public GenericRequired()
        {
            this.ErrorMessage = "Es obligatorio ingresar el campo {0}.";
        }
    }
}
