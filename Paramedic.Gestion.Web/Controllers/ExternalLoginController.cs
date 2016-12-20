using Gestion.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace Gestion.Controllers
{
    public class ExternalLoginController : Controller
    {
        private GestionDb db = new GestionDb();

        public ActionResult Index(string user = null, string pass = null, string llave = null, int shex_id = 0)
        {

            if (ModelState.IsValid && WebSecurity.Login(user, pass, true))
            {
                int cli_id = db.ClientesLicencias
                                .Where(p => p.Licencia.Serial == llave)
                                .Select(p => p.Cliente.ID)
                                .FirstOrDefault();

                int usr_id = db.UserProfiles
                                .Where(p => p.UserName == user)
                                .Select(p => p.UserId)
                                .FirstOrDefault();


                ClientesUsuario cli_usr = db.ClientesUsuarios
                                        .Where(p => p.ClienteID == cli_id)
                                        .Where(p => p.Usuario.UserName == user)
                                        .FirstOrDefault();

                if (cli_usr == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                if (cli_usr.ShamanExpressID.Equals(null))
                {
                    cli_usr.ShamanExpressID = shex_id;
                    db.Entry(cli_usr).State = EntityState.Modified;
                    db.SaveChanges();
                }
                
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }
       
        public int IsInGestion(string user = null, string pass = null, string llave = null)
        {
            int cli_id = db.ClientesLicencias
                .Where(p => p.Licencia.Serial == llave)
                .Select(p => p.Cliente.ID)
                .FirstOrDefault();

            int usr_id = db.UserProfiles
                              .Where(p => p.UserName == user)
                              .Select(p => p.UserId)
                              .FirstOrDefault();


            ClientesUsuario cli_usr = db.ClientesUsuarios
                                    .Where(p => p.ClienteID == cli_id)
                                    .Where(p => p.Usuario.UserName == user)
                                    .FirstOrDefault();

            if ((cli_id == 0) || (usr_id == 0) || (cli_usr == null))
            {
                return 0;
            }

            int ret = WebSecurity.Login(user, pass, true) ? 1 : 0;

            return ret;
        }

    }
}
