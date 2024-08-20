using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Service.Propiedades.ComposicionGnaPromedios.Dtos
{
    public class ComposicionGnaPromedioDto
    {
        public string? IdLote { get; set; }
        public string? Lote { get; set; }
        public int? IdSuministrador { get; set; }
        public string? Suministrador { get; set; }
        public DateTime? Fecha { get; set; }
        public double? Porcentaje { get; set; }

        public string? FechaCadena
        {
            get
            {
                return Fecha.HasValue ? Fecha.Value.ToString("dd/MM/yyyy") : null;
            }
        }

    }
}
