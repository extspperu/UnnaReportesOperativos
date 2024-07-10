using AutoMapper;
using DocumentFormat.OpenXml.Office2010.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Auth.Entidades;
using Unna.OperationalReport.Data.Auth.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Configuracion.Entidades;
using Unna.OperationalReport.Data.Registro.Entidades;
using Unna.OperationalReport.Service.Auth.Dtos;
using Unna.OperationalReport.Service.Registros.DiaOperativos.Dtos;
using Unna.OperationalReport.Service.Usuarios.Dtos;
using Unna.OperationalReport.Service.Usuarios.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;

namespace Unna.OperationalReport.Service.Usuarios.Servicios.Implementaciones
{
    public class UsuarioServicio : IUsuarioServicio
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
            await _personaRepositorio.UnidadDeTrabajo.Entry(usuario).Reference(e => e.Firma).LoadAsync();

            var dto = _mapper.Map<UsuarioDto>(usuario);
            return new OperacionDto<UsuarioDto>(dto);
        }


        public async Task<OperacionDto<RespuestaSimpleDto<bool>>> ActualizarFirmaAsync(long idUsuario, string idFirma)
        {
            var usuario = await _usuarioRepositorio.BuscarPorIdYNoBorradoAsync(idUsuario);
            if (usuario == null)
            {
                return new OperacionDto<RespuestaSimpleDto<bool>>(CodigosOperacionDto.UsuarioIncorrecto, "Usuario no existe");
            }
            var id = RijndaelUtilitario.DecryptRijndaelFromUrl<long>(idFirma);
            usuario.Actualizado = DateTime.UtcNow;
            usuario.IdFirma = id;
            _usuarioRepositorio.Editar(usuario);
            await _usuarioRepositorio.UnidadDeTrabajo.GuardarCambiosAsync();

            return new OperacionDto<RespuestaSimpleDto<bool>>(new RespuestaSimpleDto<bool> { Id = true, Mensaje = "Se guardó correctamente" });
        }

        public async Task<OperacionDto<RespuestaSimpleDto<string>>> GuardarAsync(ActualizarDatosUsuarioDto peticion)
        {
            if (string.IsNullOrWhiteSpace(peticion.Correo))
            {
                return new OperacionDto<RespuestaSimpleDto<string>>(CodigosOperacionDto.UsuarioIncorrecto, "Correo es requerido");
            }

            var usuario = await _usuarioRepositorio.BuscarPorIdYNoBorradoAsync(peticion.IdUsuario);
            if (usuario == null)
            {
                return new OperacionDto<RespuestaSimpleDto<string>>(CodigosOperacionDto.UsuarioIncorrecto, "Usuario no existe");
            }

            if (!usuario.Username.Equals(peticion.Correo))
            {
                var existeCorreo = await _usuarioRepositorio.BuscarPorUsernameAsync(peticion.Correo);
                if (existeCorreo != null)
                {
                    return new OperacionDto<RespuestaSimpleDto<string>>(CodigosOperacionDto.UsuarioIncorrecto, "El correo ya existe registrado para otro usuario");
                }
            }

            

            Persona? persona = default(Persona?);
            if (usuario.IdPersona.HasValue)
            {
                persona = await _personaRepositorio.BuscarPorIdAsync(usuario.IdPersona.Value);
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(peticion.Documento))
                {
                    persona = await _personaRepositorio.BuscarPorDocumentoAsync(peticion.Documento);
                }
            }
            if (persona == null)
            {
                persona = await _personaRepositorio.BuscarPorCorreoAsync(peticion.Correo);
            }
            if (persona == null)
            {
                persona = new Persona();
            }
            persona.Correo = peticion.Correo;
            persona.Documento = peticion.Documento;
            persona.Nombres = peticion.Nombres;
            persona.Paterno = peticion.Paterno;
            persona.Materno = peticion.Materno;
            persona.Telefono = peticion.Telefono;
            persona.IdTipoPersona = 100;//Pesona Natural
            if (persona.IdPersona > 0)
            {
                _personaRepositorio.Editar(persona);
            }
            else
            {
                _personaRepositorio.Insertar(persona);
            }            
            await _personaRepositorio.UnidadDeTrabajo.GuardarCambiosAsync();


            usuario.Username = peticion.Correo;
            usuario.IdPersona = persona.IdPersona;
            usuario.Actualizado = DateTime.UtcNow;
            _usuarioRepositorio.Editar(usuario);
            await _usuarioRepositorio.UnidadDeTrabajo.GuardarCambiosAsync();

            return new OperacionDto<RespuestaSimpleDto<string>>(new RespuestaSimpleDto<string> { Id = RijndaelUtilitario.EncryptRijndaelToUrl(usuario.IdUsuario), Mensaje = "Se guardó correctamente" });
        }


    }
}
