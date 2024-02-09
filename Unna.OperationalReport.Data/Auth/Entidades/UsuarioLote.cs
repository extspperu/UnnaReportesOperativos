using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Data.Auth.Entidades
{
    public class UsuarioLote
    {
        public long IdUsuario { get; set; }
        public int IdLote { get; set; }
        public DateTime Creado { get; set; }
        public bool EstaActivo { get; set; }
        public int? IdGrupo { get; set; }
    }
}
