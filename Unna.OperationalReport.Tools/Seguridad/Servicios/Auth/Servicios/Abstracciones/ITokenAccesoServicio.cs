using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Tools.Seguridad.Servicios.Auth.Dtos;

namespace Unna.OperationalReport.Tools.Seguridad.Servicios.Auth.Servicios.Abstracciones
{
    public interface ITokenAccesoServicio
    {
        string? GenerarTokenRSA(long idUsuario);

        OperacionDto<TokenAccesoDto> ObtenerTokenAccesoDeCadena(string token);
    }
}
