using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Tools.Seguridad.Servicios.General.Dtos
{
    public class GeneralDto
    {
        public string? RutaArchivos { get; set; }
        public EmailDto? Email { get; set; }
    }

    public class EmailDto
    {
        public string? Host { get; set; } 
        public int? Port { get; set; } 
        public string? User { get; set; } 
        public string? From { get; set; } 
        public string? Psw { get; set; } 
    }

  }

