using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Data.Configuracion.Entidades
{
    public class Empresa
    {
        public int IdEmpresa { get; set; }
        public string? RazonSocial { get; set; }
        public string? Ruc { get; set; }
        public string? Abreviatura { get; set; }
        public string? NombreCorto { get; set; }
        public string? Direccion { get; set; }
        public string? CodigoOsinergmin { get; set; }
        public string? NroRegistroHidrocarburo { get; set; }
        public string? Telefono { get; set; }
        public string? SitioWeb { get; set; }
    }
}
