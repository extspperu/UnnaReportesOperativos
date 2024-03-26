using Irony.Parsing;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Unna.OperationalReport.Service.Registros.CargaSupervisorPgt.Dtos;
using Unna.OperationalReport.Service.Reportes.Generales.Dtos;

namespace Unna.OperationalReport.Service.Reportes.ReporteDiario.ReporteDiarioPgt.Dtos
{
    public class ReporteDiarioDto
    {

        public ReporteDto? General { get; set; }
        public string? Fecha { get; set; }
        public List<GasNaturalAsociadoDto>? GasNaturalAsociado { get; set; }
        public double? GasProcesado { get; set; }
        public double? GasNoProcesado { get; set; }
        public double? UtilizacionPlantaParinias { get; set; }
        public double? HoraPlantaFs { get; set; }

        //2. DISTRIBUCIÓN DE GAS NATURAL SECO TOTAL (GNS):
        public List<GasNaturalSecoDto>? GasNaturalSeco { get; set; }
        
        //3. PRODUCCIÓN Y VENTA DE LÍQUIDOS DE GAS NATURAL(LGN)
        public double? EficienciaRecuperacionLgn { get; set; }
        public List<LiquidosGasNaturalProduccionVentasDto>? LiquidosGasNaturalProduccionVentas { get; set; }
        public List<VolumenDespachoDto>? VolumenDespachoGlp { get; set; }
        public List<VolumenDespachoDto>? VolumenDespachoCgn { get; set; }
        
        //4.  VOLUMEN DE GAS Y PRODUCCIÓN DE GNA ADICIONAL DEL LOTE X(CNPC PERÚ) :
        public List<VolumenGasProduccionDto>? VolumenProduccionLoteXGnaTotalCnpc { get; set; }
        public List<VolumenGasProduccionDto>? VolumenProduccionLoteXLiquidoGasNatural { get; set; }

        //5.  VOLUMEN DE GAS Y PRODUCCIÓN DE ENEL:
        public List<VolumenGasProduccionDto>? VolumenProduccionEnel { get; set; }
        public List<VolumenGasProduccionDto>? VolumenProduccionGasNaturalEnel { get; set; }

        //6.  VOLUMEN DE GAS Y PRODUCCIÓN DE PETROPERU (LOTE I, VI y Z-69):
        public List<VolumenGasProduccionPetroperuDto>? VolumenProduccionPetroperu { get; set; }
        public List<VolumenProduccionLiquidoGasNaturalDto>? VolumenProduccionLiquidoGasNatural { get; set; }
        public double? GasAlfare { get; set; }

        
        //7.  VOLUMEN DE GAS Y PRODUCCIÓN UNNA ENERGIA LOTE IV:
        public List<VolumenGasProduccionDto>? VolumenProduccionLoteIvUnnaEnegia { get; set; }
        public List<VolumenGasProduccionDto>? VolumenProduccionLoteIvLiquidoGasNatural { get; set; }


        public string? Comentario { get; set; }

        [JsonIgnore]
        public long? IdUsuario { get; set; }


    }
}
