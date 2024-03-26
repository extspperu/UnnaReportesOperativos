using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ValorizacionVtaGns.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ValorizacionVtaGns_2.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ValorizacionVtaGns_2.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;

namespace Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ValorizacionVtaGns_2.Servicios.Implementaciones
{
    public class ValorizacionVtaGns_2Servicio : IValorizacionVtaGns_2Servicio
    {
        public async Task<OperacionDto<ValorizacionVtaGns_2Dto>> ObtenerAsync(long idUsuario)
        {
            var dto = new ValorizacionVtaGns_2Dto
            {
                Periodo = "Del 1 al 15 de NOVIEMBRE 2023",
                PuntoFiscal = "MS-9225",
                TotalVolumen = 38945.68,
                TotalPoderCal = 1068.456,
                TotalEnergia = 41447.9646,
                PromPrecio = 3.32,
                TotalCosto = 137607.242472,
                EnerVolTransM = 41447.9646,
                SubTotalFact = 137607.242472,
                Igv = 24769.30364496,
                TotalFact = 162376.54611696,
                Comentario = "factura pagada"
            };

            dto.ValorizacionVtaGns_2Det = await ValorizacionVtaGns_2Det();

            return new OperacionDto<ValorizacionVtaGns_2Dto>(dto);
        }

        private async Task<List<ValorizacionVtaGns_2DetDto>?> ValorizacionVtaGns_2Det()
        {
            List<ValorizacionVtaGns_2DetDto> ValorizacionVtaGns_2Det = new List<ValorizacionVtaGns_2DetDto>();

            ValorizacionVtaGns_2Det.Add(new ValorizacionVtaGns_2DetDto
            {
                Fecha = "16/11/2023",
                Volumen = 3311.15,
                PoderCal = 1055.75,
                Energia = 3495.7466,
                Precio = 3.32,
                Costo = 11605.878712
            }
           );
            ValorizacionVtaGns_2Det.Add(new ValorizacionVtaGns_2DetDto
            {
                Fecha = "17/11/2023",
                Volumen = 3002.77,
                PoderCal = 1055.15,
                Energia = 3168.3728,
                Precio = 3.32,
                Costo = 10518.997696
            }
            );
            ValorizacionVtaGns_2Det.Add(new ValorizacionVtaGns_2DetDto
            {
                Fecha = "18/11/2023",
                Volumen = 2994.11,
                PoderCal = 1055.97,
                Energia = 3161.6903,
                Precio = 3.32,
                Costo = 10496.811796
            }
            );
            ValorizacionVtaGns_2Det.Add(new ValorizacionVtaGns_2DetDto
            {
                Fecha = "19/11/2023",
                Volumen = 2920.57,
                PoderCal = 1055.87,
                Energia = 3083.7422,
                Precio = 3.32,
                Costo = 10238.024104
            }
            );
            ValorizacionVtaGns_2Det.Add(new ValorizacionVtaGns_2DetDto
            {
                Fecha = "20/11/2023",
                Volumen = 3000.7,
                PoderCal = 1056.26,
                Energia = 3169.5194,
                Precio = 3.32,
                Costo = 10522.804408
            }
            );
            ValorizacionVtaGns_2Det.Add(new ValorizacionVtaGns_2DetDto
            {
                Fecha = "21/11/2023",
                Volumen = 3008.36,
                PoderCal = 1056.09,
                Energia = 3177.0989,
                Precio = 3.32,
                Costo = 10547.968348
            }
            );
            ValorizacionVtaGns_2Det.Add(new ValorizacionVtaGns_2DetDto
            {
                Fecha = "22/11/2023",
                Volumen = 2997.07,
                PoderCal = 1056.38,
                Energia = 3166.0448,
                Precio = 3.32,
                Costo = 10511.268736
            }
           );
            ValorizacionVtaGns_2Det.Add(new ValorizacionVtaGns_2DetDto
            {
                Fecha = "23/11/2023",
                Volumen = 3015.29,
                PoderCal = 1056.07,
                Energia = 3184.3573,
                Precio = 3.32,
                Costo = 10572.066236
            }
           );
            ValorizacionVtaGns_2Det.Add(new ValorizacionVtaGns_2DetDto
            {
                Fecha = "24/11/2023",
                Volumen = 3063.71,
                PoderCal = 1056.39,
                Energia = 3236.4726,
                Precio = 3.32,
                Costo = 10745.089032
            }
           );
            ValorizacionVtaGns_2Det.Add(new ValorizacionVtaGns_2DetDto
            {
                Fecha = "25/11/2023",
                Volumen = 2350.09,
                PoderCal = 1055.92,
                Energia = 2481.507,
                Precio = 3.32,
                Costo = 8238.60324
            }
           );
            ValorizacionVtaGns_2Det.Add(new ValorizacionVtaGns_2DetDto
            {
                Fecha = "26/11/2023",
                Volumen = 2021.63,
                PoderCal = 1056.22,
                Energia = 2135.286,
                Precio = 3.32,
                Costo = 7089.14952
            }
           );
            ValorizacionVtaGns_2Det.Add(new ValorizacionVtaGns_2DetDto
            {
                Fecha = "27/11/2023",
                Volumen = 2007.09,
                PoderCal = 1056.44,
                Energia = 2120.3702,
                Precio = 3.32,
                Costo = 7039.629064
            }
           );
            ValorizacionVtaGns_2Det.Add(new ValorizacionVtaGns_2DetDto
            {
                Fecha = "28/11/2023",
                Volumen = 2020.93,
                PoderCal = 1092.9,
                Energia = 2208.6744,
                Precio = 3.32,
                Costo = 7332.799008
            }
           );
            ValorizacionVtaGns_2Det.Add(new ValorizacionVtaGns_2DetDto
            {
                Fecha = "29/11/2023",
                Volumen = 2187.85,
                PoderCal = 1134.54,
                Energia = 2482.2033,
                Precio = 3.32,
                Costo = 8240.914956
            }
           );
            ValorizacionVtaGns_2Det.Add(new ValorizacionVtaGns_2DetDto
            {
                Fecha = "30/11/2023",
                Volumen = 1044.36,
                PoderCal = 1126.89,
                Energia = 1176.8788,
                Precio = 3.32,
                Costo = 3907.237616
            }
           );

            return ValorizacionVtaGns_2Det;
        }
    }
}
