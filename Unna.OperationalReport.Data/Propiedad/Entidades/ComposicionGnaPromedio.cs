using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Data.Propiedad.Entidades
{
    public  class ComposicionGnaPromedio
    {

        public int IdLote { get; set; }
        public int IdSuministradorComponente { get; set; }
        public double? Porcentaje { get; set; }
        public DateTime? Fecha { get; set; }
        public bool EsHabilitado { get; set; }
        public DateTime Creado { get; set; }
        public DateTime? Actualizado { get; set; }
        public long? IdUsuario { get; set; }
    }
}
