﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Paramedic.Gestion.Web.ViewModels
{
    public class ProductosAsignados
    {
        public int ProductoID { get; set; }
        public string Descripcion { get; set; }
        public bool Asignado { get; set; }
    }
}