﻿using DocumentFormat.OpenXml.Bibliography;
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
using Newtonsoft.Json;
using Unna.OperationalReport.Tools.Seguridad.Servicios.General.Dtos;

namespace Unna.OperationalReport.Service.Correos.Servicios.Implementaciones
{
    public class EnviarCorreoServicio : IEnviarCorreoServicio
    {
        private readonly IConfiguracionRepositorio _configuracionRepositorio;
        private readonly IImprimirRepositorio _imprimirRepositorio;
        private readonly IEnviarCorreoRepositorio _enviarCorreoRepositorio;
        private readonly GeneralDto _general;
        private readonly IAdjuntoCorreoRepositorio _adjuntoCorreoRepositorio;
        private readonly UrlConfiguracionDto _urlConfiguracion;

        public EnviarCorreoServicio(
            IConfiguracionRepositorio configuracionRepositorio,
            IImprimirRepositorio imprimirRepositorio,
            IEnviarCorreoRepositorio enviarCorreoRepositorio,
            GeneralDto general,
            IAdjuntoCorreoRepositorio adjuntoCorreoRepositorio,
            UrlConfiguracionDto urlConfiguracion
            )
        {
            _configuracionRepositorio = configuracionRepositorio;
            _imprimirRepositorio = imprimirRepositorio;
            _enviarCorreoRepositorio = enviarCorreoRepositorio;
            _general = general;
            _adjuntoCorreoRepositorio = adjuntoCorreoRepositorio;
            _urlConfiguracion = urlConfiguracion;
        }

