
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using TimeZoneConverter;

namespace Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios
{
    //public static class FechasUtilitario
    public class FechasUtilitario
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

            return ObtenerFechaSegunZonaHoraria(DateTime.UtcNow.AddDays(-65));
        }


        public static string? ObtenerNombreMes(DateTime fecha)
        {
            string? nombre = default(string);
            switch (fecha.Month)
            {
                case 12:
                    nombre = "Diciembre";
                    break;
                case 11:
                    nombre = "Noviembre";
                    break;
                case 10:
                    nombre = "Octubre";
                    break;
                case 9:
                    nombre = "Septiembre";
                    break;
                case 8:
                    nombre = "Agosto";
                    break;
                case 7:
                    nombre = "Julio";
                    break;
                case 6:
                    nombre = "Junio";
                    break;
                case 5:
                    nombre = "Mayo";
                    break;
                case 4:
                    nombre = "Abril";
                    break;
                case 3:
                    nombre = "Marzo";
                    break;
                case 2:
                    nombre = "Febrero";
                    break;
                case 1:
                    nombre = "Enero";
                    break;
            }
            return nombre;
        }

        public static string? ObtenerNombreMesAbrev(DateTime fecha)
        {
            string? nombre = default(string);
            switch (fecha.Month)
            {
                case 12:
                    nombre = "div";
                    break;
                case 11:
                    nombre = "nov";
                    break;
                case 10:
                    nombre = "oct";
                    break;
                case 9:
                    nombre = "sep";
                    break;
                case 8:
                    nombre = "ago";
                    break;
                case 7:
                    nombre = "jul";
                    break;
                case 6:
                    nombre = "jun";
                    break;
                case 5:
                    nombre = "may";
                    break;
                case 4:
                    nombre = "abr";
                    break;
                case 3:
                    nombre = "mar";
                    break;
                case 2:
                    nombre = "feb";
                    break;
                case 1:
                    nombre = "ene";
                    break;
            }
            return nombre;
        }
    }
}
