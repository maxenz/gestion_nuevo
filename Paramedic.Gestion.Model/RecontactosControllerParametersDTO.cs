using Paramedic.Gestion.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paramedic.Gestion.Model
{
    public class RecontactosControllerParametersDTO : QueryControllerParametersDTO
    {
        #region Properties

        public DateTime DateFrom { get; set; }

        public DateTime DateTo { get; set; }

        public GestionType GestionType { get; set; }

        #endregion

        #region Constructors

        public RecontactosControllerParametersDTO(string searchDescription, int pageSize, int page, string from, string to, GestionType gestionType) : base(searchDescription, pageSize, page)
        {
            if (!string.IsNullOrEmpty(from))
            {
                this.DateFrom = Convert.ToDateTime(from);
            }

            if (!string.IsNullOrEmpty(to))
            {
                this.DateTo = Convert.ToDateTime(to);
            }

            this.GestionType = gestionType;
        }

        #endregion
    }
}
