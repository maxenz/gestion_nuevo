using Paramedic.Gestion.Model.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Paramedic.Gestion.Model
{
    [Table("SocialServiceTypes")]
    public class SocialServiceType : AuditableEntity<int>
    {
        #region Properties

        [Required]
        [Display(Name ="Habilitado?")]
        public bool Enabled { get; set; }

        [Required]
        public string Configuration { get; set; }

        [Required]
        public SocialMediaTypes SocialMediaType { get; set; }

        public virtual string Icon
        {
            get
            {
                switch (SocialMediaType)
                {
                    case SocialMediaTypes.Facebook:
                        return "fa fa-facebook";
                    case SocialMediaTypes.Mail:
                        return "fa fa-envelope";
                }

                return "";
            }
        }

        public virtual string Description { get
            {
                return SocialMediaType.ToString();
            }
        }

        #endregion
    }
}
