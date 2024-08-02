using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Data.Registro.Entidades
{
    public class DatoComposicionUnnaEnergiaPromedio
    {
        public long idDiaOperativo { get; set; }
        public string componente { get; set; }
        public double promedioComponente { get; set; }
    }
}
