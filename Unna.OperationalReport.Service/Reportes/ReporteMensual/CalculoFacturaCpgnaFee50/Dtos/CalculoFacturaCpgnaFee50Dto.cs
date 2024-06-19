using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Unna.OperationalReport.Service.Mantenimientos.TipoCambios.Dtos;

namespace Unna.OperationalReport.Service.Reportes.ReporteMensual.CalculoFacturaCpgnaFee50.Dtos
{
    public class CalculoFacturaCpgnaFee50Dto
    {
        public string? NombreReporte { get; set; }

        //A) Determinación del PRef - (Precio de Lista del GLP de la Refinería de PETROPERU en Talara)
        public List<PrecioGlpPeriodo>? PrecioGlp { get; set; }
        public double GravedadEspecifica { get; set; }
        public double PrefPromedioPeriodo { get; set; }
        public double Factor { get; set; }
        public double TipoCambioPromedio { get; set; }
        public double PrefPeríodo { get; set; }
        public List<TipoCambioDto>? TipoCambio { get; set; }

        //B) Determinación del Precio de los Componentes Pesados.
        public double Pref { get; set; }
        public double Vglp { get; set; }
        public double Vhas { get; set; }
        public double Precio { get; set; }

        //C) Determinación del Porcentaje de Contraprestación (% CM)

        public double? PrecioDeterminacion { get; set; }
        public double? VolumenProcesamientoGna { get; set; }
        public double? Cm { get; set; }

        //D) Determinación de la Facturación por la Contraprestación del Suministro de Componentes 
        public double PrecioFacturacion { get; set; }
        public double PSec { get; set; }
        public double Vtotal { get; set; }
        public double CmPrecioPsec { get; set; }
        public double ImporteCmEep  { get; set; }
        public double IgvCmEep { get; set; }
        public double MontoTotalCmEep { get; set; }
        public double PrecioSecado { get; set; }
        public double Igv { get; set; }
        public double Total { get; set; }


        [JsonIgnore]
        public long? IdUsuario { get; set; }

        // Segunda pestaña
        public List<ResumenEntregaDto>? ResumenEntrega { get; set; }
        public List<BarrilesProductoDto>? BarrilesProducto { get; set; }
        public double CentajeGlp { get; set; }
        public double CentajeCgn { get; set; }
        public double CentajeTotal { get; set; }
        public double NumeroDiasPeriodo { get; set; }
        public double Mmpcd { get; set; }

    }

    public class PrecioGlpPeriodo
    {
        public string? Id { get; set; }
        public DateTime? Desde { get; set; }
        public DateTime? Hasta { get; set; }
        public double? PrecioKg { get; set; }
        public int? NroDias { get; set; }

        [JsonIgnore]
        public DateTime DiaOperativo { get; set; }


        public string? DesdeCadena
        {
            get
            {
                return Desde.HasValue ? Desde.Value.ToString("dd/MM/yyyy"):null;
            }
        }

        public string? HastaCadena
        {
            get
            {
                return Hasta.HasValue ? Hasta.Value.ToString("dd/MM/yyyy"):null;
            }
        }

    }
}
