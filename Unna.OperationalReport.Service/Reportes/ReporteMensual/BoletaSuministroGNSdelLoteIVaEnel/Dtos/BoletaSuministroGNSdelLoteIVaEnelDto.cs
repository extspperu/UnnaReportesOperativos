using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Unna.OperationalReport.Service.Reportes.Generales.Dtos;


namespace Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletaSuministroGNSdelLoteIVaEnel.Dtos
{
    public class BoletaSuministroGNSdelLoteIVaEnelDto
    {

        public string? Periodo { get; set; }
        public List<BoletaSuministroGNSdelLoteIVaEnelDetDto>? BoletaSuministroGNSdelLoteIVaEnelDet { get; set; }

        public double TotalVolumen { get; set; }
        public double TotalPoderCalorifico { get; set; }
        public double TotalEnergia { get; set; }
        public double TotalEnergiaTransferido { get; set; }
        public string? Comentarios { get; set; }

        [JsonIgnore]
        public long? IdUsuario { get; set; }

        public string? NombreReporte { get; set; }
        public string? UrlFirma { get; set; }

        [JsonIgnore]
        public string? RutaFirma { get; set; }
    }
}
