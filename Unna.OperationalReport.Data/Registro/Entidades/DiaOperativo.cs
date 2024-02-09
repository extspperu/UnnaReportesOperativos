using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Data.Registro.Entidades
{
    public class DiaOperativo
    {
        public long IdDiaOperativo { get; set; }
        public DateTime Fecha { get; set; }
        public int? NumeroRegistro { get; set; }
        public string? Adjuntos { get; set; }
        public string? Comentario { get; set; }
        public DateTime Creado { get; set; }
        public DateTime Actualizado { get; set; }
        public DateTime? Borrado { get; set; }
        public bool EstaBorrado { get; set; }
        public long? IdUsuario { get; set; }
        public bool? DatoCulminado { get; set; }
        public bool? DatoValidado { get; set; }
        public int? IdLote { get; set; }
        public bool? EsObservado { get; set; }
        public DateTime? FechaObservado { get; set; }
        public long? IdUsuarioObservado { get; set; }
        public DateTime? FechaValidado { get; set; }
        public long? IdUsuarioValidado { get; set; }
        public int? IdGrupo { get; set; }

        public virtual Lote? Lote { get; set; }
        public virtual ICollection<Registro>? Registros { get; set; }

        public DiaOperativo()
        {
            Creado = DateTime.UtcNow;
            Actualizado = DateTime.UtcNow;
            EstaBorrado = false;
        }

    }
}
