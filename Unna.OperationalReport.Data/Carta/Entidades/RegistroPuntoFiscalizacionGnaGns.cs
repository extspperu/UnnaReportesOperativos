using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Data.Carta.Entidades
{
    public class RegistroPuntoFiscalizacionGnaGns
    {
        public long Id { get; set; }
        public DateTime Fecha { get; set; }        
        public double? C6 { get; set; }
        public double? C3 { get; set; }
        public double? Ic4 { get; set; }
        public double? Nc4 { get; set; }
        public double? NeoC5 { get; set; }
        public double? Ic5 { get; set; }
        public double? Nc5 { get; set; }
        public double? Nitrog { get; set; }
        public double? C1 { get; set; }
        public double? Co2 { get; set; }
        public double? C2 { get; set; }
        public double? O2 { get; set; }
        public double? Grav { get; set; }
        public double? Btu { get; set; }
        public double? Lgn { get; set; }
        public double? LgnRpte { get; set; }
        public bool Conciliado { get; set; }
        public string? Comentario { get; set; }
        public DateTime Creado { get; set; }
        public DateTime? Actualizado { get; set; }
        public long? IdRegistroCromatografia { get; set; }
        public long? IdUsuario { get; set; }
    }
}
