﻿using Paramedic.Gestion.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paramedic.Gestion.Service
{
    public interface ITicketsClasificacionService : IEntityService<TicketsClasificacion>
    {
        TicketsClasificacion GetById(int id);
    }
}
