using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Microsoft.Extensions.Configuration;
using Module = Autofac.Module;

namespace Unna.OperationalReport.Tools.ComponentesWeb.Infraestructura.Modulos
{
    public class ComponentesWebModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule(new ServiciosModule());
        }
    }
}
