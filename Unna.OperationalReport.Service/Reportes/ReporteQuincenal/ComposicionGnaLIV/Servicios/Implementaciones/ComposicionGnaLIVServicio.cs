using Newtonsoft.Json;
using Unna.OperationalReport.Data.Auth.Entidades;
using Unna.OperationalReport.Data.Auth.Enums;
using Unna.OperationalReport.Data.Propiedad.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Registro.Entidades;
using Unna.OperationalReport.Data.Registro.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Reporte.Enums;
using Unna.OperationalReport.Service.Reportes.Generales.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.Impresiones.Dtos;
using Unna.OperationalReport.Service.Reportes.Impresiones.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ComposicionGnaLIV.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ComposicionGnaLIV.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Seguimiento.BalanceDiario.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;

namespace Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ComposicionGnaLIV.Servicios.Implementaciones
{

    public class ComposicionGnaLIVServicio : IComposicionGnaLIVServicio

    {
        private readonly IComposicionUnnaEnergiaPromedioRepositorio _composicionUnnaEnergiaPromedioRepositorio;
        private readonly IDatoDeltaVRepositorio _datoDeltaVRepositorio;
        private readonly IRegistroRepositorio _registroRepositorio;
        private readonly IImpresionServicio _impresionServicio;
        private readonly IComposicionRepositorio _composicionRepositorio;
        private readonly IReporteServicio _reporteServicio;
        private readonly ISuministradorComponenteRepositorio _suministradorComponenteRepositorio;
        private readonly ISeguimientoBalanceDiarioServicio _seguimientoBalanceDiarioServicio;

        public ComposicionGnaLIVServicio
        (
            IComposicionUnnaEnergiaPromedioRepositorio composicionUnnaEnergiaPromedioRepositorio,
            IDatoDeltaVRepositorio datoDeltaVRepositorio,
            IRegistroRepositorio registroRepositorio,
            IImpresionServicio impresionServicio,
            IComposicionRepositorio composicionRepositorio,
            IReporteServicio reporteServicio,
            ISuministradorComponenteRepositorio suministradorComponenteRepositorio,
            ISeguimientoBalanceDiarioServicio seguimientoBalanceDiarioServicio
        )
        {
            _composicionUnnaEnergiaPromedioRepositorio = composicionUnnaEnergiaPromedioRepositorio;
            _datoDeltaVRepositorio = datoDeltaVRepositorio;
            _registroRepositorio = registroRepositorio;
            _impresionServicio = impresionServicio;
            _composicionRepositorio = composicionRepositorio;
            _reporteServicio = reporteServicio;
            _suministradorComponenteRepositorio = suministradorComponenteRepositorio;
            _seguimientoBalanceDiarioServicio = seguimientoBalanceDiarioServicio;
        }

