using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletaVentaGnsUnnaEnergiaLimagas.Dtos
{
    public class BoletaVentaGnsUnnaEnergiaLimagasDto
    {
        public string? NombreReporte { get; set; }        
        public string? Periodo { get; set; }        
        public List<BoletaVentaMensualDto>? BoletaVentaMenensual { get; set; }
        public double TotalVolumen { get; set; }
        public double TotalPoderCalorifico { get; set; }
        public double TotalEnergia { get; set; }


        public double EnergiaVolumenSuministrado { get; set; }
        public double PrecioBase { get; set; }
        public double Fac { get; set; }
        public double CPIo { get; set; }
        public double CPIi { get; set; }
        public double SubTotal { get; set; }
        public double Igv { get; set; }
        public double IgvCentaje { get; set; }
        public double Total { get; set; }
        public string? Comentario { get; set; }
        public string? UrlFirma { get; set; }


        [JsonIgnore]
        public long? IdUsuario { get; set; }
        
        
    }

    public class BoletaVentaMensualDto
    {
        public int Id { get; set; }
        public string? Fecha { get; set; }
        public string? Placa { get; set; }
        public string? FechaInicioCarga { get; set; }
        public string? FechaFinCarga { get; set; }
        public string? NroConstanciaDespacho { get; set; }
        public double? Volumen { get; set; }
        public double? PoderCalorifico { get; set; }
        public double? Energia { get; set; }

    }
}
