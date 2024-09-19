
using Unna.OperationalReport.Service.Respaldo.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Microsoft.Graph;
using Microsoft.Identity.Client;
using Unna.OperationalReport.Tools.Seguridad.Servicios.General.Dtos;
using Unna.OperationalReport.Service.Reportes.Impresiones.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;
using Unna.OperationalReport.Data.Reporte.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Reporte.Enums;

namespace Unna.OperationalReport.Service.Respaldo.Servicios.Implementaciones
{
    public class RespaldoServicio : IRespaldoServicio
    {

        private readonly GeneralDto _general;
        private readonly IConfiguracionRepositorio _configuracionRepositorio;
        private readonly IImprimirRepositorio _imprimirRepositorio;
        public RespaldoServicio(
            GeneralDto generalDto,
            IConfiguracionRepositorio configuracionRepositorio,
            IImprimirRepositorio imprimirRepositorio
            )
        {
            _general = generalDto;
            _configuracionRepositorio = configuracionRepositorio;
            _imprimirRepositorio = imprimirRepositorio;
        }

        public async Task<OperacionDto<RespuestaSimpleDto<bool>>> EnviarAsync(long idImprimir, int idReporte)
        {
           
            var reporte = await _configuracionRepositorio.BuscarPorIdYNoBorradoAsync(idReporte);
            if (reporte == null)
            {
                return new OperacionDto<RespuestaSimpleDto<bool>>(CodigosOperacionDto.NoExiste, "Reporte no existe");
            }

            if (string.IsNullOrWhiteSpace(reporte.Grupo))
            {
                return new OperacionDto<RespuestaSimpleDto<bool>>(CodigosOperacionDto.NoExiste, "Reporte tiene grupo el reporte");
            }


            var imprimir = await _imprimirRepositorio.BuscarPorIdYNoBorradoAsync(idImprimir);
            if (imprimir == null)
            {
                return new OperacionDto<RespuestaSimpleDto<bool>>(CodigosOperacionDto.NoExiste, "No existe archivo");
            }
            if (imprimir.TieneBackup)
            {
                return new OperacionDto<RespuestaSimpleDto<bool>>(CodigosOperacionDto.NoExiste, "Ya tiene backup el documento");
            }

            var graphClient = await GetGraphClientAsync();

            DateTime diaOperativo = imprimir.Fecha;


            //string? folderPath = $"REPORTES/{diaOperativo.Year}/{diaOperativo.Month}-{FechasUtilitario.ObtenerNombreMes(diaOperativo)?.ToUpper()}/{reporte.Grupo?.ToUpper()}";
            string? folderPath = $"REPORTES/{diaOperativo.Year}/{diaOperativo.Month.ToString().PadLeft(2, '0')}/{reporte.Grupo?.ToUpper()}";
            switch (reporte.Grupo)
            {
                case TiposGruposReportes.Diario:
                    folderPath = $"{folderPath}/{diaOperativo.ToString("dd-MM-yyyy")}";
                    break;
            }

            // Valida si existe la estructura de carpetas
            string? folderId = await EnsureFolderPathExistsAsync(graphClient, folderPath);


            string? uploadMessage = default(string?);
            // sube el documento pdf 
            if (!string.IsNullOrWhiteSpace(imprimir.RutaArchivoPdf))
            {
                uploadMessage = await UploadFileAsync(graphClient, imprimir.RutaArchivoPdf, folderId);
            }

            if (!string.IsNullOrWhiteSpace(imprimir.RutaArchivoExcel))
            {
                uploadMessage = await UploadFileAsync(graphClient, imprimir.RutaArchivoExcel, folderId);
            }
            if (!string.IsNullOrWhiteSpace(uploadMessage))
            {
                imprimir.TieneBackup = true;
                imprimir.UrlBackup = uploadMessage;
            }
            imprimir.FechaBackup = DateTime.UtcNow;
            await _imprimirRepositorio.ActualizarBackupAsync(imprimir);

            return new OperacionDto<RespuestaSimpleDto<bool>>(new RespuestaSimpleDto<bool> { Id = true });


        }



