using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Service.Reportes.ReporteDiario.ReporteDiarioPgt.Dtos
{
    public class GasNaturalAsociadoDto
    {
        public int? IdLote { get; set; }
        public string? Lote { get; set; }
        public double? Volumen { get; set; }
        public double? Calorifico { get; set; }
        public double? Riqueza { get; set; }
        public double? RiquezaBls { get; set; }
        public double? EnergiaDiaria { get; set; }
        public double? VolumenPromedio { get; set; }

        public double? VolumenPorderCalorifico
        {
            get
            {
                return Volumen * Calorifico;
            }
        }

        public double? VolumenRiqueza
        {
            get
            {
                return Volumen * Riqueza;
            }
        }

        public double? VolumenRiquezaBls
        {
            get
            {
                return Volumen * RiquezaBls;
            }
        }
    }
}