        public async Task<OperacionDto<ComposicionGnaLIVDto>> ObtenerAsync(long idUsuario, string? grupo)
        {
            var diaOperativo = FechasUtilitario.ObtenerDiaOperativo();
            DateTime? desde = default(DateTime?);
            DateTime? hasta = default(DateTime?);

            int? idReporte = default(int?);
            switch (grupo)
            {
                case GruposReportes.Quincenal:
                    if (diaOperativo.Day < 16) diaOperativo = diaOperativo.AddMonths(-1);
                    desde = new DateTime(diaOperativo.Year, diaOperativo.Month, 1);
                    hasta = new DateTime(diaOperativo.Year, diaOperativo.Month, 15);
                    idReporte = (int)TiposReportes.ComposicionQuincenalGNALoteIV;
                    break;
                case GruposReportes.Mensual:
                    DateTime mensual = diaOperativo.AddMonths(-1);
                    desde = new DateTime(mensual.Year, mensual.Month, 16);
                    hasta = new DateTime(mensual.Year, mensual.Month, 1).AddMonths(1).AddDays(-1);
                    idReporte = (int)TiposReportes.ComposicionMensualGNALoteIV;
                    break;
            }

            if (!desde.HasValue)
            {
                return new OperacionDto<ComposicionGnaLIVDto>(CodigosOperacionDto.NoExiste, "No se obtuvo la fecha de inicio");
            }

            var operacionGeneral = await _reporteServicio.ObtenerAsync(idReporte ?? 0, idUsuario);
            if (!operacionGeneral.Completado)
            {
                return new OperacionDto<ComposicionGnaLIVDto>(CodigosOperacionDto.NoExiste, operacionGeneral.Mensajes);
            }



            var operacionImpresion = await _impresionServicio.ObtenerAsync(idReporte ?? 0, desde.Value);
            if (operacionImpresion != null && operacionImpresion.Completado && operacionImpresion.Resultado != null && !string.IsNullOrWhiteSpace(operacionImpresion.Resultado.Datos) && operacionImpresion.Resultado.EsEditado)
            {
                var rpta = JsonConvert.DeserializeObject<ComposicionGnaLIVDto>(operacionImpresion.Resultado.Datos);
                return new OperacionDto<ComposicionGnaLIVDto>(rpta);
            }

            var registros = await _composicionUnnaEnergiaPromedioRepositorio.ObtenerComposicionUnnaEnergiaPromedioAsync(desde, hasta);

            var componentes = registros.Select(e => new ComposicionGnaLIVDetComposicionDto
            {
                CompGnaDia = e.Fecha.HasValue ? e.Fecha.Value.ToString("dd/MM/yyyy") : null,
                CompGnaC6 = e.C6,
                CompGnaC3 = e.C3,
                CompGnaIc4 = e.IC4,
                CompGnaIc5 = e.IC5,
                CompGnaNc4 = e.NC4,
                CompGnaNc5 = e.NC5,
                CompGnaNeoC5 = e.NEOC5,
                CompGnaNitrog = e.N2,
                CompGnaC1 = e.C1,
                CompGnaCo2 = e.CO2,
                CompGnaC2 = e.C2,
                CompGnaObservacion = e.Observacion
            }).ToList();

            int totalRegistros = componentes.Count;
            var promedioComponent = new ComposicionGnaLIVDetComposicionDto();
            if (componentes != null && componentes.Count > 0)
            {
                promedioComponent = new ComposicionGnaLIVDetComposicionDto
                {
                    CompGnaC6 = Math.Round(componentes.Sum(e => e.CompGnaC6 ?? 0) / totalRegistros, 4),
                    CompGnaC3 = Math.Round(componentes.Sum(e => e.CompGnaC3 ?? 0) / totalRegistros, 4),
                    CompGnaIc4 = Math.Round(componentes.Sum(e => e.CompGnaIc4 ?? 0) / totalRegistros, 4),
                    CompGnaIc5 = Math.Round(componentes.Sum(e => e.CompGnaIc5 ?? 0) / totalRegistros, 4),
                    CompGnaNc4 = Math.Round(componentes.Sum(e => e.CompGnaNc4 ?? 0) / totalRegistros, 4),
                    CompGnaNc5 = Math.Round(componentes.Sum(e => e.CompGnaNc5 ?? 0) / totalRegistros, 4),
                    CompGnaNeoC5 = Math.Round(componentes.Sum(e => e.CompGnaNeoC5 ?? 0) / totalRegistros, 4),
                    CompGnaNitrog = Math.Round(componentes.Sum(e => e.CompGnaNitrog ?? 0) / totalRegistros, 4),
                    CompGnaC1 = Math.Round(componentes.Sum(e => e.CompGnaC1 ?? 0) / totalRegistros, 4),
                    CompGnaCo2 = Math.Round(componentes.Sum(e => e.CompGnaCo2 ?? 0) / totalRegistros, 4),
                    CompGnaC2 = Math.Round(componentes.Sum(e => e.CompGnaC2 ?? 0) / totalRegistros, 4),
                };
                componentes.Add(promedioComponent);
            }


            var suministradores = await _suministradorComponenteRepositorio.ListarComponenteAsync();
            var componet = suministradores?.Select(e => new ComposicionGnaLIVDetComponenteDto
            {
                Item = e.Id,
                Simbolo = e.Simbolo,
                Descripcion = e.Suministrador
            }).ToList();
            if (componet != null)
            {
                for (var i = 0; i < componet?.Count; i++)
                {
                    if (promedioComponent == null) continue;
                    if (componet[i] == null) continue;
                    if (string.IsNullOrWhiteSpace(componet[i].Simbolo)) continue;

                    if (componet[i].Simbolo.Equals("H2")) componet[i].MolPorc = 0;
                    if (componet[i].Simbolo.Equals("H2S")) componet[i].MolPorc = 0;
                    if (componet[i].Simbolo.Equals("C6")) componet[i].MolPorc = promedioComponent.CompGnaC6;
                    if (componet[i].Simbolo.Equals("C3")) componet[i].MolPorc = promedioComponent.CompGnaC3;
                    if (componet[i].Simbolo.Equals("iC4")) componet[i].MolPorc = promedioComponent.CompGnaIc4;
                    if (componet[i].Simbolo.Equals("nC4")) componet[i].MolPorc = promedioComponent.CompGnaNc4;
                    if (componet[i].Simbolo.Equals("NeoC5")) componet[i].MolPorc = promedioComponent.CompGnaNeoC5;
                    if (componet[i].Simbolo.Equals("iC5")) componet[i].MolPorc = promedioComponent.CompGnaIc5;
                    if (componet[i].Simbolo.Equals("nC5")) componet[i].MolPorc = promedioComponent.CompGnaNc5;
                    if (componet[i].Simbolo.Equals("N2")) componet[i].MolPorc = promedioComponent.CompGnaNitrog;
                    if (componet[i].Simbolo.Equals("C1")) componet[i].MolPorc = promedioComponent.CompGnaC1;
                    if (componet[i].Simbolo.Equals("CO2")) componet[i].MolPorc = promedioComponent.CompGnaCo2;
                    if (componet[i].Simbolo.Equals("C2")) componet[i].MolPorc = promedioComponent.CompGnaC2;
                }
            }


            var dto = new ComposicionGnaLIVDto
            {
                NombreReporte = operacionGeneral.Resultado?.NombreReporte,
                RutaFirma = operacionGeneral.Resultado?.RutaFirma,
                ComposicionGnaLIVDetComposicion = componentes,
                ComposicionGnaLIVDetComponente = componet,
                Grupo = grupo
            };
            return new OperacionDto<ComposicionGnaLIVDto>(dto);

        }

