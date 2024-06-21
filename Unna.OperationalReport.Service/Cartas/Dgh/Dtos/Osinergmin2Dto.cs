using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Service.Cartas.Dgh.Dtos
{
    public class Osinergmin2Dto
    {

        //4,-VENTAS DE LIQUIDOS DEL GAS NATURAL
        public VentaLiquidosGasNaturalDto? VentaLiquidoGasNatural { get; set; }


        //5,-VOLUMENES Y PERSONAS A LAS QUE SE VENDIERON LOS PRODUCTOS
        public List<VolumenVendieronProductosDto>? Glp { get; set; }
        public List<VolumenVendieronProductosDto>? Cgn { get; set; }
        public double TotalVolumenPorCliente { get; set; }
        public string? ComentarioVolumenVendieronProductos { get; set; }

        //6,-INVENTARIOS DE FIN DE MES DE LIQUIDOS DEL GAS NATURAL
        public List<VolumenVendieronProductosDto>? InventarioLiquidoGasNatural { get; set; }
        public double TotalInventarioLiquidoGasNatural { get; set; }
        public string? ComentarioInventarioLiquidoGasNatural { get; set; }


    }

    public class VentaLiquidosGasNaturalDto
    {
        public double? Glp { get; set; }
        public double? PropanoSaturado { get; set; }
        public double? ButanoSaturado { get; set; }
        public double? Hexano { get; set; }
        public double? CondensadoGasNatural { get; set; }
        public double? CondensadoGasolina { get; set; }
        public double? Total { get; set; }
    }

    public class VolumenVendieronProductosDto
    {
        public int Item { get; set; }
        public string? Producto { get; set; }
        public double Bls { get; set; }
    }

}
