using DocumentFormat.OpenXml.Drawing;
using Microsoft.AspNetCore.Http;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Registro.Entidades;
using Unna.OperationalReport.Data.Registro.Enums;
using Unna.OperationalReport.Data.Registro.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Registro.Repositorios.Implementaciones;
using Unna.OperationalReport.Data.Reporte.Enums;
using Unna.OperationalReport.Service.Reportes.Generales.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.Impresiones.Dtos;
using Unna.OperationalReport.Service.Reportes.Impresiones.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaBalanceEnergia.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletaVolumenesUNNAEnergiaCNPC.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.FacturacionGnsLIV.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ComposicionGnaLIV.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ComposicionGnaLIV.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ComposicionGnaLIV_2.Dtos;
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
        DateTime diaOperativo = DateTime.ParseExact("15/04/2024", "dd/MM/yyyy", CultureInfo.InvariantCulture);//FechasUtilitario.ObtenerDiaOperativo();
        double vMoleculaCO2 = 0;
        double vMoleculaN2 = 0;
        double vMoleculaC1 = 0;
        double vMoleculaC2 = 0;
        double vMoleculaC3 = 0;
        double vMoleculaIC4 = 0;
        double vMoleculaNC4 = 0;
        double vMoleculaIC5 = 0;
        double vMoleculaNC5 = 0;
        double vMoleculaNeoC5 = 0;
        double vMoleculaC6 = 0;
        double vTotalPromedioPeruPetroVol;
        double vTotalPromedioPeruPetroCO2 = 0;
        double vTotalPromedioPeruPetroN2 = 0;
        double vTotalPromedioPeruPetroC1 = 0;
        double vTotalPromedioPeruPetroC2 = 0;
        double vTotalPromedioPeruPetroC3 = 0;
        double vTotalPromedioPeruPetroIC4 = 0;
        double vTotalPromedioPeruPetroNC4 = 0;
        double vTotalPromedioPeruPetroIC5 = 0;
        double vTotalPromedioPeruPetroNC5 = 0;
        double vTotalPromedioPeruPetroNeoC5 = 0;
        double vTotalPromedioPeruPetroC6 = 0;

        public ComposicionGnaLIVServicio
        (
            IComposicionUnnaEnergiaPromedioRepositorio composicionUnnaEnergiaPromedioRepositorio,
            IDatoDeltaVRepositorio datoDeltaVRepositorio,
            IRegistroRepositorio registroRepositorio,
            IImpresionServicio impresionServicio,
            IComposicionRepositorio composicionRepositorio,
            IReporteServicio reporteServicio
        )
        {
            _composicionUnnaEnergiaPromedioRepositorio = composicionUnnaEnergiaPromedioRepositorio;
            _datoDeltaVRepositorio = datoDeltaVRepositorio;
            _registroRepositorio = registroRepositorio;
            _impresionServicio = impresionServicio;
            _composicionRepositorio = composicionRepositorio;
            _reporteServicio = reporteServicio;
        }

        public async Task<OperacionDto<ComposicionGnaLIVDto>> ObtenerAsync(long idUsuario)
        {
            var operacionGeneral = await _reporteServicio.ObtenerAsync((int)TiposReportes.ComposicionQuincenalGNALoteIV, idUsuario);
            if (!operacionGeneral.Completado)
            {
                return new OperacionDto<ComposicionGnaLIVDto>(CodigosOperacionDto.NoExiste, operacionGeneral.Mensajes);
            }

            string observacion = default(string);
            var operacionImpresion = await _impresionServicio.ObtenerAsync((int)TiposReportes.ComposicionQuincenalGNALoteIV, diaOperativo);
            if (operacionImpresion != null && operacionImpresion.Completado && operacionImpresion.Resultado != null && !string.IsNullOrWhiteSpace(operacionImpresion.Resultado.Datos))
            {
                observacion = operacionImpresion.Resultado?.Comentario;
                if (new DateTime(diaOperativo.Year, diaOperativo.Month, 1) == new DateTime(operacionImpresion.Resultado.Fecha.Year, operacionImpresion.Resultado.Fecha.Month, 1))
                {
                    var rpta = JsonConvert.DeserializeObject<ComposicionGnaLIVDto>(operacionImpresion.Resultado.Datos);
                    rpta.General = operacionGeneral.Resultado;
                    return new OperacionDto<ComposicionGnaLIVDto>(rpta);
                }
            }

            var registros = await _composicionUnnaEnergiaPromedioRepositorio.ObtenerComposicionUnnaEnergiaPromedio(diaOperativo);
            var registrosVol = await _datoDeltaVRepositorio.ObtenerVolumenDeltaVAsync(diaOperativo);
            int dia;
            if (diaOperativo.Day <= 15)
            {
                dia = diaOperativo.Day;
            }
            else
            {
                dia = 15;
            }

            for (int i = 0; i < registrosVol.Count; i++)
            {


                vTotalPromedioPeruPetroVol = (double)(vTotalPromedioPeruPetroVol + registrosVol[i].Volumen);

            }
            for (int i = 0; i < registros.Count; i++)
            {
                if (registros[i].Simbolo == "CO2")
                {
                    vTotalPromedioPeruPetroCO2 = vTotalPromedioPeruPetroCO2 + registros[i].PromedioComponente;
                }
                if (registros[i].Simbolo == "N2")
                {
                    vTotalPromedioPeruPetroN2 = vTotalPromedioPeruPetroN2 + registros[i].PromedioComponente;
                }
                if (registros[i].Simbolo == "C1")
                {
                    vTotalPromedioPeruPetroC1 = vTotalPromedioPeruPetroC1 + registros[i].PromedioComponente;
                }
                if (registros[i].Simbolo == "C2")
                {
                    vTotalPromedioPeruPetroC2 = vTotalPromedioPeruPetroC2 + registros[i].PromedioComponente;
                }
                if (registros[i].Simbolo == "c3")
                {
                    vTotalPromedioPeruPetroC3 = vTotalPromedioPeruPetroC3 + registros[i].PromedioComponente;
                }
                if (registros[i].Simbolo == "iC4")
                {
                    vTotalPromedioPeruPetroIC4 = vTotalPromedioPeruPetroIC4 + registros[i].PromedioComponente;
                }
                if (registros[i].Simbolo == "nC4")
                {
                    vTotalPromedioPeruPetroNC4 = vTotalPromedioPeruPetroNC4 + registros[i].PromedioComponente;
                }
                if (registros[i].Simbolo == "iC5")
                {
                    vTotalPromedioPeruPetroIC5 = vTotalPromedioPeruPetroIC5 + registros[i].PromedioComponente;
                }
                if (registros[i].Simbolo == "nC5")
                {
                    vTotalPromedioPeruPetroNC5 = vTotalPromedioPeruPetroNC5 + registros[i].PromedioComponente;
                }
                if (registros[i].Simbolo == "NeoC5")
                {
                    vTotalPromedioPeruPetroNeoC5 = vTotalPromedioPeruPetroNeoC5 + registros[i].PromedioComponente;
                }
                if (registros[i].Simbolo == "C6")
                {
                    vTotalPromedioPeruPetroC6 = vTotalPromedioPeruPetroC6 + registros[i].PromedioComponente;
                }
            }
            var dto = new ComposicionGnaLIVDto
            {

                Fecha = "",
                TotalPromedioPeruPetroC6 = Math.Round(Math.Round(vTotalPromedioPeruPetroC6 / dia, 5, MidpointRounding.AwayFromZero), 4, MidpointRounding.AwayFromZero),
                TotalPromedioPeruPetroC3 = Math.Round(vTotalPromedioPeruPetroC3 / dia, 4, MidpointRounding.AwayFromZero),
                TotalPromedioPeruPetroIc4 = Math.Round(Math.Round(vTotalPromedioPeruPetroIC4 / dia, 5, MidpointRounding.AwayFromZero), 4, MidpointRounding.AwayFromZero),
                TotalPromedioPeruPetroNc4 = Math.Round(Math.Round(vTotalPromedioPeruPetroNC4 / dia, 5, MidpointRounding.AwayFromZero), 4, MidpointRounding.AwayFromZero),
                TotalPromedioPeruPetroNeoC5 = Math.Round(Math.Round(vTotalPromedioPeruPetroNeoC5 / dia, 5, MidpointRounding.AwayFromZero), 4, MidpointRounding.AwayFromZero),
                TotalPromedioPeruPetroIc5 = Math.Round(vTotalPromedioPeruPetroIC5 / dia, 4, MidpointRounding.AwayFromZero), 
                TotalPromedioPeruPetroNc5 = Math.Round(Math.Round(vTotalPromedioPeruPetroNC5 / dia, 5, MidpointRounding.AwayFromZero), 4, MidpointRounding.AwayFromZero),
                TotalPromedioPeruPetroNitrog = Math.Round(Math.Round(vTotalPromedioPeruPetroN2 / dia, 5, MidpointRounding.AwayFromZero), 4, MidpointRounding.AwayFromZero),
                TotalPromedioPeruPetroC1 = Math.Round(Math.Round(vTotalPromedioPeruPetroC1 / dia, 5, MidpointRounding.AwayFromZero), 4, MidpointRounding.AwayFromZero),
                TotalPromedioPeruPetroCo2 = Math.Round(Math.Round(vTotalPromedioPeruPetroCO2 / dia, 5, MidpointRounding.AwayFromZero), 4, MidpointRounding.AwayFromZero),
                TotalPromedioPeruPetroC2 = Math.Round(Math.Round(vTotalPromedioPeruPetroC2 / dia, 5, MidpointRounding.AwayFromZero), 4, MidpointRounding.ToPositiveInfinity),
                TotalPromedioPeruPetroVol = vTotalPromedioPeruPetroVol,
                TotalPromedioUnnaC6 = Math.Round(Math.Round(vTotalPromedioPeruPetroC6 / dia, 5, MidpointRounding.AwayFromZero), 4, MidpointRounding.AwayFromZero),
                TotalPromedioUnnaC3 = Math.Round(Math.Round(vTotalPromedioPeruPetroC3 / dia, 5, MidpointRounding.AwayFromZero), 4, MidpointRounding.AwayFromZero),
                TotalPromedioUnnaIc4 = Math.Round(Math.Round(vTotalPromedioPeruPetroIC4 / dia, 5, MidpointRounding.AwayFromZero), 4, MidpointRounding.AwayFromZero),
                TotalPromedioUnnaNc4 = Math.Round(Math.Round(vTotalPromedioPeruPetroNC4 / dia, 5, MidpointRounding.AwayFromZero), 4, MidpointRounding.AwayFromZero),
                TotalPromedioUnnaNeoC5 = Math.Round(Math.Round(vTotalPromedioPeruPetroNeoC5 / dia, 5, MidpointRounding.AwayFromZero), 4, MidpointRounding.AwayFromZero),
                TotalPromedioUnnaIc5 = Math.Round(Math.Round(vTotalPromedioPeruPetroIC5 / dia, 5, MidpointRounding.AwayFromZero), 4, MidpointRounding.ToNegativeInfinity),
                TotalPromedioUnnaNc5 = Math.Round(Math.Round(vTotalPromedioPeruPetroNC5 / dia, 5, MidpointRounding.AwayFromZero), 4, MidpointRounding.AwayFromZero),
                TotalPromedioUnnaNitrog = Math.Round(Math.Round(vTotalPromedioPeruPetroN2 / dia, 5, MidpointRounding.AwayFromZero), 4, MidpointRounding.AwayFromZero),
                TotalPromedioUnnaC1 = Math.Round(Math.Round(vTotalPromedioPeruPetroC1 / dia, 5, MidpointRounding.AwayFromZero), 4, MidpointRounding.AwayFromZero),
                TotalPromedioUnnaCo2 = Math.Round(Math.Round(vTotalPromedioPeruPetroCO2 / dia, 5, MidpointRounding.AwayFromZero), 4, MidpointRounding.AwayFromZero),
                TotalPromedioUnnaC2 = Math.Round(Math.Round(vTotalPromedioPeruPetroC2 / dia, 5, MidpointRounding.AwayFromZero), 4, MidpointRounding.ToPositiveInfinity),
                TotalPromedioUnnaVol = 0,
                TotalDifC6 = 0.0000,
                TotalDifC3 = 0.0000,
                TotalDifIc4 = 0.0000,
                TotalDifNc4 = 0.0000,
                TotalDifNeoC5 = 0.0000,
                TotalDifIc5 = 0.0000,
                TotalDifNc5 = 0.0000,
                TotalDifNitrog = 0.0000,
                TotalDifC1 = 0.0000,
                TotalDifCo2 = 0.0000,
                TotalDifC2 = 0.0000,
                TotalDifVol = 0.0000

            };
            dto.ComposicionGnaLIVDetComposicion = await ComposicionGnaLIVDetComposicion();
            dto.ComposicionGnaLIVDetComponente = await ComposicionGnaLIVDetComponente();

            return new OperacionDto<ComposicionGnaLIVDto>(dto);

        }

        private async Task<List<ComposicionGnaLIVDetComposicionDto>> ComposicionGnaLIVDetComposicion()
        {
            var registros = await _composicionUnnaEnergiaPromedioRepositorio.ObtenerComposicionUnnaEnergiaPromedio(diaOperativo);
            var registrosVol = await _datoDeltaVRepositorio.ObtenerVolumenDeltaVAsync(diaOperativo);
            var registrosPC = await _registroRepositorio.ObtenerValorPoderCalorificoAsync(2, 4, diaOperativo);
            int j = -1;

            List<ComposicionGnaLIVDetComposicionDto> ComposicionGnaLIVDetComposicion = new List<ComposicionGnaLIVDetComposicionDto>();

            for (int i = 0; i < registros.Count; i = i + 11)
            {
                if (j < registrosVol.Count) { j++; }

                ComposicionGnaLIVDetComposicion.Add(new ComposicionGnaLIVDetComposicionDto
                {

                    CompGnaDia = registros[i]?.Fecha.ToString("dd/MM/yyyy"),
                    CompGnaC6 = registros[i]?.PromedioComponente,
                    CompGnaC3 = registros[i + 1]?.PromedioComponente,
                    CompGnaIc4 = registros[i + 2]?.PromedioComponente,
                    CompGnaNc4 = registros[i + 3]?.PromedioComponente,
                    CompGnaNeoC5 = registros[i + 4]?.PromedioComponente,
                    CompGnaIc5 = registros[i + 5]?.PromedioComponente,
                    CompGnaNc5 = registros[i + 6]?.PromedioComponente,
                    CompGnaNitrog = registros[i + 7]?.PromedioComponente,
                    CompGnaC1 = registros[i + 8]?.PromedioComponente,
                    CompGnaCo2 = registros[i + 9]?.PromedioComponente,
                    CompGnaC2 = registros[i + 10]?.PromedioComponente,
                    CompGnaObservacion = ""
                }
                );
            }
            return ComposicionGnaLIVDetComposicion;
        }

        private async Task<List<ComposicionGnaLIVDetComponenteDto>> ComposicionGnaLIVDetComponente()
        {
            var registros = await _composicionUnnaEnergiaPromedioRepositorio.ObtenerComposicionUnnaEnergiaPromedio(diaOperativo);
            int dia;
            if (diaOperativo.Day <= 15)
            {
                dia = diaOperativo.Day;
            }
            else
            {
                dia = 15;
            }

            for (int i = 0; i < registros.Count; i++)
            {
                if (registros[i].Simbolo == "CO2")
                {
                    vMoleculaCO2 = vMoleculaCO2 + registros[i].PromedioComponente;
                }
                if (registros[i].Simbolo == "N2")
                {
                    vMoleculaN2 = vMoleculaN2 + registros[i].PromedioComponente;
                }
                if (registros[i].Simbolo == "C1")
                {
                    vMoleculaC1 = vMoleculaC1 + registros[i].PromedioComponente;
                }
                if (registros[i].Simbolo == "C2")
                {
                    vMoleculaC2 = vMoleculaC2 + registros[i].PromedioComponente;
                }
                if (registros[i].Simbolo == "c3")
                {
                    vMoleculaC3 = vMoleculaC3 + registros[i].PromedioComponente;
                }
                if (registros[i].Simbolo == "iC4")
                {
                    vMoleculaIC4 = vMoleculaIC4 + registros[i].PromedioComponente;
                }
                if (registros[i].Simbolo == "nC4")
                {
                    vMoleculaNC4 = vMoleculaNC4 + registros[i].PromedioComponente;
                }
                if (registros[i].Simbolo == "iC5")
                {
                    vMoleculaIC5 = vMoleculaIC5 + registros[i].PromedioComponente;
                }
                if (registros[i].Simbolo == "nC5")
                {
                    vMoleculaNC5 = vMoleculaNC5 + registros[i].PromedioComponente;
                }
                if (registros[i].Simbolo == "NeoC5")
                {
                    vMoleculaNeoC5 = vMoleculaNeoC5 + registros[i].PromedioComponente;
                }
                if (registros[i].Simbolo == "C6")
                {
                    vMoleculaC6 = vMoleculaC6 + registros[i].PromedioComponente;
                }
            }
            List<ComposicionGnaLIVDetComponenteDto> ComposicionGnaLIVDetComponente = new List<ComposicionGnaLIVDetComponenteDto>();


            ComposicionGnaLIVDetComponente.Add(new ComposicionGnaLIVDetComponenteDto
            {
                CompSimbolo = "H2",
                CompDescripcion = "Hidrogen",
                CompMolPorc = 0,
            }
            );
            ComposicionGnaLIVDetComponente.Add(new ComposicionGnaLIVDetComponenteDto
            {
                CompSimbolo = "H2S",
                CompDescripcion = "Hidrogen Sulphide",
                CompMolPorc = 0,
            }
            );

            ComposicionGnaLIVDetComponente.Add(new ComposicionGnaLIVDetComponenteDto
            {

                CompSimbolo = "CO2",
                CompDescripcion = "Carbon Dioxide",
                CompMolPorc = Math.Round(Math.Round(vMoleculaCO2 / dia, 5, MidpointRounding.AwayFromZero), 4, MidpointRounding.AwayFromZero),
            }
            );

            ComposicionGnaLIVDetComponente.Add(new ComposicionGnaLIVDetComponenteDto
            {
                CompSimbolo = "N2",
                CompDescripcion = "Nitrogen",
                CompMolPorc = Math.Round(Math.Round(vMoleculaN2 / dia, 5, MidpointRounding.AwayFromZero), 4, MidpointRounding.AwayFromZero),

            }
            );
            ComposicionGnaLIVDetComponente.Add(new ComposicionGnaLIVDetComponenteDto
            {
                CompSimbolo = "C1",
                CompDescripcion = "Methane",
                CompMolPorc = Math.Round(Math.Round(vMoleculaC1 / dia, 5, MidpointRounding.AwayFromZero), 4, MidpointRounding.AwayFromZero),

            }
            );
            ComposicionGnaLIVDetComponente.Add(new ComposicionGnaLIVDetComponenteDto
            {
                CompSimbolo = "C2",
                CompDescripcion = "Ethane",
                CompMolPorc = Math.Round(Math.Round(vMoleculaC2 / dia, 5, MidpointRounding.AwayFromZero), 4, MidpointRounding.ToPositiveInfinity),

            }
            );
            ComposicionGnaLIVDetComponente.Add(new ComposicionGnaLIVDetComponenteDto
            {
                CompSimbolo = "C3",
                CompDescripcion = "Propane",
                CompMolPorc = Math.Round(vMoleculaC3 / dia, 4, MidpointRounding.AwayFromZero), //Math.Round(Math.Round(vMoleculaC3 / dia, 5, MidpointRounding.AwayFromZero), 4, MidpointRounding.AwayFromZero),

            }
            );
            ComposicionGnaLIVDetComponente.Add(new ComposicionGnaLIVDetComponenteDto
            {
                CompSimbolo = "IC4",
                CompDescripcion = "i-Butane",
                CompMolPorc = Math.Round(Math.Round(vMoleculaIC4 / dia, 5, MidpointRounding.AwayFromZero), 4, MidpointRounding.AwayFromZero),

            }
            );
            ComposicionGnaLIVDetComponente.Add(new ComposicionGnaLIVDetComponenteDto
            {
                CompSimbolo = "NC4",
                CompDescripcion = "n-Butane",
                CompMolPorc = Math.Round(Math.Round(vMoleculaNC4 / dia, 5, MidpointRounding.AwayFromZero), 4, MidpointRounding.AwayFromZero),

            }
            );
            ComposicionGnaLIVDetComponente.Add(new ComposicionGnaLIVDetComponenteDto
            {
                CompSimbolo = "IC5",
                CompDescripcion = "i-Pentane",
                CompMolPorc = Math.Round(vMoleculaIC5 / dia, 4, MidpointRounding.AwayFromZero),//Math.Round(Math.Round(vMoleculaIC5 / dia, 5, MidpointRounding.AwayFromZero), 4, MidpointRounding.ToNegativeInfinity),

            }
            );
            ComposicionGnaLIVDetComponente.Add(new ComposicionGnaLIVDetComponenteDto
            {
                CompSimbolo = "NC5",
                CompDescripcion = "n-Pentane",
                CompMolPorc = Math.Round(Math.Round(vMoleculaNC5 / dia, 5, MidpointRounding.AwayFromZero), 4, MidpointRounding.AwayFromZero),

            }
            );
            ComposicionGnaLIVDetComponente.Add(new ComposicionGnaLIVDetComponenteDto
            {
                CompSimbolo = "NeoC5",
                CompDescripcion = "NeoPentane",
                CompMolPorc = Math.Round(Math.Round(vMoleculaNeoC5 / dia, 5, MidpointRounding.AwayFromZero), 4, MidpointRounding.AwayFromZero),

            }
            );
            ComposicionGnaLIVDetComponente.Add(new ComposicionGnaLIVDetComponenteDto
            {
                CompSimbolo = "C6",
                CompDescripcion = "Hexanes",
                CompMolPorc = Math.Round(Math.Round(vMoleculaC6 / dia, 5, MidpointRounding.AwayFromZero), 4, MidpointRounding.AwayFromZero),


            }
            );
            ComposicionGnaLIVDetComponente.Add(new ComposicionGnaLIVDetComponenteDto
            {
                CompSimbolo = "C7",
                CompDescripcion = "Heptanes",
                CompMolPorc = 0,
            }
            );
            ComposicionGnaLIVDetComponente.Add(new ComposicionGnaLIVDetComponenteDto
            {
                CompSimbolo = "C8",
                CompDescripcion = "Octanes",
                CompMolPorc = 0,
            }
            );
            ComposicionGnaLIVDetComponente.Add(new ComposicionGnaLIVDetComponenteDto
            {
                CompSimbolo = "C9",
                CompDescripcion = "Nonanes",
                CompMolPorc = 0,
            }
            );
            ComposicionGnaLIVDetComponente.Add(new ComposicionGnaLIVDetComponenteDto
            {
                CompSimbolo = "C10",
                CompDescripcion = "Decanes",
                CompMolPorc = 0,
            }
            );
            ComposicionGnaLIVDetComponente.Add(new ComposicionGnaLIVDetComponenteDto
            {
                CompSimbolo = "C11",
                CompDescripcion = "Undecanes",
                CompMolPorc = 0,
            }
            );
            ComposicionGnaLIVDetComponente.Add(new ComposicionGnaLIVDetComponenteDto
            {
                CompSimbolo = "C12+",
                CompDescripcion = "Dodecanes plus",
                CompMolPorc = 0,
            }
            );

            return ComposicionGnaLIVDetComponente;
        }

        public async Task<OperacionDto<RespuestaSimpleDto<string>>> GuardarAsync(ComposicionGnaLIVDto peticion)
        {
            var operacionValidacion = ValidacionUtilitario.ValidarModelo<RespuestaSimpleDto<string>>(peticion);
            if (!operacionValidacion.Completado)
            {
                return operacionValidacion;
            }

            DateTime fecha = diaOperativo;// FechasUtilitario.ObtenerDiaOperativo();
            peticion.General = null;
            var dto = new ImpresionDto()
            {
                IdConfiguracion = RijndaelUtilitario.EncryptRijndaelToUrl((int)TiposReportes.ComposicionQuincenalGNALoteIV),
                Fecha = fecha,
                IdUsuario = peticion.IdUsuario,
                Datos = JsonConvert.SerializeObject(peticion),
                Comentario = null
            };

            //DateTime Desde = new DateTime(fecha.Year, fecha.Month, 1);
            //DateTime Hasta = new DateTime(fecha.Year, fecha.Month, 15);//.AddMonths(1).AddDays(-1);
            //if (peticion.ComposicionGnaLIVDetComposicion != null && peticion.ComposicionGnaLIVDetComposicion.Count > 0)
            //{
            //    await _composicionRepositorio.EliminarPorFechaAsync(Desde, Hasta);
            //    foreach (var item in peticion.ComposicionGnaLIVDetComposicion)
            //    {
            //        //DateTime compGnaDia = DateTime.ParseExact(item.CompGnaDia, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            //        if (!item.CompGnaDia.Equals(null))
            //        {
            //            continue;
            //        }

            //        //var compo = new ComposicionGnaLIVDetComposicionDto
            //        //{
            //        //    CompGnaDia = item.CompGnaDia,
            //        //    CompGnaC6 = item.CompGnaC6,
            //        //    CompGnaC3 = item.CompGnaC3,
            //        //    CompGnaIc4 = item.CompGnaIc4,
            //        //    CompGnaNc4 = item.CompGnaNc4,
            //        //    CompGnaNeoC5 = item.CompGnaNeoC5,
            //        //    CompGnaIc5 = item.CompGnaIc5,
            //        //    CompGnaNc5 = item.CompGnaNc5,
            //        //    CompGnaNitrog = item.CompGnaNitrog,
            //        //    CompGnaC1 = item.CompGnaC1,
            //        //    CompGnaCo2 = item.CompGnaCo2,
            //        //    CompGnaC2 = item.CompGnaC2

            //        //};

            //        //var composicion = new Data.Registro.Entidades.Composicion
            //        //{
            //        //    //Fecha = item.Fecha.Value,
            //        //    CompGnaDia = item.CompGnaDia,
            //        //    CompGnaC6 = item.CompGnaC6,
            //        //    CompGnaC3 = item.CompGnaC3,
            //        //    CompGnaIc4 = item.CompGnaIc4,
            //        //    CompGnaNc4 = item.CompGnaNc4,
            //        //    CompGnaNeoC5 = item.CompGnaNeoC5,
            //        //    CompGnaIc5 = item.CompGnaIc5,
            //        //    CompGnaNc5 = item.CompGnaNc5,
            //        //    CompGnaNitrog = item.CompGnaNitrog,
            //        //    CompGnaC1 = item.CompGnaC1,
            //        //    CompGnaCo2 = item.CompGnaCo2,
            //        //    CompGnaC2 = item.CompGnaC2
            //        //    //Orden = item.GlpBls,
            //        //    //Simbolo = item.GnsMpc,
            //        //    //Actualizado = DateTime.UtcNow
            //        //};
            //        //await _composicionRepositorio.InsertarAsync(compo);
            //        //await _composicionRepositorio.InsertarAsync(composicion);
            //    }
            //}

            return await _impresionServicio.GuardarAsync(dto);

        }
    }

}