using Paramedic.Gestion.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Paramedic.Gestion.Web.ViewModels
{
    public class MailSocialServiceViewModel : SocialServicesViewModel
    {
        #region Properties

        public MailServiceSettings MailServiceSettings { get; set; }

        #endregion

        #region Constructors

        public MailSocialServiceViewModel(SocialService ss) : base(ss)
        {
            if (!string.IsNullOrEmpty(this.Configuration))
            {
                this.MailServiceSettings = Newtonsoft.Json.JsonConvert.DeserializeObject<MailServiceSettings>(this.Configuration);
            }
        }

        public MailSocialServiceViewModel() { }

        public override SocialService ToSocialService()
        {
            this.Configuration = Newtonsoft.Json.JsonConvert.SerializeObject(this.MailServiceSettings);
            return base.ToSocialService();
        }

        #endregion
    }
}