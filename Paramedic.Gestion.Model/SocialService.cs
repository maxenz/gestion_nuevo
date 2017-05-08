using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paramedic.Gestion.Model
{
    public class SocialService : AuditableEntity<int>
    {
        #region Properties

        [Required]
        public bool Enabled { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Configuration { get; set; }

        [Required]
        [Display(Name = "SocialServiceType")]
        public int SocialServiceTypeId { get; set; }

        [ForeignKey("SocialServiceTypeId")]
        public virtual SocialServiceType SocialServiceType { get; set; }

        #endregion
    }
}
