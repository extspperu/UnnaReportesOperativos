using Autofac;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Tools.Seguridad.Datos.Infraestructura.Configuraciones.Abstracciones;
using Unna.OperationalReport.Tools.Seguridad.Datos.Infraestructura.Configuraciones.Implementaciones;
using Module = Autofac.Module;

namespace Unna.OperationalReport.Tools.Seguridad.Infraestructura.Modulos
{
    class DatosModule : Module
    {
        private readonly IConfiguration _configuration;

        public DatosModule(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void Load(ContainerBuilder builder)
        {

            builder.Register(e => new SeguridadConfiguracion(_configuration.GetConnectionString("operacional"))).As<ISeguridadConfiguracion>().InstancePerLifetimeScope();


            builder.RegisterAssemblyTypes(Assembly.Load("Unna.OperationalReport.Tools"))
              .Where(t => t.Name.EndsWith("Repositorio"))
              .AsImplementedInterfaces()
              .InstancePerLifetimeScope();

        }
    }
}
