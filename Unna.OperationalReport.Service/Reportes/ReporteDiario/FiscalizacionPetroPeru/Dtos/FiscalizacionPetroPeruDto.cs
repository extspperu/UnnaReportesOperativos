using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Service.Reportes.Generales.Dtos;

namespace Unna.OperationalReport.Service.Reportes.ReporteDiario.FiscalizacionPetroPeru.Dtos
{
    public class FiscalizacionPetroPeruDto
    {
        public ReporteDto? General { get; set; }
        public string? Fecha { get; set; }
        public double? VolumenTotalProduccion { get; set; }
        public double? ContenidoLgn { get; set; }
        public double? Eficiencia { get; set; }
        public List<FactorAsignacionLiquidoGasNaturalDto>? FactorAsignacionLiquidoGasNatural { get; set; }
        public double? FactorConversionZ69 { get; set; }
        public double? FactorConversionVi { get; set; }
        public double? FactorConversionI { get; set; }
        public List<DistribucionGasNaturalSecoDto>? DistribucionGasNaturalSeco { get; set; }


        public double? VolumenTotalGns { get; set; }
        public double? VolumenTotalGnsFlare { get; set; }
        public List<VolumenTransferidoRefineriaPorLoteDto>? VolumenTransferidoRefineriaPorLote { get; set; }

    }

    public class FactorAsignacionLiquidoGasNaturalDto
    {
        public int? Item { get; set; }
        public string? Suministrador { get; set; }
        public double Volumen { get; set; }
        public double Riqueza { get; set; }
        public double Contenido
        {
            get
            {
                return Volumen * Riqueza;
            }
        }
        public double Factor { get; set; }
        public double Asignacion { get; set; }


    }


    public class DistribucionGasNaturalSecoDto
    {
        public int? Item { get; set; }
        public string? Suministrador { get; set; }
        public double VolumenGna { get; set; }
        public double PoderCalorifico { get; set; }
        public double VolumenGns { get; set; }
        public double VolumenGnsd { get; set; }

    }

    public class VolumenTransferidoRefineriaPorLoteDto
    {
        public int? Item { get; set; }
        public double? Suministrador { get; set; }
        public double? VolumenGns { get; set; }
        public double? VolumenFlare { get; set; }
        public double? VolumenGnsTransferido { get; set; }

    }
}
