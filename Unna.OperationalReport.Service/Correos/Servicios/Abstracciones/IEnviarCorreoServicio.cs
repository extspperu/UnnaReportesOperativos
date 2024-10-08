﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Service.Configuraciones.Archivos.Dtos;
using Unna.OperationalReport.Service.Correos.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;

namespace Unna.OperationalReport.Service.Correos.Servicios.Abstracciones
{
    public interface IEnviarCorreoServicio
    {
        Task<OperacionDto<ConsultaEnvioReporteDto>> ObtenerAsync(string? idReporte, DateTime diaOperativo);        
        Task<OperacionDto<ArchivoDto>> DescargarDocumentoAsync(string? tipoArchivo, string? idReporte);
        Task<OperacionDto<RespuestaSimpleDto<bool>>> EnviarCorreoAsync(EnviarCorreoDto peticion);

        Task<OperacionDto<List<ListarCorreosEnviadosDto>>> ListarCorreosEnviadoAsync(BuscarCorreosEnviadosDto peticion);
    }
}
