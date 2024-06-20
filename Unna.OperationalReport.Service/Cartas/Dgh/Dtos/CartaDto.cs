using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Service.Cartas.Dgh.Dtos
{
    public class CartaDto
    {
        public string? Periodo { get; set; }
        public string? Direccion { get; set; }
        public string? Telefono { get; set; }
        public string? SitioWeb { get; set; }
        public string? NombreArchivo { get; set; }
        public string? UrlFirma { get; set; }


        [JsonIgnore]
        public long? IdUsuario { get; set; }


        public CartaSolicitudDto? Solicitud { get; set; }

        //INFORME MENSUAL PARA EL MINISTERIO DE ENERGIA Y MINAS (SEGUNDA HOJA)
        public Osinergmin1Dto? Osinergmin1 { get; set; }

        //INFORME MENSUAL PARA EL MINISTERIO DE ENERGIA Y MINAS (TERCERA HOJA)
        public Osinergmin2Dto? Osinergmin2 { get; set; }



        //5ta  REPORTE CALIDAD DE PRODUCTOS
        public CalidadProductoDto? CalidadProducto { get; set; }

        //6ta HOJA DE LA CARTA - REPORTE  ANÁLISIS  CROMATOGRÁFICO        
        public ReporteAnalisisCromatograficoDto? AnalisisCromatografico { get; set; }


    }


}
