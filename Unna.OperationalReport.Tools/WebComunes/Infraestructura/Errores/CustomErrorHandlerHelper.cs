using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Tools.WebComunes.Infraestructura.Extensiones;

namespace Unna.OperationalReport.Tools.WebComunes.Infraestructura.Errores
{
    public static class CustomErrorHandlerHelper
    {
        public static void UseCustomErrors(this IApplicationBuilder app, IHostEnvironment environment, bool isWebsite = false)
        {
            if (environment.IsDevelopment())
            {

                app.Use((httpContext, next) =>
                {

                    return WriteDevelopmentResponse(httpContext, next, isWebsite);
                });
            }
            else
            {
                app.Use((httpContext, next) =>
                {
                    return WriteProductionResponse(httpContext, next, isWebsite);
                });
            }
        }

        private static Task WriteDevelopmentResponse(HttpContext httpContext, Func<Task> next, bool isWebsite)
        => WriteResponse(httpContext, includeDetails: true, isWebsite);

        private static Task WriteProductionResponse(HttpContext httpContext, Func<Task> next, bool isWebsite)
            => WriteResponse(httpContext, includeDetails: false, isWebsite);

        private static async Task WriteResponse(HttpContext context, bool includeDetails, bool isWebsite)
        {
            var isAjax = context.Request.IsAjaxRequest();

            var redireccionarAPagina = isWebsite && !isAjax;

            var exceptionHandlerPathFeature =
                                context.Features.Get<IExceptionHandlerPathFeature>();

            var errorGeneral = exceptionHandlerPathFeature?.Error;

            if (errorGeneral == null)
            {
                return;
            }

            if (errorGeneral is ApiError error)
            {
                if (redireccionarAPagina)
                {
                    if (includeDetails)
                    {
                        throw errorGeneral;
                    }
                    else
                    {
                        context.Response.Redirect("/Error");
                        return;

                    }
                }

                context.Response.StatusCode = (int)error.HttpCode;
                context.Response.ContentType = "application/json";
                var result = JsonConvert.SerializeObject(new { codigo = error.CodigoError, mensajes = error.Errores });
                await context.Response.WriteAsync(result);
                return;
            }


            var title = includeDetails ? "Ocurrio un error: " + errorGeneral.Message : "Ocurrio un error";
            var details = includeDetails ? errorGeneral.ToString() : null;

            if (!includeDetails)
            {
                //var errorServicio = context.RequestServices.GetService(typeof(IErrorServicio)) as IErrorServicio;

                //if (errorServicio != null)
                //{
                //    await errorServicio.RegistrarError(errorGeneral);

                //}

            }


            if (redireccionarAPagina)
            {
                if (includeDetails)
                {

                    throw errorGeneral;
                }
                else
                {
                    context.Response.Redirect("/Error");
                    return;
                }
            }

            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonConvert.SerializeObject(new { error = title, detalle = details }));

        }



    }
}
