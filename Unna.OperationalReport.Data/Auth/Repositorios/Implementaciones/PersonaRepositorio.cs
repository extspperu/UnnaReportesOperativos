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
    public class PersonaRepositorio : OperacionalRepositorio<Persona, long>, IPersonaRepositorio
    {
        public PersonaRepositorio(IOperacionalUnidadDeTrabajo unidadDeTrabajo, IOperacionalConfiguracion configuracion) : base(unidadDeTrabajo, configuracion) { }


        public override async Task<Persona?> BuscarPorIdAsync(long id)
       => await UnidadDeTrabajo.AuthPersonas.Where(e => e.IdPersona == id).FirstOrDefaultAsync();

        public async Task<Persona?> BuscarPorDocumentoAsync(string documento)
       => await UnidadDeTrabajo.AuthPersonas.Where(e => e.Documento == documento).FirstOrDefaultAsync();

        public async Task<Persona?> BuscarPorCorreoAsync(string correo)
       => await UnidadDeTrabajo.AuthPersonas.Where(e => e.Correo == correo).FirstOrDefaultAsync();


    }
}
