using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Registro.Entidades;
using Unna.OperationalReport.Service.Reportes.Generales.Dtos;

namespace Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaDeterminacionVolumenGna.Dtos
{
    public class BoletaDeterminacionVolumenGnaDto
    {
        public ReporteDto? General { get; set; }
        public string? Fecha { get; set; }

        //Cuadro N° 1. Asignación de Volumen de Gas Combustible (GC) - LOTE IV
        public double? VolumenTotalGasCombustible { get; set; }
        public List<FactoresAsignacionGasCombustibleDto>? FactoresAsignacionGasCombustible { get; set; }

        //Cuadro N° 2. Asignación de Volumen de Gas Natural Seco (GNS) - LOTE IV
        public double? VolumenTotalGns { get; set; }
        public List<FactoresAsignacionGasCombustibleDto>? FactorAsignacionGns { get; set; }

        // Cuadro N° 3. Asignación de Volumen de Líquidos del Gas Natural (LGN) - LOTE IV
        public double? VolumenProduccionTotalGlp { get; set; }
        public double? VolumenProduccionTotalCgn { get; set; }
        public double? VolumenProduccionTotalLgn { get; set; }
        public List<FactorAsignacionLiquidosGasNaturalDto>? FactorAsignacionLiquidosGasNatural { get; set; }
        public double? VolumenProduccionTotalGlpLoteIv { get; set; }
        public double? VolumenProduccionTotalCgnLoteIv { get; set; }
        public double? FactorCoversion { get; set; }

        //Cuadro N° 4. Volumen Fiscalizado del Gas Natural Asociado(GNA) - LOTE IV
        public DistribucionGasNaturalAsociadoDto? DistribucionGasNaturalAsociado { get; set; }
        public double? VolumenGnsVentaVgnsvTotal { get; set; }
        public double? VolumenGnsVentaVgnsvEnel { get; set; }
        public double? VolumenGnsVentaVgnsvGasnorp { get; set; }
        public double? VolumenGnsVentaVgnsvLimagas { get; set; }
        public double? VolumenGnsFlareVgnsrf { get; set; }

        public double? SumaVolumenGasCombustibleVolumen { get; set; }
        //public double? GnsEquivalenteLgn { get; set; }
        public double? VolumenGnaFiscalizado { get; set; }

        //Para Composicion del LIV
        //public List<ComposicionUnnaEnergiaPromedio>? ComposicionUnnaEnergiaPromedio_ { get; set; }
        //Para PropiedadesFisicas de la Composicion de Componentes o Suministro
        //public List<PropiedadFisicaGpsaDto>? PropiedadFisicaGpsa { get; set; }

        [JsonIgnore]
        public long? IdUsuario { get; set; }


    }
}
