using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Configuracion.Entidades;

namespace Unna.OperationalReport.Data.Reporte.Entidades
{
    public class RegistroSupervisor
    {
        public long IdRegistroSupervisor { get; set; }
        public DateTime Fecha { get; set; }
        public string? Comentario { get; set; }
        public long? IdArchivo { get; set; }
        public DateTime Creado { get; set; }
        public DateTime Actualizado { get; set; }
        public DateTime? Borrado { get; set; }
        public bool EstaBorrado { get; set; }
        public long? IdUsuario { get; set; }

        public virtual ICollection<AdjuntoSupervisor>? AdjuntoSupervisores { get; set; }
        public virtual Archivo? Archivo { get; set; }
        public RegistroSupervisor()
        {
            Creado = DateTime.UtcNow;
            Actualizado = DateTime.UtcNow;
            EstaBorrado = false;
        }
    }
}
