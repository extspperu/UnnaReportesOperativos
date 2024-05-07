using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Data.Registro.Entidades
{
    public class ComposicionUnnaEnergiaPromedio
    {
        public long IdAdjuntoSupevisor { get; set; }
        public long IdComponente { get; set; }
        public double PromedioComponente { get; set; }
        public int Orden { get; set; }
        public string Simbolo { get; set; }
        public DateTime Fecha { get; set; }
    }
}
