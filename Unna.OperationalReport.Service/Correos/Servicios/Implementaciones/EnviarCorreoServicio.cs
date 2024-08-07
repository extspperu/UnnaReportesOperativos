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
using Unna.OperationalReport.Data.Reporte.Repositorios.Implementaciones;

namespace Unna.OperationalReport.Service.Correos.Servicios.Implementaciones
{
    public class EnviarCorreoServicio : IEnviarCorreoServicio
    {
        private readonly IConfiguracionRepositorio _configuracionRepositorio;
        private readonly IImprimirRepositorio _imprimirRepositorio;
        private readonly IEnviarCorreoRepositorio _enviarCorreoRepositorio;
        private readonly GeneralDto _general;

        public EnviarCorreoServicio(
            IConfiguracionRepositorio configuracionRepositorio,
            IImprimirRepositorio imprimirRepositorio,
            IEnviarCorreoRepositorio enviarCorreoRepositorio,
            GeneralDto general
            )
        {
            _configuracionRepositorio = configuracionRepositorio;
            _imprimirRepositorio = imprimirRepositorio;
            _enviarCorreoRepositorio = enviarCorreoRepositorio;
            _general = general;
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
                Destinatario = !string.IsNullOrWhiteSpace(entidad.CorreoDestinatario) ? JsonConvert.DeserializeObject<List<string>>(entidad.CorreoDestinatario) : new List<string>(),
                Cc = !string.IsNullOrWhiteSpace(entidad.CorreoCc) ? JsonConvert.DeserializeObject<List<string>>(entidad.CorreoCc) : new List<string>(),
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
                dto.Destinatario = !string.IsNullOrWhiteSpace(enviarCorreo.Destinatario) ? JsonConvert.DeserializeObject<List<string>>(enviarCorreo.Destinatario) : new List<string>();
                dto.Cc = !string.IsNullOrWhiteSpace(enviarCorreo.Cc) ? JsonConvert.DeserializeObject<List<string>>(enviarCorreo.Cc) : new List<string>();
                dto.Asunto = enviarCorreo.Asunto;
                dto.Cuerpo = enviarCorreo.Cuerpo;
            }

            if (enviarCorreo != null && enviarCorreo.FueEnviado)
            {
                dto.MensajeAlert = $"El correo ya fue enviado para el reporte seleccionado para periodo {fechaCadena} y para enviar nuevamente confirme";
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
            correo.Adjuntos = null;
            correo.IdReporte = id;
            correo.Actualizado = DateTime.UtcNow;

            correo.FueEnviado = await EnviarMailAsync(correo);
            if (correo.FueEnviado)
            {
                correo.FechaEnvio = DateTime.UtcNow;
            }


            var imprimir = await _imprimirRepositorio.BuscarPorIdConfiguracionYFechaAsync(id, diaOperativo);
            if (imprimir != null && entidad.EnviarAdjunto)
            {
                List<string>? adjuntos = new List<string>();
                if (!string.IsNullOrWhiteSpace(imprimir.RutaArchivoExcel))
                {
                    adjuntos.Add(imprimir.RutaArchivoExcel);
                }
                if (!string.IsNullOrWhiteSpace(imprimir.RutaArchivoPdf))
                {
                    adjuntos.Add(imprimir.RutaArchivoPdf);
                }
                correo.Adjuntos = JsonConvert.SerializeObject(adjuntos);
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
    }
}
