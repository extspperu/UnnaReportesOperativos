using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Data.Registro.Entidades
{
    public class PreciosGLP
    {
        public DateTime Fecha { get; set; }
        public double PrecioGLP_E {  get; set; }
        public double PrecioGLP_G { get; set; }
        public double CostoUnitarioMaquila { get; set; }
        public DateTime Creado { get; set; }
        public DateTime Actualizado { get; set; }

    }
}
