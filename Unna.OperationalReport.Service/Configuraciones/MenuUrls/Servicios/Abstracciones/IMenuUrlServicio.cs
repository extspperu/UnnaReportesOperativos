using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Service.Configuraciones.MenuUrls.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;

namespace Unna.OperationalReport.Service.Configuraciones.MenuUrls.Servicios.Abstracciones
{
    public interface IMenuUrlServicio
    {
        Task<OperacionDto<List<MenuUrlAdminDto>>> ObtenerListaMenuUrl(long idUsuario);
        Task<OperacionDto<MenuUrlAdminDto>> ObtenerListaMenuUrl(string idMenuUrl);
    }
}
