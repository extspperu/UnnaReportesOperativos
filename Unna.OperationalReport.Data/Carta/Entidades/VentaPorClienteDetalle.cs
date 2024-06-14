using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Data.Carta.Entidades
{
    public class VentaPorClienteDetalle
    {
        public long Id { get; set; }
        public DateTime Periodo { get; set; }
        public string? Producto { get; set; }
        public long? IdCliente { get; set; }
        public string? Cliente { get; set; }
        public string? Uom { get; set; }
        public double Volumen { get; set; }
        public double Centaje { get; set; }
        public double Brl { get; set; }
        public DateTime Creado { get; set; }
        public long? IdVentaPorCliente { get; set; }

    }
}
