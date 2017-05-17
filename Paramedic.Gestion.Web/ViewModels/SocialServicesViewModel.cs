using Paramedic.Gestion.Model;
using Paramedic.Gestion.Model.Enums;
using System.ComponentModel.DataAnnotations;

namespace Paramedic.Gestion.Web.ViewModels
{
    public class SocialServicesViewModel
    {
        #region Properties

        public int Id { get; set; }

        [Required]
        [Display(Name = "Habilitado?")]
        public bool Enabled { get; set; }

        public string Configuration { get; set; }

        public string Description { get; set; }

        [Required]
        public int SocialServiceTypeId { get; set; }

        #endregion

        #region Constructors

        public SocialServicesViewModel(SocialService ss)
        {
            this.Id = ss.Id;
            this.SocialServiceTypeId = ss.SocialServiceTypeId;
            this.Enabled = ss.Enabled;
            this.Description = ss.Description;
            this.Configuration = ss.Configuration;
        }

        public SocialServicesViewModel() { }

        #endregion

        #region Public Methods

        public virtual SocialService ToSocialService()
        {
            SocialService ss = new SocialService();
            ss.Id = this.Id;
            ss.SocialServiceTypeId = this.SocialServiceTypeId;
            ss.Enabled = this.Enabled;
            ss.Description = this.Description;
            ss.Configuration = this.Configuration;
            return ss;
        }

        #endregion
    }
}