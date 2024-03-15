using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Registro.Entidades;
using Unna.OperationalReport.Service.Reportes.Generales.Dtos;

namespace Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaCnpc.Dtos
{
    public class BoletaCnpcDto
    {
        public ReporteDto? General { get; set; }

        public string? Fecha { get; set; }
        public BoletaCnpcTabla1Dto? Tabla1 { get; set; }
        public double? VolumenTotalGnsEnMs { get; set; }
        public double? VolumenTotalGns { get; set; }
        public double? FlareGna { get; set; }
        public List<FactoresDistribucionGasNaturalDto>? FactoresDistribucionGasNaturalSeco { get; set; }

        // Cuadro N° 2. Asignación de Gas Combustible al GNA Adicional del Lote X
        public double? VolumenTotalGasCombustible { get; set; }
        public List<FactoresDistribucionGasNaturalDto>? FactoresDistribucionGasDeCombustible { get; set; }

        public double? VolumenProduccionTotalGlp { get; set; }
        public double? VolumenProduccionTotalCgn { get; set; }
        public double? VolumenProduccionTotalLgn { get; set; }
        public List<FactoresDistribucionLiquidoGasNaturalDto>? FactoresDistribucionLiquidoGasNatural { get; set; }

        public double? GravedadEspecifica { get; set; }
        public double? VolumenProduccionTotalGlpCnpc { get; set; }
        public double? VolumenProduccionTotalCgnCnpc { get; set; }

        [JsonIgnore]
        public long? IdUsuario { get; set; }
    }
}
