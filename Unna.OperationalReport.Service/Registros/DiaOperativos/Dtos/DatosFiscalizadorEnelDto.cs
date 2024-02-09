using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Service.Registros.Datos.Dtos;

namespace Unna.OperationalReport.Service.Registros.DiaOperativos.Dtos
{
    public class DatosFiscalizadorEnelDto
    {
        public List<DatoDto>? Datos { get; set; }
        public string? IdGrupo { get; set; }
        public int? NumeroRegistro { get; set; }
        public string? Titulo { get; set; }
        public bool TieneDatoConsiliado { get; set; }
        public bool PermitirEditar { get; set; }
        public string? IdDiaOperativo { get; set; }

    }
}
