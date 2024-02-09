using Autofac;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Tools.DatosBase.Infraestructura.Configuraciones.Abstracciones;
using Unna.OperationalReport.Tools.DatosEF.Infraestructura.Contexto.Abstracciones;
using Unna.OperationalReport.Tools.DatosEF.Infraestructura.Contexto.Implementaciones;
using Module = Autofac.Module;

namespace Unna.OperationalReport.Tools.Cargadores.Bd
{
    public class CargarSqlServerEF<Ctx, IConf, Conf, UdT> : Module
        where Ctx : EFDbContext
        where Conf : IBaseDatosConfiguracion, new()
        where IConf : IBaseDatosConfiguracion
        where UdT : IEFUnidadDeTrabajo
    {
        private readonly IConfiguration _configuration;
        private readonly string _nombreConexion;
        private readonly string _nombreNamespace;
        public CargarSqlServerEF(
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
            var opcionesBaseDatos = new DbContextOptionsBuilder<Ctx>();

            opcionesBaseDatos.UseSqlServer(_configuration.GetConnectionString(_nombreConexion), options => options.EnableRetryOnFailure());

            builder.Register(e => new Conf()
            {
                CadenaConexion = _configuration.GetConnectionString(_nombreConexion)
            }).As<IConf>().InstancePerLifetimeScope();
          
            builder.RegisterType<Ctx>().WithParameter("opciones", opcionesBaseDatos.Options)
                .As<UdT>().InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(Assembly.Load(_nombreNamespace))
              .Where(t => t.Name.EndsWith("Repositorio"))
              .AsImplementedInterfaces()
              .InstancePerLifetimeScope();
        }
    }
}
