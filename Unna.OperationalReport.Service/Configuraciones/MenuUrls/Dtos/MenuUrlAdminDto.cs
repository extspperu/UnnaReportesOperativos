using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Service.Configuraciones.MenuUrls.Dtos
{
    public class MenuUrlAdminDto
    {
        public long IdMenuUrl { get; set; }
        public string? Nombre { get; set; }
        public string? Icono { get; set; }
        public string? Url { get; set; }
        public long? IdMenuUrlPadre { get; set; }
        public int Orden { get; set; }
    }
}
