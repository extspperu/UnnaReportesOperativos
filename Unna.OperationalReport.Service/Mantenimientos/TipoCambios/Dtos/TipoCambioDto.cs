using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Service.Mantenimientos.TipoCambios.Dtos
{
    public class TipoCambioDto
    {
        public DateTime Fecha { get; set; } 
        public double Cambio { get; set; } 
        public int IdTipoMoneda { get; set; }

 
        public string FechaCadena
        {
            get
            {
                return Fecha.ToString("dd/MM/yyyy");
            }
        }

    }
}
