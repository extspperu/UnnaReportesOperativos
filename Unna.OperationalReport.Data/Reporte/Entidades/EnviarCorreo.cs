using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Data.Reporte.Entidades
{
    public class EnviarCorreo
    {
        public long IdEnviarCorreo { get; set; }
        public DateTime? Fecha { get; set; }
        public string? Destinatario { get; set; }
        public string? Cc { get; set; }
        public string? Asunto { get; set; }
        public string? Cuerpo { get; set; }
        public string? Adjuntos { get; set; }
        public DateTime Creado { get; set; }
        public DateTime? Actualizado { get; set; }
        public long? IdUsuario { get; set; }
        public int? IdReporte { get; set; }
        public bool FueEnviado { get; set; }
        public DateTime? FechaEnvio { get; set; }
        public bool IsBodyHtml { get; set; }
        
    }
}
