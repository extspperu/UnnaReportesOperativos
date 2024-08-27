
using Unna.OperationalReport.Service.Respaldo.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Service.Respaldo.Dtos;
using Microsoft.Graph;
using Microsoft.Identity.Client;
using System.Net.Http.Headers;

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





            //string clientId = "b05ddf25-3e5c-4e3f-8b4f-c465130e7a6d";
            //string tenantId = "your-tenant-id";
            //string clientSecret = "lU58Q~5SOHyPOVmHwr3x~IDtt1Ob-.Sty0TjxcEw";
            ////string filePath = @"C:\ruta\del\archivo.txt"; // Ruta del archivo local a subir
            //string oneDriveFolderPath = "/"; // Carpeta de destino en OneDrive (root en este caso)
            //// Autenticación usando Azure Identity
            //var clientSecretCredential = new ClientSecretCredential(tenantId, clientId, clientSecret);
            ////var graphClient = new GraphServiceClient(clientSecretCredential);


            //var interactiveBrowserCredential = new InteractiveBrowserCredential(new InteractiveBrowserCredentialOptions
            //{
            //    ClientId = clientId,
            //    TenantId = tenantId,
            //});

            //var graphClient = new GraphServiceClient(interactiveBrowserCredential);


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
            await Main();

            return new OperacionDto<RespuestaSimpleDto<bool>>(new RespuestaSimpleDto<bool> { Id = true });


        }



        public async Task<string> GetAccessTokenAsync()
        {
            //var clientId = "your_client_id";
            var tenantId = "your_tenant_id";
            //var clientSecret = "your_client_secret";
            
            string clientId = "b05ddf25-3e5c-4e3f-8b4f-c465130e7a6d";            
            string clientSecret = "lU58Q~5SOHyPOVmHwr3x~IDtt1Ob-.Sty0TjxcEw";

            var authority = $"https://login.microsoftonline.com/{tenantId}";
            var app = ConfidentialClientApplicationBuilder.Create(clientId)
                .WithClientSecret(clientSecret)
                .WithAuthority(new Uri(authority))
                .Build();

            var scopes = new[] { "https://graph.microsoft.com/.default" };

            var authResult = await app.AcquireTokenForClient(scopes).ExecuteAsync();
            return authResult.AccessToken;
        }

        public async Task UploadFileToOneDrive(string filePath)
        {
            var token = await GetAccessTokenAsync();

            var graphClient = new GraphServiceClient(
                new DelegateAuthenticationProvider(
                    (requestMessage) =>
                    {
                        requestMessage.Headers.Authorization =
                            new AuthenticationHeaderValue("Bearer", token);

                        return Task.CompletedTask;
                    }));

            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                await graphClient.Me.Drive.Root.ItemWithPath(Path.GetFileName(filePath)).Content.Request().PutAsync<DriveItem>(stream);
            }
        }

        private async Task Main()
        {            
            
            await UploadFileToOneDrive("C:\\Users\\Meliton\\Downloads\\Screenshot_1.png");
            Console.WriteLine("File uploaded successfully.");
        }
        public class OneDriveUploader
        {
            // Insert methods here
        }


    }
}
