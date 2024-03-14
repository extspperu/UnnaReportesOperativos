using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Data.Registro.Procedimientos
{
    public class ListarValoresRegistrosPorFecha
    {
        public int IdLote { get; set; }
        public string? Lote { get; set; }
        public double? Volumen { get; set; }
        public double? Calorifico { get; set; }
        public double? Riqueza { get; set; }
        public double? Mmbtu
        {
            get
            {
                return Volumen * Calorifico / 1000;
            }
        }

        public double? Lgn
        {
            get
            {
                return Volumen * Mmbtu / 42;
            }
        }

        public double? VolRenominado { get; set; }
    }
}
