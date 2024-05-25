using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ResBalanceEnergLIV.Dtos;

namespace Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ValorizacionVtaGns.Dtos
{
    public class ValorizacionVtaGnsPost
    {
        public long IdUsuario { get; set; }
        public string Lote {  get; set; }
        public string Mes { get; set; }
        public string Anio { get; set; }
        public string Comentario { get; set; }
        public List<MedicionDto> Mediciones { get; set; }
    }
    public class MedicionDto
    {
        public string Id { get; set; }
        public double Valor { get; set; }
    }
}
