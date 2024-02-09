using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Configuracion.Entidades;
using Unna.OperationalReport.Service.Configuraciones.Archivos.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;
using Unna.OperationalReport.Tools.Seguridad.Servicios.General.Dtos;

namespace Unna.OperationalReport.Service.Configuraciones.Archivos.Mapeo
{
    public class ArchivoProfileAction : IMappingAction<Archivo, ArchivoRespuestaDto>
    {
        private readonly UrlConfiguracionDto _urlConfiguracion;
        public ArchivoProfileAction(UrlConfiguracionDto urlConfiguracion)
        {
            _urlConfiguracion = urlConfiguracion;
        }

        public void Process(Archivo source, ArchivoRespuestaDto destination, ResolutionContext context)
        {
            destination.Id = RijndaelUtilitario.EncryptRijndaelToUrl(source.Id);
            destination.Url = $"{_urlConfiguracion.UrlBase}{"api/Archivo/"}{RijndaelUtilitario.EncryptRijndaelToUrl(source.Id)}";
        }
    }
}
