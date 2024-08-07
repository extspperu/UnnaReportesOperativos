using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Service.Registros.CargaSupervisorPgt.Dtos
{
    public class DataEntryDto
    {
        public string Date { get; set; }
        public string Average { get; set; }
    }

    public class GasDataDto
    {
        public string Name { get; set; }
        public string LastAverage { get; set; }
    }

    public class ExtractionResultDto
    {
        public List<GasDataDto> GasDataList { get; set; }
    }

}
