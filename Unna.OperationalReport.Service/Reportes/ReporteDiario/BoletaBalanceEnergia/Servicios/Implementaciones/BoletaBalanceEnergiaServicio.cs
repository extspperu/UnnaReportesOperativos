using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaBalanceEnergia.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaBalanceEnergia.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaCnpc.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;

namespace Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaBalanceEnergia.Servicios.Implementaciones
{
    public class BoletaBalanceEnergiaServicio: IBoletaBalanceEnergiaServicio
    {
        public async Task<OperacionDto<BoletaBalanceEnergiaDto>> ObtenerAsync(long idUsuario)
        {
            var dto = new BoletaBalanceEnergiaDto
            {
                Fecha = FechasUtilitario.ObtenerDiaOperativo().ToString("dd/MM/yyyy"),
                VolumenCnpcPeruEntregado = 13000,
                PoderCalBrutoCnpcPeruEntregado = 1133.72,
                EnergiaCnpcPeruEntregado = 14738,
                RiquezaCnpcPeruEntregado = 14670,
                ComPesadoLgnRecEnel=407.71,
                ComPesadoLgnRecBlsdTotal=999.90,
                ProduccionGlpEnel=299.31,
                ProduccionGlpBlsdTotal = 734.05,
                ProduccionCgnEnel=108.40,
                ProduccionCgnBlsdTotal = 265.85,
                ComPesadosGna=454.07,
                PorcentajeEficiencia=89.80,
                ContenidoCalorificoPromLgn=4311,
                PnsAMalacasVolumenDistribuido=7545,
                PnsAMalacasPoderCalBrutoDistribuido=1056.32,
                PnsAMalacasEnergiaDistribuido=7970,
                PnsARefineriaVolumenDistribuido=0,
                PnsARefineriaPoderCalBrutoDistribuido=1056.32,
                PnsARefineriaEnergiaDistribuido=0,
                AjusteBalanceGnsVolumenDistribuido=0,
                AjusteBalanceGnsPoderCalBrutoDistribuido=1056.32,
                AjusteBalanceGnsEnergiaDistribuido=0,
                HumedadEnGnaVolumenDistribuido=31,
                HumedadEnGnaPoderCalBrutoDistribuido=1133.72,
                HumedadEnGnaEnergiaDistribuido=35,
                GasAFlare1pns1VolumenDistribuido=11912,
                GasAFlare1pns1PoderCalBrutoDistribuido=1056.52,
                GasAFlare1pns1EnergiaDistribuido=4579,
                GasAHornoHotOilVolumenDistribuido=488,
                GasAHornoHotOilPoderCalBrutoDistribuido=1056.32,
                GasAHornoHotOilEnergiaDistribuido=515,
                GnsVendidoLoteIvVolumenDistribuido=0,
                GnsVendidoLoteIvPoderCalBrutoDistribuido=1056.32,
                GnsVendidoLoteIvEnergiaDistribuido=0,
                EntregasGnaMpcsdBalance=13000,
                EntregasGnaBarrilesLgnBalance=0,
                EntregasGnaEnergiaMmbtuBalance=14738,
                GnsRestituidoAEnelMpcsdBalance=11912,
                GnsRestituidoAEnelBarrilesLgnBalance=0,
                GnsRestituidoAEnelEnergiaMmbtuBalance=12584,
                GnsConsumoPropioUnnaMpcsdBalance=488,
                GnsConsumoPropioUnnaBarrilesLgnBalance=0,
                GnsConsumoPropioUnnaEnergiaMmbtuBalance=515,
                RecuperacionDeComPesMpcsdBalance=0,
                RecuperacionDeComPesBarrilesLgnBalance=408,
                RecuperacionDeComPesEnergiaMmbtuBalance=1758,
                DiferenciaEnergeticaMpcsdBalance=0,
                DiferenciaEnergeticaBarrilesLgnBalance=0,
                DiferenciaEnergeticaEnergiaMmbtuBalance=0,
                ExcesoEnConsumoPropioMpcsdBalance=0,
                ExcesoEnConsumoPropioBarrilesLgnBalance=0,
                ExcesoEnConsumoPropioEnergiaMmbtuBalance=0,
                Comentarios01 = "Unidad TG4: En reserva.",
                Comentarios02 = "Unidad TG5: En reserva.",
                Comentarios03 = "Unidad TG6: En operación continua por despacho COES. De 07:20 a 11:40 horas operó con 25 MW y de 11:40 a 17:40 horas salió de servicio por menor demanda a solicitud del ",
                Comentarios04 = "Resto de horas del día operativo se mantuvo con 48 MW.",
                Comentarios05 = "LOTE IV: Entrega de GNA 3415 MPCS. Valores de poder calorífico y riqueza del medidor principal.",
                Comentarios06 = "LOTE IV: Venta de GNS a EGPIURA de 0 MPCS."

            };

            dto.TotalGsnEnelVolumenDistribuido = dto.PnsAMalacasVolumenDistribuido +
                dto.PnsARefineriaVolumenDistribuido +
                dto.AjusteBalanceGnsVolumenDistribuido +
                dto.HumedadEnGnaVolumenDistribuido +
                dto.GasAFlare1pns1VolumenDistribuido;

            dto.TotalGsnEnelPoderCalBrutoDistribuido = dto.PnsAMalacasPoderCalBrutoDistribuido +
                dto.PnsARefineriaPoderCalBrutoDistribuido +
                dto.AjusteBalanceGnsPoderCalBrutoDistribuido +
                dto.HumedadEnGnaPoderCalBrutoDistribuido +
                dto.GasAFlare1pns1PoderCalBrutoDistribuido;

            dto.TotalGsnEnelEnergiaDistribuido = dto.PnsAMalacasEnergiaDistribuido +
                dto.PnsARefineriaEnergiaDistribuido +
                dto.AjusteBalanceGnsEnergiaDistribuido +
                dto.HumedadEnGnaEnergiaDistribuido +
                dto.GasAFlare1pns1EnergiaDistribuido;

            dto.TotalConsPropVolumenDistribuido = dto.GasAHornoHotOilVolumenDistribuido;
            dto.TotalConsPropPoderCalBrutoDistribuido = dto.GasAHornoHotOilPoderCalBrutoDistribuido;
            dto.TotalConsPropEnergiaDistribuido = dto.GasAHornoHotOilEnergiaDistribuido;

            dto.TotalGnsVendidoVolumenDistribuido = dto.GnsVendidoLoteIvVolumenDistribuido;
            dto.TotalGnsVendidoPoderCalBrutoDistribuido = dto.GnsVendidoLoteIvPoderCalBrutoDistribuido;
            dto.TotalGnsVendidoEnergiaDistribuido = dto.GnsVendidoLoteIvEnergiaDistribuido;


            return new OperacionDto<BoletaBalanceEnergiaDto>(dto);
        }

    }
}
