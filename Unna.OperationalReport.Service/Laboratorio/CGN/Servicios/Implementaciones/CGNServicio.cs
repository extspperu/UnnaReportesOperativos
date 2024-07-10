
using Unna.OperationalReport.Service.Laboratorio.CGN.Dtos;
using Unna.OperationalReport.Service.Laboratorio.CGN.Servicios.Abstracciones;

namespace Unna.OperationalReport.Service.Laboratorio.CGN.Servicios.Implementaciones
{
    public class CGNServicio : ICGN
    {
        public List<CGNDataDto> ObtenerAsync()
        {
            var daysInMonth = DateTime.DaysInMonth(2024, 6);
            var dataList = new List<CGNDataDto>();

            for (int day = 1; day <= daysInMonth; day++)
            {
                dataList.Add(new CGNDataDto
                {
                    Day = day
                    // Todos los demás campos se inicializan a 0 por defecto
                });
            }

            return dataList;
        }
    }
}
