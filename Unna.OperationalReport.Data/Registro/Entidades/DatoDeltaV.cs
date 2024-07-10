using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Data.Registro.Entidades
{
    public class DatoDeltaV
    {
        public long Id { get; set; }
        public string? Tanque { get; set; }
        public double? Nivel { get; set; }
        public double? Pres { get; set; }
        public double? Temp { get; set; }
        public DateTime Creado { get; set; }
        public DateTime? Actualizado { get; set; }
        public long? IdRegistroSupervisor { get; set; }
        public double? GravedadEspecifica { get; set; }
        public double? Api { get; set; }
        




        
        public string? Producto { get; set; }

    }
}
