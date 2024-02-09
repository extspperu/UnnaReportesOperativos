using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Data.Auth.Entidades
{
    public class Grupo
    {
        public int IdGrupo { get; set; }
        public string? Nombre { get; set; }
        public bool? EstaHabilitado { get; set; }
        public string? UrlDefecto { get; set; }
        public bool EstaBorrado { get; set; }
        public DateTime Creado { get; set; }
        public DateTime Actualizado { get; set; }
        public DateTime? Borrado { get; set; }

        public Grupo()
        {
            Creado = DateTime.UtcNow;
            Actualizado = DateTime.UtcNow;
            EstaBorrado = false;
        }

    }
}
