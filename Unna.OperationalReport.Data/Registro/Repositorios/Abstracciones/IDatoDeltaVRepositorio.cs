using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Infraestructura.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Registro.Entidades;

namespace Unna.OperationalReport.Data.Registro.Repositorios.Abstracciones
{
    public interface IDatoDeltaVRepositorio : IOperacionalRepositorio<DatoDeltaV, object>
    {

        Task EliminarDatosDeltaVAsync(long? idRegistroSupervisor);
        Task GuardarDatosDeltaVAsync(DatoDeltaV entidad);
        Task<List<DatoDeltaV>> BuscarDatosDeltaVAsync(long idRegistroSupervisor);

        Task EliminarVolumenDeltaVAsync(long? idRegistroSupervisor);
        Task GuardarVolumenDeltaVAsync(VolumenDeltaV entidad);
        Task<List<VolumenDeltaV>> BuscarVolumenDeltaVAsync(long idRegistroSupervisor);


        Task EliminarProduccionDiariaMsAsync(long? idRegistroSupervisor);
        Task GuardarProduccionDiariaMsAsync(ProduccionDiariaMs entidad);


        Task EliminarGnsVolumeMsYPcBrutoAsync(long? idRegistroSupervisor, string? tipo);
        Task GuardarGnsVolumeMsYPcBrutoAsync(GnsVolumeMsYPcBruto entidad);

        Task EliminarDatosCgnAsync(long? idRegistroSupervisor);
        Task GuardarDatosCgnAsync(DatoCgn entidad);

        Task EliminarVolumenDeDespachoAsync(long? idRegistroSupervisor, string? tipo);
        Task GuardarVolumenDeDespachoAsync(VolumenDespacho entidad);

        Task EliminarDespachoGlpEnvasadoAsync(long? idRegistroSupervisor);
        Task GuardarDespachoGlpEnvasadoAsync(DespachoGlpEnvasado entidad);
    }
}
