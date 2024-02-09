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
    public class UsuarioLoteRepositorio : OperacionalRepositorio<UsuarioLote, object>, IUsuarioLoteRepositorio
    {
        public UsuarioLoteRepositorio(IOperacionalUnidadDeTrabajo unidadDeTrabajo, IOperacionalConfiguracion configuracion) : base(unidadDeTrabajo, configuracion) { }

        public async Task<UsuarioLote?> BuscarPorIdUsuarioActivoAsync(long idUsuario)
       => await UnidadDeTrabajo.AuthUsuarioLotes.Where(e => e.IdUsuario == idUsuario && e.EstaActivo == true).FirstOrDefaultAsync();

        public async Task<List<UsuarioLote>?> ListarPorIdGrupoAsync(int? idGrupo)
        => await UnidadDeTrabajo.AuthUsuarioLotes.Where(e => e.IdGrupo == idGrupo && e.EstaActivo == true).ToListAsync();


    }
}
