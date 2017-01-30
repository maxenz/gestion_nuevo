using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using Gestion.ViewModels;
using Paramedic.Gestion.Service;
using Paramedic.Gestion.Model;

namespace Gestion.Controllers
{
    [Authorize(Roles = "Cliente,Administrador")]
    public class CuentaCorrienteController : Controller
    {

        #region Properties

        IClienteService _ClienteService;
        IClientesUsuarioService _ClientesUsuarioService;

        #endregion

        #region Constructors

        public CuentaCorrienteController(IClienteService ClienteService, IClientesUsuarioService ClientesUsuarioService)
        {
            _ClienteService = ClienteService;
            _ClientesUsuarioService = ClientesUsuarioService;
        }

        #endregion

        public ActionResult Index(int ClienteID = 0)
        {
            //si el que accede a cuentasCorrientes es Administrador
            if (User.IsInRole("Administrador"))
            {
                if (ClienteID == 0)
                {
                    //Si en la url paso un cliente cualquiera
                    return HttpNotFound("La cuenta corriente solicitada no existe");
                }
                else
                {
                    Cliente cliente = _ClienteService.FindBy(x => x.Id == ClienteID).FirstOrDefault();
                    //El cliente tiene cuenta corriente id?
                    if (cliente.CuentaCorrienteId == null)
                    {
                        return PartialView("NoCuentaCorriente");
                    }
                    else
                    {
                        //Le paso al partial de cuentas corrientes el objeto de cuenta corriente
                        int cc = Convert.ToInt32(cliente.CuentaCorrienteId);
                        return PartialView("_PartialVistaGeneral", setCuentaCorriente(cc));
                    }
                }
            }
            else
            {
                //El que está ingresando a la vista es el mismo cliente que quiere ver su cuenta corriente

                string actualUser = User.Identity.Name;
                ClientesUsuario cliUsr = _ClientesUsuarioService
                            .FindBy(x => x.Usuario.UserName == actualUser)
                            .FirstOrDefault();

                var ctaCorrID = cliUsr.Cliente.CuentaCorrienteId;

                if (ctaCorrID == null)
                {
                    return View("NoCuentaCorriente");
                }
                else
                {
                    int cc = Convert.ToInt32(ctaCorrID);
                    return View("Index", setCuentaCorriente(cc));
                }
            }

        }

        private List<CuentaCorrienteViewModel> setCuentaCorriente(int ctaCorrID)
        {

            try
            {
                Paramedic.Gestion.Web.wsCuentaCorriente.ClientesDocumentos svcCliDoc = new Paramedic.Gestion.Web.wsCuentaCorriente.ClientesDocumentos();

                var ctaCorr = new List<CuentaCorrienteViewModel>();

                string facturacion = svcCliDoc.GetCuentaCorriente((int)ctaCorrID, true);

                string[] vFacturacion = facturacion.Split('$');

                double acSaldo = 0;

                foreach (string factura in vFacturacion)
                {
                    string[] vFactura = factura.Split('^');

                    string fecha = getFormattedDate(vFactura[0]).ToShortDateString();
                    double debe = Convert.ToDouble(vFactura[3].Replace('.', ','));
                    double haber = Convert.ToDouble(vFactura[4].Replace('.', ','));
                    double saldo = debe - haber;
                    acSaldo += saldo;

                    ctaCorr.Add(new CuentaCorrienteViewModel
                    {
                        Fecha = getFormattedDate(vFactura[0]).ToShortDateString(),
                        TipoComprobante = vFactura[1],
                        NroComprobante = vFactura[2],
                        Debe = debe,
                        Haber = haber,
                        Saldo = acSaldo
                    });
                }

                if (acSaldo < 0)
                {
                    CuentaCorrienteViewModel lastCta = ctaCorr
                                                        .Where(x => x.TipoComprobante == "FAC").Last();

                    DateTime fecUltFactura = Convert.ToDateTime(lastCta.Fecha);

                    DateTime fec = DateTime.Now.AddDays(-60);

                    if (fecUltFactura < fec)
                    {
                        ViewBag.Deudor = 1;
                    }
                }

                ViewBag.Saldo = acSaldo;

                return ctaCorr;
            }
            catch
            {
                var ctaCorr = new List<CuentaCorrienteViewModel>();
                return ctaCorr;
            }

        }

        private DateTime getFormattedDate(string strFecha)
        {

            int year = Convert.ToInt32(strFecha.Substring(0, 4));
            int month = Convert.ToInt32(strFecha.Substring(4, 2));
            int day = Convert.ToInt32(strFecha.Substring(6, 2));

            DateTime dtFecha = new DateTime(year, month, day);

            return dtFecha;

        }

    }
}
