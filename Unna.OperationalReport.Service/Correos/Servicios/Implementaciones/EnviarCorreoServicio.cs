using DocumentFormat.OpenXml.Bibliography;
using NPOI.HPSF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Registro.Entidades;
using Unna.OperationalReport.Data.Reporte.Entidades;
using Unna.OperationalReport.Data.Reporte.Enums;
using Unna.OperationalReport.Data.Reporte.Repositorios.Abstracciones;
using Unna.OperationalReport.Service.Configuraciones.Archivos.Dtos;
using Unna.OperationalReport.Service.Correos.Dtos;
using Unna.OperationalReport.Service.Correos.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;
using DocumentFormat.OpenXml.Wordprocessing;
using Newtonsoft.Json;

namespace Unna.OperationalReport.Service.Correos.Servicios.Implementaciones
{
    public class EnviarCorreoServicio : IEnviarCorreoServicio
    {
        private readonly IConfiguracionRepositorio _configuracionRepositorio;
        private readonly IImprimirRepositorio _imprimirRepositorio;
        private readonly IEnviarCorreoRepositorio _enviarCorreoRepositorio;

        public EnviarCorreoServicio(
            IConfiguracionRepositorio configuracionRepositorio,
            IImprimirRepositorio imprimirRepositorio,
            IEnviarCorreoRepositorio enviarCorreoRepositorio
            )
        {
            _configuracionRepositorio = configuracionRepositorio;
            _imprimirRepositorio = imprimirRepositorio;
            _enviarCorreoRepositorio = enviarCorreoRepositorio;
        }

        public async Task<OperacionDto<ConsultaEnvioReporteDto>> ObtenerAsync(string? idReporte)
        {
            var id = RijndaelUtilitario.DecryptRijndaelFromUrl<int>(idReporte);
            var entidad = await _configuracionRepositorio.BuscarPorIdYNoBorradoAsync(id);
            if (entidad == null)
            {
                return new OperacionDto<ConsultaEnvioReporteDto>(CodigosOperacionDto.NoExiste, "No existe registro");
            }


            string? fechaCadena = default(string);

            DateTime diaOperativo = FechasUtilitario.ObtenerDiaOperativo();
            switch (entidad.Grupo)
            {
                case TiposGruposReportes.Mensual:
                case TiposGruposReportes.Quincenal:
                    DateTime fecha = diaOperativo.AddDays(1).AddMonths(-1);
                    diaOperativo = new DateTime(fecha.Year, fecha.Month, 1);
                    fechaCadena = $"{FechasUtilitario.ObtenerNombreMes(diaOperativo)} {diaOperativo.Year}";
                    break;
                case TiposGruposReportes.Diario:
                    fechaCadena = diaOperativo.ToString("dd/MM/yyyy");
                    break;
            }


            var imprimir = await _imprimirRepositorio.BuscarPorIdConfiguracionYFechaAsync(id, diaOperativo);


            var dto = new ConsultaEnvioReporteDto()
            {
                Destinatario = entidad.CorreoDestinatario,
                Cc = entidad.CorreoCc,
                Asunto = entidad.CorreoAsunto,
                Cuerpo = entidad.CorreoCuerpo,
                ReporteFueGenerado = imprimir != null,
                TieneArchivoExcel = !string.IsNullOrWhiteSpace(imprimir?.RutaArchivoExcel),
                TieneArchivoPdf = !string.IsNullOrWhiteSpace(imprimir?.RutaArchivoPdf),
                MensajeAlert = $"Valide los datos y luego confirme para poder enviar el correo de {fechaCadena}"
            };


            var enviarCorreo = await _enviarCorreoRepositorio.BuscarPorIdReporteYFechaAsync(id, diaOperativo);
            if (enviarCorreo != null)
            {
                dto.FueEnviado = enviarCorreo.FueEnviado;
                dto.Destinatario = enviarCorreo.Destinatario;
                dto.Asunto = enviarCorreo.Asunto;
                dto.Cuerpo = enviarCorreo.Cuerpo;
            }

            return new OperacionDto<ConsultaEnvioReporteDto>(dto);

        }



