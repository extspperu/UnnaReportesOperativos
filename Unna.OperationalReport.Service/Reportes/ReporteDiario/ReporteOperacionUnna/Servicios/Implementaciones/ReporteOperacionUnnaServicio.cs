using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaCnpc.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.ReporteOperacionUnna.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.ReporteOperacionUnna.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;

namespace Unna.OperationalReport.Service.Reportes.ReporteDiario.ReporteOperacionUnna.Servicios.Implementaciones
{
    public class ReporteOperacionUnnaServicio: IReporteOperacionUnnaServicio
    {
        public async Task<OperacionDto<ReporteOperacionUnnaDto>> ObtenerAsync(long idUsuario)
        {
            var dto = new ReporteOperacionUnnaDto
            {
                ReporteNro = "1322024-UNNA",
                EmpresaNombre = "UNNA ENERGIA S.A",
                FechaEmision = FechasUtilitario.ObtenerDiaOperativo().ToString("dd/MM/yyyy"),
                DiaOperativo = FechasUtilitario.ObtenerDiaOperativo().ToString("dd/MM/yyyy")
            };

            ReporteOperacionUnnaPlantaSepGasNatDto PlantaSepGasNat = new ReporteOperacionUnnaPlantaSepGasNatDto();
            PlantaSepGasNat.CapacidadDisPlanta = 44;
            PlantaSepGasNat.VolumenGasNatHumedo = 25.9;
            PlantaSepGasNat.VolumenGasNatSecoReinyFlare = 1.8;
            PlantaSepGasNat.VolumenGasNatSecoVentas = 21.5;
            PlantaSepGasNat.ProcGasNatSecoTotal = PlantaSepGasNat.VolumenGasNatSecoReinyFlare + PlantaSepGasNat.VolumenGasNatSecoVentas;
            PlantaSepGasNat.VolumenLgnProducidoPlanta = 948;
            dto.PlantaSepGasNat = PlantaSepGasNat;


            ReporteOperacionUnnaPlantaFracLiqGasNatDto PlantaFracLiqGasNat = new ReporteOperacionUnnaPlantaFracLiqGasNatDto();
            PlantaFracLiqGasNat.VolumenLgnProcesado = 948;
            PlantaFracLiqGasNat.VolumenLgnProducidoCgn = 262;
            PlantaFracLiqGasNat.VolumenLgnProducidoGlp = 686;
            PlantaFracLiqGasNat.VolumenLgnProducidoTotal = PlantaFracLiqGasNat.VolumenLgnProducidoCgn + PlantaFracLiqGasNat.VolumenLgnProducidoGlp;
            PlantaFracLiqGasNat.VolumenProductosCondensadosLgn = 739;
            PlantaFracLiqGasNat.VolumenProductosGlp = 4139;
            PlantaFracLiqGasNat.VolumenProductosTotal = PlantaFracLiqGasNat.VolumenProductosCondensadosLgn + PlantaFracLiqGasNat.VolumenProductosGlp;
            PlantaFracLiqGasNat.EventosOperativos = "*Planta de Gas Pariñas:  Planta operando en condiciones normales.";
            dto.PlantaFracLiqGasNat = PlantaFracLiqGasNat;


            return new OperacionDto<ReporteOperacionUnnaDto>(dto);
        }
    }
}
