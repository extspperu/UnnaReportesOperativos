using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Data.Registro.Entidades
{
    public class GnsVolumeMsYPcBruto
    {
        public long Id { get; set; }
        public string? Nombre { get; set; }
        public double? VolumeMs { get; set; }
        public double? PcBrutoRepCroma { get; set; }
        public string? Tipo { get; set; }
        public DateTime Creado { get; set; }
        public DateTime? Actualizado { get; set; }
        public long? IdRegistroSupervisor { get; set; }
    }
}
