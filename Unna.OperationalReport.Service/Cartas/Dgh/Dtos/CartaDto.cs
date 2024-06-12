using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Service.Cartas.Dgh.Dtos
{
    public class CartaDto
    {
        public CartaSolicitudDto? Solicitud { get; set; }
        public Osinerg1Dto? Osinergmin1 { get; set; }

        //public CartaSolicitudDto? InformeMensual { get; set; }
    }

   
}
