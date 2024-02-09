using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Configuracion.Entidades;

namespace Unna.OperationalReport.Data.Reporte.Entidades
{
    public class AdjuntoSupervisor
    {
        public long IdAdjuntoSupervisor { get; set; }
        public bool? EsConciliado { get; set; }
        public int? IdAdjunto { get; set; }
        public long? IdArchivo { get; set; }
        public long? IdRegistroSupervisor { get; set; }       

        public DateTime Creado { get; set; }
        public DateTime Actualizado { get; set; }
        public DateTime? Borrado { get; set; }
        public bool EstaBorrado { get; set; }

        public virtual RegistroSupervisor? RegistroSupervisor { get; set; }
        public virtual Adjunto? Adjunto { get; set; }
        public virtual Archivo? Archivo { get; set; }

        public AdjuntoSupervisor()
        {
            Creado = DateTime.UtcNow;
            Actualizado = DateTime.UtcNow;
            EstaBorrado = false;
        }

    }
}
