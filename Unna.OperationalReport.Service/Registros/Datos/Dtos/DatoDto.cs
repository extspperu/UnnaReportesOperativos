﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Service.Registros.Datos.Dtos
{
    public class DatoDto
    {
        public int IdDato { get; set; }
        public string? Nombre { get; set; }
        public string? UnidadMedida { get; set; }
        public string? Tipo { get; set; }
    }
}
