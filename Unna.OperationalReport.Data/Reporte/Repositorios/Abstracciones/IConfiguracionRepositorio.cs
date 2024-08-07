﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Infraestructura.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Reporte.Entidades;

namespace Unna.OperationalReport.Data.Reporte.Repositorios.Abstracciones
{
    public interface IConfiguracionRepositorio : IOperacionalRepositorio<Entidades.Configuracion, int>
    {
        Task<List<Entidades.Configuracion>?> ListarAsync();
    }
}
