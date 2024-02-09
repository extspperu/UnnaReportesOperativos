using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;

namespace Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios
{
    public static class RestSharpUtilitario
    {
        public static SimpleRespuestaPeticionWebDto RealizarJsonPost(string url, object cuerpoPeticion, IDictionary<string, string?>? cabeceras = null, int segundosTimeout = 60, string? userAgent = null)
        {
            var restClient = new RestClient(url);

            //if (!string.IsNullOrWhiteSpace(userAgent))
            //{
            //    restClient.UserAgent = userAgent;
            //}

            var restRequest = new RestRequest()
            {
                Method = Method.Post
            };

            restRequest.AddHeader("Content-Type", "application/json");
            if (!string.IsNullOrWhiteSpace(userAgent))
            {
                restRequest.AddHeader("User-Agent", userAgent);
            }

            if (cabeceras != null)
            {
                foreach (var cabecera in cabeceras)
                {
                    restRequest.AddHeader(cabecera.Key, cabecera.Value ?? "");
                }
            }

            restRequest.AddJsonBody(cuerpoPeticion);

            restRequest.Timeout = segundosTimeout * 1000;
            var response = restClient.Execute(restRequest);

            return new SimpleRespuestaPeticionWebDto(
                response.StatusCode,
                response.Content
                );

        }

        public static byte[]? DescargarArchivo(string url)
        {

            var client = new RestClient(url);
            var request = new RestRequest()
            {
                Method = Method.Get
            };

            return client.DownloadData(request);
        }

        public static SimpleRespuestaPeticionWebDto RealizarJsonGet(string url, IDictionary<string, string?>? cabeceras = null, int segundosTimeout = 60, string? userAgent = null)
        {
            var restClient = new RestClient(url);

            //if (!string.IsNullOrWhiteSpace(userAgent))
            //{
            //    restClient.UserAgent = userAgent;
            //}

            var restRequest = new RestRequest()
            {
                Method = Method.Get
            };


            restRequest.AddHeader("Content-Type", "application/json");
            if (!string.IsNullOrWhiteSpace(userAgent))
            {
                restRequest.AddHeader("User-Agent", userAgent);
            }


            if (cabeceras != null)
            {
                foreach (var cabecera in cabeceras)
                {
                    restRequest.AddHeader(cabecera.Key, cabecera.Value ?? "");
                }
            }




            restRequest.Timeout = segundosTimeout * 1000;
            var response = restClient.Execute(restRequest);

            return new SimpleRespuestaPeticionWebDto(
                    response.StatusCode,
                    response.Content
                );
        }

        public static SimpleRespuestaPeticionWebDto RealizarJsonDelete(string url, IDictionary<string, string?>? cabeceras = null, int segundosTimeout = 60)
        {
            var restClient = new RestClient(url);
            var restRequest = new RestRequest()
            {
                Method = Method.Delete
            };

            restRequest.AddHeader("Content-Type", "application/json");
            if (cabeceras != null)
            {
                foreach (var cabecera in cabeceras)
                {
                    restRequest.AddHeader(cabecera.Key, cabecera.Value ?? "");
                }
            }


            restRequest.Timeout = segundosTimeout * 1000;
            var response = restClient.Execute(restRequest);

            return new SimpleRespuestaPeticionWebDto(
                 response.StatusCode,
                 response.Content
             );
        }
    }
}
