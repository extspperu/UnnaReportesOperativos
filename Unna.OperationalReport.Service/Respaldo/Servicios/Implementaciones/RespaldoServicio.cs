using Microsoft.Graph;
using Azure.Identity;
using System;
using System.IO;
using System.Threading.Tasks;
using Unna.OperationalReport.Service.Respaldo.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Microsoft.Graph.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using RestSharp;
using Microsoft.Identity.Client;
using Unna.OperationalReport.Data.Registro.Entidades;
using Unna.OperationalReport.Service.Usuarios.Dtos;
using Unna.OperationalReport.Service.Respaldo.Dtos;

namespace Unna.OperationalReport.Service.Respaldo.Servicios.Implementaciones
{
    public class RespaldoServicio : IRespaldoServicio
    {



        public async Task<OperacionDto<RespuestaSimpleDto<bool>>> EnviarAsync(RespaldoDto peticion)
        {
            await Task.Delay(0);
            if (peticion == null)
            {
                return new OperacionDto<RespuestaSimpleDto<bool>>(CodigosOperacionDto.NoExiste, "Parametro incorrecto");
            }

            //Configuración de credenciales
            //string clientId = "your-client-id";
            //string tenantId = "your-tenant-id";
            //string clientSecret = "your-client-secret";
            //string filePath = @"C:\ruta\del\archivo.txt"; // Ruta del archivo local a subir
            //string oneDriveFolderPath = "/"; // Carpeta de destino en OneDrive (root en este caso)
            //Autenticación usando Azure Identity
            //var clientSecretCredential = new ClientSecretCredential(tenantId, clientId, clientSecret);
            //var graphClient = new GraphServiceClient(clientSecretCredential);
            //Leer el archivo desde el disco y subirlo a OneDrive
            //using (var stream = new FileStream(filePath, FileMode.Open))
            //{
            //    var uploadedFile = await graphClient.Me
            //                                   .Drive
            //                                   .Root
            //                                   .ItemWithPath(oneDriveFolderPath + Path.GetFileName(filePath))
            //                                   .Content
            //                                   .Request()
            //                                   .PutAsync<DriveItem>(stream);
            //    var driveItem = await graphClient.Me.Drive.GetAsync();
            //    var uploadedFile = await graphClient.Me
            //                                        .Drives[driveItem.Id]
            //                                        .ItemWithPath(oneDriveFolderPath + Path.GetFileName(filePath))
            //                                        .Content
            //                                        .Request()
            //                                        .PutAsync<DriveItem>(stream);
            //    var driveItem = await graphClient.Me.Drive.GetAsync();
            //    var children = await graphClient.Drives[driveItem.Id].Root.ItemWithPath(oneDriveFolderPath + Path.GetFileName(filePath))
            //                                        .Content
            //                                        .Request()
            //                                        .PutAsync<DriveItem>(stream);
            //    Console.WriteLine($"Archivo '{uploadedFile.Name}' subido con éxito a OneDrive. ID del archivo: {uploadedFile.Id}");
            //}





            string clientId = "your-client-id";
            string tenantId = "your-tenant-id";
            string clientSecret = "your-client-secret";
            //string filePath = @"C:\ruta\del\archivo.txt"; // Ruta del archivo local a subir
            string oneDriveFolderPath = "/"; // Carpeta de destino en OneDrive (root en este caso)
            // Autenticación usando Azure Identity
            var clientSecretCredential = new ClientSecretCredential(tenantId, clientId, clientSecret);
            //var graphClient = new GraphServiceClient(clientSecretCredential);


            var interactiveBrowserCredential = new InteractiveBrowserCredential(new InteractiveBrowserCredentialOptions
            {
                ClientId = clientId,
                TenantId = tenantId,
            });

            var graphClient = new GraphServiceClient(interactiveBrowserCredential);


            // Leer el archivo desde el disco y subirlo a OneDrive
            //using (var stream = new FileStream(peticion.FilePath, FileMode.Open))
            //{
            //    var uploadedFile = await graphClient.Me
            //                                        .Drive
            //                                        .Root
            //                                        .ItemWithPath(oneDriveFolderPath + Path.GetFileName(filePath))
            //                                        .Content
            //                                        .Request()
            //                                        .PutAsync<DriveItem>(stream);

            //    Console.WriteLine($"Archivo '{uploadedFile.Name}' subido con éxito a OneDrive. ID del archivo: {uploadedFile.Id}");
            //}


            return new OperacionDto<RespuestaSimpleDto<bool>>(new RespuestaSimpleDto<bool> { Id = true });


        }







    }
}
