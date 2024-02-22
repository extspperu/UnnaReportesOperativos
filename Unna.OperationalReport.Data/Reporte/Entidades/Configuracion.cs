using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Data.Reporte.Entidades
{
    public class Configuracion
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public string? Version { get; set; }
        public string? PreparadoPör { get; set; }
        public string? AprobadoPor { get; set; }
        public string? Detalle { get; set; }
        public string? Grupo { get; set; }
        public DateTime Creado { get; set; }
        public DateTime Actualizado { get; set; }
        public DateTime? Borrado { get; set; }
        public bool EstaBorrado { get; set; }
        public string? NombreReporte { get; set; }
        public virtual ICollection<Imprimir>? Impresiones { get; set; }
    }
}
