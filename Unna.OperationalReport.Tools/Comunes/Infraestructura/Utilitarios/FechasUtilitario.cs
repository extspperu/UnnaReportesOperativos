using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using TimeZoneConverter;

namespace Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios
{
    public static class FechasUtilitario
    {
        public static string timeZoneDefault = "SA Pacific Standard Time";

        public static DateTime ObtenerFechaSegunZonaHoraria(DateTime fecha, string? zonaHoraria=null)
        {
            bool esLinux = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);

            var zonaHorariaUsar = !string.IsNullOrWhiteSpace(zonaHoraria) ? zonaHoraria : timeZoneDefault;

            if (esLinux)
            {
                zonaHorariaUsar = TZConvert.WindowsToIana(zonaHorariaUsar);
            }

            var utcTime = default(TimeZoneInfo);

            try
            {
                utcTime = TimeZoneInfo.FindSystemTimeZoneById(zonaHorariaUsar);
            }
            catch
            {

                if (esLinux)
                {
                    utcTime = TimeZoneInfo.FindSystemTimeZoneById(TZConvert.WindowsToIana(timeZoneDefault));
                }
                else {
                    utcTime = TimeZoneInfo.FindSystemTimeZoneById(timeZoneDefault);

                }
            }

            return TimeZoneInfo.ConvertTime(fecha, TimeZoneInfo.Utc, utcTime);
        }

        public static DateTime ObtenerDiaOperativo()
        {
            return ObtenerFechaSegunZonaHoraria(DateTime.UtcNow.AddDays(-6));
            //return ObtenerFechaSegunZonaHoraria(DateTime.UtcNow.AddDays(-1));
        }
    }
}
