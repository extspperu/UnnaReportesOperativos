using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletaValorizacionProcesamientoGNALoteIV.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletaValorizacionProcesamientoGNALoteIV.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;

namespace Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletaValorizacionProcesamientoGNALoteIV.Servicios.Implementaciones
{
    public class BoletaValorizacionProcesamientoGNALoteIVServicio : IBoletaValorizacionProcesamientoGNALoteIVServicio
    {
        
        public async Task<OperacionDto<BoletaValorizacionProcesamientoGNALoteIVDto>> ObtenerAsync(long idUsuario)
        {
            var dto = new BoletaValorizacionProcesamientoGNALoteIVDto
            {
                Año = "2023",//FechasUtilitario.ObtenerDiaOperativo().ToString("dd-MMMM-yyyy").Substring(3),
                Mes = "Noviembre",
                TotalVolumenMPC = 122540.57,
                TotalPCBTUPC = 1154.78,
                TotalEnergiaMMBTU = 141507.7155,

                TotalEnergiaVolProcesadoMMBTU = 141507.7155,

                PrecioUSDMMBTU = 1.2000,
                SubTotalAFacturarUSD = 169809.2586,
                IGV18Porcentaje = 30565.6665,
                TotalAFacturarUSD = 200374.9251



            };
            dto.BoletaValorizacionProcesamientoGNALoteIVDet = await BoletaValorizacionProcesamientoGNALoteIVDet();

            return new OperacionDto<BoletaValorizacionProcesamientoGNALoteIVDto>(dto);
        }
        private async Task<List<BoletaValorizacionProcesamientoGNALoteIVDetDto>> BoletaValorizacionProcesamientoGNALoteIVDet()
        {
            List<BoletaValorizacionProcesamientoGNALoteIVDetDto> BoletaValorizacionProcesamientoGNALoteIVDet = new List<BoletaValorizacionProcesamientoGNALoteIVDetDto>();
            BoletaValorizacionProcesamientoGNALoteIVDet.Add(new BoletaValorizacionProcesamientoGNALoteIVDetDto
            {
                Dia = "01-nov.-23",
                VolumneMPC = 4193.708,
                PCBTUPC = 1152.93,
                EnergiaMMBTU = 4835.0518
                //(VolumneMPC * PCBTUPC)/1000



            });
            BoletaValorizacionProcesamientoGNALoteIVDet.Add(new BoletaValorizacionProcesamientoGNALoteIVDetDto
            {
                Dia = "02-nov.-23",
                VolumneMPC = 4193.708,
                PCBTUPC = 1152.93,
                EnergiaMMBTU = 4835.0518
                //(VolumneMPC * PCBTUPC)/1000



            });
            BoletaValorizacionProcesamientoGNALoteIVDet.Add(new BoletaValorizacionProcesamientoGNALoteIVDetDto
            {
                Dia = "03-nov.-23",
                VolumneMPC = 4193.708,
                PCBTUPC = 1152.93,
                EnergiaMMBTU = 4835.0518
                //(VolumneMPC * PCBTUPC)/1000



            });
            BoletaValorizacionProcesamientoGNALoteIVDet.Add(new BoletaValorizacionProcesamientoGNALoteIVDetDto
            {
                Dia = "04-nov.-23",
                VolumneMPC = 4193.708,
                PCBTUPC = 1152.93,
                EnergiaMMBTU = 4835.0518
                //(VolumneMPC * PCBTUPC)/1000



            });
            BoletaValorizacionProcesamientoGNALoteIVDet.Add(new BoletaValorizacionProcesamientoGNALoteIVDetDto
            {
                Dia = "05-nov.-23",
                VolumneMPC = 4193.708,
                PCBTUPC = 1152.93,
                EnergiaMMBTU = 4835.0518
                //(VolumneMPC * PCBTUPC)/1000



            });
            BoletaValorizacionProcesamientoGNALoteIVDet.Add(new BoletaValorizacionProcesamientoGNALoteIVDetDto
            {
                Dia = "06-nov.-23",
                VolumneMPC = 4193.708,
                PCBTUPC = 1152.93,
                EnergiaMMBTU = 4835.0518
                //(VolumneMPC * PCBTUPC)/1000



            });
            BoletaValorizacionProcesamientoGNALoteIVDet.Add(new BoletaValorizacionProcesamientoGNALoteIVDetDto
            {
                Dia = "07-nov.-23",
                VolumneMPC = 4193.708,
                PCBTUPC = 1152.93,
                EnergiaMMBTU = 4835.0518
                //(VolumneMPC * PCBTUPC)/1000



            });
            BoletaValorizacionProcesamientoGNALoteIVDet.Add(new BoletaValorizacionProcesamientoGNALoteIVDetDto
            {
                Dia = "08-nov.-23",
                VolumneMPC = 4193.708,
                PCBTUPC = 1152.93,
                EnergiaMMBTU = 4835.0518
                //(VolumneMPC * PCBTUPC)/1000



            });
            BoletaValorizacionProcesamientoGNALoteIVDet.Add(new BoletaValorizacionProcesamientoGNALoteIVDetDto
            {
                Dia = "09-nov.-23",
                VolumneMPC = 4193.708,
                PCBTUPC = 1152.93,
                EnergiaMMBTU = 4835.0518
                //(VolumneMPC * PCBTUPC)/1000



            });
            BoletaValorizacionProcesamientoGNALoteIVDet.Add(new BoletaValorizacionProcesamientoGNALoteIVDetDto
            {
                Dia = "10-nov.-23",
                VolumneMPC = 4193.708,
                PCBTUPC = 1152.93,
                EnergiaMMBTU = 4835.0518
                //(VolumneMPC * PCBTUPC)/1000



            });
            BoletaValorizacionProcesamientoGNALoteIVDet.Add(new BoletaValorizacionProcesamientoGNALoteIVDetDto
            {
                Dia = "11-nov.-23",
                VolumneMPC = 4193.708,
                PCBTUPC = 1152.93,
                EnergiaMMBTU = 4835.0518
                //(VolumneMPC * PCBTUPC)/1000



            });
            BoletaValorizacionProcesamientoGNALoteIVDet.Add(new BoletaValorizacionProcesamientoGNALoteIVDetDto
            {
                Dia = "12-nov.-23",
                VolumneMPC = 4193.708,
                PCBTUPC = 1152.93,
                EnergiaMMBTU = 4835.0518
                //(VolumneMPC * PCBTUPC)/1000



            });
            BoletaValorizacionProcesamientoGNALoteIVDet.Add(new BoletaValorizacionProcesamientoGNALoteIVDetDto
            {
                Dia = "13-nov.-23",
                VolumneMPC = 4193.708,
                PCBTUPC = 1152.93,
                EnergiaMMBTU = 4835.0518
                //(VolumneMPC * PCBTUPC)/1000



            });
            BoletaValorizacionProcesamientoGNALoteIVDet.Add(new BoletaValorizacionProcesamientoGNALoteIVDetDto
            {
                Dia = "14-nov.-23",
                VolumneMPC = 4193.708,
                PCBTUPC = 1152.93,
                EnergiaMMBTU = 4835.0518
                //(VolumneMPC * PCBTUPC)/1000



            });
            BoletaValorizacionProcesamientoGNALoteIVDet.Add(new BoletaValorizacionProcesamientoGNALoteIVDetDto
            {
                Dia = "15-nov.-23",
                VolumneMPC = 4193.708,
                PCBTUPC = 1152.93,
                EnergiaMMBTU = 4835.0518
                //(VolumneMPC * PCBTUPC)/1000



            });
            BoletaValorizacionProcesamientoGNALoteIVDet.Add(new BoletaValorizacionProcesamientoGNALoteIVDetDto
            {
                Dia = "16-nov.-23",
                VolumneMPC = 4193.708,
                PCBTUPC = 1152.93,
                EnergiaMMBTU = 4835.0518
                //(VolumneMPC * PCBTUPC)/1000



            });
            BoletaValorizacionProcesamientoGNALoteIVDet.Add(new BoletaValorizacionProcesamientoGNALoteIVDetDto
            {
                Dia = "17-nov.-23",
                VolumneMPC = 4193.708,
                PCBTUPC = 1152.93,
                EnergiaMMBTU = 4835.0518
                //(VolumneMPC * PCBTUPC)/1000



            });
            BoletaValorizacionProcesamientoGNALoteIVDet.Add(new BoletaValorizacionProcesamientoGNALoteIVDetDto
            {
                Dia = "18-nov.-23",
                VolumneMPC = 4193.708,
                PCBTUPC = 1152.93,
                EnergiaMMBTU = 4835.0518
                //(VolumneMPC * PCBTUPC)/1000



            });
            BoletaValorizacionProcesamientoGNALoteIVDet.Add(new BoletaValorizacionProcesamientoGNALoteIVDetDto
            {
                Dia = "19-nov.-23",
                VolumneMPC = 4193.708,
                PCBTUPC = 1152.93,
                EnergiaMMBTU = 4835.0518
                //(VolumneMPC * PCBTUPC)/1000



            });
            BoletaValorizacionProcesamientoGNALoteIVDet.Add(new BoletaValorizacionProcesamientoGNALoteIVDetDto
            {
                Dia = "20-nov.-23",
                VolumneMPC = 4193.708,
                PCBTUPC = 1152.93,
                EnergiaMMBTU = 4835.0518
                //(VolumneMPC * PCBTUPC)/1000



            });
            BoletaValorizacionProcesamientoGNALoteIVDet.Add(new BoletaValorizacionProcesamientoGNALoteIVDetDto
            {
                Dia = "21-nov.-23",
                VolumneMPC = 4193.708,
                PCBTUPC = 1152.93,
                EnergiaMMBTU = 4835.0518
                //(VolumneMPC * PCBTUPC)/1000



            });
            BoletaValorizacionProcesamientoGNALoteIVDet.Add(new BoletaValorizacionProcesamientoGNALoteIVDetDto
            {
                Dia = "22-nov.-23",
                VolumneMPC = 4193.708,
                PCBTUPC = 1152.93,
                EnergiaMMBTU = 4835.0518
                //(VolumneMPC * PCBTUPC)/1000



            });
            BoletaValorizacionProcesamientoGNALoteIVDet.Add(new BoletaValorizacionProcesamientoGNALoteIVDetDto
            {
                Dia = "23-nov.-23",
                VolumneMPC = 4193.708,
                PCBTUPC = 1152.93,
                EnergiaMMBTU = 4835.0518
                //(VolumneMPC * PCBTUPC)/1000



            });
            BoletaValorizacionProcesamientoGNALoteIVDet.Add(new BoletaValorizacionProcesamientoGNALoteIVDetDto
            {
                Dia = "24-nov.-23",
                VolumneMPC = 4193.708,
                PCBTUPC = 1152.93,
                EnergiaMMBTU = 4835.0518
                //(VolumneMPC * PCBTUPC)/1000



            });
            BoletaValorizacionProcesamientoGNALoteIVDet.Add(new BoletaValorizacionProcesamientoGNALoteIVDetDto
            {
                Dia = "25-nov.-23",
                VolumneMPC = 4193.708,
                PCBTUPC = 1152.93,
                EnergiaMMBTU = 4835.0518
                //(VolumneMPC * PCBTUPC)/1000



            });
            BoletaValorizacionProcesamientoGNALoteIVDet.Add(new BoletaValorizacionProcesamientoGNALoteIVDetDto
            {
                Dia = "26-nov.-23",
                VolumneMPC = 4193.708,
                PCBTUPC = 1152.93,
                EnergiaMMBTU = 4835.0518
                //(VolumneMPC * PCBTUPC)/1000



            });
            BoletaValorizacionProcesamientoGNALoteIVDet.Add(new BoletaValorizacionProcesamientoGNALoteIVDetDto
            {
                Dia = "27-nov.-23",
                VolumneMPC = 4193.708,
                PCBTUPC = 1152.93,
                EnergiaMMBTU = 4835.0518
                //(VolumneMPC * PCBTUPC)/1000



            });
            BoletaValorizacionProcesamientoGNALoteIVDet.Add(new BoletaValorizacionProcesamientoGNALoteIVDetDto
            {
                Dia = "28-nov.-23",
                VolumneMPC = 4193.708,
                PCBTUPC = 1152.93,
                EnergiaMMBTU = 4835.0518
                //(VolumneMPC * PCBTUPC)/1000



            });
            BoletaValorizacionProcesamientoGNALoteIVDet.Add(new BoletaValorizacionProcesamientoGNALoteIVDetDto
            {
                Dia = "29-nov.-23",
                VolumneMPC = 4193.708,
                PCBTUPC = 1152.93,
                EnergiaMMBTU = 4835.0518
                //(VolumneMPC * PCBTUPC)/1000



            });
            BoletaValorizacionProcesamientoGNALoteIVDet.Add(new BoletaValorizacionProcesamientoGNALoteIVDetDto
            {
                Dia = "30-nov.-23",
                VolumneMPC = 4193.708,
                PCBTUPC = 1152.93,
                EnergiaMMBTU = 4835.0518
                //(VolumneMPC * PCBTUPC)/1000



            });
            return BoletaValorizacionProcesamientoGNALoteIVDet;
        }
    }
    //BoletaValorizacionProcesamientoGNALoteIV

    
}
