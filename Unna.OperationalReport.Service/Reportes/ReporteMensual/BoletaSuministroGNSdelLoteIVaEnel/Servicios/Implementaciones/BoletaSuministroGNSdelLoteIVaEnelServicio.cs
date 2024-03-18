using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletaSuministroGNSdelLoteIVaEnel.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletaSuministroGNSdelLoteIVaEnel.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;

namespace Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletaSuministroGNSdelLoteIVaEnel.Servicios.Implementaciones
{
    public class BoletaSuministroGNSdelLoteIVaEnelServicio : IBoletaSuministroGNSdelLoteIVaEnelServicio
    {
        public async Task<OperacionDto<BoletaSuministroGNSdelLoteIVaEnelDto>> ObtenerAsync(long idUsuario)
        {
            var dto = new BoletaSuministroGNSdelLoteIVaEnelDto
            {
                Periodo = "Noviembre-2023",//FechasUtilitario.ObtenerDiaOperativo().ToString("dd-MMMM-yyyy").Substring(3),
                TotalVolumenMPC = 0.00,
                TotalPCBTUPC = 1063.25,
                TotalEnergiaMMBTU = 0.00,

                TotalEnergiaVolTransferidoMMBTU = 0.00,

                Comentarios = "",

                
            };
            dto.BoletaSuministroGNSdelLoteIVaEnelDet = await BoletaSuministroGNSdelLoteIVaEnelDet();

            return new OperacionDto<BoletaSuministroGNSdelLoteIVaEnelDto>(dto);
        }

