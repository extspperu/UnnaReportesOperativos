using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Service.Laboratorio.GLP.Dto;
using Unna.OperationalReport.Service.Laboratorio.GLP.Servicios.Abstracciones;

namespace Unna.OperationalReport.Service.Laboratorio.GLP.Servicios.Implementaciones
{
    public class GLPServicio : IGLP
    {
        public List<GLPDataDto> ObtenerAsync()
        {
            var daysInMonth = DateTime.DaysInMonth(2024, 6);
            var dataList = new List<GLPDataDto>();

            for (int day = 1; day <= daysInMonth; day++)
            {
                dataList.Add(new GLPDataDto
                {
                    Day = day
                });
            }

            return dataList;
        }
    }
}
