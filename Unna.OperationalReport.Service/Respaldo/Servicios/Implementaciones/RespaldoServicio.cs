
using Unna.OperationalReport.Service.Respaldo.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Service.Respaldo.Dtos;
using Microsoft.Identity.Client;
using Azure.Identity;
using Microsoft.Graph;
using System.Net.Http.Headers;
using System.IO;


namespace Unna.OperationalReport.Service.Respaldo.Servicios.Implementaciones
{
    public class RespaldoServicio : IRespaldoServicio
    {

        string tenantId = "3bd7f920-eddd-400d-8684-c6d4bf29d104";
        string clientId = "8d1b2c24-0f07-4a97-800f-562725a468e0";
        string clientSecret = "bHC8Q~Fo1~d.KlfccdvVPFNkYwpbXUeAxIUtDcl7";
        private static readonly string[] Scopes = { "https://graph.microsoft.com/.default" };
        string oneDriveFolderPath = "/Documentos"; // Carpeta de destino en OneDrive (root en este caso)

        //string tenantId = "65bb3aad-376d-42f5-83f9-9beaea39fdc4";
        //string clientId = "b05ddf25-3e5c-4e3f-8b4f-c465130e7a6d";            
        //string clientSecret = "lU58Q~5SOHyPOVmHwr3x~IDtt1Ob-.Sty0TjxcEw";

        public async Task<OperacionDto<RespuestaSimpleDto<bool>>> EnviarAsync(RespaldoDto peticion)
        {
            await Task.Delay(0);
            if (peticion == null)
            {
                return new OperacionDto<RespuestaSimpleDto<bool>>(CodigosOperacionDto.NoExiste, "Parametro incorrecto");
            }

            //var tenantId = "65bb3aad-376d-42f5-83f9-9beaea39fdc4";
            //string clientId = "b05ddf25-3e5c-4e3f-8b4f-c465130e7a6d";
            //string clientSecret = "lU58Q~5SOHyPOVmHwr3x~IDtt1Ob-.Sty0TjxcEw";


            string filePath = "C:\\Users\\Meliton\\Downloads\\Screenshot_1.png"; // Ruta del archivo local a subir


            

            var clientSecretCredential = new ClientSecretCredential(tenantId, clientId, clientSecret);
            var graphClient = new GraphServiceClient(clientSecretCredential);
            try
            {
                

                using (var stream = new FileStream(filePath, FileMode.Open))
                {
                    await graphClient.Users["mvillanuevaco20@outlook.com"].Drive.Root.ItemWithPath(Path.GetFileName(filePath)).Content.Request().PutAsync<DriveItem>(stream);
                    //await graphClient.Users["mvillanuevaco20@outlook.com"].Drive.Root.ItemWithPath(Path.GetFileName(filePath)).Content.Request().PutAsync<DriveItem>(stream);
                    //var uploadedFile = await graphClient.Me
                    //                               .Drive
                    //                               .Root
                    //                               .ItemWithPath(oneDriveFolderPath + Path.GetFileName(filePath))
                    //                               .Content
                    //                               .Request()
                    //                               .PutAsync<DriveItem>(stream);
                }
            }
            catch (Exception ex)
            {

            }






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
            //var tenantId = "65bb3aad-376d-42f5-83f9-9beaea39fdc4";
            //string clientId = "b05ddf25-3e5c-4e3f-8b4f-c465130e7a6d";            
            //string clientSecret = "lU58Q~5SOHyPOVmHwr3x~IDtt1Ob-.Sty0TjxcEw";

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
                

                //var uploadedFile = await graphClient.Me
                //                                    .Drive
                //                                    .Root
                //                                    .ItemWithPath(oneDriveFolderPath + Path.GetFileName(filePath))
                //                                    .Content
                //                                    .Request()
                //                                    .PutAsync<DriveItem>(stream);
                
                var uploadedFile = await graphClient.Users["mvillanuevaco20@outlook.com"]
                                                    .Drive
                                                    .Root
                                                    .ItemWithPath(oneDriveFolderPath + Path.GetFileName(filePath))
                                                    .Content
                                                    .Request()
                                                    .PutAsync<DriveItem>(stream);
            }
        }

        private async Task Main()
        {

            await UploadFileToOneDrive("C:\\Users\\Meliton\\Downloads\\Screenshot_1.png");
            Console.WriteLine("File uploaded successfully.");
        }


        
    }
}
