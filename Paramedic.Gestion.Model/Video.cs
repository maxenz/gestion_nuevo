using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Paramedic.Gestion.Model
{
    [Table("Videos")]
    public class Video : AuditableEntity<int>
    {
        #region Properties

        [Required]
        public string Descripcion { get; set; }

        [Required]
        public string Alias { get; set; }

        [Required]
        public bool EsPublico { get; set; }

        public virtual ICollection<VideosCliente> ClientesVideos { get; set; }

        #endregion

        #region Constructors

        public Video()
        {
            this.ClientesVideos = new List<VideosCliente>();
        }

        #endregion
    }
}