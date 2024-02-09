using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Tools.Seguridad.Datos.Repositorios.Abstracciones;
using Unna.OperationalReport.Tools.Seguridad.Servicios.Errores.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Seguridad.Servicios.General.Dtos;

namespace Unna.OperationalReport.Tools.Seguridad.Servicios.Errores.Servicios.Implementaciones
{
    public class ErrorServicio : IErrorServicio
    {
        private readonly IErrorRepositorio _errorRepositorio;
        private readonly SeguridadConfiguracionDto _seguridadConfiguracionDto;

        public ErrorServicio(
            IErrorRepositorio errorRepositorio,
            SeguridadConfiguracionDto seguridadConfiguracionDto
            )
        {
            _errorRepositorio = errorRepositorio;
            _seguridadConfiguracionDto = seguridadConfiguracionDto;
        }

        public async Task RegistrarError(Exception ex, string? jsonAdicionales = null)
        {
            if (ex == null)
            {
                return;
            }

            await _errorRepositorio.GuardarErrorAsync(ex.Message, ex.StackTrace, jsonAdicionales, _seguridadConfiguracionDto.NombreAplicacion);

        }
    }
}
