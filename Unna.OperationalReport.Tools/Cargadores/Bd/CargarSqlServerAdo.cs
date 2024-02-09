using Autofac;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Tools.DatosBase.Infraestructura.Configuraciones.Abstracciones;
using Module = Autofac.Module;

namespace Unna.OperationalReport.Tools.Cargadores.Bd
{
    public class CargarSqlServerAdo<IConf, Conf> : Module
        where Conf : IBaseDatosConfiguracion, new()
        where IConf : IBaseDatosConfiguracion
    {
        private readonly IConfiguration _configuration;
        private readonly string _nombreConexion;
        private readonly string _nombreNamespace;
        public CargarSqlServerAdo(
            IConfiguration configuration,
            string nombreConexion,
            string nombreNamespace)
        {
            _configuration = configuration;
            _nombreConexion = nombreConexion;
            _nombreNamespace = nombreNamespace;
        }

        protected override void Load(ContainerBuilder builder)
        {


            builder.Register(e => new Conf()
            {
                CadenaConexion = _configuration.GetConnectionString(_nombreConexion)
            }).As<IConf>().InstancePerLifetimeScope();


            builder.RegisterAssemblyTypes(Assembly.Load(_nombreNamespace))
              .Where(t => t.Name.EndsWith("Repositorio"))
              .AsImplementedInterfaces()
              .InstancePerLifetimeScope();
        }

    }
}