        private async Task<GraphServiceClient> GetGraphClientAsync()
        {
            var confidentialClient = ConfidentialClientApplicationBuilder
              .Create(_general.Sharepoint?.ClientId)
              .WithClientSecret(_general.Sharepoint?.ClientSecret)
              .WithAuthority(new Uri($"https://login.microsoftonline.com/{_general.Sharepoint?.TenantId}"))
              .Build();

            string[] scopes = { "https://graph.microsoft.com/.default" };

            var authResult = await confidentialClient.AcquireTokenForClient(scopes).ExecuteAsync();
            return new GraphServiceClient(new DelegateAuthenticationProvider((requestMessage) =>
            {
                requestMessage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authResult.AccessToken);
                return Task.CompletedTask;
            }));
        }

        private async Task<string?> EnsureFolderPathExistsAsync(GraphServiceClient graphClient, string folderPath)
        {
            // Divide la ruta en carpetas
            var folders = folderPath.Split('/');
            string? parentFolderId = default(string?);
            foreach (var folderName in folders)
            {
                parentFolderId = await CreateOrGetFolderAsync(graphClient, folderName, parentFolderId ?? "");
            }
            return parentFolderId; // Devuelve el ID de la última carpeta creada.
        }

        private async Task<string?> CreateOrGetFolderAsync(GraphServiceClient graphClient, string folderName, string parentFolderId)
        {
            try
            {
                // Verifica si la carpeta existe
                IDriveItemChildrenCollectionPage items;

                if (string.IsNullOrEmpty(parentFolderId))
                {
                    // Busca en la raíz
                    items = await graphClient.Sites[_general.Sharepoint?.Site]
                        .Drives[_general.Sharepoint?.DriveId]
                        .Root
                        .Children
                        .Request()
                        .GetAsync();
                }
                else
                {
                    // Busca dentro de la carpeta padre
                    items = await graphClient.Sites[_general.Sharepoint?.Site]
                        .Drives[_general.Sharepoint?.DriveId]
                        .Items[parentFolderId]
                        .Children
                        .Request()
                        .GetAsync();
                }

                // Intenta encontrar la carpeta
                var folder = items.FirstOrDefault(i => i.Folder != null && i.Name == folderName);

                if (folder != null)
                {
                    // Si la carpeta existe, devuelve su ID
                    return folder.Id;
                }

                // Si no existe, crea la carpeta
                var newFolder = new DriveItem
                {
                    Name = folderName,
                    Folder = new Folder(),
                    AdditionalData = new Dictionary<string, object>
            {
                { "@microsoft.graph.conflictBehavior", "rename" }
            }
                };

                DriveItem? createdFolder = default(DriveItem?);
                if (string.IsNullOrEmpty(parentFolderId))
                {
                    createdFolder = await graphClient.Sites[_general.Sharepoint?.Site]
                        .Drives[_general.Sharepoint?.DriveId]
                        .Root
                        .Children
                        .Request()
                        .AddAsync(newFolder);
                }
                else
                {
                    createdFolder = await graphClient.Sites[_general.Sharepoint?.Site]
                        .Drives[_general.Sharepoint?.DriveId]
                        .Items[parentFolderId]
                        .Children
                        .Request()
                        .AddAsync(newFolder);
                }
                return createdFolder?.Id; // Devuelve el ID de la nueva carpeta creada.
            }
            catch { }
            return null;
        }



        private async Task<string?> UploadFileAsync(GraphServiceClient graphClient, string? filePath, string? folderId)
        {
            using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            var fileName = Path.GetFileName(filePath);
            try
            {
                var driveItem = await graphClient.Sites[_general.Sharepoint?.Site]
                .Drives[_general.Sharepoint?.DriveId]
                .Items[folderId]
                .ItemWithPath(fileName)
                .Content
                .Request()
                .PutAsync<DriveItem>(fileStream);

                if (driveItem != null)
                {
                    return driveItem.WebUrl;
                }
            }
            catch
            {
            }
            return null;

        }




    }
}
