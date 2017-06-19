using Paramedic.Gestion.Model;
using System.Collections.Generic;

namespace Paramedic.Gestion.Service
{
	public interface INoticiaService : IEntityService<Noticia>
	{
		IEnumerable<Noticia> GetNoticiasNoVencidas();
	}
}