        public async Task<OperacionDto<ConsultaEnvioReporteDto>> ObtenerAsync(string? idReporte, DateTime diaOperativo)
        {
            var id = RijndaelUtilitario.DecryptRijndaelFromUrl<int>(idReporte);
            var entidad = await _configuracionRepositorio.BuscarPorIdYNoBorradoAsync(id);
            if (entidad == null)
            {
                return new OperacionDto<ConsultaEnvioReporteDto>(CodigosOperacionDto.NoExiste, "No existe registro");
            }

            string? fechaCadena = default(string);

            switch (entidad.Grupo)
            {
                case GruposReportes.Quincenal:
                    if (diaOperativo.Day < 16) diaOperativo = diaOperativo.AddMonths(-1);
                    diaOperativo = new DateTime(diaOperativo.Year, diaOperativo.Month, 1);
                    fechaCadena = $"{FechasUtilitario.ObtenerNombreMes(diaOperativo)} {diaOperativo.Year}";
                    break;
                case GruposReportes.Mensual:
                    DateTime mensual = diaOperativo.AddMonths(-1);
                    diaOperativo = new DateTime(mensual.Year, mensual.Month, 16);
                    fechaCadena = $"{FechasUtilitario.ObtenerNombreMes(diaOperativo)} {diaOperativo.Year}";
                    break;
                case TiposGruposReportes.Diario:
                    fechaCadena = diaOperativo.ToString("dd/MM/yyyy");
                    break;
                default:
                    return new OperacionDto<ConsultaEnvioReporteDto>(CodigosOperacionDto.NoExiste, "No existe el grupo de reporte");
            }


            string? fechaActual = FechasUtilitario.ObtenerFechaSegunZonaHoraria(DateTime.UtcNow).ToString("dd/MM/yyyy");

            var imprimir = await _imprimirRepositorio.BuscarPorIdConfiguracionYFechaAsync(id, diaOperativo);

            string? cuerpo = !string.IsNullOrWhiteSpace(entidad.CorreoCuerpo) ? entidad.CorreoCuerpo.Replace("{{diaOperativo}}", fechaCadena).Replace("{{fecha}}", fechaActual) : null;

            string? mes = FechasUtilitario.ObtenerNombreMes(diaOperativo);
            if (!string.IsNullOrWhiteSpace(cuerpo))
            {
                cuerpo = cuerpo.Replace("{{anio}}", diaOperativo.Year.ToString());
                cuerpo = cuerpo.Replace("{{mes}}", mes);
                cuerpo = cuerpo.Replace("{{periodo}}", mes);
            }

            var dto = new ConsultaEnvioReporteDto()
            {
                IdReporte = idReporte,
                Destinatario = !string.IsNullOrWhiteSpace(entidad.CorreoDestinatario) ? JsonConvert.DeserializeObject<List<string>>(entidad.CorreoDestinatario) : new List<string>(),
                Cc = !string.IsNullOrWhiteSpace(entidad.CorreoCc) ? JsonConvert.DeserializeObject<List<string>>(entidad.CorreoCc) : new List<string>(),
                Asunto = ValidarAsunto(entidad.CorreoAsunto, entidad.Grupo),
                Cuerpo = cuerpo,
                NombreReporte = entidad.NombreReporte,
                DiaOperativo = diaOperativo,
                ReporteFueGenerado = imprimir != null,
                MensajeAlert = $"Valide los datos y luego confirme para poder enviar el correo de {fechaCadena}"
            };

            if (imprimir != null)
            {
                List<AdjuntoCorreoDto> adjuntos = new List<AdjuntoCorreoDto>();
                var adjuntosCorreo = await _adjuntoCorreoRepositorio.ListarPorIdReporteAsync(imprimir.IdConfiguracion, diaOperativo);
                foreach (var item in adjuntosCorreo)
                {
                    if (!string.IsNullOrWhiteSpace(item?.RutaArchivoExcel) && File.Exists(item?.RutaArchivoExcel))
                    {
                        adjuntos.Add(new AdjuntoCorreoDto
                        {
                            Id = RijndaelUtilitario.EncryptRijndaelToUrl(item.IdImprimir),
                            Tipo = "Excel",
                            Nombre = Path.GetFileName(item?.RutaArchivoExcel),
                            Url = $"{_urlConfiguracion.UrlBase}api/admin/correos/Adjunto/Descargar/{RijndaelUtilitario.EncryptRijndaelToUrl(item.IdImprimir)}/Excel"
                        });
                    }
                    if (!string.IsNullOrWhiteSpace(item?.RutaArchivoPdf) && File.Exists(item?.RutaArchivoPdf))
                    {
                        adjuntos.Add(new AdjuntoCorreoDto
                        {
                            Id = RijndaelUtilitario.EncryptRijndaelToUrl(item.IdImprimir),
                            Tipo = "Pdf",
                            Nombre = Path.GetFileName(item?.RutaArchivoPdf),
                            Url = $"{_urlConfiguracion.UrlBase}api/admin/correos/Adjunto/Descargar/{RijndaelUtilitario.EncryptRijndaelToUrl(item.IdImprimir)}/Pdf"
                        });
                    }
                }
                dto.Adjuntos = adjuntos;
            }

            var enviarCorreo = await _enviarCorreoRepositorio.BuscarPorIdReporteYFechaAsync(id, diaOperativo);
            if (enviarCorreo != null)
            {
                dto.IdReporte = idReporte;
                dto.FueEnviado = enviarCorreo.FueEnviado;
                dto.Destinatario = !string.IsNullOrWhiteSpace(enviarCorreo.Destinatario) ? JsonConvert.DeserializeObject<List<string>>(enviarCorreo.Destinatario) : new List<string>();
                dto.Cc = !string.IsNullOrWhiteSpace(enviarCorreo.Cc) ? JsonConvert.DeserializeObject<List<string>>(enviarCorreo.Cc) : new List<string>();
                dto.Asunto = enviarCorreo.Asunto;
                dto.Cuerpo = enviarCorreo.Cuerpo;
                dto.NombreReporte = entidad.NombreReporte;
                dto.DiaOperativo = diaOperativo;
                dto.Adjuntos = !string.IsNullOrWhiteSpace(enviarCorreo.Adjuntos) ? JsonConvert.DeserializeObject<List<AdjuntoCorreoDto>>(enviarCorreo.Adjuntos) : new List<AdjuntoCorreoDto>();
            }

            if (enviarCorreo != null && enviarCorreo.FueEnviado)
            {
                dto.MensajeAlert = $"El correo ya fue enviado para el reporte seleccionado para periodo {fechaCadena} y para enviar nuevamente confirme";
            }
            return new OperacionDto<ConsultaEnvioReporteDto>(dto);

        }

