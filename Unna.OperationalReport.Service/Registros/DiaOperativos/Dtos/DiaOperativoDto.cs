using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Service.Registros.Lotes.Dtos;

namespace Unna.OperationalReport.Service.Registros.DiaOperativos.Dtos
{
    public class DiaOperativoDto
    {

        public string? Id { get; set; }

        [Required(ErrorMessage = "Fecha es requerido")]
        public DateTime? Fecha { get; set; }
        public int? NumeroRegistro { get; set; }
        public string? Adjuntos { get; set; }
        public string? Comentario { get; set; }
        public DateTime Creado { get; set; }
        
        [JsonIgnore]
        public long? IdUsuario { get; set; }
        public bool? DatoCulminado { get; set; }
        public bool? DatoValidado { get; set; }

        public string? IdLote { get; set; }
        public bool? EsObservado { get; set; }
        public List<RegistroDto>? Registros { get; set; }
        public LoteDto? Lote { get; set; }

        [Required(ErrorMessage = "Grupo es requerido")]
        public string? IdGrupo { get; set; }

    }
}
