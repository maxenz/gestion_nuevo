using LinqKit;
using Paramedic.Gestion.Model;
using Paramedic.Gestion.Model.Enums;
using Paramedic.Gestion.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Paramedic.Gestion.Service
{
    public class ClienteService : EntityService<Cliente>, IClienteService
    {
        #region Properties

        IUnitOfWork _unitOfWork;
        IClienteRepository _clienteRepository;

        #endregion

        #region Constructors

        public ClienteService(IUnitOfWork unitOfWork, IClienteRepository clienteRepository)
    : base(unitOfWork, clienteRepository)
        {
            _unitOfWork = unitOfWork;
            _clienteRepository = clienteRepository;
        }

        #endregion

        #region Public Methods

        public IEnumerable<Cliente> GetClientsByType(ClientControllerParametersDTO parameters)
        {
            var predicate = getPredicateByConditions(parameters);
            return FindByPage(predicate, "RazonSocial ASC", parameters.PageSize, parameters.Page);

        }

        public Expression<Func<Cliente, bool>> getPredicateByConditions(ClientControllerParametersDTO parameters)
        {
            var predicate = PredicateBuilder.New<Cliente>();

            if (string.IsNullOrEmpty(parameters.SearchDescription) && parameters.SelectedClientType == ClientType.Default) return null;

            if (!string.IsNullOrEmpty(parameters.SearchDescription))
            {
                var description = parameters.SearchDescription.ToUpper();
                var predicateSearch = PredicateBuilder.New<Cliente>();
                predicateSearch = predicateSearch.Or(x => x.RazonSocial.ToUpper().Contains(description));
                predicateSearch = predicateSearch.Or(x => x.Calle.ToUpper().Contains(description));
                predicateSearch = predicateSearch.Or(x => x.Domicilio.ToUpper().Contains(description));
                predicateSearch = predicateSearch.Or(x => x.Localidad.Descripcion.ToUpper().Contains(description));
                predicateSearch = predicateSearch.Or(x => x.Referencia.ToUpper().Contains(description));
                predicateSearch = predicateSearch.Or(x => x.Localidad.Provincia.Descripcion.ToUpper().Contains(description));
                predicate = predicate.And(predicateSearch);                
            }

            switch (parameters.SelectedClientType)
            {
                case ClientType.InNegotiation:
                    predicate = predicate.And(x => !x.ClientesLicencias.Any());
                    predicate = predicate.And(x => x.ClientesGestiones.Any());
                    break;
                case ClientType.WithLicense:
                    predicate = predicate.And(x => x.ClientesLicencias.Any());
                    break;
            }

            return predicate;
        }

        #endregion

    }
}
