using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos
{
    public class SimpleErrorDto
    {
        public int Codigo { get; set; }

        public List<string>? Mensajes { get; set; }
    }
}
