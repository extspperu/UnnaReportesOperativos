using Newtonsoft.Json;
using Unna.OperationalReport.Data.Reporte.Enums;
using Unna.OperationalReport.Service.Laboratorio.GNSGNA.Dtos;
using Unna.OperationalReport.Service.Laboratorio.GNSGNA.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.Impresiones.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;

namespace Unna.OperationalReport.Service.Laboratorio.GNSGNA.Servicios.Implementaciones
{
    public class GNSGNAServicio : IGNSGNA
    {
        public List<GNSGNAData> ObtenerAsync()
        {
            var daysInMonth = DateTime.DaysInMonth(2024, 6);
            var dataList = new List<GNSGNAData>();

            for (int day = 1; day <= daysInMonth; day++)
            {
                dataList.Add(new GNSGNAData
                {
                    Day = day
                    // Todos los demás campos se inicializan a 0 por defecto
                });
            }

            return dataList;
        }
        public async Task<OperacionDto<RespuestaSimpleDto<string>>> GuardarAsync(GNSGNADto peticion)
        {
            var dto = new ImpresionDto()
            {
                Id = "",
                IdConfiguracion = RijndaelUtilitario.EncryptRijndaelToUrl((int)TiposReportes.ResumenBalanceEnergiaLIVQuincenal),
                Fecha = DateTime.Now,
                IdUsuario = 1,
                Datos = JsonConvert.SerializeObject(peticion),
                Comentario = "TEst"
            };

            return new OperacionDto<RespuestaSimpleDto<string>>(new RespuestaSimpleDto<string>() { Id = RijndaelUtilitario.EncryptRijndaelToUrl(1), Mensaje = "Se guardó correctamente" });
        }
    }
}
