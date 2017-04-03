using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Paramedic.Gestion.Model
{
    [Table("UserProfile")]
    public class UserProfile : AuditableEntity<int>
    {
        #region Properties

        [Display(Name = "Usuario")]
        public string UserName { get; set; }

        public List<UserProfileEmail> Emails { get; set; }

        public string Nombre { get; set; }

        public string Apellido { get; set; }

        #endregion

        #region Constructors

        public UserProfile()
        {
            this.Emails = new List<UserProfileEmail>();
        }

        #endregion
    }
}
