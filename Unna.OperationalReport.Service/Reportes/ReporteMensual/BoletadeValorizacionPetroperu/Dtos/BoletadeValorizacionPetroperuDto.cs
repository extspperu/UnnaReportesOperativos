using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Unna.OperationalReport.Service.Reportes.Generales.Dtos;

namespace Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletadeValorizacionPetroperu.Dtos
{
    public class BoletadeValorizacionPetroperuDto
    {
        
        public string? Periodo { get; set; } 
        public List<BoletadeValorizacionPetroperuDetDto>? BoletadeValorizacionPetroperu {  get; set; }

        public double GnaLoteI { get; set; }
        public double EnergiaLoteI { get; set; }
        public double LgnRecuperadosLoteI { get; set; }
        public double GnaLoteVi { get; set; }
        public double EnergiaLoteVi { get; set; }
        public double LgnRecuperadosLoteVi { get; set; }
        public double GnaLoteZ69 { get; set; }
        public double EnergiaLoteZ69 { get; set; }
        public double LgnRecuperadosLoteZ69 { get; set; }
        public double TotalGna { get; set; }
        public double Eficiencia { get; set; }
        public double LiquidosRecuperados { get; set; }
        public double GnsLoteI { get; set; }
        public double GnsLoteVi { get; set; }
        public double GnsLoteZ69 { get; set; }
        public double GnsTotal { get; set; }
        public double EnergiaMmbtu { get; set; }
        public double ValorLiquidosUs { get; set; }
        public double CostoUnitMaquilaUsMmbtu { get; set; }
        public double CostoMaquilaUs { get; set; }

        public string? Observacion { get; set; }
        public string? ObservacionLoteI { get; set; }
        public string? ObservacionLoteVi { get; set; }
        public string? ObservacionLoteZ69 { get; set; }
        public double DensidadGlp { get; set; }
        public double MontoFacturarUnna { get; set; }
        public double MontoFacturarPetroperu { get; set; }



        public double EnergiaMmbtuLoteI { get; set; }
        public double EnergiaMmbtuLoteVi { get; set; }
        public double EnergiaMmbtuLoteZ69 { get; set; }
        public double ValorLiquidosLoteI { get; set; }
        public double ValorLiquidosLoteVi { get; set; }
        public double ValorLiquidosLoteZ69 { get; set; }
        public double CostoMaquillaLoteI { get; set; }
        public double CostoMaquillaLoteVi { get; set; }
        public double CostoMaquillaLoteZ69 { get; set; }
        public double MontoFacturarLoteI { get; set; }
        public double MontoFacturarLoteVi { get; set; }
        public double MontoFacturarLoteZ69 { get; set; }
        


        [JsonIgnore]
        public long? IdUsuario { get; set; }

        public string? NombreReporte { get; set; }
        public string? VersionReporte { get; set; }
        public string? CompaniaReporte { get; set; }
        public string? UrlFirma { get; set; }

        [JsonIgnore]
        public string? RutaFirma { get; set; }


    }
}
