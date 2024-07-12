using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Service.Usuarios.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;

namespace Unna.OperationalReport.Service.Usuarios.Servicios.Abstracciones
{
    public interface IUsuarioServicio
    {
        Task<OperacionDto<UsuarioDto>> ObtenerAsync(string idUsuario);
        Task<OperacionDto<UsuarioDto>> ObtenerAsync(long idUsuario);
        Task<OperacionDto<RespuestaSimpleDto<bool>>> ActualizarFirmaAsync(long idUsuario, string idFirma);
        Task<OperacionDto<RespuestaSimpleDto<string>>> GuardarAsync(ActualizarDatosUsuarioDto peticion);
        Task<OperacionDto<List<ListarUsuariosDto>>> ListarUsuariosAsync();
        Task<OperacionDto<RespuestaSimpleDto<string>>> CrearActualizarAsync(CrearActualizarUsuarioDto peticion);
    }
}
