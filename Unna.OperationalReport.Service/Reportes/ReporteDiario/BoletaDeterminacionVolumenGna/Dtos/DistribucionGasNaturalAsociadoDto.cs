using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaDeterminacionVolumenGna.Dtos
{
    public class DistribucionGasNaturalAsociadoDto
    {

        public int? Item { get; set; }
        public string? Suministrador { get; set; }

        private double? _volumenGnsd;
        public double? VolumenGnsd
        {
            get => _volumenGnsd;
            set => _volumenGnsd = double.IsNaN(value.GetValueOrDefault()) ? 0 : value;
        }

        private double? _gasCombustible;
        public double? GasCombustible
        {
            get => _gasCombustible;
            set => _gasCombustible = double.IsNaN(value.GetValueOrDefault()) ? 0 : value;
        }

        private double? _volumenGns;
        public double? VolumenGns
        {
            get => _volumenGns;
            set => _volumenGns = double.IsNaN(value.GetValueOrDefault()) ? 0 : value;
        }

        public double VolumenGna { get; set; }


    }
}
