﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Data.Reporte.Entidades
{
    public class AdjuntoCorreo
    {
        public int IdConfiguracion { get; set; }
        public int IdConfiguracionReferencia { get; set; }
        public string? RutaArchivoPdf { get; set; }
        public string? RutaArchivoExcel { get; set; }
        public long? IdImprimir { get; set; }
    }
}
