using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Data.Mensual.Entidades
{
    public class ServicioCompresionGnaLimaGas
    {
        public long? Id { get; set; }
        public DateTime Fecha { get; set; }
        public string? Periodo { get; set; }
        public double? EnergiaVolumenProcesado { get; set; }
        public double? SubTotalFacturar { get; set; }
        public string? IgvCentaje { get; set; }
        public double? Igv { get; set; }
        public double? TotalFacturar { get; set; }
        public string? Comentario { get; set; }


        public DateTime Creado { get; set; }
        public DateTime? Actualizado { get; set; }
        public long? IdUsuario { get; set; }

        public virtual ICollection<ServicioCompresionGnaLimaGasVentas>? ServicioCompresionGnaLimaGasVentas { get; set; }
    }
}
