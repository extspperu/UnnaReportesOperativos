using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Auth.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Registro.Repositorios.Abstracciones;
using Unna.OperationalReport.Service.Registros.Lotes.Dtos;
using Unna.OperationalReport.Service.Registros.Lotes.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;

namespace Unna.OperationalReport.Service.Registros.Lotes.Servicios.Implementaciones
{
    public class UsuarioLoteServicio: IUsuarioLoteServicio
    {
        private readonly IUsuarioLoteRepositorio _usuarioLoteRepositorio;
        private readonly ILoteRepositorio _loteRepositorio;
        private readonly IMapper _mapper;
        public UsuarioLoteServicio(
            IUsuarioLoteRepositorio usuarioLoteRepositorio,
            ILoteRepositorio loteRepositorio,
            IMapper mapper
            )
        {
            _usuarioLoteRepositorio = usuarioLoteRepositorio;
            _loteRepositorio = loteRepositorio;
            _mapper = mapper;
        }


        public async Task<OperacionDto<RespuestaSimpleDto<int>>> ObtenerIdLotePorIdUsuarioAsync(long idUsuario)
        {
            var usuario = await _usuarioLoteRepositorio.BuscarPorIdUsuarioActivoAsync(idUsuario);
            if (usuario == null)
            {
                return new OperacionDto<RespuestaSimpleDto<int>>(CodigosOperacionDto.Invalido, "No tiene permiso a ningún lote");
            }
            
            return new OperacionDto<RespuestaSimpleDto<int>>(new RespuestaSimpleDto<int>() { Id = usuario.IdLote, Mensaje = "Correcto" });
        }

        public async Task<OperacionDto<LoteDto>> ObtenerLotePorIdUsuarioAsync(long idUsuario)
        {
            var operacion = await ObtenerIdLotePorIdUsuarioAsync(idUsuario);
            if (operacion == null || operacion.Resultado == null || !operacion.Completado)
            {
                return new OperacionDto<LoteDto>(CodigosOperacionDto.Invalido, "No tiene permiso a ningún lote");
            }
            var lote = await _loteRepositorio.BuscarPorIdYNoBorradoAsync(operacion.Resultado.Id);
            if (lote == null)
            {
                return new OperacionDto<LoteDto>(CodigosOperacionDto.Invalido, "No existe registro de lote");
            }
            var dto = _mapper.Map<LoteDto>(lote);
            return new OperacionDto<LoteDto>(dto);
        }

        public async Task<OperacionDto<List<UsuarioLoteDto>>> ListarPorIdGrupoAsync(int? idGrupo)
        {
            var usuario = await _usuarioLoteRepositorio.ListarPorIdGrupoAsync(idGrupo);
            var dto = _mapper.Map<List<UsuarioLoteDto>>(usuario);
            return new OperacionDto<List<UsuarioLoteDto>>(dto);
        }
    }
}
