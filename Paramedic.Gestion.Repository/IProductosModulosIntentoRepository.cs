﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Paramedic.Gestion.Model;

namespace Paramedic.Gestion.Repository
{
	public interface IProductosModulosIntentoRepository : IGenericRepository<ProductosModulosIntento>
	{
		IEnumerable<ProductosModulosIntento> GetByModuloId(int modId);
	}
}
