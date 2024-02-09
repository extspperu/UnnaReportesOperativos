using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Auth.Entidades;
using Unna.OperationalReport.Data.Auth.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Infraestructura.Configuraciones.Abstracciones;
using Unna.OperationalReport.Data.Infraestructura.Contextos.Abstracciones;
using Unna.OperationalReport.Data.Infraestructura.Repositorios.Implementaciones;

namespace Unna.OperationalReport.Data.Auth.Repositorios.Implementaciones
{
    public class GrupoRepositorio : OperacionalRepositorio<Grupo, int>, IGrupoRepositorio
    {
        public GrupoRepositorio(IOperacionalUnidadDeTrabajo unidadDeTrabajo, IOperacionalConfiguracion configuracion) : base(unidadDeTrabajo, configuracion) { }

        
        public async Task<Grupo?> BuscarPorIdGrupoAsync(long id)
        => await UnidadDeTrabajo.AuthGrupos.Where(e => e.IdGrupo == id).FirstOrDefaultAsync();

        
    }
}
