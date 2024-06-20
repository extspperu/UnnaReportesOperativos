using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Data.Carta.Procedimientos
{
    public class ListarCromatograficoPorLotes
    {
        public int Id { get; set; }
        public string? Componente { get; set; }
        public string? MetodoAstm { get; set; }
        public double? LoteZ69 { get; set; }
        public double? LoteX { get; set; }
        public double? LoteVi { get; set; }
        public double? LoteI { get; set; }
        public double? LoteIv { get; set; }
        public string? Grupo { get; set; }
    }
}
