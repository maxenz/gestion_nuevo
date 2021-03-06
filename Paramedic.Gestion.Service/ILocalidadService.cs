﻿using Paramedic.Gestion.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paramedic.Gestion.Service
{
    public interface ILocalidadService : IEntityService<Localidad>
    {
        Localidad GetById(int Id);
    }
}
