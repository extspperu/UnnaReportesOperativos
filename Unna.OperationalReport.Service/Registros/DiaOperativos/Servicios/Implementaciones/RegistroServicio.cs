using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Registro.Entidades;
using Unna.OperationalReport.Data.Registro.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Mantenimiento.Enums;
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

            int loteSeleccionado = 0;
            int estadoLote = Convert.ToInt32(EstadoSeguimiento.Pendiente);
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

                        estadoLote = Convert.ToInt32(EstadoSeguimiento.Rechazado);
                    }
                    else
                    {
                        diaOperativo.DatoValidado = true;
                        diaOperativo.FechaValidado = DateTime.UtcNow;
                        diaOperativo.IdUsuarioValidado = idUsuario;
                        diaOperativo.EsObservado = false;

                        estadoLote = Convert.ToInt32(EstadoSeguimiento.Aprobado);

                    }
                    diaOperativo.Actualizado = DateTime.UtcNow;
                    _diaOperativoRepositorio.Editar(diaOperativo);
                    loteSeleccionado = diaOperativo.IdLote.Value;
                    await _diaOperativoRepositorio.UnidadDeTrabajo.GuardarCambiosAsync();
                }

            }



            switch (loteSeleccionado)
            {
                case(1):
                    await _seguimientoBalanceDiarioServicio.ActualizarEstadoSeguimientoDiarioAsync(5, estadoLote);
                    break;
                case (2):
                    await _seguimientoBalanceDiarioServicio.ActualizarEstadoSeguimientoDiarioAsync(3, estadoLote);
                    break;
                case (3):
                    await _seguimientoBalanceDiarioServicio.ActualizarEstadoSeguimientoDiarioAsync(1, estadoLote);
                    break;
                case (4):
                    await _seguimientoBalanceDiarioServicio.ActualizarEstadoSeguimientoDiarioAsync(2, estadoLote);
                    break;
                case (5):
                    await _seguimientoBalanceDiarioServicio.ActualizarEstadoSeguimientoDiarioAsync(4, estadoLote);
                    break;
            }


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
