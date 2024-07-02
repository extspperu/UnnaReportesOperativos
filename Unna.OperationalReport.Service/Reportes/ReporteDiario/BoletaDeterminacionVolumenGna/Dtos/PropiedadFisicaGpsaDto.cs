using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Registro.Entidades;

namespace Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaDeterminacionVolumenGna.Dtos
{
    public class PropiedadFisicaGpsaDto 
    {
        public int IdSuministradorComponente {  get; set; }
        public string NombreSuministrador {  get; set; }
        public double PoderCalorificoPropiedad {  get; set; }
        public double RelacionVolumen {  get; set; }
        public double PesoMolecular {  get; set; }
        public double Densidad {  get; set; }
        public double MolecularCentaje {  get; set; }
        //public DateTime Fecha { get; set; }
        public double PoderCalorificoBruto { get; set; }
    }
}
