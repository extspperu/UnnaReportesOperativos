using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Service.Mantenimientos.ValoresDefectoReportes.Dtos
{
    public class ValoresDefectoReporteDto
    {
        public string? Id {  get; set; }
        public string? Llave {  get; set; }
        public double? Valor {  get; set; }
        public string? Comentario {  get; set; }
        public bool EstaHabilitado {  get; set; }
        public string? Creado {  get; set; }
        public string? Actualizado {  get; set; }

        [JsonIgnore]
        public long? IdUsuario {  get; set; }

    }
}
