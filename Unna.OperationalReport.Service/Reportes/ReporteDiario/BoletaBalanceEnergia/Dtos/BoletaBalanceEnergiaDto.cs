using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaBalanceEnergia.Dtos
{
    public class BoletaBalanceEnergiaDto
    {
        public string? Fecha { get; set; }

        public double? VolumenCnpcPeruEntregado { get; set; }
        public double? PoderCalBrutoCnpcPeruEntregado { get; set; }
        public double? EnergiaCnpcPeruEntregado { get; set; }
        public double? RiquezaCnpcPeruEntregado { get; set; }
        public double? ComPesadoLgnRecEnel { get; set; }
        public double? ComPesadoLgnRecBlsdTotal { get; set; }
        public double? ProduccionGlpEnel { get; set; }
        public double? ProduccionGlpBlsdTotal { get; set; }
        public double? ProduccionCgnEnel { get; set; }
        public double? ProduccionCgnBlsdTotal { get; set; }

        public double? ComPesadosGna { get; set; }
        public double? PorcentajeEficiencia { get; set; }

        public double? ContenidoCalorificoPromLgn { get; set; }

        public double? PnsAMalacasVolumenDistribuido { get; set; }
        public double? PnsAMalacasPoderCalBrutoDistribuido { get; set; }
        public double? PnsAMalacasEnergiaDistribuido { get; set; }
        public double? PnsARefineriaVolumenDistribuido { get; set; }
        public double? PnsARefineriaPoderCalBrutoDistribuido { get; set; }
        public double? PnsARefineriaEnergiaDistribuido { get; set; }
        public double? AjusteBalanceGnsVolumenDistribuido { get; set; }
        public double? AjusteBalanceGnsPoderCalBrutoDistribuido { get; set; }
        public double? AjusteBalanceGnsEnergiaDistribuido { get; set; }
        public double? HumedadEnGnaVolumenDistribuido { get; set; }
        public double? HumedadEnGnaPoderCalBrutoDistribuido { get; set; }
        public double? HumedadEnGnaEnergiaDistribuido { get; set; }
        public double? GasAFlare1pns1VolumenDistribuido { get; set; }
        public double? GasAFlare1pns1PoderCalBrutoDistribuido { get; set; }
        public double? GasAFlare1pns1EnergiaDistribuido { get; set; }
        public double? TotalGsnEnelVolumenDistribuido { get; set; }
        public double? TotalGsnEnelPoderCalBrutoDistribuido { get; set; }
        public double? TotalGsnEnelEnergiaDistribuido { get; set; }

        public double? GasAHornoHotOilVolumenDistribuido { get; set; }
        public double? GasAHornoHotOilPoderCalBrutoDistribuido { get; set; }
        public double? GasAHornoHotOilEnergiaDistribuido { get; set; }
        public double? TotalConsPropVolumenDistribuido { get; set; }
        public double? TotalConsPropPoderCalBrutoDistribuido { get; set; }
        public double? TotalConsPropEnergiaDistribuido { get; set; }

        public double? GnsVendidoLoteIvVolumenDistribuido { get; set; }
        public double? GnsVendidoLoteIvPoderCalBrutoDistribuido { get; set; }
        public double? GnsVendidoLoteIvEnergiaDistribuido { get; set; }
        public double? TotalGnsVendidoVolumenDistribuido { get; set; }
        public double? TotalGnsVendidoPoderCalBrutoDistribuido { get; set; }
        public double? TotalGnsVendidoEnergiaDistribuido { get; set; }

        public double? EntregasGnaMpcsdBalance { get; set; }
        public double? EntregasGnaBarrilesLgnBalance { get; set; }
        public double? EntregasGnaEnergiaMmbtuBalance { get; set; }

        public double? GnsRestituidoAEnelMpcsdBalance { get; set; }
        public double? GnsRestituidoAEnelBarrilesLgnBalance { get; set; }
        public double? GnsRestituidoAEnelEnergiaMmbtuBalance { get; set; }

        public double? GnsConsumoPropioUnnaMpcsdBalance { get; set; }
        public double? GnsConsumoPropioUnnaBarrilesLgnBalance { get; set; }
        public double? GnsConsumoPropioUnnaEnergiaMmbtuBalance { get; set; }

        public double? RecuperacionDeComPesMpcsdBalance { get; set; }
        public double? RecuperacionDeComPesBarrilesLgnBalance { get; set; }
        public double? RecuperacionDeComPesEnergiaMmbtuBalance { get; set; }

        public double? DiferenciaEnergeticaMpcsdBalance { get; set; }
        public double? DiferenciaEnergeticaBarrilesLgnBalance { get; set; }
        public double? DiferenciaEnergeticaEnergiaMmbtuBalance { get; set; }

        public double? ExcesoEnConsumoPropioMpcsdBalance { get; set; }
        public double? ExcesoEnConsumoPropioBarrilesLgnBalance { get; set; }
        public double? ExcesoEnConsumoPropioEnergiaMmbtuBalance { get; set; }

        public string? Comentarios01 { get; set; }
        public string? Comentarios02 { get; set; }
        public string? Comentarios03 { get; set; }
        public string? Comentarios04 { get; set; }
        public string? Comentarios05 { get; set; }
        public string? Comentarios06 { get; set; }

    }
}
