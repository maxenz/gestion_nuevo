using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Gestion.Models
{
    [Table("VideosClientes")]
    public class VideosCliente
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [ForeignKey("Cliente")]
        public int ClienteID { get; set; }

        [Required]
        [ForeignKey("Video")]
        public int VideoID { get; set; }

        public virtual Cliente Cliente { get; set; }
        public virtual Video Video { get; set; }

    }
}