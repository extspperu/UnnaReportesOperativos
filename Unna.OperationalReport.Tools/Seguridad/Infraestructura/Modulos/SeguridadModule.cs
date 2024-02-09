using Autofac;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Tools.ComponentesWeb.Infraestructura.Modulos;
using Module = Autofac.Module;

namespace Unna.OperationalReport.Tools.Seguridad.Infraestructura.Modulos
{
    public class SeguridadModule : Module
    {
        private readonly IConfiguration _configuration;

        public SeguridadModule(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule(new ConfiguracionModule(_configuration));
            builder.RegisterModule(new DatosModule(_configuration));
            builder.RegisterModule(new ServiciosModule());
        }
    }
}
