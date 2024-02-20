using Unna.OperationalReport.Data.Auth.Enums;
using Unna.OperationalReport.Data.Registro.Enums;
using Unna.OperationalReport.Data.Registro.Repositorios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaCnpc.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaCnpc.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;

namespace Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaCnpc.Servicios.Implementaciones
{
    public class BoletaCnpcServicio : IBoletaCnpcServicio
    {



        private readonly IDiaOperativoRepositorio _diaOperativoRepositorio;
        private readonly IRegistroRepositorio _registroRepositorio;
        public BoletaCnpcServicio(
            IDiaOperativoRepositorio diaOperativoRepositorio,
            IRegistroRepositorio registroRepositorio
            )
        {
            _diaOperativoRepositorio = diaOperativoRepositorio;
            _registroRepositorio = registroRepositorio;
        }

        public async Task<OperacionDto<BoletaCnpcDto>> ObtenerAsync(long idUsuario)
        {

            BoletaCnpcTabla1Dto tabla1 = new BoletaCnpcTabla1Dto();

            double gasMpcd1 = 0;
            double gasMpcd2 = 0;
            var primerDato = await _diaOperativoRepositorio.ObtenerPorIdLoteYFechaAsync((int)TiposLote.LoteIv, FechasUtilitario.ObtenerDiaOperativo(), (int)TipoGrupos.FiscalizadorEnel, (int)TiposNumeroRegistro.PrimeroRegistro);
            if (primerDato != null)
            {
                var dato = await _registroRepositorio.ObtenerPorIdDatoYDiaOperativoAsync((int)TiposDatos.VolumenMpcd, primerDato.IdDiaOperativo);
                if (dato != null)
                {
                    gasMpcd1 = dato.Valor ?? 0;
                }
            }
            var segundoDato = await _diaOperativoRepositorio.ObtenerPorIdLoteYFechaAsync((int)TiposLote.LoteIv, FechasUtilitario.ObtenerDiaOperativo(), (int)TipoGrupos.FiscalizadorEnel, (int)TiposNumeroRegistro.SegundoRegistro);
            if (segundoDato != null)
            {
                var dato = await _registroRepositorio.ObtenerPorIdDatoYDiaOperativoAsync((int)TiposDatos.CnpcPeruGnaRecibido, segundoDato.IdDiaOperativo);
                if (dato != null)
                {
                    gasMpcd2 = dato.Valor ?? 0;
                }
            }

            var dto = new BoletaCnpcDto
            {
                Fecha = FechasUtilitario.ObtenerDiaOperativo().ToString("dd/MM/yyyy")
            };


            tabla1.Fecha = dto.Fecha;
            tabla1.GasMpcd = Math.Round(gasMpcd1, 0) - Math.Round(gasMpcd2);
            tabla1.GlpBls = 13;
            tabla1.CgnBls = 14;
            tabla1.GlpBls = 15;
            dto.Tabla1 = tabla1;


            dto.VolumenTotalGns = 123;
            dto.VolumenTotalGnsEnMs = 125;
            dto.FlareGna = dto.VolumenTotalGns + dto.VolumenTotalGnsEnMs;




            //tabla1 01
            List<FactoresDistribucionGasNaturalDto> factoresDistribucionGasNaturalSeco = new List<FactoresDistribucionGasNaturalDto>();

            factoresDistribucionGasNaturalSeco.Add(new FactoresDistribucionGasNaturalDto
            {
                Item = 1,
                Sumistrador = "LOTE Z69",
                Volumen = 0,
                ConcentracionC1 = 1,
                VolumenC1 =1,
                FactoresDistribucion =1,
                AsignacionGns =1,
            });
            factoresDistribucionGasNaturalSeco.Add(new FactoresDistribucionGasNaturalDto
            {
                Item = 2,
                Sumistrador = "CNPC",
                Volumen = 0,
                ConcentracionC1 = 1,
                VolumenC1 = 1,
                FactoresDistribucion = 1,
                AsignacionGns = 1,
            });
            factoresDistribucionGasNaturalSeco.Add(new FactoresDistribucionGasNaturalDto
            {
                Item = 3,
                Sumistrador = "LOTE VI",
                Volumen = 0,
                ConcentracionC1 = 1,
                VolumenC1 = 1,
                FactoresDistribucion = 1,
                AsignacionGns = 1,
            });
            factoresDistribucionGasNaturalSeco.Add(new FactoresDistribucionGasNaturalDto
            {
                Item = 4,
                Sumistrador = "LOTE I",
                Volumen = 0,
                ConcentracionC1 = 1,
                VolumenC1 = 1,
                FactoresDistribucion = 1,
                AsignacionGns = 1,
            });
            factoresDistribucionGasNaturalSeco.Add(new FactoresDistribucionGasNaturalDto
            {
                Item = 5,
                Sumistrador = "LOTE IV",
                Volumen = 0,
                ConcentracionC1 = 1,
                VolumenC1 = 1,
                FactoresDistribucion = 1,
                AsignacionGns = 1,
            });
            factoresDistribucionGasNaturalSeco.Add(new FactoresDistribucionGasNaturalDto
            {
                Item = 6,
                Sumistrador = "CNPC ADICIONAL",
                Volumen = 0,
                ConcentracionC1 = 1,
                VolumenC1 = 1,
                FactoresDistribucion = 1,
                AsignacionGns = 1,
            });

            dto.FactoresDistribucionGasNaturalSeco = factoresDistribucionGasNaturalSeco;
            return new OperacionDto<BoletaCnpcDto>(dto);
        }

    }
}
