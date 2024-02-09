using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Service.General.Dtos
{
    public class CifradoDto
    {
        public string LlaveDispositivo { get; set; }
        public string LlaveToken { get; set; }

        public CifradoDto(string llaveDispositivo,
                          string llaveToken)
        {
            LlaveDispositivo = llaveDispositivo;
            LlaveToken = llaveToken;
        }
    }
}
