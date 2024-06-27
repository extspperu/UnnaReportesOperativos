using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaDeterminacionVolumenGna.Calculos.Dtos
{
    public class CalculosLoteIvDto
    {
        // Hoja 1
        public List<PropiedadesFisicasDto>? PropiedadesGpa { get; set; }
        public List<PropiedadesFisicasDto>? PropiedadesGpsa { get; set; }

        // Hoja 2
        public List<ComponsicionGnaEntradaDto>? ComponsicionGnaEntrada { get; set; }

        // Hoja 3
        public CantidadCalidadDto? CantidadCalidad { get; set; }

        // Hoja 4
        public DeterminacionFactorConvertirVolumenLgnDto? DeterminacionFactorConvertirVolumenLgn { get; set; }

    }
}
