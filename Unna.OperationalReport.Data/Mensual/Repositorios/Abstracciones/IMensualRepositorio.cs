﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Infraestructura.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Mensual.Entidades;
using Unna.OperationalReport.Data.Mensual.Procedimientos;

namespace Unna.OperationalReport.Data.Mensual.Repositorios.Abstracciones
{
    public interface IMensualRepositorio : IOperacionalRepositorio<object, object>
    {
        Task<DatoCpgna50?> BuscarDatoCpgna50Async(DateTime desde, DateTime hasta, int? idLote);
    }
}
