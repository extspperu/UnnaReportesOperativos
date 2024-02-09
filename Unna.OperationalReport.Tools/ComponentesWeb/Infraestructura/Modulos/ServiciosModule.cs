using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Module = Autofac.Module;
using System.Reflection;
using Autofac;

namespace Unna.OperationalReport.Tools.ComponentesWeb.Infraestructura.Modulos
{
    public class ServiciosModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(Assembly.Load("Unna.OperationalReport.Tools"))
              .Where(t => t.Name.EndsWith("Servicio"))
              .AsImplementedInterfaces()
              .InstancePerLifetimeScope();
        }
    }
}
