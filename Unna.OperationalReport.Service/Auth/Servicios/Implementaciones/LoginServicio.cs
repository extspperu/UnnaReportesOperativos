using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Auth.Repositorios.Abstracciones;
using Unna.OperationalReport.Service.Auth.Dtos;
using Unna.OperationalReport.Service.Auth.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;
using Unna.OperationalReport.Tools.Seguridad.Servicios.Auth.Servicios.Abstracciones;

namespace Unna.OperationalReport.Service.Auth.Servicios.Implementaciones
{
    public class LoginServicio : ILoginServicio
    {

        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly ITokenAccesoServicio _tokenAccesoServicio;
        public LoginServicio(
            IUsuarioRepositorio usuarioRepositorio,
            ITokenAccesoServicio tokenAccesoServicio
            )
        {
            _usuarioRepositorio = usuarioRepositorio;
            _tokenAccesoServicio = tokenAccesoServicio;
        }

        public async Task<OperacionDto<LoginRespuestaDto>> LoginAsync(LoginPeticionDto peticion)
        {
            var operacionModelo = ValidacionUtilitario.ValidarModelo<LoginRespuestaDto>(peticion);
            if (!operacionModelo.Completado)
            {
                return operacionModelo;
            }

            var usuario = await _usuarioRepositorio.BuscarPorUsernameAsync(peticion.Username);
            if (usuario == null)
            {
                return new OperacionDto<LoginRespuestaDto>(CodigosOperacionDto.UsuarioIncorrecto, "Usuario o Contraseña inválida");
            }

            if (!usuario.EstaHabilitado)
            {
                return new OperacionDto<LoginRespuestaDto>(CodigosOperacionDto.UsuarioInhabilitado, "Usuario no habilitado");
            }


            if (Md5Utilitario.Cifrar(peticion.Password, usuario.PasswordSalt) != usuario.Password)
            {
                return new OperacionDto<LoginRespuestaDto>(CodigosOperacionDto.UsuarioIncorrecto, "Usuario o Contraseña inválida");
            }

            usuario.UltimoLogin = DateTime.UtcNow;
            _usuarioRepositorio.Editar(usuario);
            await _usuarioRepositorio.UnidadDeTrabajo.GuardarCambiosAsync();

            return new OperacionDto<LoginRespuestaDto>(new LoginRespuestaDto()
            {
                Suceso = true,
                Mensaje = "Login Correcto",
                TokenAcceso = _tokenAccesoServicio.GenerarTokenRSA(usuario.IdUsuario),
                IdUsuario = usuario.IdUsuario
            });
        }

    }
}
