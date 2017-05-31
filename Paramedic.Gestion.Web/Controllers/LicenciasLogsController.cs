using System;
using System.Collections.Generic;
using System.Web.Mvc;
using PagedList;
using System.Globalization;
using Paramedic.Gestion.Service;
using Paramedic.Gestion.Model;
using Paramedic.Gestion.Web.ViewModels;
using System.Linq;

namespace Paramedic.Gestion.Web.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class LicenciasLogsController : Controller
    {
        #region Properties

        ILicenciasLogService _LicenciasLogService;
        IClientesLicenciaService _ClientesLicenciaService;
        private int controllersPageSize = 10;

        #endregion

        #region Constructors

        public LicenciasLogsController(ILicenciasLogService LicenciasLogService, IClientesLicenciaService ClientesLicenciaService)
        {
            _LicenciasLogService = LicenciasLogService;
            _ClientesLicenciaService = ClientesLicenciaService;
        }

        #endregion

        #region Public Methods

        public ActionResult Index(string searchName = null, int page = 1, string fechaDesde = null, string fechaHasta = null, string chkAndroidLogs = null)
        {
            try
            {
                IEnumerable<ClientesLicencia> clientesLicencias = _ClientesLicenciaService.GetAll();

                LicenciasLogControllerParametersDTO parameters = new LicenciasLogControllerParametersDTO(searchName, controllersPageSize, page, fechaDesde, fechaHasta, Convert.ToBoolean(chkAndroidLogs));

                ViewBag.dftDesde = parameters.DateFrom.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                ViewBag.dftHasta = parameters.DateTo.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

                IEnumerable<LicenciasLog> logs = _LicenciasLogService.GetLicenciasLog(parameters);
                IList<LicenciasLogViewModel> vmLogs = new List<LicenciasLogViewModel>();

                foreach (LicenciasLog log in logs)
                {
                    ClientesLicencia cliLic = clientesLicencias.Where(x => x.LicenciaId == log.LicenciaId).FirstOrDefault();
                    vmLogs.Add(new LicenciasLogViewModel(log, cliLic));
                }

                int count = _LicenciasLogService.GetCount(_LicenciasLogService.getPredicateByConditions(parameters));
                var resultAsPagedList = new StaticPagedList<LicenciasLogViewModel>(vmLogs, page, controllersPageSize, count);

                if (Request.IsAjaxRequest())
                {
                    return PartialView("_Logs", resultAsPagedList);
                }

                return View(resultAsPagedList);
            }
            catch (Exception exception)
            {
                var ex = exception.Message;
                return PartialView("_Logs", null);
            }
        }

        #endregion
    }
}
