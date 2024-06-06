using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Service.Cartas.Dgh.Dtos
{
    public class CartaDto
    {
        public CartaSolicitudDto? Solicitud { get; set; }
        //public CartaSolicitudDto? InformeMensual { get; set; }
        //public CartaSolicitudDto? InformeMensual { get; set; }
    }

    public class CartaSolicitudDto
    {
        public string? Periodo { get; set; }
        public string? Asunto { get; set; }
        public string? Contenido { get; set; }
        public string? UrlFirma { get; set; }
        public string? Direccion { get; set; }
        public string? Telefono { get; set; }
        public string? SitioWeb { get; set; }
    }
}