        public async Task<OperacionDto<RespuestaSimpleDto<string>>> GuardarAsync(ComposicionGnaLIVDto peticion)
        {
            var operacionValidacion = ValidacionUtilitario.ValidarModelo<RespuestaSimpleDto<string>>(peticion);
            if (!operacionValidacion.Completado)
            {
                return operacionValidacion;
            }

            DateTime diaOperativo = FechasUtilitario.ObtenerDiaOperativo();
            DateTime? fecha = default(DateTime?);

            int? idReporte = default(int?);
         
            switch (peticion.Grupo)
            {
                case GruposReportes.Quincenal:
                    fecha = new DateTime(diaOperativo.Year, diaOperativo.Month, 1);
                    idReporte = (int)TiposReportes.ComposicionQuincenalGNALoteIV;
                    break;
                case GruposReportes.Mensual:
                    fecha = new DateTime(diaOperativo.Year, diaOperativo.Month, 16);
                    idReporte = (int)TiposReportes.ComposicionMensualGNALoteIV;
                    break;
                default:
                    return new OperacionDto<RespuestaSimpleDto<string>>(CodigosOperacionDto.NoExiste, "No existe el grupo de reporte");
            }
            var dto = new ImpresionDto()
            {
                IdConfiguracion = RijndaelUtilitario.EncryptRijndaelToUrl(idReporte),
                Fecha = fecha.Value,
                IdUsuario = peticion.IdUsuario,
                Datos = JsonConvert.SerializeObject(peticion),
                EsEditado = true,
            };

            await _seguimientoBalanceDiarioServicio.ActualizarEstadoSeguimientoDiarioAsync(25, 1);
            await _seguimientoBalanceDiarioServicio.ActualizarEstadoSeguimientoDiarioAsync(29, 1);
            return await _impresionServicio.GuardarAsync(dto);

        }
    }

}