using Paramedic.Gestion.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;


namespace Paramedic.Gestion.Repository
{
	public class NoticiaRepository : GenericRepository<Noticia>, INoticiaRepository
	{
		#region Constructors

		public NoticiaRepository(DbContext context) : base(context)
		{

		}

		#endregion

		#region Public Methods

		public IEnumerable<Noticia> GetNoticiasNoVencidas()
		{
			var now = DateTime.Now.Date;
			return _dbset.Where(x => now <= DbFunctions.TruncateTime(x.FechaVencimiento)).OrderBy(x => x.CreatedDate);
		}

		#endregion
	}
}
