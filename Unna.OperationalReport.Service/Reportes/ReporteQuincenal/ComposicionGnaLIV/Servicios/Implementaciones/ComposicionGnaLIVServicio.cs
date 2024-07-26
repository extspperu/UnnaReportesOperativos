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

        public ComposicionGnaLIVServicio
        (
            IComposicionUnnaEnergiaPromedioRepositorio composicionUnnaEnergiaPromedioRepositorio,
            IDatoDeltaVRepositorio datoDeltaVRepositorio,
            IRegistroRepositorio registroRepositorio,
            IImpresionServicio impresionServicio,
            IComposicionRepositorio composicionRepositorio,
            IReporteServicio reporteServicio,
            ISuministradorComponenteRepositorio suministradorComponenteRepositorio
        )
        {
            _composicionUnnaEnergiaPromedioRepositorio = composicionUnnaEnergiaPromedioRepositorio;
            _datoDeltaVRepositorio = datoDeltaVRepositorio;
            _registroRepositorio = registroRepositorio;
            _impresionServicio = impresionServicio;
            _composicionRepositorio = composicionRepositorio;
            _reporteServicio = reporteServicio;
            _suministradorComponenteRepositorio = suministradorComponenteRepositorio;
        }

        public async Task<OperacionDto<ComposicionGnaLIVDto>> ObtenerAsync(long idUsuario, string? grupo)
        {
            var operacionGeneral = await _reporteServicio.ObtenerAsync((int)TiposReportes.ComposicionQuincenalGNALoteIV, idUsuario);
            if (!operacionGeneral.Completado)
            {
                return new OperacionDto<ComposicionGnaLIVDto>(CodigosOperacionDto.NoExiste, operacionGeneral.Mensajes);
            }

            var diaOperativo = FechasUtilitario.ObtenerDiaOperativo();
            DateTime? desde = default(DateTime?);
            DateTime? hasta = default(DateTime?);
            switch (grupo)
            {
                case GruposReportes.Quincenal:
                    desde = new DateTime(diaOperativo.Year, diaOperativo.Month, 1);
                    hasta = new DateTime(diaOperativo.Year, diaOperativo.Month, 15);
                    break;
                case GruposReportes.Mensual:
                    desde = new DateTime(diaOperativo.Year, diaOperativo.Month, 16);
                    hasta = new DateTime(diaOperativo.Year, diaOperativo.Month, 1).AddMonths(1).AddDays(-1);
                    break;
            }
            if (!desde.HasValue)
            {
                return new OperacionDto<ComposicionGnaLIVDto>(CodigosOperacionDto.NoExiste, "No se obtuvo la fecha de inicio");
            }

            var operacionImpresion = await _impresionServicio.ObtenerAsync((int)TiposReportes.ComposicionQuincenalGNALoteIV, desde.Value);
            if (operacionImpresion != null && operacionImpresion.Completado && operacionImpresion.Resultado != null && !string.IsNullOrWhiteSpace(operacionImpresion.Resultado.Datos))
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

        //private async Task<List<ComposicionGnaLIVDetComposicionDto>> ComposicionGnaLIVDetComposicion(List<ComposicionUnnaEnergiaPromedio?> registros, DateTime diaOperativo)
        //{
        //    //var registros = await _composicionUnnaEnergiaPromedioRepositorio.ObtenerComposicionUnnaEnergiaPromedio(diaOperativo);
        //    var registrosVol = await _datoDeltaVRepositorio.ObtenerVolumenDeltaVAsync(diaOperativo);
        //    //var registrosPC = await _registroRepositorio.ObtenerValorPoderCalorificoAsync(2, 4, diaOperativo);
        //    //int j = -1;

        //    List<ComposicionGnaLIVDetComposicionDto> ComposicionGnaLIVDetComposicion = new List<ComposicionGnaLIVDetComposicionDto>();

        //    for (int i = 0; i < registros.Count; i = i + 11)
        //    {
        //        //if (j < registrosVol.Count) { j++; }
        //        if (registros[i] == null)
        //        {
        //            continue;
        //        }

        //        ComposicionGnaLIVDetComposicion.Add(new ComposicionGnaLIVDetComposicionDto
        //        {
        //            CompGnaDia = registros[i].Fecha.HasValue ? registros[i]?.Fecha.Value.ToString("dd/MM/yyyy") : null,
        //            CompGnaC6 = registros[i]?.PromedioComponente,
        //            CompGnaC3 = registros[i + 1]?.PromedioComponente,
        //            CompGnaIc4 = registros[i + 2]?.PromedioComponente,
        //            CompGnaNc4 = registros[i + 3]?.PromedioComponente,
        //            CompGnaNeoC5 = registros[i + 4]?.PromedioComponente,
        //            CompGnaIc5 = registros[i + 5]?.PromedioComponente,
        //            CompGnaNc5 = registros[i + 6]?.PromedioComponente,
        //            CompGnaNitrog = registros[i + 7]?.PromedioComponente,
        //            CompGnaC1 = registros[i + 8]?.PromedioComponente,
        //            CompGnaCo2 = registros[i + 9]?.PromedioComponente,
        //            CompGnaC2 = registros[i + 10]?.PromedioComponente,
        //            CompGnaObservacion = ""
        //        }
        //        );
        //    }
        //    return ComposicionGnaLIVDetComposicion;
        //}

        //private async Task<List<ComposicionGnaLIVDetComponenteDto>> ComposicionGnaLIVDetComponente(DateTime diaOperativo)
        //{
        //    var registros = await _composicionUnnaEnergiaPromedioRepositorio.ObtenerComposicionUnnaEnergiaPromedio(diaOperativo);
        //    int dia;
        //    if (diaOperativo.Day <= 15)
        //    {
        //        dia = diaOperativo.Day;
        //    }
        //    else
        //    {
        //        dia = 15;
        //    }

        //    for (int i = 0; i < registros.Count; i++)
        //    {
        //        if (registros[i].Simbolo == "CO2")
        //        {
        //            vMoleculaCO2 = vMoleculaCO2 + registros[i].PromedioComponente;
        //        }
        //        if (registros[i].Simbolo == "N2")
        //        {
        //            vMoleculaN2 = vMoleculaN2 + registros[i].PromedioComponente;
        //        }
        //        if (registros[i].Simbolo == "C1")
        //        {
        //            vMoleculaC1 = vMoleculaC1 + registros[i].PromedioComponente;
        //        }
        //        if (registros[i].Simbolo == "C2")
        //        {
        //            vMoleculaC2 = vMoleculaC2 + registros[i].PromedioComponente;
        //        }
        //        if (registros[i].Simbolo == "c3")
        //        {
        //            vMoleculaC3 = vMoleculaC3 + registros[i].PromedioComponente;
        //        }
        //        if (registros[i].Simbolo == "iC4")
        //        {
        //            vMoleculaIC4 = vMoleculaIC4 + registros[i].PromedioComponente;
        //        }
        //        if (registros[i].Simbolo == "nC4")
        //        {
        //            vMoleculaNC4 = vMoleculaNC4 + registros[i].PromedioComponente;
        //        }
        //        if (registros[i].Simbolo == "iC5")
        //        {
        //            vMoleculaIC5 = vMoleculaIC5 + registros[i].PromedioComponente;
        //        }
        //        if (registros[i].Simbolo == "nC5")
        //        {
        //            vMoleculaNC5 = vMoleculaNC5 + registros[i].PromedioComponente;
        //        }
        //        if (registros[i].Simbolo == "NeoC5")
        //        {
        //            vMoleculaNeoC5 = vMoleculaNeoC5 + registros[i].PromedioComponente;
        //        }
        //        if (registros[i].Simbolo == "C6")
        //        {
        //            vMoleculaC6 = vMoleculaC6 + registros[i].PromedioComponente;
        //        }
        //    }
        //    List<ComposicionGnaLIVDetComponenteDto> ComposicionGnaLIVDetComponente = new List<ComposicionGnaLIVDetComponenteDto>();


        //    ComposicionGnaLIVDetComponente.Add(new ComposicionGnaLIVDetComponenteDto
        //    {
        //        CompSimbolo = "H2",
        //        CompDescripcion = "Hidrogen",
        //        CompMolPorc = 0,
        //    }
        //    );
        //    ComposicionGnaLIVDetComponente.Add(new ComposicionGnaLIVDetComponenteDto
        //    {
        //        CompSimbolo = "H2S",
        //        CompDescripcion = "Hidrogen Sulphide",
        //        CompMolPorc = 0,
        //    }
        //    );

        //    ComposicionGnaLIVDetComponente.Add(new ComposicionGnaLIVDetComponenteDto
        //    {

        //        CompSimbolo = "CO2",
        //        CompDescripcion = "Carbon Dioxide",
        //        CompMolPorc = Math.Round(Math.Round(vMoleculaCO2 / dia, 5, MidpointRounding.AwayFromZero), 4, MidpointRounding.AwayFromZero),
        //    }
        //    );

        //    ComposicionGnaLIVDetComponente.Add(new ComposicionGnaLIVDetComponenteDto
        //    {
        //        CompSimbolo = "N2",
        //        CompDescripcion = "Nitrogen",
        //        CompMolPorc = Math.Round(Math.Round(vMoleculaN2 / dia, 5, MidpointRounding.AwayFromZero), 4, MidpointRounding.AwayFromZero),

        //    }
        //    );
        //    ComposicionGnaLIVDetComponente.Add(new ComposicionGnaLIVDetComponenteDto
        //    {
        //        CompSimbolo = "C1",
        //        CompDescripcion = "Methane",
        //        CompMolPorc = Math.Round(Math.Round(vMoleculaC1 / dia, 5, MidpointRounding.AwayFromZero), 4, MidpointRounding.AwayFromZero),

        //    }
        //    );
        //    ComposicionGnaLIVDetComponente.Add(new ComposicionGnaLIVDetComponenteDto
        //    {
        //        CompSimbolo = "C2",
        //        CompDescripcion = "Ethane",
        //        CompMolPorc = Math.Round(Math.Round(vMoleculaC2 / dia, 5, MidpointRounding.AwayFromZero), 4, MidpointRounding.ToPositiveInfinity),

        //    }
        //    );
        //    ComposicionGnaLIVDetComponente.Add(new ComposicionGnaLIVDetComponenteDto
        //    {
        //        CompSimbolo = "C3",
        //        CompDescripcion = "Propane",
        //        CompMolPorc = Math.Round(vMoleculaC3 / dia, 4, MidpointRounding.AwayFromZero), //Math.Round(Math.Round(vMoleculaC3 / dia, 5, MidpointRounding.AwayFromZero), 4, MidpointRounding.AwayFromZero),

        //    }
        //    );
        //    ComposicionGnaLIVDetComponente.Add(new ComposicionGnaLIVDetComponenteDto
        //    {
        //        CompSimbolo = "IC4",
        //        CompDescripcion = "i-Butane",
        //        CompMolPorc = Math.Round(Math.Round(vMoleculaIC4 / dia, 5, MidpointRounding.AwayFromZero), 4, MidpointRounding.AwayFromZero),

        //    }
        //    );
        //    ComposicionGnaLIVDetComponente.Add(new ComposicionGnaLIVDetComponenteDto
        //    {
        //        CompSimbolo = "NC4",
        //        CompDescripcion = "n-Butane",
        //        CompMolPorc = Math.Round(Math.Round(vMoleculaNC4 / dia, 5, MidpointRounding.AwayFromZero), 4, MidpointRounding.AwayFromZero),

        //    }
        //    );
        //    ComposicionGnaLIVDetComponente.Add(new ComposicionGnaLIVDetComponenteDto
        //    {
        //        CompSimbolo = "IC5",
        //        CompDescripcion = "i-Pentane",
        //        CompMolPorc = Math.Round(vMoleculaIC5 / dia, 4, MidpointRounding.AwayFromZero),//Math.Round(Math.Round(vMoleculaIC5 / dia, 5, MidpointRounding.AwayFromZero), 4, MidpointRounding.ToNegativeInfinity),

        //    }
        //    );
        //    ComposicionGnaLIVDetComponente.Add(new ComposicionGnaLIVDetComponenteDto
        //    {
        //        CompSimbolo = "NC5",
        //        CompDescripcion = "n-Pentane",
        //        CompMolPorc = Math.Round(Math.Round(vMoleculaNC5 / dia, 5, MidpointRounding.AwayFromZero), 4, MidpointRounding.AwayFromZero),

        //    }
        //    );
        //    ComposicionGnaLIVDetComponente.Add(new ComposicionGnaLIVDetComponenteDto
        //    {
        //        CompSimbolo = "NeoC5",
        //        CompDescripcion = "NeoPentane",
        //        CompMolPorc = Math.Round(Math.Round(vMoleculaNeoC5 / dia, 5, MidpointRounding.AwayFromZero), 4, MidpointRounding.AwayFromZero),

        //    }
        //    );
        //    ComposicionGnaLIVDetComponente.Add(new ComposicionGnaLIVDetComponenteDto
        //    {
        //        CompSimbolo = "C6",
        //        CompDescripcion = "Hexanes",
        //        CompMolPorc = Math.Round(Math.Round(vMoleculaC6 / dia, 5, MidpointRounding.AwayFromZero), 4, MidpointRounding.AwayFromZero),


        //    }
        //    );
        //    ComposicionGnaLIVDetComponente.Add(new ComposicionGnaLIVDetComponenteDto
        //    {
        //        CompSimbolo = "C7",
        //        CompDescripcion = "Heptanes",
        //        CompMolPorc = 0,
        //    }
        //    );
        //    ComposicionGnaLIVDetComponente.Add(new ComposicionGnaLIVDetComponenteDto
        //    {
        //        CompSimbolo = "C8",
        //        CompDescripcion = "Octanes",
        //        CompMolPorc = 0,
        //    }
        //    );
        //    ComposicionGnaLIVDetComponente.Add(new ComposicionGnaLIVDetComponenteDto
        //    {
        //        CompSimbolo = "C9",
        //        CompDescripcion = "Nonanes",
        //        CompMolPorc = 0,
        //    }
        //    );
        //    ComposicionGnaLIVDetComponente.Add(new ComposicionGnaLIVDetComponenteDto
        //    {
        //        CompSimbolo = "C10",
        //        CompDescripcion = "Decanes",
        //        CompMolPorc = 0,
        //    }
        //    );
        //    ComposicionGnaLIVDetComponente.Add(new ComposicionGnaLIVDetComponenteDto
        //    {
        //        CompSimbolo = "C11",
        //        CompDescripcion = "Undecanes",
        //        CompMolPorc = 0,
        //    }
        //    );
        //    ComposicionGnaLIVDetComponente.Add(new ComposicionGnaLIVDetComponenteDto
        //    {
        //        CompSimbolo = "C12+",
        //        CompDescripcion = "Dodecanes plus",
        //        CompMolPorc = 0,
        //    }
        //    );

        //    return ComposicionGnaLIVDetComponente;
        //}

        public async Task<OperacionDto<RespuestaSimpleDto<string>>> GuardarAsync(ComposicionGnaLIVDto peticion)
        {
            var operacionValidacion = ValidacionUtilitario.ValidarModelo<RespuestaSimpleDto<string>>(peticion);
            if (!operacionValidacion.Completado)
            {
                return operacionValidacion;
            }

            DateTime diaOperativo = FechasUtilitario.ObtenerDiaOperativo();
            DateTime? fecha = default(DateTime?);
            switch (peticion.Grupo)
            {
                case GruposReportes.Quincenal:
                    fecha = new DateTime(diaOperativo.Year, diaOperativo.Month, 1);
                    break;
                case GruposReportes.Mensual:
                    fecha = new DateTime(diaOperativo.Year, diaOperativo.Month, 16);
                    break;
                default:
                    return new OperacionDto<RespuestaSimpleDto<string>>(CodigosOperacionDto.NoExiste, "No existe el grupo de reporte");
            }
            var dto = new ImpresionDto()
            {
                IdConfiguracion = RijndaelUtilitario.EncryptRijndaelToUrl((int)TiposReportes.ComposicionQuincenalGNALoteIV),
                Fecha = fecha.Value,
                IdUsuario = peticion.IdUsuario,
                Datos = JsonConvert.SerializeObject(peticion),
                EsEditado = true,
            };

            return await _impresionServicio.GuardarAsync(dto);

        }
    }

}