using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaDeterminacionVolumenGna.Dtos
{
    public class FactoresAsignacionGasCombustibleDto
    {

        public int? Item { get; set; }
        public string? Suministrador { get; set; }
        public double Volumen { get; set; }
        public double Calorifico { get; set; }
        public double EnergiaMmbtu { get; set; }
        public double FactorAsignacion{ get; set; }
        public double Asignacion { get; set; }
        
        public double VolumenCalorifico
        {
            get
            {
                return Volumen * Calorifico;
            }
        }
    }
}
