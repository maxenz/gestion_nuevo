using Paramedic.Gestion.Model;
using System.Data.Entity;


namespace Paramedic.Gestion.Repository
{
	public class NoticiaRepository : GenericRepository<Noticia>, INoticiaRepository
	{
		public NoticiaRepository(DbContext context)
			: base(context)
		{

		}
	}
}
