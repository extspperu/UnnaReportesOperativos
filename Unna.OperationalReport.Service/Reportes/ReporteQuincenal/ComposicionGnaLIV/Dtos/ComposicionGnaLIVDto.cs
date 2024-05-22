using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Unna.OperationalReport.Service.Reportes.Generales.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.FacturacionGnsLIV.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ComposicionGnaLIV_2.Dtos;

namespace Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ComposicionGnaLIV.Dtos
{
    public class ComposicionGnaLIVDto
    {
        public string? Fecha { get; set; }
        public double? TotalPromedioPeruPetroC6 { get; set; }
        public double? TotalPromedioPeruPetroC3 { get; set; }
        public double? TotalPromedioPeruPetroIc4 { get; set; }
        public double? TotalPromedioPeruPetroNc4 { get; set; }
        public double? TotalPromedioPeruPetroNeoC5 { get; set; }
        public double? TotalPromedioPeruPetroIc5 { get; set; }
        public double? TotalPromedioPeruPetroNc5 { get; set; }
        public double? TotalPromedioPeruPetroNitrog { get; set; }
        public double? TotalPromedioPeruPetroC1 { get; set; }
        public double? TotalPromedioPeruPetroCo2 { get; set; }
        public double? TotalPromedioPeruPetroC2 { get; set; }
        public double? TotalPromedioPeruPetroVol { get; set; }
        public double? TotalPromedioUnnaC6 { get; set; }
        public double? TotalPromedioUnnaC3 { get; set; }
        public double? TotalPromedioUnnaIc4 { get; set; }
        public double? TotalPromedioUnnaNc4 { get; set; }
        public double? TotalPromedioUnnaNeoC5 { get; set; }
        public double? TotalPromedioUnnaIc5 { get; set; }
        public double? TotalPromedioUnnaNc5 { get; set; }
        public double? TotalPromedioUnnaNitrog { get; set; }
        public double? TotalPromedioUnnaC1 { get; set; }
        public double? TotalPromedioUnnaCo2 { get; set; }
        public double? TotalPromedioUnnaC2 { get; set; }
        public double? TotalPromedioUnnaVol { get; set; }
        public double? TotalDifC6 { get; set; }
        public double? TotalDifC3 { get; set; }
        public double? TotalDifIc4 { get; set; }
        public double? TotalDifNc4 { get; set; }
        public double? TotalDifNeoC5 { get; set; }
        public double? TotalDifIc5 { get; set; }
        public double? TotalDifNc5 { get; set; }
        public double? TotalDifNitrog { get; set; }
        public double? TotalDifC1 { get; set; }
        public double? TotalDifCo2 { get; set; }
        public double? TotalDifC2 { get; set; }
        public double? TotalDifVol { get; set; }
        public List<ComposicionGnaLIVDetComposicionDto>? ComposicionGnaLIVDetComposicion { get; set; }
        public List<ComposicionGnaLIVDetComponenteDto>? ComposicionGnaLIVDetComponente { get; set; }


        [JsonIgnore]
        public long? IdUsuario { get; set; }

        public ReporteDto? General { get; set; }

    }
}
