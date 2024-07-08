using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Data.Mantenimiento.Entidades
{
    public class ValoresDefectoReporte
    {
        public string? Llave { get;set; }
        public double? Valor { get;set; }
        public string? Comentario { get;set; }
        public bool EstaHabilitado { get;set; }
        public DateTime Creado { get;set; }
        public DateTime Actualizado { get;set; }
        public long? IdUsuario { get;set; }
    }
}
