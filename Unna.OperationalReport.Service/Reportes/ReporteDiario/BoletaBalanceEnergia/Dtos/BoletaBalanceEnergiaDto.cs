using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Unna.OperationalReport.Service.Reportes.Generales.Dtos;

namespace Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaBalanceEnergia.Dtos
{
    public class BoletaBalanceEnergiaDto
    {
        public ReporteDto? General { get; set; }
        public string? Fecha { get; set; }

        //GNA Entregado a UNNA ENERGIA Gas Natural
        public GnaEntregaAUnnaDto? GnaEntregaUnna { get; set; }

        // LIQUIDOS(Barriles)
        public List<LiquidosBarrilesDto>? LiquidosBarriles { get; set; }        

        //EFICIENCIA DE RECUPERACION DE LGN
        public double? ComPesadosGna { get; set; }
        public double? PorcentajeEficiencia { get; set; }
        
        //Contenido Calórico promedio del  LGN
        public double? ContenidoCalorificoPromLgn { get; set; }

        // Distribución, Volumen MPCSD, Poder Calorifico Bruto Btu/pc
        public List<DistribucionVolumenPorderCalorificoDto>? GnsAEnel { get; set; }
        public List<DistribucionVolumenPorderCalorificoDto>? ConsumoPropio { get; set; }
        public List<DistribucionVolumenPorderCalorificoDto>? ConsumoPropioGnsVendioEnel { get; set; }

        //análisis actualizado. UNNA ENERGIA no valida dicho valor propuesto para el balance de energía.
        public BalanceDto? EntregaGna { get; set; }
        public BalanceDto? GnsRestituido { get; set; }
        public BalanceDto? GnsConsumoPropio { get; set; }
        public BalanceDto? Recuperacion { get; set; }
            
        public double? DiferenciaEnergetica { get; set; }
        public double? ExesoConsumoPropio { get; set; }

        public string? Comentario { get; set; }

        [JsonIgnore]
        public long? IdUsuario { get; set; }

    }
}
