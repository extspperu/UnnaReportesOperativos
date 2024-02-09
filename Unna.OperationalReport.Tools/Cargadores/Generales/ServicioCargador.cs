using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Module = Autofac.Module;
using Autofac;
using System.Reflection;

namespace Unna.OperationalReport.Tools.Cargadores.Generales
{
    public class ServicioCargador : Module
    {
        private readonly string _nombreNamespace;

        public ServicioCargador(string nombreNamespace)
        {
            _nombreNamespace = nombreNamespace;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(Assembly.Load(_nombreNamespace))
                .Where(t => t.Name.EndsWith("Servicio"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }
    }
}
