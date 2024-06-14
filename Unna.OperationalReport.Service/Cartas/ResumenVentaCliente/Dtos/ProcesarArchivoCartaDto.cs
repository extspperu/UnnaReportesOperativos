using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Service.Cartas.ResumenVentaCliente.Dtos
{
    public class ProcesarArchivoCartaDto
    {
        [Required(ErrorMessage = "File es requerido")]
        public IFormFile? File { get; set; }

        [Required(ErrorMessage = "Tipo es requerido")]
        public string? Tipo { get; set; }

        [Required(ErrorMessage = "Producto es requerido")]
        public string? Producto { get; set; }

        [JsonIgnore]
        public long? IdUsuario { get; set; }
    }
}
