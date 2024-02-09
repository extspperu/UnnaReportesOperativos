using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Reporte.Entidades;

namespace Unna.OperationalReport.Data.Configuracion.Entidades
{
    public class Archivo
    {
        public long Id { get; set; }
        public string NombreArchivoOriginal { get; set; }
        public string NombreArchivo { get; set; }
        public string RutaArchivo { get; set; }
        public int? IdTipoArchivo { get; set; }
        public DateTime Creado { get; set; }

        public DateTime Actualizado { get; set; }

        public virtual TipoArchivo TipoArchivo { get; set; }
        public virtual ICollection<AdjuntoSupervisor>? AdjuntoSupervisores { get; set; }
        public virtual ICollection<RegistroSupervisor>? RegistroSupervisores { get; set; }
        public Archivo()
        {
            Creado = DateTime.UtcNow;
            Actualizado = DateTime.UtcNow;
        }
    }
}
