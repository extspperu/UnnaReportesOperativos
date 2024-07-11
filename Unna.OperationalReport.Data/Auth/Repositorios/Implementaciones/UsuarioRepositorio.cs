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
    public class UsuarioRepositorio : OperacionalRepositorio<Usuario, long>, IUsuarioRepositorio
    {
        public UsuarioRepositorio(IOperacionalUnidadDeTrabajo unidadDeTrabajo, IOperacionalConfiguracion configuracion) : base(unidadDeTrabajo, configuracion) { }

        public override async Task<Usuario?> BuscarPorIdAsync(long id)
        => await UnidadDeTrabajo.AuthUsuarios.Where(e => e.IdUsuario == id).FirstOrDefaultAsync();

        public override async Task<Usuario?> BuscarPorIdYNoBorradoAsync(long id)
        => await UnidadDeTrabajo.AuthUsuarios.Where(e => e.IdUsuario == id && e.EstaBorrado == false).FirstOrDefaultAsync();

        public async Task<Usuario?> BuscarPorUsernameAsync(string username)
        => await UnidadDeTrabajo.AuthUsuarios.Where(x => x.Username == username).FirstOrDefaultAsync();
         
        

        
               

    }
}
