using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Configuracion.Entidades;
using Unna.OperationalReport.Data.Configuracion.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Infraestructura.Configuraciones.Abstracciones;
using Unna.OperationalReport.Data.Infraestructura.Contextos.Abstracciones;
using Unna.OperationalReport.Data.Infraestructura.Repositorios.Implementaciones;

namespace Unna.OperationalReport.Data.Configuracion.Repositorios.Implementaciones
{
    public class EmpresaRepositorio : OperacionalRepositorio<Empresa, int>, IEmpresaRepositorio
    {
        public EmpresaRepositorio(IOperacionalUnidadDeTrabajo unidadDeTrabajo, IOperacionalConfiguracion configuracion) : base(unidadDeTrabajo, configuracion) { }

        public override async Task<Empresa?> BuscarPorIdAsync(int id)
      => await UnidadDeTrabajo.ConfiguracionEmpresas.Where(e => e.IdEmpresa == id).FirstOrDefaultAsync();

    }
}
