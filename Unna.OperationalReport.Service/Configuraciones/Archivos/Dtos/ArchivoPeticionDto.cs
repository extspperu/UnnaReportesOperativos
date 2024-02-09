using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Service.Configuraciones.Archivos.Dtos
{
    public class ArchivoPeticionDto
    {
        [DataType(DataType.Upload)]
        [Required(ErrorMessage = "Imagen es requerido")]
        public IFormFile? Imagen { get; set; }
        public string? RutaCarpeta { get; set; }
    }
}
