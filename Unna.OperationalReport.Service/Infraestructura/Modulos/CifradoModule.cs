using Autofac;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Service.General.Dtos;
using Module = Autofac.Module;

namespace Unna.OperationalReport.Service.Infraestructura.Modulos
{
    public class CifradoModule : Module
    {
        private readonly IConfiguration _configuration;

        public CifradoModule(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(e => new CifradoDto(
                    llaveDispositivo: _configuration["cifrado:llaveDispositivo"],
                    llaveToken: _configuration["cifrado:llaveToken"]
                )
            ).InstancePerLifetimeScope();

            builder.Register(e => new ConfiguracionDto(
                    idDispositivo: _configuration["configuracion:idDipositivo"],
                    token: _configuration["seguridad:tokenAplicacion"]
                )
            ).InstancePerLifetimeScope();
        }
    }
}
