using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Gestion
{
	public class RouteConfig
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			routes.MapRoute(
				name: "Modulos.Asignados",
				url: "{ClientesLicenciaID}/ClientesLicencias/{action}/{ProductoID}",
				defaults: new { controller = "ClientesLicencias", action = "GetModAsignados", ProductoID = UrlParameter.Optional }
			);

			routes.MapRoute(
				name: "Provincias.Localidades",
				url: "Paises/{PaisID}/Provincias/{ProvinciaID}/Localidades/{action}/{id}",
				defaults: new { controller = "Localidades", action = "Index", id = UrlParameter.Optional }
			);

			routes.MapRoute(
				name: "Paises.Provincias",
				url: "Paises/{PaisID}/Provincias/{action}/{id}",
				defaults: new { controller = "Provincias", action = "Index", id = UrlParameter.Optional }
			);

			routes.MapRoute(
				name: "Proyectos.Tareas",
				url: "Proyectos/{ProyectoId}/Tareas/{action}/{id}",
				defaults: new { controller = "Tareas", action = "Index", id = UrlParameter.Optional }
			);

			routes.MapRoute(
				name: "ProductosModulos.Intentos",
				url: "Productos/{ProductoId}/ProductosModulos/{ProductosModuloId}/Intentos/{action}/{id}",
				defaults: new { controller = "ProductosModulosIntentos", action = "Index", id = UrlParameter.Optional }
			);

			routes.MapRoute(
				name: "Productos.Modulos",
				url: "Productos/{ProductoID}/ProductosModulos/{action}/{id}",
				defaults: new { controller = "ProductosModulos", action = "Index", id = UrlParameter.Optional }
			);

			routes.MapRoute(
				name: "Default",
				url: "{controller}/{action}/{id}",
				defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
			);

			routes.MapRoute(
			   name: "Clientes.Contactos",
			   url: "Clientes/Edit/{ClienteID}/{controller}/{action}/{id}",
			   defaults: new { controller = "ClientesContactos", action = "Edit", id = UrlParameter.Optional }
			);

		}
	}
}