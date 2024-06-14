using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Data.Carta.Entidades
{
    public class VentaPorCliente
    {
        public long IdVentaPorCliente { get;set; }
        public DateTime? Fecha { get;set; }
        public string? Periodo { get;set; }
        public string? Producto { get;set; }
        public DateTime Creado { get;set; }
        public long? IdArchivo { get;set; }
        public VentaPorCliente()
        {
            Creado = DateTime.UtcNow;
        }
    }
}
