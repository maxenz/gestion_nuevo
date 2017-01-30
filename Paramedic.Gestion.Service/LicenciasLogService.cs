using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Paramedic.Gestion.Model;
using Paramedic.Gestion.Repository;
using LinqKit;
using Paramedic.Gestion.Model.Enums;

namespace Paramedic.Gestion.Service
{
    public class LicenciasLogService : EntityService<LicenciasLog>, ILicenciasLogService
    {
        #region Properties

        IUnitOfWork _unitOfWork;
        ILicenciasLogRepository _repo;

        #endregion

        #region Constructors

        public LicenciasLogService(IUnitOfWork unitOfWork, ILicenciasLogRepository repo)
    : base(unitOfWork, repo)
        {
            _unitOfWork = unitOfWork;
            _repo = repo;
        }

        #endregion

        #region Public Methods

        public IEnumerable<LicenciasLog> GetLicenciasLog(LicenciasLogControllerParametersDTO parameters)
        {
            var predicate = getPredicateByConditions(parameters);
            return FindByPage(predicate, "CreatedDate DESC", parameters.PageSize, parameters.Page);
        }

        public Expression<Func<LicenciasLog, bool>> getPredicateByConditions(LicenciasLogControllerParametersDTO parameters)
        {
            var predicate = PredicateBuilder.New<LicenciasLog>();
            
            if (!string.IsNullOrEmpty(parameters.SearchDescription))
            {
                predicate = predicate
                    .And(x => x.GenericDescription.ToUpper().Contains(parameters.SearchDescription.ToUpper()));
            }

            predicate = predicate.And(x => x.CreatedDate >= parameters.DateFrom);
            predicate = predicate.And(x => x.CreatedDate <= parameters.DateTo);

            if (!parameters.AndroidLogsVisible)
            {
                predicate = predicate.And(x => x.Type != LicenciasLogType.Android);
            }
            
            return predicate;
        }

        #endregion
    }
}
