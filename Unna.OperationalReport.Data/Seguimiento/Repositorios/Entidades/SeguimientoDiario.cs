using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Data.Seguimiento.Repositorios.Entidades
{
    public class SeguimientoDiario
    {
        public int IdSeguimientoDiario { get; set; }
        public string Titulo { get; set; }
        public int IdEstadoColor { get; set; }
        public string Color { get; set; } 
        public string ColorTexto { get; set; }
        public bool EsVisible { get; set; }
        public string NombreColumna { get; set; }
        public int Orden { get; set; }
        public int IdEstadoSeguimiento { get; set; }
        public DateTime Fecha { get; set; }
        public int IdModuloSeguimiento { get; set; }
    }
}
