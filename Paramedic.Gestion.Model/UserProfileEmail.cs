using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Paramedic.Gestion.Model
{
    public class UserProfileEmail : AuditableEntity<int>
    {
        #region Properties

        [ForeignKey("UserProfile")]
        [Display(Name = "Usuario")]
        public int UserProfileId { get; set; }

        [Required]
        public string Email { get; set; }

        public bool EmailPrincipal { get; set; }

        #region Virtual Properties

        public virtual UserProfile UserProfile { get; set; }

        #endregion

        #endregion

        #region Constructors

        public UserProfileEmail() { } //EF

        public UserProfileEmail(int userProfileId, string email, bool emailPrincipal)
        {
            this.UserProfileId = userProfileId;
            this.Email = email;
            this.EmailPrincipal = emailPrincipal;
        }

        #endregion
    }
}
