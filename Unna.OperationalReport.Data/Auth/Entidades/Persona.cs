using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Data.Auth.Entidades
{
    public class Persona
    {
        public long IdPersona { get; set; }
        public string? Documento { get; set; }
        public string? Nombres { get; set; }
        public string? Paterno { get; set; }
        public string? Materno { get; set; }
        public string? Sexo { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public string? Direccion { get; set; }
        public string? Distrito { get; set; }
        public string? Provincia { get; set; }
        public string? Departamento { get; set; }
        public int? IdTipoPersona { get; set; }
        public string? Telefono { get; set; }
        public string? Correo { get; set; }
    }
}
