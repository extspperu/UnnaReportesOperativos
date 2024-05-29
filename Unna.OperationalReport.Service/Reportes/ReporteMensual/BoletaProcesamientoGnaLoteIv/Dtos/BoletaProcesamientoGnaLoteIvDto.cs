using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletaProcesamientoGnaLoteIv.Dtos
{
    public class BoletaProcesamientoGnaLoteIvDto
    {

        public string? NombreReporte { get; set; }
        public string? UrlFirma { get; set; }
        public string? Mes { get; set; }
        public int? Anio { get; set; }
        public double TotalVolumen { get; set; }
        public double TotalPc { get; set; }
        public double TotalEnergia { get; set; }
        public double EnergiaVolumenProcesado { get; set; }
        public double PrecioUsd { get; set; }
        public double SubTotal { get; set; }
        public double IgvCentaje { get; set; }
        public double Igv { get; set; }
        public double TotalFacturar { get; set; }
        public List<DatosProcesamientoLoteiVDto>? Valores { get; set; }

        [JsonIgnore]
        public long? IdUsuario { get; set; }
    }
}