        private async Task<List<BoletaSuministroGNSdelLoteIVaEnelDetDto>> BoletaSuministroGNSdelLoteIVaEnelDet()
        {
            List<BoletaSuministroGNSdelLoteIVaEnelDetDto> BoletaSuministroGNSdelLoteIVaEnelDet = new List<BoletaSuministroGNSdelLoteIVaEnelDetDto>();
            BoletaSuministroGNSdelLoteIVaEnelDet.Add(new BoletaSuministroGNSdelLoteIVaEnelDetDto
            {
                Fecha = "01-nov.-23",
                VolumneMPC = 0.00,
                PCBTUPC = 1054.94,
                EnergiaMMBTU = 0.00//(VolumneMPC * PCBTUPC)/1000



            });

            BoletaSuministroGNSdelLoteIVaEnelDet.Add(new BoletaSuministroGNSdelLoteIVaEnelDetDto
            {
                Fecha = "02-nov.-23",
                VolumneMPC = 0.00,
                PCBTUPC = 1055.15,
                EnergiaMMBTU = 0.00//(VolumneMPC * PCBTUPC)/1000



            });

            BoletaSuministroGNSdelLoteIVaEnelDet.Add(new BoletaSuministroGNSdelLoteIVaEnelDetDto
            {
                Fecha = "03-nov.-23",
                VolumneMPC = 0.00,
                PCBTUPC = 1055.97,
                EnergiaMMBTU = 0.00//(VolumneMPC * PCBTUPC)/1000



            });

            BoletaSuministroGNSdelLoteIVaEnelDet.Add(new BoletaSuministroGNSdelLoteIVaEnelDetDto
            {
                Fecha = "04-nov.-23",
                VolumneMPC = 0.00,
                PCBTUPC = 1056.58,
                EnergiaMMBTU = 0.00//(VolumneMPC * PCBTUPC)/1000



            });

            BoletaSuministroGNSdelLoteIVaEnelDet.Add(new BoletaSuministroGNSdelLoteIVaEnelDetDto
            {
                Fecha = "05-nov.-23",
                VolumneMPC = 0.00,
                PCBTUPC = 1056.83,
                EnergiaMMBTU = 0.00//(VolumneMPC * PCBTUPC)/1000



            });

            BoletaSuministroGNSdelLoteIVaEnelDet.Add(new BoletaSuministroGNSdelLoteIVaEnelDetDto
            {
                Fecha = "06-nov.-23",
                VolumneMPC = 0.00,
                PCBTUPC = 1056.65,
                EnergiaMMBTU = 0.00//(VolumneMPC * PCBTUPC)/1000



            });
            BoletaSuministroGNSdelLoteIVaEnelDet.Add(new BoletaSuministroGNSdelLoteIVaEnelDetDto
            {
                Fecha = "07-nov.-23",
                VolumneMPC = 0.00,
                PCBTUPC = 1057.15,
                EnergiaMMBTU = 0.00//(VolumneMPC * PCBTUPC)/1000



            });
            BoletaSuministroGNSdelLoteIVaEnelDet.Add(new BoletaSuministroGNSdelLoteIVaEnelDetDto
            {
                Fecha = "08-nov.-23",
                VolumneMPC = 0.00,
                PCBTUPC = 1056.69,
                EnergiaMMBTU = 0.00//(VolumneMPC * PCBTUPC)/1000



            });
            BoletaSuministroGNSdelLoteIVaEnelDet.Add(new BoletaSuministroGNSdelLoteIVaEnelDetDto
            {
                Fecha = "09-nov.-23",
                VolumneMPC = 0.00,
                PCBTUPC = 1057.00,
                EnergiaMMBTU = 0.00//(VolumneMPC * PCBTUPC)/1000



            });
            BoletaSuministroGNSdelLoteIVaEnelDet.Add(new BoletaSuministroGNSdelLoteIVaEnelDetDto
            {
                Fecha = "10-nov.-23",
                VolumneMPC = 0.00,
                PCBTUPC = 1056.59,
                EnergiaMMBTU = 0.00//(VolumneMPC * PCBTUPC)/1000



            });
            BoletaSuministroGNSdelLoteIVaEnelDet.Add(new BoletaSuministroGNSdelLoteIVaEnelDetDto
            {
                Fecha = "11-nov.-23",
                VolumneMPC = 0.00,
                PCBTUPC = 1056.74,
                EnergiaMMBTU = 0.00//(VolumneMPC * PCBTUPC)/1000



            });
            BoletaSuministroGNSdelLoteIVaEnelDet.Add(new BoletaSuministroGNSdelLoteIVaEnelDetDto
            {
                Fecha = "12-nov.-23",
                VolumneMPC = 0.00,
                PCBTUPC = 1057.15,
                EnergiaMMBTU = 0.00//(VolumneMPC * PCBTUPC)/1000



            });
            BoletaSuministroGNSdelLoteIVaEnelDet.Add(new BoletaSuministroGNSdelLoteIVaEnelDetDto
            {
                Fecha = "13-nov.-23",
                VolumneMPC = 0.00,
                PCBTUPC = 1092.9,
                EnergiaMMBTU = 0.00//(VolumneMPC * PCBTUPC)/1000



            });
            BoletaSuministroGNSdelLoteIVaEnelDet.Add(new BoletaSuministroGNSdelLoteIVaEnelDetDto
            {
                Fecha = "14-nov.-23",
                VolumneMPC = 0.00,
                PCBTUPC = 1134.53,
                EnergiaMMBTU = 0.00//(VolumneMPC * PCBTUPC)/1000



            });
            BoletaSuministroGNSdelLoteIVaEnelDet.Add(new BoletaSuministroGNSdelLoteIVaEnelDetDto
            {
                Fecha = "15-nov.-23",
                VolumneMPC = 0.00,
                PCBTUPC = 1126.89,
                EnergiaMMBTU = 0.00//(VolumneMPC * PCBTUPC)/1000



            });
            BoletaSuministroGNSdelLoteIVaEnelDet.Add(new BoletaSuministroGNSdelLoteIVaEnelDetDto
            {
                Fecha = "16-nov.-23",
                VolumneMPC = 0.00,
                PCBTUPC = 1061.76,
                EnergiaMMBTU = 0.00//(VolumneMPC * PCBTUPC)/1000



            });
            BoletaSuministroGNSdelLoteIVaEnelDet.Add(new BoletaSuministroGNSdelLoteIVaEnelDetDto
            {
                Fecha = "17-nov.-23",
                VolumneMPC = 0.00,
                PCBTUPC = 1057.59,
                EnergiaMMBTU = 0.00//(VolumneMPC * PCBTUPC)/1000



            });
            BoletaSuministroGNSdelLoteIVaEnelDet.Add(new BoletaSuministroGNSdelLoteIVaEnelDetDto
            {
                Fecha = "18-nov.-23",
                VolumneMPC = 0.00,
                PCBTUPC = 1058.42,
                EnergiaMMBTU = 0.00//(VolumneMPC * PCBTUPC)/1000



            });
            BoletaSuministroGNSdelLoteIVaEnelDet.Add(new BoletaSuministroGNSdelLoteIVaEnelDetDto
            {
                Fecha = "19-nov.-23",
                VolumneMPC = 0.00,
                PCBTUPC = 1055.04,
                EnergiaMMBTU = 0.00//(VolumneMPC * PCBTUPC)/1000



            });
            BoletaSuministroGNSdelLoteIVaEnelDet.Add(new BoletaSuministroGNSdelLoteIVaEnelDetDto
            {
                Fecha = "20-nov.-23",
                VolumneMPC = 0.00,
                PCBTUPC = 1056.12,
                EnergiaMMBTU = 0.00//(VolumneMPC * PCBTUPC)/1000



            });
            BoletaSuministroGNSdelLoteIVaEnelDet.Add(new BoletaSuministroGNSdelLoteIVaEnelDetDto
            {
                Fecha = "21-nov.-23",
                VolumneMPC = 0.00,
                PCBTUPC = 1058.03,
                EnergiaMMBTU = 0.00//(VolumneMPC * PCBTUPC)/1000



            });
            BoletaSuministroGNSdelLoteIVaEnelDet.Add(new BoletaSuministroGNSdelLoteIVaEnelDetDto
            {
                Fecha = "22-nov.-23",
                VolumneMPC = 0.00,
                PCBTUPC = 1058.51,
                EnergiaMMBTU = 0.00//(VolumneMPC * PCBTUPC)/1000



            });
            BoletaSuministroGNSdelLoteIVaEnelDet.Add(new BoletaSuministroGNSdelLoteIVaEnelDetDto
            {
                Fecha = "23-nov.-23",
                VolumneMPC = 0.00,
                PCBTUPC = 1057.29,
                EnergiaMMBTU = 0.00//(VolumneMPC * PCBTUPC)/1000



            });
            BoletaSuministroGNSdelLoteIVaEnelDet.Add(new BoletaSuministroGNSdelLoteIVaEnelDetDto
            {
                Fecha = "24-nov.-23",
                VolumneMPC = 0.00,
                PCBTUPC = 1058.31,
                EnergiaMMBTU = 0.00//(VolumneMPC * PCBTUPC)/1000



            });
            BoletaSuministroGNSdelLoteIVaEnelDet.Add(new BoletaSuministroGNSdelLoteIVaEnelDetDto
            {
                Fecha = "25-nov.-23",
                VolumneMPC = 0.00,
                PCBTUPC = 1057.53,
                EnergiaMMBTU = 0.00//(VolumneMPC * PCBTUPC)/1000



            });
            BoletaSuministroGNSdelLoteIVaEnelDet.Add(new BoletaSuministroGNSdelLoteIVaEnelDetDto
            {
                Fecha = "26-nov.-23",
                VolumneMPC = 0.00,
                PCBTUPC = 1058.2,
                EnergiaMMBTU = 0.00//(VolumneMPC * PCBTUPC)/1000



            });
            BoletaSuministroGNSdelLoteIVaEnelDet.Add(new BoletaSuministroGNSdelLoteIVaEnelDetDto
            {
                Fecha = "27-nov.-23",
                VolumneMPC = 0.00,
                PCBTUPC = 1057.59,
                EnergiaMMBTU = 0.00//(VolumneMPC * PCBTUPC)/1000



            });
            BoletaSuministroGNSdelLoteIVaEnelDet.Add(new BoletaSuministroGNSdelLoteIVaEnelDetDto
            {
                Fecha = "28-nov.-23",
                VolumneMPC = 0.00,
                PCBTUPC = 1057.01,
                EnergiaMMBTU = 0.00//(VolumneMPC * PCBTUPC)/1000



            });
            BoletaSuministroGNSdelLoteIVaEnelDet.Add(new BoletaSuministroGNSdelLoteIVaEnelDetDto
            {
                Fecha = "29-nov.-23",
                VolumneMPC = 0.00,
                PCBTUPC = 1056.92,
                EnergiaMMBTU = 0.00//(VolumneMPC * PCBTUPC)/1000



            });
            BoletaSuministroGNSdelLoteIVaEnelDet.Add(new BoletaSuministroGNSdelLoteIVaEnelDetDto
            {
                Fecha = "30-nov.-23",
                VolumneMPC = 0.00,
                PCBTUPC = 1057.56,
                EnergiaMMBTU = 0.00//(VolumneMPC * PCBTUPC)/1000



            });
            return BoletaSuministroGNSdelLoteIVaEnelDet;
        }
    }
}
