using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Auth.Enums;
using Unna.OperationalReport.Data.Registro.Enums;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaCnpc.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaCnpc.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaVentaGns.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;

namespace Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaCnpc.Servicios.Implementaciones
{
    public class BoletaCnpcServicio: IBoletaCnpcServicio
    {


        public async Task<OperacionDto<BoletaCnpcDto>> ObtenerAsync(long idUsuario)
        {
            var dto = new BoletaCnpcDto
            {
                Fecha = FechasUtilitario.ObtenerDiaOperativo().ToString("dd/MM/yyyy")
            };

            BoletaCnpcTabla1Dto tabla1 = new BoletaCnpcTabla1Dto();
            tabla1.Fecha = "12/02/2024";
            tabla1.GasMpcd = 12;
            tabla1.GlpBls = 13;
            tabla1.CgnBls = 14;
            tabla1.GlpBls= 15;
            dto.Tabla1 = tabla1;


            dto.VolumenTotalGns = 123;
            dto.VolumenTotalGnsEnMs = 125;
            dto.FlareGna = dto.VolumenTotalGns + dto.VolumenTotalGnsEnMs;



            return new OperacionDto<BoletaCnpcDto>(dto);
        }

    }
}
