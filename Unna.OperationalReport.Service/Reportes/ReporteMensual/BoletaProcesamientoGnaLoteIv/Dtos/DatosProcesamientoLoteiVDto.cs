using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletaProcesamientoGnaLoteIv.Dtos
{
    public class DatosProcesamientoLoteiVDto
    {
        public int Id { get; set; }
        public string? Dia { get; set; }
        public double Volumen { get; set; }
        public double PoderCalorifico { get; set; }
        public double Energia { get; set; }

    }
}
