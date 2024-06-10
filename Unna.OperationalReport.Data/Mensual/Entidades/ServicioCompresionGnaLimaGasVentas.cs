using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Registro.Entidades;

namespace Unna.OperationalReport.Data.Mensual.Entidades
{
    public class ServicioCompresionGnaLimaGasVentas
    {
        public long Id { get; set; }
        public string? FechaDespacho { get; set; }
        public string? Placa { get; set; }
        public DateTime? FechaInicioCarga { get; set; }
        public DateTime? FechaFinCarga { get; set; }
        public string? NroConstanciaDespacho { get; set; }
        public double? VolumenSm3 { get; set; }
        public double? VolumenMmpcs { get; set; }
        public double? PoderCalorifico { get; set; }
        public double? Energia { get; set; }
        public double? Precio { get; set; }
        public double? SubTotal { get; set; }
        public long? IdServicioCompresionGnaLimaGas { get; set; }

        public DateTime Creado { get; set; }
        public DateTime? Actualizado { get; set; }
        public long? IdUsuario { get; set; }

        public virtual ServicioCompresionGnaLimaGas? ServicioCompresionGnaLimaGas { get; set; }
    }
}
