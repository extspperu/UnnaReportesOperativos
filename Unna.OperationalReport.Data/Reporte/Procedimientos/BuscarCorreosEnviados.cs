using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Data.Reporte.Procedimientos
{
    public class BuscarCorreosEnviados
    {
        public int IdReporte { get; set; }
        public string? NombreReporte { get; set; }
        public string? Asunto { get; set; }
        public bool? FueEnviado { get; set; }
        public DateTime? FechaEnvio { get; set; }
        public long? IdEnviarCorreo { get; set; }
        public string? Grupo { get; set; }
    }
}
