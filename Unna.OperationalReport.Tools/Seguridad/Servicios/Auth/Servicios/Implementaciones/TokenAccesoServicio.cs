using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;
using Unna.OperationalReport.Tools.Seguridad.Servicios.Auth.Dtos;
using Unna.OperationalReport.Tools.Seguridad.Servicios.Auth.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Seguridad.Servicios.General.Dtos;

namespace Unna.OperationalReport.Tools.Seguridad.Servicios.Auth.Servicios.Implementaciones
{
    public class TokenAccesoServicio : ITokenAccesoServicio
    {

        private readonly SeguridadConfiguracionDto _seguridadConfiguracion;

        public TokenAccesoServicio(
            SeguridadConfiguracionDto seguridadConfiguracion
            )
        {
            _seguridadConfiguracion = seguridadConfiguracion;
        }

        public string? GenerarTokenRSA(long idUsuario)
        {
            var tiempo = DateTime.UtcNow.Ticks;

            var cadena = $@"{idUsuario}___{tiempo}";

            var cifrado = RsaUtilitario.EncryptText(_seguridadConfiguracion.LlaveRsaPublica, cadena);


            return cifrado;
        }

        public OperacionDto<TokenAccesoDto> ObtenerTokenAccesoDeCadena(string token)
        {
            try
            {

                if (_seguridadConfiguracion.LlaveRsaPrivada == null)
                {
                    return new OperacionDto<TokenAccesoDto>(CodigosOperacionDto.Invalido, "Dalta configurar RSA");
                }

                var cadena = RsaUtilitario.DecryptText(_seguridadConfiguracion.LlaveRsaPrivada, token);
                var datos = cadena.Split(new string[] { "___" }, StringSplitOptions.None);
                var idUsuario = Convert.ToInt64(datos[0]);
                var creado = new DateTime(Convert.ToInt64(datos[1]));

                return new OperacionDto<TokenAccesoDto>(new TokenAccesoDto()
                {
                    IdUsuario = idUsuario,
                    FechaCreacion = creado,
                    TokenAcceso = token
                });

            }
            catch (FormatException)
            {
                return new OperacionDto<TokenAccesoDto>(CodigosOperacionDto.Invalido, "Token Inválido");
            }
        }
    }
}
