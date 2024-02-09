using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Tools.Seguridad.Servicios.Errores.Servicios.Abstracciones
{
    public interface IErrorServicio
    {
        Task RegistrarError(Exception ex, string? jsonAdicionales = null);
    }
}