        public async Task<OperacionDto<RespuestaSimpleDto<bool>>> EnviarCorreoAsync(EnviarCorreoDto peticion)
        {
            if (!peticion.DiaOperativo.HasValue)
            {
                return new OperacionDto<RespuestaSimpleDto<bool>>(CodigosOperacionDto.NoExiste, "Dia operativo es requerido");
            }

            DateTime diaOperativo = peticion.DiaOperativo.Value;

            var id = RijndaelUtilitario.DecryptRijndaelFromUrl<int>(peticion.IdReporte);
            var entidad = await _configuracionRepositorio.BuscarPorIdYNoBorradoAsync(id);
            if (entidad == null)
            {
                return new OperacionDto<RespuestaSimpleDto<bool>>(CodigosOperacionDto.NoExiste, "No existe registro");
            }

            switch (entidad.Grupo)
            {
                case GruposReportes.Quincenal:
                    if (diaOperativo.Day < 16) diaOperativo = diaOperativo.AddMonths(-1);
                    diaOperativo = new DateTime(diaOperativo.Year, diaOperativo.Month, 1);
                    break;
                case GruposReportes.Mensual:
                    DateTime mensual = diaOperativo.AddMonths(-1);
                    diaOperativo = new DateTime(mensual.Year, mensual.Month, 16);
                    break;
            }

            var correo = await _enviarCorreoRepositorio.BuscarPorIdReporteYFechaAsync(id, diaOperativo);
            if (correo == null)
            {
                correo = new EnviarCorreo();
            }
            correo.Asunto = peticion.Asunto;
            correo.Cc = JsonConvert.SerializeObject(peticion.Cc);
            correo.Destinatario = JsonConvert.SerializeObject(peticion.Destinatario);
            correo.Cuerpo = peticion.Cuerpo;
            correo.Fecha = diaOperativo;
            correo.Creado = DateTime.UtcNow;
            correo.IdUsuario = peticion.IdUsuario;
            correo.IdReporte = id;
            correo.Actualizado = DateTime.UtcNow;
            if (correo.FueEnviado)
            {
                correo.FechaEnvio = DateTime.UtcNow;
            }

            //var imprimir = await _imprimirRepositorio.BuscarPorIdConfiguracionYFechaAsync(id, diaOperativo);
            //if (imprimir != null && entidad.EnviarAdjunto)
            //{
            //    List<string>? adjuntos = new List<string>();
            //    if (!string.IsNullOrWhiteSpace(imprimir.RutaArchivoExcel))
            //    {
            //        adjuntos.Add(imprimir.RutaArchivoExcel);
            //    }
            //    if (!string.IsNullOrWhiteSpace(imprimir.RutaArchivoPdf))
            //    {
            //        adjuntos.Add(imprimir.RutaArchivoPdf);
            //    }

            //}
            if (peticion.Adjuntos != null)
            {
                correo.Adjuntos = JsonConvert.SerializeObject(peticion.Adjuntos);
            }
            correo.IsBodyHtml = false;
            correo.FueEnviado = await EnviarMailAsync(correo);
            if (correo.FueEnviado)
            {
                correo.FechaEnvio = DateTime.UtcNow;
            }
            if (correo.IdEnviarCorreo > 0 && !correo.FueEnviado)
            {
                await _enviarCorreoRepositorio.EditarAsync(correo);
            }
            else
            {
                await _enviarCorreoRepositorio.InsertarAsync(correo);
            }

            if (!correo.FueEnviado)
            {
                return new OperacionDto<RespuestaSimpleDto<bool>>(CodigosOperacionDto.NoExiste, "No se pudo enviar el correo, no se obtuvo correctamente los credenciales de acceso");
            }

            return new OperacionDto<RespuestaSimpleDto<bool>>(new RespuestaSimpleDto<bool> { Id = true, Mensaje = "Se envió correctamente" });

        }


