using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Service.Reportes.ReporteDiario.ReporteExistencias.Dtos
{
    public class ReporteExistenciaDto
    {

        public string? Fecha { get; set; }
        public string? NombreReporte { get; set; }
        public string? Compania { get; set; }
        public string? Detalle { get; set; }
        public List<ReporteExistenciaDetalleDto>? Datos { get; set; }

        [JsonIgnore]
        public long? IdUsuario { get; set; }
       
    }

    public class ReporteExistenciaDetalleDto
    {
        public int Item { get; set; }
        public string? RazonSocial { get; set; }
        public string? CodigoOsinergmin { get; set; }
        public string? NroRegistroHidrocarburo { get; set; }
        public string? Direccion { get; set; }
        public double? CapacidadInstalada { get; set; }
        public double? ExistenciaDiaria { get; set; }


    }
}
