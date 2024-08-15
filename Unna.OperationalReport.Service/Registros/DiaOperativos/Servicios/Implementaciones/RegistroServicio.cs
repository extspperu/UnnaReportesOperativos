using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Registro.Repositorios.Abstracciones;
using Unna.OperationalReport.Service.Registros.DiaOperativos.Dtos;
using Unna.OperationalReport.Service.Registros.DiaOperativos.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Seguimiento.BalanceDiario.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;

namespace Unna.OperationalReport.Service.Registros.DiaOperativos.Servicios.Implementaciones
{
    public class RegistroServicio : IRegistroServicio
    {

        private readonly IRegistroRepositorio _registroRepositorio;
        private readonly IDiaOperativoRepositorio _diaOperativoRepositorio;
        private readonly ISeguimientoBalanceDiarioServicio _seguimientoBalanceDiarioServicio;

        public RegistroServicio(IRegistroRepositorio registroRepositorio,
            IDiaOperativoRepositorio diaOperativoRepositorio,
            ISeguimientoBalanceDiarioServicio seguimientoBalanceDiarioServicio)
        {
            _registroRepositorio = registroRepositorio;
            _diaOperativoRepositorio = diaOperativoRepositorio;
            _seguimientoBalanceDiarioServicio = seguimientoBalanceDiarioServicio;
        }

        public async Task<OperacionDto<RespuestaSimpleDto<bool>>> GuardarAsync(List<RegistroDto>? peticion, long? idUsuario, bool? esEditado)
        {
            if (peticion == null || peticion.Count == 0)
            {
                return new OperacionDto<RespuestaSimpleDto<bool>>(CodigosOperacionDto.NoExiste, "No existe validaciones del registro");
            }

            var operacionValidacion = ValidacionUtilitario.ValidarModelo<RespuestaSimpleDto<bool>>(peticion);
            if (!operacionValidacion.Completado)
            {
                return operacionValidacion;
            }

            if (esEditado == true)
            {
                if (peticion.Where(e => e.EsValido != true).Count() > 0)
                {
                    return new OperacionDto<RespuestaSimpleDto<bool>>(CodigosOperacionDto.NoExiste, "Si es editado debe marcar como válido los registros");
                };
            }


            foreach (var item in peticion)
            {
                var id = RijndaelUtilitario.DecryptRijndaelFromUrl<long>(item.Id);
                var registro = await _registroRepositorio.BuscarPorIdYNoBorradoAsync(id);
                if (registro == null)
                {
                    continue;
                }
                registro.Actualizado = DateTime.UtcNow;
                registro.EsConciliado = item.EsConciliado;
                registro.Valor = item.Valor;
                if (esEditado == true)
                {
                    registro.EsEditadoPorProceso = esEditado == true ? true : registro.EsEditadoPorProceso;
                }
                if (item.EsValido == true)
                {
                    registro.FechaValido = DateTime.UtcNow;
                    registro.IdUsuarioValidador = idUsuario;
                }
                if (item.EsValido == false)
                {
                    registro.EsDevuelto = true;
                }
                registro.EsValido = item.EsValido;
                _registroRepositorio.Editar(registro);
                await _registroRepositorio.UnidadDeTrabajo.GuardarCambiosAsync();
            }


            if (peticion.FirstOrDefault() != null)
            {
                long idDiaOperativo = RijndaelUtilitario.DecryptRijndaelFromUrl<long>(peticion.FirstOrDefault().IdDiaOperativo);
                var diaOperativo = await _diaOperativoRepositorio.BuscarPorIdYNoBorradoAsync(idDiaOperativo);
                if (diaOperativo != null)
                {

                    if (peticion.Where(e => e.EsValido != true).Count() > 0)
                    {
                        diaOperativo.EsObservado = false;
                        diaOperativo.DatoValidado = false;
                        diaOperativo.EsObservado = true;
                        diaOperativo.FechaObservado = DateTime.UtcNow;
                        diaOperativo.IdUsuarioObservado = idUsuario;
                    }
                    else
                    {
                        diaOperativo.DatoValidado = true;
                        diaOperativo.FechaValidado = DateTime.UtcNow;
                        diaOperativo.IdUsuarioValidado = idUsuario;
                        diaOperativo.EsObservado = false;
                    }
                    diaOperativo.Actualizado = DateTime.UtcNow;
                    _diaOperativoRepositorio.Editar(diaOperativo);
                    await _diaOperativoRepositorio.UnidadDeTrabajo.GuardarCambiosAsync();
                }

            }


            var operacion = await _seguimientoBalanceDiarioServicio.ActualizarEstadoSeguimientoDiarioAsync(6, 3);

            return new OperacionDto<RespuestaSimpleDto<bool>>(
                new RespuestaSimpleDto<bool>()
                {
                    Id = true,
                    Mensaje = "Se guardo correctamente"
                }
                );
        }
    }
}
