﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Infraestructura.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Propiedad.Entidades;

namespace Unna.OperationalReport.Data.Propiedad.Repositorios.Abstracciones
{
    public interface ISuministradorComponenteRepositorio : IOperacionalRepositorio<SuministradorComponente, int>
    {
        Task<List<SuministradorComponente>?> ListarComponenteAsync();
    }
}
