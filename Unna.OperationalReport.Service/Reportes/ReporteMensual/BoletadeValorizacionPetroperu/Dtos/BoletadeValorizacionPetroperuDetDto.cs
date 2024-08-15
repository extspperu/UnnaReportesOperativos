using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletadeValorizacionPetroperu.Dtos
{
    public class BoletadeValorizacionPetroperuDetDto
    {
        public DateTime Fecha { get; set; }
        public int Dia { get; set; }
        public double GnaLoteI { get; set; }
        public double PoderCalorificoLoteI { get; set; }
        public double EnergiaLoteI { get; set; }
        public double RiquezaLoteI { get; set; }
        public double RiquezaBlLoteI { get; set; }
        public double LgnRecuperadosLoteI { get; set; }
        public double GnaLoteVi { get; set; }
        public double PoderCalorificoLoteVi { get; set; }
        public double EnergiaLoteVi { get; set; }
        public double RiquezaLoteVi { get; set; }
        public double RiquezaBlLoteVi { get; set; }
        public double LgnRecuperadosLoteVi { get; set; }
        public double GnaLoteZ69 { get; set; }
        public double PoderCalorificoLoteZ69 { get; set; }
        public double EnergiaLoteZ69 { get; set; }
        public double RiquezaLoteZ69 { get; set; }
        public double RiquezaBlLoteZ69 { get; set; }
        public double LgnRecuperadosLoteZ69 { get; set; }
        public double TotalGna { get; set; }
        public double Eficiencia { get; set; }
        public double TotalLiquidosRecuperados { get; set; }
        public double GnsLoteI { get; set; }
        public double GnsLoteVi { get; set; }
        public double GnsLoteZ69 { get; set; }
        public double GnsTotal { get; set; }
        public double PoderCalorificoBtuPcsd { get; set; }
        public double EnergiaMmbtu { get; set; }

        public double PrecioGlpESinIgvSolesKg { get; set; }
        public double PrecioGlpGSinIGVSolesKg { get; set; }
        public double PrecioRefGlpSinIgvSolesKg { get; set; }
        public double PrecioGLPSinIgvUsBl { get; set; }
        public double TipodeCambioSolesUs { get; set; }
        public double PrecioCgnUsBl { get; set; }
        public double ValorLiquidosUs { get; set; }
        public double CostoUnitMaquilaUsMmbtu { get; set; }
        public double CostoMaquilaUs { get; set; }


        public double EnergiaMmbtuLoteI
        {
            get
            {
                return Math.Round(GnsLoteI * PoderCalorificoBtuPcsd, 4);
            }
        }

        public double EnergiaMmbtuLoteZ69
        {
            get
            {
                return Math.Round(GnsLoteZ69 * PoderCalorificoBtuPcsd, 4);
            }
        }

        public double EnergiaMmbtuLoteVi
        {
            get
            {
                return Math.Round(GnsLoteVi * PoderCalorificoBtuPcsd, 4);
            }
        }


        public double ValorLiquidosLotI
        {
            get
            {
                return ((0.75 * PrecioGLPSinIgvUsBl) + (0.25 * PrecioCgnUsBl)) * LgnRecuperadosLoteI;
            }
        }
        public double ValorLiquidosLotVi
        {
            get
            {
                return ((0.75 * PrecioGLPSinIgvUsBl) + (0.25 * PrecioCgnUsBl)) * LgnRecuperadosLoteVi;
            }
        }
        public double ValorLiquidosLotZ69
        {
            get
            {
                return ((0.75 * PrecioGLPSinIgvUsBl) + (0.25 * PrecioCgnUsBl)) * LgnRecuperadosLoteZ69;
            }
        }

        public double CostoMaquilaUsLotI
        {
            get
            {
                return Math.Round(GnsLoteI * PoderCalorificoBtuPcsd * CostoUnitMaquilaUsMmbtu, 4);
            }
        }
        public double CostoMaquilaUsLotVi
        {
            get
            {
                return Math.Round(GnsLoteVi * PoderCalorificoBtuPcsd * CostoUnitMaquilaUsMmbtu, 4);
            }
        }
        public double CostoMaquilaUsLotZ69
        {
            get
            {
                return Math.Round(GnsLoteZ69 * PoderCalorificoBtuPcsd * CostoUnitMaquilaUsMmbtu, 4);
            }
        }
        
        
       


    }
}
