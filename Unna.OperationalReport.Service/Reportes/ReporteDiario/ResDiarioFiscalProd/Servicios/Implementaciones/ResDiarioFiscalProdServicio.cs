using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.ReporteDiarioPgt.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.ResDiarioFiscalProd.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.ResDiarioFiscalProd.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;

namespace Unna.OperationalReport.Service.Reportes.ReporteDiario.ResDiarioFiscalProd.Servicios.Implementaciones
{
    public class ResDiarioFiscalProdServicio : IResDiarioFiscalProdServicio
    {
        public async Task<OperacionDto<ResDiarioFiscalProdDto>> ObtenerAsync(long idUsuario)
        {
            var dto = new ResDiarioFiscalProdDto
            {
                Fecha = "27/12/23",
                Observacion = "Los valores reportados son la producción total (Bls) de productos terminados de GLP/CGN, el volumen correspondiente a ENEL Generación Piura S.A. se cuantifica en el documento."
            };

            dto.ResDiarioFiscalProdDet = await ResDiarioFiscalProdDet();

            return new OperacionDto<ResDiarioFiscalProdDto>(dto);
        }

        private async Task<List<ResDiarioFiscalProdDetDto>> ResDiarioFiscalProdDet()
        {

            List<ResDiarioFiscalProdDetDto> ResDiarioFiscalProdDet = new List<ResDiarioFiscalProdDetDto>();

            ResDiarioFiscalProdDet.Add(new ResDiarioFiscalProdDetDto
            {
                Producto = "PRODUCTO PARA REPROCESO",
                Tanques = "TK - 4601",
                Niveles = 0,
                Inventario = 0
            }
            );

            ResDiarioFiscalProdDet.Add(new ResDiarioFiscalProdDetDto
            {
                Producto = "PRODUCTO PARA REPROCESO",
                Tanques = "TK-4605",
                Niveles = 0,
                Inventario = 0
            }
            );

            ResDiarioFiscalProdDet.Add(new ResDiarioFiscalProdDetDto
            {
                Producto = "GLP",
                Tanques = "4605",
                Niveles = 0,
                Inventario = 0
            }
            );

            ResDiarioFiscalProdDet.Add(new ResDiarioFiscalProdDetDto
            {
                Producto = "GLP",
                Tanques = "4606",
                Niveles = 0,
                Inventario = 0
            }
            );

            ResDiarioFiscalProdDet.Add(new ResDiarioFiscalProdDetDto
            {
                Producto = "GLP",
                Tanques = "4607",
                Niveles = 0,
                Inventario = 0
            }
            );

            ResDiarioFiscalProdDet.Add(new ResDiarioFiscalProdDetDto
            {
                Producto = "GLP",
                Tanques = "4608",
                Niveles = 23.65,
                Inventario = 1152.35
            }
            );


            ResDiarioFiscalProdDet.Add(new ResDiarioFiscalProdDetDto
            {
                Producto = "GLP",
                Tanques = "4609",
                Niveles = 19.0,
                Inventario = 245.25
            }
            );

            ResDiarioFiscalProdDet.Add(new ResDiarioFiscalProdDetDto
            {
                Producto = "GLP",
                Tanques = "4610",
                Niveles = 69.58,
                Inventario = 253.68
            }
            );

            ResDiarioFiscalProdDet.Add(new ResDiarioFiscalProdDetDto
            {
                Producto = "GLP",
                Tanques = "4611",
                Niveles = 69.58,
                Inventario = 253.68
            }
            );

            ResDiarioFiscalProdDet.Add(new ResDiarioFiscalProdDetDto
            {
                Producto = "GLP",
                Tanques = "46120",
                Niveles = 69.58,
                Inventario = 253.68
            }
            );

            ResDiarioFiscalProdDet.Add(new ResDiarioFiscalProdDetDto
            {
                Producto = "GLP",
                Tanques = "4613",
                Niveles = 69.58,
                Inventario = 253.68
            }
            );

            ResDiarioFiscalProdDet.Add(new ResDiarioFiscalProdDetDto
            {
                Producto = "GLP",
                Tanques = "4614",
                Niveles = 69.58,
                Inventario = 253.68
            }
            );

            ResDiarioFiscalProdDet.Add(new ResDiarioFiscalProdDetDto
            {
                Producto = "CGN",
                Tanques = "4605 B",
                Niveles = 7.2,
                Inventario = 48.4
            }
            );

            ResDiarioFiscalProdDet.Add(new ResDiarioFiscalProdDetDto
            {
                Producto = "CGN",
                Tanques = "4606 B",
                Niveles = 7.2,
                Inventario = 48.4
            }
            );

            ResDiarioFiscalProdDet.Add(new ResDiarioFiscalProdDetDto
            {
                Producto = "CGN",
                Tanques = "4607 B",
                Niveles = 7.2,
                Inventario = 48.4
            }
            );

            ResDiarioFiscalProdDet.Add(new ResDiarioFiscalProdDetDto
            {
                Producto = "GLP",
                Tanques = "726.42",
                Niveles = 1054.6,
                Inventario = 2602.97
            }
            );

            ResDiarioFiscalProdDet.Add(new ResDiarioFiscalProdDetDto
            {
                Producto = "CGN",
                Tanques = "726.42",
                Niveles = 1054.6,
                Inventario = 2602.97
            }
            );

            return ResDiarioFiscalProdDet;
        }

    }
}
