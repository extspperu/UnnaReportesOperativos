using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ResBalanceEnergLIV.Dtos
{
    public class ResBalanceEnergLIVPost
    {
        public long IdUsuario { get; set; }
        public string Mes { get; set; }
        public string Anio { get; set; }
        public List<DiaDatosDto> DatosDiarios { get; set; } = new List<DiaDatosDto>();
    }
    public class DiaDatosDto
    {
        public string Dia { get; set; }
        public List<MedicionDto> Mediciones { get; set; } = new List<MedicionDto>();
    }

    public class MedicionDto
    {
        public string ID { get; set; }
        public string Valor { get; set; }
    }
}
