using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Auth.Enums;
using Unna.OperationalReport.Data.Registro.Enums;
using Unna.OperationalReport.Data.Registro.Repositorios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaVentaGns.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaVentaGns.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Usuarios.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;

namespace Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaVentaGns.Servicios.Implementaciones
{
    public class BoletaVentaGnsServicio: IBoletaVentaGnsServicio
    {

        private readonly IDiaOperativoRepositorio _diaOperativoRepositorio;
        private readonly IRegistroRepositorio _registroRepositorio;
        private readonly IUsuarioServicio _usuarioServicio;
        public BoletaVentaGnsServicio(
            IDiaOperativoRepositorio diaOperativoRepositorio,
            IRegistroRepositorio registroRepositorio,
            IUsuarioServicio usuarioServicio
            )
        {
            _diaOperativoRepositorio = diaOperativoRepositorio;
            _registroRepositorio = registroRepositorio;
            _usuarioServicio = usuarioServicio;
        }

        public async Task<OperacionDto<BoletaVentaGnsDto>> ObtenerAsync(long idUsuario)
        {
            var dto = new BoletaVentaGnsDto
            {
                Fecha = FechasUtilitario.ObtenerDiaOperativo().ToString("dd/MM/yyyy")
            };
            var segundoDato = await _diaOperativoRepositorio.ObtenerPorIdLoteYFechaAsync((int)TiposLote.LoteIv, FechasUtilitario.ObtenerDiaOperativo(),(int)TipoGrupos.FiscalizadorEnel,(int)TiposNumeroRegistro.SegundoRegistro);
            if (segundoDato != null)
            {
                var dato = await _registroRepositorio.ObtenerPorIdDatoYDiaOperativoAsync((int)TiposDatos.GnsTotalConsumoEnel, segundoDato.IdDiaOperativo);
                if (dato !=null)
                {
                    dto.Mmbtu = dato.Valor??0;
                }
            }
            var primerDato = await _diaOperativoRepositorio.ObtenerPorIdLoteYFechaAsync((int)TiposLote.LoteIv, FechasUtilitario.ObtenerDiaOperativo(), (int)TipoGrupos.FiscalizadorEnel, (int)TiposNumeroRegistro.PrimeroRegistro);
            if (primerDato != null)
            {
                var dato = await _registroRepositorio.ObtenerPorIdDatoYDiaOperativoAsync((int)TiposDatos.PoderCalorifico, primerDato.IdDiaOperativo);
                if (dato != null)
                {
                    dto.BtuPcs = dato.Valor ?? 0;
                }
            }
            dto.Mmbtu = dto.Mmbtu * dto.BtuPcs / 1000;


            var operacionUsuario = await _usuarioServicio.ObtenerAsync(idUsuario);
            if (operacionUsuario != null && operacionUsuario.Completado && operacionUsuario.Resultado !=null && !string.IsNullOrWhiteSpace(operacionUsuario.Resultado.UrlFirma))
            {
                dto.UrlFirma = operacionUsuario.Resultado.UrlFirma;
            }
            return new OperacionDto<BoletaVentaGnsDto>(dto);
        }

    }
}
