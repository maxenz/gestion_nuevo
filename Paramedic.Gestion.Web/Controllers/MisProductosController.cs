using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Gestion.Models;
using PagedList;
using System.Data.SqlClient;
using Paramedic.Gestion.Model;
using Paramedic.Gestion.Service;

namespace Gestion.Controllers
{
    [Authorize]
    public class MisProductosController : Controller
    {

        #region Properties

        IClienteService _ClienteService;

        #endregion

        #region Constructors

        public MisProductosController(IClienteService ClienteService)
        {
            _ClienteService = ClienteService;
        }

        #endregion

        #region Public Methods

        public ActionResult Index(int ClienteID = 0)
        {

            if (User.IsInRole("Administrador"))
            {
                if (ClienteID == 0)
                {
                    return HttpNotFound("Los productos solicitados no existen");
                }
                else
                {
                    Cliente cli = db.Clientes.Find(ClienteID);
                    var productos = setProductos(cli);
                    if (productos != null)
                    {
                        return PartialView("Index", productos);
                    }
                    else
                    {
                        return PartialView("_NoProductos");
                    }

                }
            }
            else
            {
                var userName = User.Identity.Name;
                var userID = db.UserProfiles.Where(x => x.UserName == userName).FirstOrDefault().UserId;

                ClientesUsuario cliUsr = db.ClientesUsuarios.Where(x => x.UsuarioID == userID).FirstOrDefault();

                getUserForShamanWeb(1, cliUsr);

                Cliente cliente = cliUsr.Cliente;
                var productos = setProductos(cliente);

                if (productos != null)
                {
                    return View("Index",productos);
                }
                else
                {
                    return View("_NoProductos");
                }
            }

        }

        private void getUserForShamanWeb(int typeShaman, ClientesUsuario cliUsr)
        {
            int shmID;
            if (typeShaman == 1)
            {
                shmID = Convert.ToInt32(cliUsr.ShamanExpressID);
            }
            else
            {
                shmID = Convert.ToInt32(cliUsr.ShamanFullID);
            }

            if (shmID == 0)
            {
                ViewBag.ShamanExpressUser = "";
                return;
            }


            ClientesLicencia cliLic = cliUsr.Cliente.ClientesLicencias.FirstOrDefault();
            string dbConexion = cliLic.ConexionServidor;
            string dbName = cliLic.CnnCatalog;
            string dbUser = cliLic.CnnUser;
            string dbPass = cliLic.CnnPassword;

            ViewBag.dbConexion = dbConexion;
            ViewBag.dbName = dbName;
            ViewBag.dbUser = dbUser;
            ViewBag.dbPass = dbPass;
            ViewBag.Cliente = cliLic.Cliente.RazonSocial;

            string connectionString = null;
            SqlConnection cnn;
            SqlCommand sqlCmd;
            connectionString = "Data Source=" + dbConexion + ";Initial Catalog=" + dbName + ";User ID=" + dbUser + ";Password=" + dbPass + ";";
            cnn = new SqlConnection(connectionString);
            string sql = "SELECT Identificacion FROM Usuarios WHERE ID = " + shmID;
            try
            {
                cnn.Open();
                sqlCmd = new SqlCommand(sql, cnn);
                SqlDataReader sqlReader = sqlCmd.ExecuteReader();
                while (sqlReader.Read())
                {
                   ViewBag.ShamanExpressUser = sqlReader.GetValue(0);

                }
                sqlReader.Close();
                sqlCmd.Dispose();
                cnn.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }



        }

        private IList<ClientesLicenciasProducto> setProductos(Cliente cliente)
        {
            var cliLic = db.Clientes.Find(cliente.ID).ClientesLicencias.FirstOrDefault();
            if (cliLic == null)
            {
                return null;
            }
            else
            {
                // HARCODEADO A PEDIDO DE JAVI. 30/05/2016
                return cliLic.ClientesLicenciasProductos.Where(x => x.Producto.Descripcion == "Shaman Express").ToList();
            }
        }

        #endregion
    }
}
