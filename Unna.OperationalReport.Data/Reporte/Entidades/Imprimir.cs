﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Configuracion.Entidades;

namespace Unna.OperationalReport.Data.Reporte.Entidades
{
    public class Imprimir
    {

        public long IdImprimir { get; set; }
        public int IdConfiguracion { get; set; }
        public DateTime Fecha { get; set; }
        public string? Datos { get; set; }
        public long? IdUsuario { get; set; }
        public DateTime Creado { get; set; }
        public DateTime Actualizado { get; set; }
        public DateTime? Borrado { get; set; }
        public bool EstaBorrado { get; set; }
        public string? Comentario { get; set; }
        public bool EsEditado { get; set; }
        public string? RutaArchivoPdf { get; set; }
        public string? RutaArchivoExcel { get; set; }
        public bool TieneBackup { get; set; }
        public string? UrlBackup { get; set; }
        public DateTime? FechaBackup { get; set; }

        public virtual Configuracion? Configuracion { get; set; }

        public Imprimir()
        {
            Creado = DateTime.UtcNow;
            Actualizado = DateTime.UtcNow;
            EstaBorrado = false;
        }
    }
}
