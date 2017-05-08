using Paramedic.Gestion.Model;
using Paramedic.Gestion.Model.Enums;
using System.ComponentModel.DataAnnotations;

namespace Paramedic.Gestion.Web.ViewModels
{
    public class SocialServiceTypesViewModel
    {
        #region Properties

        [Required]
        [Display(Name = "Habilitado?")]
        public bool Enabled { get; set; }

        public string Configuration { get; set; }

        [Required]
        public SocialMediaTypes SocialMediaType { get; set; }

        #endregion

        #region Public Methods

        public SocialServiceType ToSocialServiceType()
        {
            SocialServiceType socialServiceType = new SocialServiceType();
            socialServiceType.Configuration = "por ahora hardcodeado";
            socialServiceType.SocialMediaType = this.SocialMediaType;
            socialServiceType.Enabled = this.Enabled;
            return socialServiceType;
        }

        #endregion
    }
}