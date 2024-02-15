using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Registro.Repositorios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaVentaGns.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaVentaGns.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;

namespace Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaVentaGns.Servicios.Implementaciones
{
    public class BoletaVentaGnsServicio: IBoletaVentaGnsServicio
    {

        private readonly IDiaOperativoRepositorio _diaOperativoRepositorio;
        public BoletaVentaGnsServicio(IDiaOperativoRepositorio diaOperativoRepositorio)
        {
            _diaOperativoRepositorio = diaOperativoRepositorio;
        }

        public async Task<OperacionDto<BoletaVentaGnsDto>> ObtenerAsync(long idUsuario)
        {
            //var usuario = await _diaOperativoRepositorio.ObtenerPorIdLoteYFechaAsync(idUsuario);
            //if (usuario == null)
            //{
            //    return new OperacionDto<BoletaVentaGnsDto>(CodigosOperacionDto.Invalido, "No tiene permiso a ningún lote");
            //}
            var dto = new BoletaVentaGnsDto();
            return new OperacionDto<BoletaVentaGnsDto>(dto);
        }

    }
}
