using SocialMedia.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Paramedic.Gestion.Model;
using Paramedic.Gestion.Service;

namespace Paramedic.Gestion.SocialMedia.Services
{
    public class MailSocialService : ISocialMediaService
    {
        #region Properties

        ISocialServicesService _SocialServicesService;

        #endregion

        public MailSocialService(ISocialServicesService SocialServicesService)
        {
            _SocialServicesService = SocialServicesService;
        }

        public void CheckMessages()
        {
            //IEnumerable<SocialService> socialServices = _SocialServicesService.GetAll();
            //foreach (SocialService socialService in socialServices)
            //{
            //    LoggingService.Instance.Write(Model.Enums.LoggingTypes.Information, socialService.Description);
            //}            
        }

        public void Send(Message message)
        {
            throw new NotImplementedException();
        }
    }
}
