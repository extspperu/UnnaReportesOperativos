using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Service.Seguimiento.BalanceDiario.Dtos
{
    public class ColumnaDto
    {
        public string Header { get; set; }
        public List<SeguimientoBoxDto> Boxes { get; set; }

        public ColumnaDto(string header, List<SeguimientoBoxDto> boxes)
        {
            Header = header;
            Boxes = boxes;
        }
    }

    public class SeguimientoBoxDto
    {
        public string Titulo { get; set; }
        public string Color { get; set; }
        public string ColorTexto { get; set; }
        public bool EsVisible { get; set; }
        public int Orden { get; set; }

        public SeguimientoBoxDto(string titulo, string color, string colorTexto, bool esVisible, int orden)
        {
            Titulo = titulo;
            Color = color;
            ColorTexto = colorTexto;
            EsVisible = esVisible;
            Orden = orden;
        }
    }

}
