using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;

namespace Unna.OperationalReport.Tools.Seguridad.Servicios.General.Dtos
{
    public class SeguridadConfiguracionDto
    {
        public string? LlaveRsaPublica { get; set; }

        public string? LlaveRsaPrivada { get; set; }
        public string? PasswordSalt { get; set; }

        public string? NombreAplicacion { get; set; }

        public string? TokenAplicacion { get; set; }

        public string? AppName { get; set; }

        public string? IdAplicacionActual
        {
            get
            {
                return RijndaelUtilitario.DecryptRijndaelFromUrl<string>(AppName);

            }
        }
    }
}
