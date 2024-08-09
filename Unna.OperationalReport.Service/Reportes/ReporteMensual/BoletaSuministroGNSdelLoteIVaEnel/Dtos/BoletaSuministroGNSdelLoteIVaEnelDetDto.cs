﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletaSuministroGNSdelLoteIVaEnel.Dtos
{
    public class BoletaSuministroGNSdelLoteIVaEnelDetDto
    {
        public int? Id { get; set; }
        public string? Fecha { get; set; }
        public double Volumen { get; set; }
        public double PoderCalorifico { get; set; }
        public double Energia { get; set; }
    }
}
