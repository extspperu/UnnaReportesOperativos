
﻿using DocumentFormat.OpenXml.Drawing;
using Microsoft.AspNetCore.Http;
using Microsoft.Win32;
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
using Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaBalanceEnergia.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.FacturacionGnsLIV.Dtos;
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
        DateTime diaOperativo = DateTime.ParseExact("16/11/2023", "dd/MM/yyyy", CultureInfo.InvariantCulture);//FechasUtilitario.ObtenerDiaOperativo();
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
            IRegistroRepositorio registroRepositorio
        )
        {
            _composicionUnnaEnergiaPromedioRepositorio = composicionUnnaEnergiaPromedioRepositorio;
            _datoDeltaVRepositorio = datoDeltaVRepositorio;
            _registroRepositorio = registroRepositorio;
        }

        public async Task<OperacionDto<ComposicionGnaLIVDto>> ObtenerAsync(long idUsuario)
        {

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
            //var dto = new ComposicionGnaLIVDto
            //{

            //    Fecha = "NOVIEMBRE 2023",
            //    TotalPromedioPeruPetroC6 = Math.Round(Math.Round(vTotalPromedioPeruPetroC6 / dia, 5, MidpointRounding.AwayFromZero), 4, MidpointRounding.AwayFromZero),
            //    TotalPromedioPeruPetroC3 = Math.Round(Math.Round(vTotalPromedioPeruPetroC3 / dia, 5, MidpointRounding.AwayFromZero), 4, MidpointRounding.AwayFromZero),
            //    TotalPromedioPeruPetroIc4 = Math.Round(Math.Round(vTotalPromedioPeruPetroIC4 / dia, 5, MidpointRounding.AwayFromZero), 4, MidpointRounding.AwayFromZero),
            //    TotalPromedioPeruPetroNc4 = Math.Round(Math.Round(vTotalPromedioPeruPetroNC4 / dia, 5, MidpointRounding.AwayFromZero), 4, MidpointRounding.AwayFromZero),
            //    TotalPromedioPeruPetroNeoC5 = Math.Round(Math.Round(vTotalPromedioPeruPetroNeoC5 / dia, 5, MidpointRounding.AwayFromZero), 4, MidpointRounding.AwayFromZero),
            //    TotalPromedioPeruPetroIc5 = Math.Round(Math.Round(vTotalPromedioPeruPetroIC5 / dia, 5, MidpointRounding.AwayFromZero), 4, MidpointRounding.ToNegativeInfinity),
            //    TotalPromedioPeruPetroNc5 = Math.Round(Math.Round(vTotalPromedioPeruPetroNC5 / dia, 5, MidpointRounding.AwayFromZero), 4, MidpointRounding.AwayFromZero),
            //    TotalPromedioPeruPetroNitrog = Math.Round(Math.Round(vTotalPromedioPeruPetroN2 / dia, 5, MidpointRounding.AwayFromZero), 4, MidpointRounding.AwayFromZero),
            //    TotalPromedioPeruPetroC1 = Math.Round(Math.Round(vTotalPromedioPeruPetroC1 / dia, 5, MidpointRounding.AwayFromZero), 4, MidpointRounding.AwayFromZero),
            //    TotalPromedioPeruPetroCo2 = Math.Round(Math.Round(vTotalPromedioPeruPetroCO2 / dia, 5, MidpointRounding.AwayFromZero), 4, MidpointRounding.AwayFromZero),
            //    TotalPromedioPeruPetroC2 = Math.Round(Math.Round(vTotalPromedioPeruPetroC2 / dia, 5, MidpointRounding.AwayFromZero), 4, MidpointRounding.ToPositiveInfinity),
            //    TotalPromedioPeruPetroVol = vTotalPromedioPeruPetroVol,
            //    TotalPromedioUnnaC6 = Math.Round(Math.Round(vTotalPromedioPeruPetroC6 / dia, 5, MidpointRounding.AwayFromZero), 4, MidpointRounding.AwayFromZero),
            //    TotalPromedioUnnaC3 = Math.Round(Math.Round(vTotalPromedioPeruPetroC3 / dia, 5, MidpointRounding.AwayFromZero), 4, MidpointRounding.AwayFromZero),
            //    TotalPromedioUnnaIc4 = Math.Round(Math.Round(vTotalPromedioPeruPetroIC4 / dia, 5, MidpointRounding.AwayFromZero), 4, MidpointRounding.AwayFromZero),
            //    TotalPromedioUnnaNc4 = Math.Round(Math.Round(vTotalPromedioPeruPetroNC4 / dia, 5, MidpointRounding.AwayFromZero), 4, MidpointRounding.AwayFromZero),
            //    TotalPromedioUnnaNeoC5 = Math.Round(Math.Round(vTotalPromedioPeruPetroNeoC5 / dia, 5, MidpointRounding.AwayFromZero), 4, MidpointRounding.AwayFromZero),
            //    TotalPromedioUnnaIc5 = Math.Round(Math.Round(vTotalPromedioPeruPetroIC5 / dia, 5, MidpointRounding.AwayFromZero), 4, MidpointRounding.ToNegativeInfinity),
            //    TotalPromedioUnnaNc5 = Math.Round(Math.Round(vTotalPromedioPeruPetroNC5 / dia, 5, MidpointRounding.AwayFromZero), 4, MidpointRounding.AwayFromZero),
            //    TotalPromedioUnnaNitrog = Math.Round(Math.Round(vTotalPromedioPeruPetroN2 / dia, 5, MidpointRounding.AwayFromZero), 4, MidpointRounding.AwayFromZero),
            //    TotalPromedioUnnaC1 = Math.Round(Math.Round(vTotalPromedioPeruPetroC1 / dia, 5, MidpointRounding.AwayFromZero), 4, MidpointRounding.AwayFromZero),
            //    TotalPromedioUnnaCo2 = Math.Round(Math.Round(vTotalPromedioPeruPetroCO2 / dia, 5, MidpointRounding.AwayFromZero), 4, MidpointRounding.AwayFromZero),
            //    TotalPromedioUnnaC2 = Math.Round(Math.Round(vTotalPromedioPeruPetroC2 / dia, 5, MidpointRounding.AwayFromZero), 4, MidpointRounding.ToPositiveInfinity),
            //    TotalPromedioUnnaVol = 0,
            //    TotalDifC6 = 0.0000,
            //    TotalDifC3 = 0.0000,
            //    TotalDifIc4 = 0.0000,
            //    TotalDifNc4 = 0.0000,
            //    TotalDifNeoC5 = 0.0000,
            //    TotalDifIc5 = 0.0000,
            //    TotalDifNc5 = 0.0000,
            //    TotalDifNitrog = 0.0000,
            //    TotalDifC1 = 0.0000,
            //    TotalDifCo2 = 0.0000,
            //    TotalDifC2 = 0.0000,
            //    TotalDifVol = 0.0000

            //};
            var dto = new ComposicionGnaLIVDto();
            dto.Composicion = await ComposicionGnaLIVDetComposicion();
            dto.Componente = await ComposicionGnaLIVDetComponente();

            return new OperacionDto<ComposicionGnaLIVDto>(dto);

        }

        private async Task<List<ComposicionUnnaEnergiaLIVDto>> ComposicionGnaLIVDetComposicion()
        {
            var registros = await _composicionUnnaEnergiaPromedioRepositorio.ObtenerComposicionUnnaEnergiaPromedio(diaOperativo);
            var registrosVol = await _datoDeltaVRepositorio.ObtenerVolumenDeltaVAsync(diaOperativo);
            var registrosPC = await _registroRepositorio.ObtenerValorPoderCalorificoAsync(2, 4, diaOperativo);
            int j = -1;

            List<ComposicionUnnaEnergiaLIVDto> ComposicionGnaLIVDetComposicion = new List<ComposicionUnnaEnergiaLIVDto>();

            for (int i = 0; i < registros.Count; i = i + 11)
            {
                if (j < registrosVol.Count) { j++; }

                ComposicionGnaLIVDetComposicion.Add(new ComposicionUnnaEnergiaLIVDto
                {

                    Fecha = registros[i]?.Fecha.ToString("dd/MM/yyyy"),
                    C6 = registros[i]?.PromedioComponente,
                    C3 = registros[i + 1]?.PromedioComponente,
                    Ic4 = registros[i + 2]?.PromedioComponente,
                    Nc4 = registros[i + 3]?.PromedioComponente,
                    NeoC5 = registros[i + 4]?.PromedioComponente,
                    Ic5 = registros[i + 5]?.PromedioComponente,
                    Nc5 = registros[i + 6]?.PromedioComponente,
                    Nitrog = registros[i + 7]?.PromedioComponente,
                    C1 = registros[i + 8]?.PromedioComponente,
                    Co2 = registros[i + 9]?.PromedioComponente,
                    Observacion = ""
                }
                );

            }

            return ComposicionGnaLIVDetComposicion;
        }

        private async Task<List<ComposicionComponenteDto>> ComposicionGnaLIVDetComponente()
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
            List<ComposicionComponenteDto> ComposicionGnaLIVDetComponente = new List<ComposicionComponenteDto>();


            ComposicionGnaLIVDetComponente.Add(new ComposicionComponenteDto
            {
                Simbolo = "H2",
                Componente = "Hidrogen",
                Molecula = 0,
            }
            );
            ComposicionGnaLIVDetComponente.Add(new ComposicionComponenteDto
            {
                Simbolo = "H2S",
                Componente = "Hidrogen Sulphide",
                Molecula = 0,
            }
            );

            ComposicionGnaLIVDetComponente.Add(new ComposicionComponenteDto
            {

                Simbolo = "CO2",
                Componente = "Carbon Dioxide",
                Molecula = Math.Round(Math.Round(vMoleculaCO2 / dia, 5, MidpointRounding.AwayFromZero), 4, MidpointRounding.AwayFromZero),
               
            }
            );

            ComposicionGnaLIVDetComponente.Add(new ComposicionComponenteDto
            {
                Simbolo = "N2",
                Componente = "Nitrogen",
                Molecula = Math.Round(Math.Round(vMoleculaN2 / dia, 5, MidpointRounding.AwayFromZero), 4, MidpointRounding.AwayFromZero),
                
            }
            );
            ComposicionGnaLIVDetComponente.Add(new ComposicionComponenteDto
            {
                Simbolo = "C1",
                Componente = "Methane",
                Molecula = Math.Round(Math.Round(vMoleculaC1 / dia, 5, MidpointRounding.AwayFromZero), 4, MidpointRounding.AwayFromZero),
              
            }
            );
            ComposicionGnaLIVDetComponente.Add(new ComposicionComponenteDto
            {
                Simbolo = "C2",
                Componente = "Ethane",
                Molecula = Math.Round(Math.Round(vMoleculaC2 / dia, 5, MidpointRounding.AwayFromZero), 4, MidpointRounding.ToPositiveInfinity),
               
            }
            );
            ComposicionGnaLIVDetComponente.Add(new ComposicionComponenteDto
            {
                Simbolo = "C3",
                Componente = "Propane",
                Molecula = Math.Round(Math.Round(vMoleculaC3 / dia, 5, MidpointRounding.AwayFromZero), 4, MidpointRounding.AwayFromZero),
               
            }
            );
            ComposicionGnaLIVDetComponente.Add(new ComposicionComponenteDto
            {
                Simbolo = "IC4",
                Componente = "i-Butane",
                Molecula = Math.Round(Math.Round(vMoleculaIC4 / dia, 5, MidpointRounding.AwayFromZero), 4, MidpointRounding.AwayFromZero),
               
            }
            );
            ComposicionGnaLIVDetComponente.Add(new ComposicionComponenteDto
            {
                Simbolo = "NC4",
                Componente = "n-Butane",
                Molecula = Math.Round(Math.Round(vMoleculaNC4 / dia, 5, MidpointRounding.AwayFromZero), 4, MidpointRounding.AwayFromZero),
               
            }
            );
            ComposicionGnaLIVDetComponente.Add(new ComposicionComponenteDto
            {
                Simbolo = "IC5",
                Componente = "i-Pentane",
                Molecula = Math.Round(Math.Round(vMoleculaIC5 / dia, 5, MidpointRounding.AwayFromZero), 4, MidpointRounding.ToNegativeInfinity),
               
            }
            );
            ComposicionGnaLIVDetComponente.Add(new ComposicionComponenteDto
            {
                Simbolo = "NC5",
                Componente = "n-Pentane",
                Molecula = Math.Round(Math.Round(vMoleculaNC5 / dia, 5, MidpointRounding.AwayFromZero), 4, MidpointRounding.AwayFromZero),
               
            }
            );
            ComposicionGnaLIVDetComponente.Add(new ComposicionComponenteDto
            {
                Simbolo = "NeoC5",
                Componente = "NeoPentane",
                Molecula = Math.Round(Math.Round(vMoleculaNeoC5 / dia, 5, MidpointRounding.AwayFromZero), 4, MidpointRounding.AwayFromZero),
                
            }
            );
            ComposicionGnaLIVDetComponente.Add(new ComposicionComponenteDto
            {
                Simbolo = "C6",
                Componente = "Hexanes",
                Molecula = Math.Round(Math.Round(vMoleculaC6 / dia, 5, MidpointRounding.AwayFromZero), 4, MidpointRounding.AwayFromZero),
                

            }
            );
            ComposicionGnaLIVDetComponente.Add(new ComposicionComponenteDto
            {
                Simbolo = "C7",
                Componente = "Heptanes",
                Molecula = 0,
            }
            );
            ComposicionGnaLIVDetComponente.Add(new ComposicionComponenteDto
            {
                Simbolo = "C8",
                Componente = "Octanes",
                Molecula = 0,
            }
            );
            ComposicionGnaLIVDetComponente.Add(new ComposicionComponenteDto
            {
                Simbolo = "C9",
                Componente = "Nonanes",
                Molecula = 0,
            }
            );
            ComposicionGnaLIVDetComponente.Add(new ComposicionComponenteDto
            {
                Simbolo = "C10",
                Componente = "Decanes",
                Molecula = 0,
            }
            );
            ComposicionGnaLIVDetComponente.Add(new ComposicionComponenteDto
            {
                Simbolo = "C11",
                Componente = "Undecanes",
                Molecula = 0,
            }
            );
            ComposicionGnaLIVDetComponente.Add(new ComposicionComponenteDto
            {
                Simbolo = "C12+",
                Componente = "Dodecanes plus",
                Molecula = 0,
            }
            );

            return ComposicionGnaLIVDetComponente;
        }
    }

}
