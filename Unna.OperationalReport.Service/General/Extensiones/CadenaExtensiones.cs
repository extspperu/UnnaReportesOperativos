using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Service.General.Extensiones
{
    public static class CadenaExtensiones
    {
        public static string ObtenerUltimos(this string source, int ultimos)
        {
            if (ultimos >= source.Length)
                return source;
            return source.Substring(source.Length - ultimos);
        }
    }
}
