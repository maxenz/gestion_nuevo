using Paramedic.Gestion.Model;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Paramedic.Gestion.Web.ViewModels
{
    public class VideoViewModel
    {
        #region Properties

        public int Id { get; set; }

        [Required]
        public string Descripcion { get; set; }

        [Required]
        public string Alias { get; set; }

        [Required]
        public bool EsPublico { get; set; }

        [Required]
        public int ClienteId { get; set; }

        #endregion

        #region Constructors

        public VideoViewModel(Video video)
        {
            this.Alias = video.Alias;
            this.Descripcion = video.Descripcion;
            this.EsPublico = video.EsPublico;
            this.Id = video.Id;
            // --> Si cambiamos la interfaz esto va a cambiar porque pueden seleccionar muchos clientes
            // --> para un video.
            if (!this.EsPublico)
            {
                this.ClienteId = video.ClientesVideos.Select(x => x.ClienteId).FirstOrDefault();
            }
        }

        public VideoViewModel() { } // VM

        #endregion

        #region Public Methods

        public Video ConvertVideoViewModelToVideo()
        {
            Video video = new Video();
            video.Alias = this.Alias;
            video.Descripcion = this.Descripcion;
            video.EsPublico = this.EsPublico;
            video.Id = this.Id;

            // --> Es un video nuevo
            if (this.Id == 0)
            {
                // --> No es publico
                if (this.ClienteId != 0)
                {
                    VideosCliente videosCliente = new VideosCliente();
                    videosCliente.ClienteId = this.ClienteId;
                    videosCliente.VideoId = this.Id;
                    video.ClientesVideos.Add(videosCliente);
                }
            }
           
            return video;
        }

        #endregion
    }
}