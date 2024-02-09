using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Tools.Seguridad.Servicios.Auth.Dtos
{
    public class TokenAccesoDto
    {
        public long IdUsuario { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string? TokenAcceso { get; set; }
    }
}
