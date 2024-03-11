using Irony.Parsing;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Service.Registros.CargaSupervisorPgt.Dtos;
using Unna.OperationalReport.Service.Reportes.Generales.Dtos;

namespace Unna.OperationalReport.Service.Reportes.ReporteDiario.ReporteDiarioPgt.Dtos
{
    public class ReporteDiarioDto
    {

        public ReporteDto? General { get; set; }
        public string? Fecha { get; set; }
        public List<object>? GasNaturalAsociado { get; set; }
        public double? GasProcesado { get; set; }
        public double? GasNoProcesado { get; set; }
        public double? UtilizacionPlantaParinias { get; set; }
        public double? HoraPlantaFs { get; set; }

        //2. DISTRIBUCIÓN DE GAS NATURAL SECO TOTAL (GNS):
        public List<object>? GasNaturalSeco { get; set; }
        
        //3. PRODUCCIÓN Y VENTA DE LÍQUIDOS DE GAS NATURAL(LGN)
        public double? EficienciaRecuperacionLgn { get; set; }
        public List<object>? LiquidosGasNaturalProduccionVentas { get; set; }
        public List<object>? VolumenDespachoGlp { get; set; }
        public List<object>? VolumenDespachoCgn { get; set; }
        
        //4.  VOLUMEN DE GAS Y PRODUCCIÓN DE GNA ADICIONAL DEL LOTE X(CNPC PERÚ) :
        public List<object>? VolumenProduccionLoteXGnaTotalCnpc { get; set; }
        public List<object>? VolumenProduccionLoteXLiquidoGasNatural { get; set; }

        //5.  VOLUMEN DE GAS Y PRODUCCIÓN DE ENEL:
        public List<object>? VolumenProduccionEnel { get; set; }
        public List<object>? VolumenProduccionGasNaturalEnel { get; set; }

        //6.  VOLUMEN DE GAS Y PRODUCCIÓN DE PETROPERU (LOTE I, VI y Z-69):
        public List<object>? VolumenProduccionPetroperu { get; set; }
        public List<object>? VolumenProduccionLiquidoGasNatural { get; set; }

        
        //7.  VOLUMEN DE GAS Y PRODUCCIÓN UNNA ENERGIA LOTE IV:
        public List<object>? VolumenProduccionLoteIvUnnaEnegia { get; set; }
        public List<object>? VolumenProduccionLoteIvLiquidoGasNatural { get; set; }


        public string? Comentario { get; set; }


    }
}
