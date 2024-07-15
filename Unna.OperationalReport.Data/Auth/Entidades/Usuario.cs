using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Configuracion.Entidades;

namespace Unna.OperationalReport.Data.Auth.Entidades
{
    public class Usuario
    {
        public long IdUsuario { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? PasswordSalt { get; set; }
        public int? IdGrupo { get; set; }
        public bool EstaHabilitado { get; set; }
        public bool EstaBorrado { get; set; }
        public DateTime Creado { get; set; }
        public DateTime Actualizado { get; set; }
        public DateTime? Borrado { get; set; }
        public long? IdPersona { get; set; }
        public DateTime? UltimoLogin { get; set; }
        public bool EsAdministrador { get; set; }
        public long? IdFirma { get; set; }
        public bool EsUsuarioExterno { get; set; }

        public virtual Persona? Persona { get; set; }
        public virtual Archivo? Firma { get; set; }
    }
}