        public async Task<OperacionDto<RespuestaSimpleDto<bool>>> EnviarCorreoAsync(EnviarCorreoDto peticion)
        {
            var id = RijndaelUtilitario.DecryptRijndaelFromUrl<int>(peticion.IdReporte);
            var entidad = await _configuracionRepositorio.BuscarPorIdYNoBorradoAsync(id);
            if (entidad == null)
            {
                return new OperacionDto<RespuestaSimpleDto<bool>>(CodigosOperacionDto.NoExiste, "No existe registro");
            }

            DateTime diaOperativo = FechasUtilitario.ObtenerDiaOperativo();
            switch (entidad.Grupo)
            {
                case TiposGruposReportes.Mensual:
                case TiposGruposReportes.Quincenal:
                    DateTime fecha = diaOperativo.AddDays(1).AddMonths(-1);
                    diaOperativo = new DateTime(fecha.Year, fecha.Month, 1);
                    break;
            }


            List<string> Destinatarios = new List<string>();
            string[] peticionDestinatario = peticion.Destinatario.Split(',');
            foreach (var c in peticionDestinatario)
            {
                if (string.IsNullOrEmpty(c)) Destinatarios.Add(c.Trim());
            }

            List<string> Ccs = new List<string>();
            string[] peticionCc = peticion.Cc.Split(',');
            foreach (var c in peticionCc)
            {
                if (string.IsNullOrEmpty(c)) Ccs.Add(c.Trim());
            }

            var enviarCorreo = new EnviarCorreo
            {
                Asunto = peticion.Asunto,
                Cc = JsonConvert.SerializeObject(Ccs),
                Destinatario = JsonConvert.SerializeObject(Destinatarios),
                Cuerpo = peticion.Cuerpo,
                Fecha = diaOperativo,
                Creado = DateTime.UtcNow,
                IdUsuario = peticion.IdUsuario,
                Adjuntos = null,
                IdReporte = id,
                Actualizado = DateTime.UtcNow
            };

            enviarCorreo.FueEnviado = await EnviarMailAsync(enviarCorreo);

            await _enviarCorreoRepositorio.InsertarAsync(enviarCorreo);

            if (!enviarCorreo.FueEnviado)
            {
                return new OperacionDto<RespuestaSimpleDto<bool>>(CodigosOperacionDto.NoExiste, "No se pudo enviar el correo");
            }

            return new OperacionDto<RespuestaSimpleDto<bool>>(new RespuestaSimpleDto<bool> { Id = true, Mensaje = "Se envió correctamente" });

        }


        public async Task<OperacionDto<ArchivoDto>> DescargarDocumentoAsync(string? tipoArchivo, string? idReporte)
        {
            var id = RijndaelUtilitario.DecryptRijndaelFromUrl<int>(idReporte);
            var entidad = await _configuracionRepositorio.BuscarPorIdYNoBorradoAsync(id);
            if (entidad == null)
            {
                return new OperacionDto<ArchivoDto>(CodigosOperacionDto.NoExiste, "No existe registro");
            }

            DateTime diaOperativo = FechasUtilitario.ObtenerDiaOperativo();
            switch (entidad.Grupo)
            {
                case TiposGruposReportes.Mensual:
                case TiposGruposReportes.Quincenal:
                    DateTime fecha = diaOperativo.AddDays(1).AddMonths(-1);
                    diaOperativo = new DateTime(fecha.Year, fecha.Month, 1);
                    break;
            }

            var imprimir = await _imprimirRepositorio.BuscarPorIdConfiguracionYFechaAsync(id, diaOperativo);
            if (imprimir == null)
            {
                return new OperacionDto<ArchivoDto>(CodigosOperacionDto.NoExiste, "No existe registro");
            }

            string? rutaArchivo = default(string?);
            string? tipoMime = default(string?);
            switch (tipoArchivo)
            {
                case "Pdf":
                    rutaArchivo = imprimir.RutaArchivoPdf;
                    tipoMime = "application/pdf";
                    break;
                case "Excel":
                    rutaArchivo = imprimir.RutaArchivoExcel;
                    tipoMime = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    break;
            }

            if (string.IsNullOrWhiteSpace(rutaArchivo))
            {
                return new OperacionDto<ArchivoDto>(CodigosOperacionDto.NoExiste, "No existe archivo");
            }

            byte[] imageByteData = File.ReadAllBytes(rutaArchivo);

            var dto = new ArchivoDto()
            {
                Contenido = imageByteData,
                Nombre = Path.GetFileName(rutaArchivo),
                TipoMime = tipoMime,
                Ruta = rutaArchivo
            };


            return new OperacionDto<ArchivoDto>(dto);

        }





        public async Task<bool> EnviarMailAsync(EnviarCorreo entidad)
        {
            await Task.Delay(0);

            bool val = true;
            var smtp = new SmtpClient
            {
                Host = "smtp-mail.outlook.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("junior_vcocha@hotmail.com", "cochachin"),
                Timeout = 40000,
            };




            List<string> participantes = new List<string>();

            participantes.Add("villajmet@gmail.com");


            using (var message = new MailMessage()
            {
                From = new MailAddress(participantes.First()),
                Subject = entidad.Asunto,
            })
            {
                message.IsBodyHtml = true;
                if (!string.IsNullOrWhiteSpace(entidad.Adjuntos))
                {
                    List<string>? adjuntos = JsonConvert.DeserializeObject<List<string>>(entidad.Adjuntos);
                    foreach (var item in adjuntos)
                    {
                        var attachment = new System.Net.Mail.Attachment(item);
                        message.Attachments.Add(attachment);
                    }
                }
                participantes.ForEach(e => message.To.Add(e));
                try
                {
                    smtp.Send(message);
                }
                catch (Exception ex)
                {
                    val = false;
                }
                finally
                {
                    smtp.Dispose();
                }
            }
            return val;
        }
    }
}
