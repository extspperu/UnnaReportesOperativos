using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Service.Correos.Dtos
{
    public class ListarCorreosEnviadosDto
    {
        public string? IdReporte { get; set; }
        public string? NombreReporte { get; set; }
        public string? Asunto { get; set; }
        public bool? FueEnviado { get; set; }
        public DateTime? FechaEnvio { get; set; }
        public string? IdEnviarCorreo { get; set; }
        public string? Grupo { get; set; }

        public string? FechaEnvioCadena
        {
            get
            {
                return FechaEnvio.HasValue ? FechaEnvio.Value.ToString("dd/MM/yyyy hh:mm:ss tt") : null;
            }
        }
        
        public string? DiaOperativo { get; set; }
    }
}
