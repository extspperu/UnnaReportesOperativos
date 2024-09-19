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
        public SharepointDto? Sharepoint { get; set; }
    }

    public class EmailDto
    {
        public string? Host { get; set; } 
        public int? Port { get; set; } 
        public string? User { get; set; } 
        public string? From { get; set; } 
        public string? Psw { get; set; } 
    }

    public class SharepointDto
    {
        public string? Instance { get; set; }
        public string? TenantId { get; set; }
        public string? ClientId { get; set; }
        public string? ClientSecret { get; set; }
        public string? DriveId { get; set; }
        public string? Site { get; set; }
        public bool Active { get; set; }
    }
}

