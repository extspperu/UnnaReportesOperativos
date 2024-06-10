using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Data.Registro.Entidades
{
    public class TipodeCambio
    {
        public DateTime? Fecha {  get; set; }
        public int IdTipoMoneda {  get; set; }  
        public double Cambio { get; set; }
        public DateTime? Creado { get; set; }
        public DateTime? Actualizado { get;  set; }
        public bool EstaBorrado { get; set; }   
    }
}
