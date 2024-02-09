using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Configuracion.Repositorios.Abstracciones;
using Unna.OperationalReport.Service.Configuraciones.Errores.Servicios.Abstracciones;

namespace Unna.OperationalReport.Service.Configuraciones.Errores.Servicios.Implementaciones
{
    public class ErrorServicio: IErrorServicio
    {

        private readonly IErrorRepositorio _errorRepositorio;
        public ErrorServicio(IErrorRepositorio errorRepositorio)
        {
            _errorRepositorio = errorRepositorio;
        }

        public async Task RegistrarError(Exception ex, string? jsonAdicionales = null)
        {
            if (ex == null)
            {
                return;
            }
            await _errorRepositorio.GuardarErrorAsync(ex.Message, ex.StackTrace, jsonAdicionales);
        }
    }
}
