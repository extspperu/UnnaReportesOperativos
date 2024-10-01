using Autofac;
using Microsoft.Extensions.Configuration;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Tools.Seguridad.Servicios.General.Dtos;
using Module = Autofac.Module;

namespace Unna.OperationalReport.Tools.Seguridad.Infraestructura.Modulos
{
    public class ConfiguracionModule : Module
    {
        private readonly IConfiguration _configuration;

        public ConfiguracionModule(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(e => new SeguridadConfiguracionDto()
            {
                LlaveRsaPrivada = _configuration["seguridad:llaveRsaPrivada"],
                LlaveRsaPublica = _configuration["seguridad:llaveRsaPublica"],
                PasswordSalt = _configuration["seguridad:passwordSalt"],
                NombreAplicacion = _configuration["seguridad:nombreAplicacion"],
                TokenAplicacion = _configuration["seguridad:tokenAplicacion"],
                AppName = _configuration["seguridad:appname"],
            }).InstancePerLifetimeScope();

            builder.Register(e => new UrlConfiguracionDto()
            {
                ApiKeyGoogle = "AIzaSyDMw1IzqA_bxMtQxnnYs2Zm16Hodwp8NAc",
                UrlBase = _configuration["general:urlBase"],
            }).InstancePerLifetimeScope();

            builder.Register(e => new GeneralDto()
            {
                RutaArchivos = _configuration["archivo:rutaArchivos"],
                Email = new EmailDto
                {
                    Host = _configuration["email:host"],
                    Port = !string.IsNullOrWhiteSpace(_configuration["email:port"]) ? int.Parse(_configuration["email:port"]) : new int?(),
                    From = _configuration["email:from"],
                    User = _configuration["email:user"],
                    Psw = _configuration["email:psw"],
                },
                Sharepoint = new SharepointDto
                {
                    ClientId = _configuration["sharepoint:clientId"],
                    ClientSecret = _configuration["sharepoint:clientSecret"],
                    TenantId = _configuration["sharepoint:tenantId"],
                    DriveId = _configuration["sharepoint:driveId"],
                    Site = _configuration["sharepoint:site"],
                    Active = !string.IsNullOrWhiteSpace(_configuration["sharepoint:active"]) ? bool.Parse(_configuration["sharepoint:active"]) : false,
                }
            }).InstancePerLifetimeScope();



        }

    }
}
