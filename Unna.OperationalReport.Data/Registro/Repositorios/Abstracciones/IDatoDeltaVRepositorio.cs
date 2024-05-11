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
        Task<List<ProduccionDiariaMs>> BuscarProduccionDiariaMsAsync(long idRegistroSupervisor);

        Task EliminarGnsVolumeMsYPcBrutoAsync(long? idRegistroSupervisor, string? tipo);
        Task GuardarGnsVolumeMsYPcBrutoAsync(GnsVolumeMsYPcBruto entidad);
        Task<List<GnsVolumeMsYPcBruto>> BuscarGnsVolumeMsYPcBrutoAsync(long idRegistroSupervisor, string? tipo);


        Task EliminarDatosCgnAsync(long? idRegistroSupervisor);
        Task GuardarDatosCgnAsync(DatoCgn entidad);
        Task<List<DatoCgn>> BuscarDatosCgnAsync(long idRegistroSupervisor);


        Task EliminarVolumenDeDespachoAsync(long? idRegistroSupervisor, string? tipo);
        Task GuardarVolumenDeDespachoAsync(VolumenDespacho entidad);
        Task<List<VolumenDespacho>> BuscarVolumenDeDespachoAsync(long idRegistroSupervisor, string? tipo);


        Task EliminarDespachoGlpEnvasadoAsync(long? idRegistroSupervisor);
        Task GuardarDespachoGlpEnvasadoAsync(DespachoGlpEnvasado entidad);
        Task<List<DespachoGlpEnvasado>> BuscarDespachoGlpEnvasadoAsync(long idRegistroSupervisor);

        Task<List<VolumenDeltaV>> ObtenerVolumenDeltaVAsync(DateTime? diaOperativo);
        Task<List<VolumenDeltaV>> ObtenerVolumenDeltaVAsync2(DateTime? diaOperativo);
    }
}
