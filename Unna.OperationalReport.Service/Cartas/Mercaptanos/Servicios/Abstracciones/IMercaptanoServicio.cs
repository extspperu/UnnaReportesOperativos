using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Service.Cartas.Mercaptanos.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;

namespace Unna.OperationalReport.Service.Cartas.Mercaptanos.Servicios.Abstracciones
{
    public interface IMercaptanoServicio
    {
        Task<OperacionDto<MercaptanoDto>> ObtenerAsync(MercaptanoDto parametro);
    }
}
