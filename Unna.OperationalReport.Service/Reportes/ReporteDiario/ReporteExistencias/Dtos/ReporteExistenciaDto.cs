using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; 
using System.Threading.Tasks;

namespace Unna.OperationalReport.Service.Reportes.ReporteDiario.ReporteExistencias.Dtos
{
    public class ReporteExistenciaDto
    {

        public DateTime? Fecha { get; set; }
        public List<ReporteExistenciaDetalleDto>? Datos { get; set; }
       
    }

    public class ReporteExistenciaDetalleDto
    {

        public string? RazonSocial { get; set; }
        public string? Nombre { get; set; }
        public decimal? Valor { get; set; }


    }
}
