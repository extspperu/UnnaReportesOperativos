using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Service.Cartas.Dgh.Dtos;

namespace Unna.OperationalReport.Service.Cartas.Mercaptanos.Dtos
{
    public class MercaptanoDto
    {
        public DateTime? DiaOperativo { get; set; }
        public string? Periodo { get; set; }
        public string? Direccion { get; set; }
        public string? Telefono { get; set; }
        public string? SitioWeb { get; set; }
        public string? NombreArchivo { get; set; }
        public string? UrlFirma { get; set; }


        [JsonIgnore]
        public long? IdUsuario { get; set; }

        public CartaSolicitudDto? Solicitud { get; set; }
        public ControlMercaptanoDto? Mercaptano { get; set; }
        
        public string? Key { get; set; }

    }
}
