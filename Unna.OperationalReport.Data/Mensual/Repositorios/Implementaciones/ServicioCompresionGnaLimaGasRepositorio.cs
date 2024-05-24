using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Infraestructura.Configuraciones.Abstracciones;
using Unna.OperationalReport.Data.Infraestructura.Contextos.Abstracciones;
using Unna.OperationalReport.Data.Infraestructura.Repositorios.Implementaciones;
using Unna.OperationalReport.Data.Mensual.Entidades;
using Unna.OperationalReport.Data.Mensual.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Propiedad.Entidades;
using Unna.OperationalReport.Data.Propiedad.Repositorios.Abstracciones;

namespace Unna.OperationalReport.Data.Mensual.Repositorios.Implementaciones
{
    internal class ServicioCompresionGnaLimaGasRepositorio : OperacionalRepositorio<ServicioCompresionGnaLimaGas, long>, IServicioCompresionGnaLimaGasRepositorio
    {
        public ServicioCompresionGnaLimaGasRepositorio(IOperacionalUnidadDeTrabajo unidadDeTrabajo, IOperacionalConfiguracion configuracion) : base(unidadDeTrabajo, configuracion) { }


        public async Task<ServicioCompresionGnaLimaGas?> BuscarPorFechaAsync(DateTime fecha)        
        => await UnidadDeTrabajo.MensualServicioCompresionGnaLimaGas.Where(e => e.Fecha == fecha).Include(e => e.ServicioCompresionGnaLimaGasVentas).FirstOrDefaultAsync();


    }
}
