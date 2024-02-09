using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Service.General.Dtos
{
    public class ConfiguracionDto
    {
        public string IdDispositivo { get; set; }
        public string Token { get; set; }
        public ConfiguracionDto(string idDispositivo, string token)
        {
            IdDispositivo = idDispositivo;
            Token = token;
        }
    }

    }
