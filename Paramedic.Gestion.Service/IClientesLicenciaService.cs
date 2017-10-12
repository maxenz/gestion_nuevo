using Paramedic.Gestion.Model;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Paramedic.Gestion.Service
{
    public interface IClientesLicenciaService : IEntityService<ClientesLicencia>
    {
        ClientesLicencia GetById(int id);

		ClientesLicenciasProductosModulosHistorial GetAddonHistorial(string license, int prodModId);

		IEnumerable<ProductosModulo> GetProductosModulosForAddon(string licencia);

		void DeleteClientesLicencia(int id);

		IEnumerable<ClientesLicencia> GetLicencias(VencimientosQueryControllerParametersDTO queryParameters);

		Expression<Func<ClientesLicencia, bool>> GetPredicateByConditions(VencimientosQueryControllerParametersDTO queryParameters);
	}
}
