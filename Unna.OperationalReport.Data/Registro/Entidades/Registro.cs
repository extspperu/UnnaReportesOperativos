using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Data.Registro.Entidades
{
    public class Registro
    {
        public long IdRegistro { get; set; }
        public double? Valor { get; set; }
        public bool? EsConciliado { get; set; }
        public int IdDato { get; set; }
        public long IdDiaOperativo { get; set; }        
        public long? IdUsuario { get; set; }
        public bool? EsValido { get; set; }
        public DateTime? FechaValido { get; set; }
        public long? IdUsuarioValidador { get; set; }
        public bool? EsEditadoPorProceso { get; set; }
        public bool? EsDevuelto { get; set; }

        public DateTime Creado { get; set; }
        public DateTime Actualizado { get; set; }
        public DateTime? Borrado { get; set; }
        public bool EstaBorrado { get; set; }

        public virtual DiaOperativo? DiaOpetarivo { get; set; }
        public virtual Dato? Dato { get; set; }
        public Registro()
        {
            Creado = DateTime.UtcNow;
            Actualizado = DateTime.UtcNow;
            EstaBorrado = false;
        }
        public DateTime Fecha { get; set; }

    }
}
