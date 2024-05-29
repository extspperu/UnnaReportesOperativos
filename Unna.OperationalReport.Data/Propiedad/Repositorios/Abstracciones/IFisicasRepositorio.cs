﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Infraestructura.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Propiedad.Entidades;
using Unna.OperationalReport.Data.Propiedad.Procedimientos;
using Unna.OperationalReport.Data.Registro.Entidades;
using Unna.OperationalReport.Data.Reporte.Procedimientos;

namespace Unna.OperationalReport.Data.Propiedad.Repositorios.Abstracciones
{
    public interface IFisicasRepositorio : IOperacionalRepositorio<Fisicas, object>
    {
        Task<List<ListarPropiedadesFisicas>?> ListarPropiedadesFisicasAsync(string? grupo);
        Task<List<CantidadCalidadVolumenGnaLoteIv>?> ListarCantidadCalidadVolumenGnaLoteIvAsync(DateTime fecha);
        Task<List<VolumenGasNaturalPorTipoLoteIv>?> ListarVolumenGasNaturalPorTipoLoteIvAsync(DateTime fecha);
    }
}
