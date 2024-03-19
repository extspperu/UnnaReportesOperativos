﻿using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Auth.Enums;
using Unna.OperationalReport.Data.Configuracion.Enums;
using Unna.OperationalReport.Data.Configuracion.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Registro.Enums;
using Unna.OperationalReport.Data.Registro.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Registro.Repositorios.Implementaciones;
using Unna.OperationalReport.Data.Reporte.Enums;
using Unna.OperationalReport.Service.Reportes.Generales.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.Impresiones.Dtos;
using Unna.OperationalReport.Service.Reportes.Impresiones.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaCnpc.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaVentaGns.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaVentaGns.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.FiscalizacionPetroPeru.Dtos;
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
        private readonly IReporteServicio _reporteServicio;
        private readonly IImpresionServicio _impresionServicio;
        private readonly IEmpresaRepositorio _empresaRepositorio;
        private readonly IGnsVolumeMsYPcBrutoRepositorio _gnsVolumeMsYPcBrutoRepositorio;

        public BoletaVentaGnsServicio(
            IDiaOperativoRepositorio diaOperativoRepositorio,
            IRegistroRepositorio registroRepositorio,
            IUsuarioServicio usuarioServicio,
            IReporteServicio reporteServicio,
            IImpresionServicio impresionServicio,
            IEmpresaRepositorio empresaRepositorio,
            IGnsVolumeMsYPcBrutoRepositorio gnsVolumeMsYPcBrutoRepositorio
            )
        {
            _diaOperativoRepositorio = diaOperativoRepositorio;
            _registroRepositorio = registroRepositorio;
            _usuarioServicio = usuarioServicio;
            _reporteServicio = reporteServicio;
            _impresionServicio = impresionServicio;
            _empresaRepositorio = empresaRepositorio;
            _gnsVolumeMsYPcBrutoRepositorio = gnsVolumeMsYPcBrutoRepositorio;
        }

        public async Task<OperacionDto<BoletaVentaGnsDto>> ObtenerAsync(long idUsuario)
        {
            var operacionGeneral = await _reporteServicio.ObtenerAsync((int)TiposReportes.BoletaVentaGasNaturalSecoUnnaLoteIVEnel, idUsuario);
            if (!operacionGeneral.Completado)
            {
                return new OperacionDto<BoletaVentaGnsDto>(CodigosOperacionDto.NoExiste, operacionGeneral.Mensajes);
            }

            var operacionImpresion = await _impresionServicio.ObtenerAsync((int)TiposReportes.BoletaVentaGasNaturalSecoUnnaLoteIVEnel, FechasUtilitario.ObtenerDiaOperativo());
            if (operacionImpresion.Completado && operacionImpresion.Resultado != null && !string.IsNullOrWhiteSpace(operacionImpresion.Resultado.Datos))
            {
                var rpta = JsonConvert.DeserializeObject<BoletaVentaGnsDto>(operacionImpresion.Resultado.Datos);
                rpta.General = operacionGeneral.Resultado;
                return new OperacionDto<BoletaVentaGnsDto>(rpta);
            }


            DateTime diaOperativo = FechasUtilitario.ObtenerDiaOperativo();
            var dto = new BoletaVentaGnsDto
            {
                Fecha = diaOperativo.ToString("dd/MM/yyyy"),

            };
            dto.General = operacionGeneral.Resultado;

            var segundoDato = await _diaOperativoRepositorio.ObtenerPorIdLoteYFechaAsync((int)TiposLote.LoteX, FechasUtilitario.ObtenerDiaOperativo(),(int)TipoGrupos.FiscalizadorEnel,(int)TiposNumeroRegistro.SegundoRegistro);
            if (segundoDato != null)
            {
                var dato = await _registroRepositorio.ObtenerPorIdDatoYDiaOperativoAsync((int)TiposDatos.GnsVentaUnnaLoteIv, segundoDato.IdDiaOperativo);
                if (dato !=null)
                {
                    dto.Mpcs = dato.Valor??0;
                }
            }
            var gnsVolumeMsYPcBrutoRepositorio = await _gnsVolumeMsYPcBrutoRepositorio.ObtenerPorTipoYNombreDiaOperativoAsync(TiposTablasSupervisorPgt.VolumenMsGnsAgpsa, TiposGnsVolumeMsYPcBruto.GnsAEgpsa, diaOperativo);
            if (gnsVolumeMsYPcBrutoRepositorio != null)
            {
                dto.BtuPcs = gnsVolumeMsYPcBrutoRepositorio.PcBrutoRepCroma??0;
            }


            dto.Mmbtu =  dto.Mpcs * dto.BtuPcs / 1000;
            var empresa = await _empresaRepositorio.BuscarPorIdAsync((int)TiposEmpresas.UnnaEnergiaSa);
            if (empresa != null)
            {
                dto.Empresa = empresa.RazonSocial;
                dto.Abreviatura = empresa.Abreviatura;
            }
            return new OperacionDto<BoletaVentaGnsDto>(dto);
        }

        public async Task<OperacionDto<RespuestaSimpleDto<bool>>> GuardarAsync(BoletaVentaGnsDto peticion)
        {
            var operacionValidacion = ValidacionUtilitario.ValidarModelo<RespuestaSimpleDto<bool>>(peticion);
            if (!operacionValidacion.Completado)
            {
                return operacionValidacion;
            }
            peticion.General = null;
            var dto = new ImpresionDto()
            {
                IdConfiguracion = RijndaelUtilitario.EncryptRijndaelToUrl((int)TiposReportes.BoletaVentaGasNaturalSecoUnnaLoteIVEnel),
                Fecha = FechasUtilitario.ObtenerDiaOperativo(),
                IdUsuario = peticion.IdUsuario,
                Datos = JsonConvert.SerializeObject(peticion)
            };
            return await _impresionServicio.GuardarAsync(dto);

        }

    }
}
