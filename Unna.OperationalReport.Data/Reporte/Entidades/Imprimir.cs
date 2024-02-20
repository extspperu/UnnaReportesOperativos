using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Configuracion.Entidades;

namespace Unna.OperationalReport.Data.Reporte.Entidades
{
    public class Imprimir
    {

        public long IdImprimir { get; set; }
        public int IdConfiguracion { get; set; }
        public DateTime Fecha { get; set; }
        public string? Datos { get; set; }
        public long? IdUsuario { get; set; }
        public DateTime Creado { get; set; }
        public DateTime Actualizado { get; set; }
        public DateTime? Borrado { get; set; }
        public bool EstaBorrado { get; set; }

        public virtual Configuracion? Configuracion { get; set; }
    }
}
