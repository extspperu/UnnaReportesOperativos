using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Tools.Seguridad.Datos.Infraestructura.Repositorios.Abstracciones;

namespace Unna.OperationalReport.Tools.Seguridad.Datos.Repositorios.Abstracciones
{
    public interface IErrorRepositorio : ISeguridadRepositorio<object, object>
    {
        Task GuardarErrorAsync(string mensaje, string? traza, string? jsonAdicionales, string? appName);
    }
}
