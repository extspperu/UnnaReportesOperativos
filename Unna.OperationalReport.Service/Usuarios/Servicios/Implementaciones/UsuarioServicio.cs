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
using Unna.OperationalReport.Tools.Seguridad.Servicios.General.Dtos;

namespace Unna.OperationalReport.Service.Usuarios.Servicios.Implementaciones
{
    public class UsuarioServicio : IUsuarioServicio
    {
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly IPersonaRepositorio _personaRepositorio;
        private readonly IMapper _mapper;
        private readonly SeguridadConfiguracionDto _seguridadConfiguracion;
        public UsuarioServicio(
            IUsuarioRepositorio usuarioRepositorio,
            IPersonaRepositorio personaRepositorio,
            IMapper mapper,
            SeguridadConfiguracionDto seguridadConfiguracion
            )
        {
            _usuarioRepositorio = usuarioRepositorio;
            _personaRepositorio = personaRepositorio;
            _mapper = mapper;
            _seguridadConfiguracion = seguridadConfiguracion;
        }


        public async Task<OperacionDto<UsuarioDto>> ObtenerAsync(string idUsuario)
        {
            var id = RijndaelUtilitario.DecryptRijndaelFromUrl<long>(idUsuario);
            return await ObtenerAsync(id);
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



        public async Task<OperacionDto<List<ListarUsuariosDto>>> ListarUsuariosAsync()
        {
            var usuarios = await _usuarioRepositorio.ListarUsuariosAsync();
          
            var dto = usuarios.Select(e=> new ListarUsuariosDto
            {
                Correo = e.Correo,
                Documento = e.Documento,
                Nombres = e.Nombres,
                EsAdministrador = e.EsAdministrador,
                EstaHabilitado = e.EstaHabilitado,
                UltimoLogin = e.UltimoLogin,
                Grupo = e.Grupo,
                Creado = e.Creado,
                Telefono = e.Telefono,
                Username = e.Username,
                IdUsuario = RijndaelUtilitario.EncryptRijndaelToUrl(e.IdUsuario)
            }).ToList();
            return new OperacionDto<List<ListarUsuariosDto>>(dto);
        }

        public async Task<OperacionDto<RespuestaSimpleDto<string>>> CrearActualizarAsync(CrearActualizarUsuarioDto peticion)
        {
            var usuarioAdmin = await _usuarioRepositorio.BuscarPorIdYNoBorradoAsync(peticion.IdUsuarioAdmin??0);
            if (usuarioAdmin == null)
            {
                return new OperacionDto<RespuestaSimpleDto<string>>(CodigosOperacionDto.UsuarioIncorrecto, "Debe iniciar su sesión");
            }
            if (!usuarioAdmin.EsAdministrador)
            {
                return new OperacionDto<RespuestaSimpleDto<string>>(CodigosOperacionDto.UsuarioIncorrecto, "Usted no tiene permiso para guardar datos de un usuario");
            }

            if (string.IsNullOrWhiteSpace(peticion.Correo) || string.IsNullOrWhiteSpace(peticion.Username))
            {
                return new OperacionDto<RespuestaSimpleDto<string>>(CodigosOperacionDto.UsuarioIncorrecto, "Correo es requerido");
            }


            if (peticion.EsUsuarioExterno)
            {
                if (!string.IsNullOrWhiteSpace(peticion.Password) && "******".Equals(peticion.Password))
                {
                    peticion.Password = null;
                }
                if (!string.IsNullOrWhiteSpace(peticion.PasswordConfirmar) && "******".Equals(peticion.PasswordConfirmar))
                {
                    peticion.PasswordConfirmar = null;
                }

                if (!string.IsNullOrWhiteSpace(peticion.Password) && !string.IsNullOrWhiteSpace(peticion.PasswordConfirmar))
                {
                    if (!peticion.Password.Equals(peticion.PasswordConfirmar))
                    {
                        return new OperacionDto<RespuestaSimpleDto<string>>(CodigosOperacionDto.UsuarioIncorrecto, "Confirme correctamente su contraseña");
                    }
                }

            }

            var id = RijndaelUtilitario.DecryptRijndaelFromUrl<long>(peticion.IdUsuario);
            var usuario = await _usuarioRepositorio.BuscarPorIdYNoBorradoAsync(id);
            if (usuario == null)
            {
                var correo = await _usuarioRepositorio.BuscarPorUsernameAsync(peticion.Username.Trim());
                if (correo != null)
                {
                    return new OperacionDto<RespuestaSimpleDto<string>>(CodigosOperacionDto.UsuarioIncorrecto, "Correo se encuentra registrado");
                }
                usuario = new Usuario();
            }
            if (!string.IsNullOrWhiteSpace(peticion.Password))
            {
                usuario.Password = Md5Utilitario.Cifrar(peticion.Password, _seguridadConfiguracion.PasswordSalt);
                usuario.PasswordSalt = _seguridadConfiguracion.PasswordSalt;
            }
            usuario.Username = peticion.Username;            
            usuario.Actualizado = DateTime.UtcNow;
            usuario.EstaHabilitado = peticion.EstaHabilitado;
            usuario.EsAdministrador = peticion.EsAdministrador;
            if (!string.IsNullOrWhiteSpace(peticion.IdGrupo))
            {
                usuario.IdGrupo = RijndaelUtilitario.DecryptRijndaelFromUrl<int>(peticion.IdGrupo);
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


            usuario.IdPersona = persona.IdPersona;
            if (usuario.IdUsuario > 0)
            {
                _usuarioRepositorio.Editar(usuario);
            }
            else
            {
                _usuarioRepositorio.Insertar(usuario);
            }
            await _usuarioRepositorio.UnidadDeTrabajo.GuardarCambiosAsync();

            return new OperacionDto<RespuestaSimpleDto<string>>(new RespuestaSimpleDto<string> { Id = RijndaelUtilitario.EncryptRijndaelToUrl(usuario.IdUsuario), Mensaje = "Se guardó correctamente" });
        }


    }
}
