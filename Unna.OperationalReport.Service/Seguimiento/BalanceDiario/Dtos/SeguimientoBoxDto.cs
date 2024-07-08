using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Service.Seguimiento.BalanceDiario.Dtos
{
    public class SeguimientoBoxDto
    {
        public string Title { get; set; }
        public string Color { get; set; }
        public string TextColor { get; set; }
        public bool IsVisible { get; set; }
        public int Order { get; set; }

        public SeguimientoBoxDto(string title, string color, string textColor, bool isVisible, int order)
        {
            Title = title;
            Color = color;
            TextColor = textColor;
            IsVisible = isVisible;
            Order = order;
        }
    }
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
}
