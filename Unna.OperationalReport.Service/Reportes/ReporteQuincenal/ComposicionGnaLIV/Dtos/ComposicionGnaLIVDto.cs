using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Service.Reportes.Generales.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.FacturacionGnsLIV.Dtos;

namespace Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ComposicionGnaLIV.Dtos
{
    public class ComposicionGnaLIVDto
    {
        public List<ComposicionUnnaEnergiaLIVDto>? Composicion { get; set; }
        public List<ComposicionComponenteDto>? Componente { get; set; }

    }
}
