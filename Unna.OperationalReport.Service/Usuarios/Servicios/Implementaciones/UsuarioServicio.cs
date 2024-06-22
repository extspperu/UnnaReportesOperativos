using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Auth.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Registro.Entidades;
using Unna.OperationalReport.Service.Auth.Dtos;
using Unna.OperationalReport.Service.Registros.DiaOperativos.Dtos;
using Unna.OperationalReport.Service.Usuarios.Dtos;
using Unna.OperationalReport.Service.Usuarios.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;

namespace Unna.OperationalReport.Service.Usuarios.Servicios.Implementaciones
{
    public class UsuarioServicio: IUsuarioServicio
    {
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly IPersonaRepositorio _personaRepositorio;
        private readonly IMapper _mapper;
        public UsuarioServicio(
            IUsuarioRepositorio usuarioRepositorio,
            IPersonaRepositorio personaRepositorio,
            IMapper mapper
            )
        {
            _usuarioRepositorio = usuarioRepositorio;
            _personaRepositorio = personaRepositorio;
            _mapper = mapper;
        }



        public async Task<OperacionDto<UsuarioDto>> ObtenerAsync(long idUsuario)
        {          
            var usuario = await _usuarioRepositorio.BuscarPorIdYNoBorradoAsync(idUsuario);
            if (usuario == null)
            {
                return new OperacionDto<UsuarioDto>(CodigosOperacionDto.UsuarioIncorrecto, "Usuario no existe");
            }
            await _personaRepositorio.UnidadDeTrabajo.Entry(usuario).Reference(e => e.Persona).LoadAsync();
            
            var dto = _mapper.Map<UsuarioDto>(usuario);
            return new OperacionDto<UsuarioDto>(dto);
        }
    }
}