        public async Task<OperacionDto<ArchivoDto>> DescargarDocumentoAsync(string? tipoArchivo, string? idImprimir)
        {
            long id = RijndaelUtilitario.DecryptRijndaelFromUrl<long>(idImprimir);
            var imprimir = await _imprimirRepositorio.BuscarPorIdYNoBorradoAsync(id);
            if (imprimir == null)
            {
                return new OperacionDto<ArchivoDto>(CodigosOperacionDto.NoExiste, "No existe archivo");
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

            if (!File.Exists(rutaArchivo))
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

            if (_general == null || _general.Email == null)
            {
                return false;
            }

            bool val = true;
            var smtp = new SmtpClient
            {
                Host = _general.Email.Host,
                Port = _general.Email.Port ?? 0,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_general.Email.User, _general.Email.Psw),
                Timeout = 40000,
            };

            using (var message = new MailMessage()
            {
                From = new MailAddress(_general.Email.From),
                Subject = entidad.Asunto,
            })
            {
                message.IsBodyHtml = entidad.IsBodyHtml;
                message.Body = entidad.Cuerpo;
                if (!string.IsNullOrWhiteSpace(entidad.Adjuntos))
                {
                    List<AdjuntoCorreoDto>? adjuntos = JsonConvert.DeserializeObject<List<AdjuntoCorreoDto>>(entidad.Adjuntos);
                    foreach (var item in adjuntos)
                    {
                        var id = RijndaelUtilitario.DecryptRijndaelFromUrl<long>(item.Id);
                        var imprimir = await _imprimirRepositorio.BuscarPorIdYNoBorradoAsync(id);
                        if (imprimir == null || string.IsNullOrWhiteSpace(item.Tipo)) continue;

                        if (item.Tipo.Equals("Pdf") && !string.IsNullOrWhiteSpace(imprimir.RutaArchivoPdf))
                        {
                            var attachment = new System.Net.Mail.Attachment(imprimir.RutaArchivoPdf);
                            message.Attachments.Add(attachment);
                        }
                        if (item.Tipo.Equals("Excel") && !string.IsNullOrWhiteSpace(imprimir.RutaArchivoExcel))
                        {
                            var attachment = new System.Net.Mail.Attachment(imprimir.RutaArchivoExcel);
                            message.Attachments.Add(attachment);
                        }

                    }
                }
                List<string>? destinatarios = JsonConvert.DeserializeObject<List<string>>(entidad.Destinatario);
                if (destinatarios != null)
                {
                    destinatarios.ForEach(e => message.To.Add(e));
                }

                if (!string.IsNullOrWhiteSpace(entidad.Cc))
                {
                    List<string>? cc = JsonConvert.DeserializeObject<List<string>>(entidad.Cc);
                    if (cc != null)
                    {
                        cc.ForEach(e => message.CC.Add(e));
                    }
                }

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


        public async Task<OperacionDto<List<ListarCorreosEnviadosDto>>> ListarCorreosEnviadoAsync(BuscarCorreosEnviadosDto peticion)
        {
            int? idReporte = new int?();
            if (!string.IsNullOrWhiteSpace(peticion.IdReporte))
            {
                idReporte = RijndaelUtilitario.DecryptRijndaelFromUrl<int>(peticion.IdReporte);
            }
            var correos = await _enviarCorreoRepositorio.BuscarCorreosEnviadoAsync(peticion.DiaOperativo, peticion.Grupo, idReporte);

            var dto = correos.Select(e => new ListarCorreosEnviadosDto
            {
                IdReporte = RijndaelUtilitario.EncryptRijndaelToUrl(e.IdReporte),
                Asunto = ValidarAsunto(e.Asunto, peticion.Grupo),
                FechaEnvio = e.FechaEnvio.HasValue ? FechasUtilitario.ObtenerFechaSegunZonaHoraria(e.FechaEnvio.Value) : null,
                FueEnviado = e.FueEnviado,
                Grupo = e.Grupo,
                DiaOperativo = peticion.DiaOperativo.HasValue ? peticion.DiaOperativo.Value.ToString("dd/MM/yyyy") : null,
                NombreReporte = e.NombreReporte,
                IdEnviarCorreo = e.IdEnviarCorreo.HasValue ? RijndaelUtilitario.EncryptRijndaelToUrl(e.IdEnviarCorreo.Value) : null,
            }).ToList();

            return new OperacionDto<List<ListarCorreosEnviadosDto>>(dto);

        }

        private string? ValidarAsunto(string? asunto, string grupo)
        {
            if (string.IsNullOrWhiteSpace(asunto)) return asunto;

            DateTime diaOperativo = FechasUtilitario.ObtenerDiaOperativo();
            string? fechaInicial = default(string);
            string? fechaFinal = default(string);
            switch (grupo)
            {
                case GruposReportes.Quincenal:
                    if (diaOperativo.Day < 16) diaOperativo = diaOperativo.AddMonths(-1);
                    fechaInicial = new DateTime(diaOperativo.Year, diaOperativo.Month, 1).ToString("dd.MM.yyyy");
                    fechaFinal = new DateTime(diaOperativo.Year, diaOperativo.Month, 15).ToString("dd.MM.yyyy");
                    break;
                case GruposReportes.Mensual:
                    DateTime mensual = diaOperativo.AddMonths(-1);
                    fechaInicial = new DateTime(mensual.Year, mensual.Month, 1).ToString("dd.MM.yyyy");
                    fechaFinal = new DateTime(mensual.Year, mensual.Month, 1).AddMonths(1).AddDays(-1).ToString("dd.MM.yyyy");
                    break;
            }

            string? mes = FechasUtilitario.ObtenerNombreMes(diaOperativo);
            string? fechaActual = FechasUtilitario.ObtenerFechaSegunZonaHoraria(DateTime.UtcNow).ToString("dd.MM.yyyy");
            asunto = asunto.Replace("{{fecha}}", fechaActual);
            asunto = asunto.Replace("{{diaOperativo}}", diaOperativo.ToString("dd.MM.yyyy"));
            asunto = asunto.Replace("{{anio}}", diaOperativo.Year.ToString());
            asunto = asunto.Replace("{{mes}}", mes);
            asunto = asunto.Replace("{{periodo}}", mes);
            asunto = asunto.Replace("{{fechaInicial}}", fechaInicial);
            asunto = asunto.Replace("{{fechaFinal}}", fechaFinal);
            return asunto;
        }




    }
}
