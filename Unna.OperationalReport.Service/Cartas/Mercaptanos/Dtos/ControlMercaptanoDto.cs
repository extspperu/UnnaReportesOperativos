using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Service.Cartas.Mercaptanos.Dtos
{
    public class ControlMercaptanoDto
    {
        public string? Fecha { get; set; }
        public string? Reponsable { get; set; }
        public string? Periodo { get; set; }
        public string? NivelInicial { get; set; }
        public string? FechaInicial { get; set; }
        public string? LitrosInicial { get; set; }
        public string? NivelFinal { get; set; }
        public string? FechaFinal { get; set; }
        public string? LitrosFinal { get; set; }

        public double? VolumenReposicionGal { get; set; }
        public double? VolumenReposicionLitros { get; set; }
        public double? ConsumoLitros { get; set; }
        public double? DespachoBarril { get; set; }
        public double? DespachoGalones { get; set; }
        public double? VolumenGlpBarriles { get; set; }
        public double? VolumenGlpM3 { get; set; }
        public double? CantidadDosificadaM3 { get; set; }
        public double? CantidadDosificadaGal { get; set; }
        public double? ConsumoMensual { get; set; }




    }
}
