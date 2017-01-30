using Paramedic.Gestion.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paramedic.Gestion.Model
{
    public abstract class Message
    {
        #region Properties

        public string Body { get; set; }

        public string From { get; set; }

        public string To { get; set; }

        public SocialMediaTypes SocialMediaType { get; set; }

        #endregion

        #region Constructors

        public Message(string body, string to)
        {
            this.Body = body;
            this.To = to;
        }

        #endregion
    }
}
