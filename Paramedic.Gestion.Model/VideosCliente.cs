using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Paramedic.Gestion.Model
{
    [Table("VideosClientes")]
    public class VideosCliente : AuditableEntity<int>
    {
        #region Properties

        [Required]
        [Display(Name = "Cliente")]
        public int ClienteId { get; set; }

        [Required]
        [Display(Name = "Video")]
        public int VideoId { get; set; }

        [ForeignKey("ClienteId")]
        public virtual Cliente Cliente { get; set; }

        [ForeignKey("VideoId")]
        public virtual Video Video { get; set; }

        #endregion
    }
}