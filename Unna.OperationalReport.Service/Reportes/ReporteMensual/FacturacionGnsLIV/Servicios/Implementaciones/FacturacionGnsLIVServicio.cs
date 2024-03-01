using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Fuentes.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Fuentes.Repositorios.Implementaciones;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaCnpc.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaVentaGns.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.FacturacionGnsLIV.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.FacturacionGnsLIV.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;

namespace Unna.OperationalReport.Service.Reportes.ReporteMensual.FacturacionGnsLIV.Servicios.Implementaciones
{
    public class FacturacionGnsLIVServicio: IFacturacionGnsLIVServicio
    {

        public async Task<OperacionDto<FacturacionGnsLIVDto>>ObtenerAsync(long idUsuario)
        {
            var dto = new FacturacionGnsLIVDto
            {
                MesAnio = "NOVIEMBRE 2023",
                Periodo = "PERIODO DEL 01 AL 30 DE OCTUBRE DEL 2023",
            };
            
            dto.FacturacionGnsLIVDet = await FacturacionGnsLIVDet();

            return new OperacionDto<FacturacionGnsLIVDto>(dto);
        }

        private async Task<List<FacturacionGnsLIVDetDto>> FacturacionGnsLIVDet()
        {

            List<FacturacionGnsLIVDetDto> FacturacionGnsLIVDet = new List<FacturacionGnsLIVDetDto>();

            FacturacionGnsLIVDet.Add(new FacturacionGnsLIVDetDto
            {
                Concepto = "concepto",
                Mpc = 0,
                Mmbtu = 0,
                PrecioUs = 27700,
                ImporteUs = 0,
                
            });

            FacturacionGnsLIVDet.ForEach(e => e.TotalUs = e.PrecioUs * e.ImporteUs);
      
            return FacturacionGnsLIVDet;
        }
    }

}