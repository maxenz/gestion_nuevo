using LinqKit;
using Newtonsoft.Json;
using Paramedic.Gestion.Model;
using Paramedic.Gestion.Service;
using Paramedic.Gestion.Web.ViewModels;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Paramedic.Gestion.Web.Controllers
{
    [Authorize(Roles = "Administrador")]

    public class MapaController : Controller
    {
        #region Properties

        IMedioDifusionService _MediosDifusionService;
        IClienteService _ClienteService;

        #endregion

        #region Constructors

        public MapaController(IMedioDifusionService MedioDifusionService, IClienteService ClienteService)
        {
            _MediosDifusionService = MedioDifusionService;
            _ClienteService = ClienteService;
        }

        #endregion

        #region Public Methods

        public ActionResult Index()
        {
            ViewBag.MediosDifusion = _MediosDifusionService.GetAll();
            return View();
        }

        [AllowAnonymous]
        public string GetPositionsOfClients(int[] vData)
        {

            if (vData != null)
            {
                IEnumerable<Cliente> clientes = SearchClients(vData);

                List<MapaViewModel> mapViewModel = new List<MapaViewModel>();

                foreach (Cliente cli in clientes)
                {
                    mapViewModel.Add(new MapaViewModel(cli));
                }

                string json = JsonConvert.SerializeObject(mapViewModel);

                return json;
            }
            else
            {
                return JsonConvert.SerializeObject(null);
            }

        }

        #endregion

        #region Private Methods

        private IEnumerable<Cliente> SearchClients(params int[] vMediosDifusion)
        {

            var predicate = PredicateBuilder.New<Cliente>();

            foreach (int item in vMediosDifusion)
            {
                int temp = item;
                predicate = predicate.Or(p => p.MedioDifusionId == temp);
            }

            return _ClienteService.FindBy(predicate);
        }

        #endregion

        //private string validateNotRepeatedLatitude(List<GeoPositions> lst, string lat)
        //{
        //    foreach (var pos in lst)
        //    {
        //        if (pos.Latitud == lat)
        //        {
        //            int lengthLat = lat.Length;
        //            int lastFourDigits = Convert.ToInt32(lat.Substring(lengthLat - 6, 6));
        //            lastFourDigits = lastFourDigits + 400000;
        //            string strLastFourDigits = lastFourDigits.ToString();
        //            string firstPartLat = lat.Substring(0, lengthLat - 6);
        //            return firstPartLat + strLastFourDigits;
        //            //double dblLat = Convert.ToDouble(lat);
        //            //lat = lat + 2000;
        //            //string strLat = Convert.ToString(dblLat);
        //            //return strLat;

        //        }
        //    }
        //    return lat;
        //}

    }
}
