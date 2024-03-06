using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Service.Registros.CargaSupervisorPgt.Dtos
{
    public class GnsVolumeMsYPcBrutoDto
    {
        public long Id { get; set; }
        public string? Nombre { get; set; }
        public double? VolumeMs { get; set; }
        public double? PcBrutoRepCroma { get; set; }
        public string? Tipo { get; set; }
    }
}
