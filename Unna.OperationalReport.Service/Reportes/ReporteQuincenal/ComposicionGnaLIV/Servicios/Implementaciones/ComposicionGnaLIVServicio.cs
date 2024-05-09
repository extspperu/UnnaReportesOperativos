using DocumentFormat.OpenXml.Drawing;
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
        DateTime diaOperativo = FechasUtilitario.ObtenerDiaOperativo();// DateTime.ParseExact("15/11/2023", "dd/MM/yyyy", CultureInfo.InvariantCulture);//FechasUtilitario.ObtenerDiaOperativo();
        double vCompMolPorcCO2 = 0;
        double vCompMolPorcN2 = 0;
        double vCompMolPorcC1 = 0;
        double vCompMolPorcC2 = 0;
        double vCompMolPorcC3 = 0;
        double vCompMolPorcIC4 = 0;
        double vCompMolPorcNC4 = 0;
        double vCompMolPorcIC5 = 0;
        double vCompMolPorcNC5 = 0;
        double vCompMolPorcNeoC5=0;
        double vCompMolPorcC6=0 ;
        double vTotalPromedioPeruPetroVol;
        double vTotalPromedioPeruPetroCO2 = 0;
        double vTotalPromedioPeruPetroN2 = 0;
        double vTotalPromedioPeruPetroC1 =0;
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

                    Fecha = "NOVIEMBRE 2023",
                    TotalPromedioPeruPetroC6 = Math.Round(Math.Round(vTotalPromedioPeruPetroC6 / diaOperativo.Day, 5, MidpointRounding.AwayFromZero), 4, MidpointRounding.AwayFromZero),      
                    TotalPromedioPeruPetroC3 = Math.Round(Math.Round(vTotalPromedioPeruPetroC3 / diaOperativo.Day, 5, MidpointRounding.AwayFromZero), 4, MidpointRounding.AwayFromZero),      
                    TotalPromedioPeruPetroIc4 = Math.Round(Math.Round(vTotalPromedioPeruPetroIC4 / diaOperativo.Day, 5, MidpointRounding.AwayFromZero), 4, MidpointRounding.AwayFromZero),    
                    TotalPromedioPeruPetroNc4 = Math.Round(Math.Round(vTotalPromedioPeruPetroNC4 / diaOperativo.Day, 5, MidpointRounding.AwayFromZero), 4, MidpointRounding.AwayFromZero),    
                    TotalPromedioPeruPetroNeoC5 = Math.Round(Math.Round(vTotalPromedioPeruPetroNeoC5 / diaOperativo.Day, 5, MidpointRounding.AwayFromZero), 4, MidpointRounding.AwayFromZero),
                    TotalPromedioPeruPetroIc5 = Math.Round(Math.Round(vTotalPromedioPeruPetroIC5 / diaOperativo.Day, 5, MidpointRounding.AwayFromZero), 4, MidpointRounding.ToNegativeInfinity),
                    TotalPromedioPeruPetroNc5 = Math.Round(Math.Round(vTotalPromedioPeruPetroNC5 / diaOperativo.Day, 5, MidpointRounding.AwayFromZero), 4, MidpointRounding.AwayFromZero),      
                    TotalPromedioPeruPetroNitrog = Math.Round(Math.Round(vTotalPromedioPeruPetroN2 / diaOperativo.Day, 5, MidpointRounding.AwayFromZero), 4, MidpointRounding.AwayFromZero),    
                    TotalPromedioPeruPetroC1 = Math.Round(Math.Round(vTotalPromedioPeruPetroC1 / diaOperativo.Day, 5, MidpointRounding.AwayFromZero), 4, MidpointRounding.AwayFromZero),        
                    TotalPromedioPeruPetroCo2 = Math.Round(Math.Round(vTotalPromedioPeruPetroCO2 / diaOperativo.Day, 5, MidpointRounding.AwayFromZero), 4, MidpointRounding.AwayFromZero),      
                    TotalPromedioPeruPetroC2 = Math.Round(Math.Round(vTotalPromedioPeruPetroC2 / diaOperativo.Day, 5, MidpointRounding.AwayFromZero), 4, MidpointRounding.ToPositiveInfinity),  
                    TotalPromedioPeruPetroVol = vTotalPromedioPeruPetroVol,
                    TotalPromedioUnnaC6 = Math.Round(Math.Round(vTotalPromedioPeruPetroC6 / diaOperativo.Day, 5, MidpointRounding.AwayFromZero), 4, MidpointRounding.AwayFromZero),             
                    TotalPromedioUnnaC3 = Math.Round(Math.Round(vTotalPromedioPeruPetroC3 / diaOperativo.Day, 5, MidpointRounding.AwayFromZero), 4, MidpointRounding.AwayFromZero),             
                    TotalPromedioUnnaIc4 = Math.Round(Math.Round(vTotalPromedioPeruPetroIC4 / diaOperativo.Day, 5, MidpointRounding.AwayFromZero), 4, MidpointRounding.AwayFromZero),           
                    TotalPromedioUnnaNc4 = Math.Round(Math.Round(vTotalPromedioPeruPetroNC4 / diaOperativo.Day, 5, MidpointRounding.AwayFromZero), 4, MidpointRounding.AwayFromZero),           
                    TotalPromedioUnnaNeoC5 = Math.Round(Math.Round(vTotalPromedioPeruPetroNeoC5 / diaOperativo.Day, 5, MidpointRounding.AwayFromZero), 4, MidpointRounding.AwayFromZero),       
                    TotalPromedioUnnaIc5 = Math.Round(Math.Round(vTotalPromedioPeruPetroIC5 / diaOperativo.Day, 5, MidpointRounding.AwayFromZero), 4, MidpointRounding.ToNegativeInfinity),     
                    TotalPromedioUnnaNc5 = Math.Round(Math.Round(vTotalPromedioPeruPetroNC5 / diaOperativo.Day, 5, MidpointRounding.AwayFromZero), 4, MidpointRounding.AwayFromZero),           
                    TotalPromedioUnnaNitrog = Math.Round(Math.Round(vTotalPromedioPeruPetroN2 / diaOperativo.Day, 5, MidpointRounding.AwayFromZero), 4, MidpointRounding.AwayFromZero),         
                    TotalPromedioUnnaC1 = Math.Round(Math.Round(vTotalPromedioPeruPetroC1 / diaOperativo.Day, 5, MidpointRounding.AwayFromZero), 4, MidpointRounding.AwayFromZero),             
                    TotalPromedioUnnaCo2 = Math.Round(Math.Round(vTotalPromedioPeruPetroCO2 / diaOperativo.Day, 5, MidpointRounding.AwayFromZero), 4, MidpointRounding.AwayFromZero),           
                    TotalPromedioUnnaC2 = Math.Round(Math.Round(vTotalPromedioPeruPetroC2 / diaOperativo.Day, 5, MidpointRounding.AwayFromZero), 4, MidpointRounding.ToPositiveInfinity),      
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
            
            List <ComposicionGnaLIVDetComposicionDto> ComposicionGnaLIVDetComposicion = new List<ComposicionGnaLIVDetComposicionDto>();
            
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
                        CompGnaTotal = 100.0000,
                        CompGnaVol = registrosVol[j].Volumen,
                        CompGnaPCalorifico = registrosPC[j].Valor,
                        CompGnaObservacion = ""
                    }
                    );

                }
            
            return ComposicionGnaLIVDetComposicion;
        }

        private async Task<List<ComposicionGnaLIVDetComponenteDto>> ComposicionGnaLIVDetComponente()
        {
           var registros = await _composicionUnnaEnergiaPromedioRepositorio.ObtenerComposicionUnnaEnergiaPromedio( diaOperativo);
           
            for (int i = 0; i < registros.Count; i++)
            {
                if (registros[i].Simbolo == "CO2") 
                {
                    vCompMolPorcCO2 = vCompMolPorcCO2 + registros[i].PromedioComponente;
                }
                if (registros[i].Simbolo == "N2")
                {
                    vCompMolPorcN2 = vCompMolPorcN2 + registros[i].PromedioComponente;
                }
                if (registros[i].Simbolo == "C1")
                {
                    vCompMolPorcC1 = vCompMolPorcC1 + registros[i].PromedioComponente;
                }
                if (registros[i].Simbolo == "C2")
                {
                    vCompMolPorcC2 = vCompMolPorcC2 + registros[i].PromedioComponente;
                }
                if (registros[i].Simbolo == "c3")
                {
                    vCompMolPorcC3 = vCompMolPorcC3 + registros[i].PromedioComponente;
                }
                if (registros[i].Simbolo == "iC4")
                {
                    vCompMolPorcIC4 = vCompMolPorcIC4 + registros[i].PromedioComponente;
                }
                if (registros[i].Simbolo == "nC4")
                {
                    vCompMolPorcNC4 = vCompMolPorcNC4 + registros[i].PromedioComponente;
                }
                if (registros[i].Simbolo == "iC5")
                {
                    vCompMolPorcIC5 = vCompMolPorcIC5 + registros[i].PromedioComponente;
                }
                if (registros[i].Simbolo == "nC5")
                {
                    vCompMolPorcNC5 = vCompMolPorcNC5 + registros[i].PromedioComponente;
                }
                if (registros[i].Simbolo == "NeoC5")
                {
                    vCompMolPorcNeoC5 = vCompMolPorcNeoC5 + registros[i].PromedioComponente;
                }
                if (registros[i].Simbolo == "C6")
                {
                    vCompMolPorcC6 = vCompMolPorcC6 + registros[i].PromedioComponente;
                }
            }
                List<ComposicionGnaLIVDetComponenteDto> ComposicionGnaLIVDetComponente = new List<ComposicionGnaLIVDetComponenteDto>();
           
                
                ComposicionGnaLIVDetComponente.Add(new ComposicionGnaLIVDetComponenteDto
                {
                    CompSimbolo = "H2",
                    CompDescripcion = "Hidrogen",
                    CompMolPorc = 0,
                    CompUnna = 0,
                    CompDif = 0
                }
                );
                ComposicionGnaLIVDetComponente.Add(new ComposicionGnaLIVDetComponenteDto
                {
                    CompSimbolo = "H2S",
                    CompDescripcion = "Hidrogen Sulphide",
                    CompMolPorc = 0,
                    CompUnna = 0,
                    CompDif = 0
                }
                );
                
                ComposicionGnaLIVDetComponente.Add(new ComposicionGnaLIVDetComponenteDto
                {
                     
                    CompSimbolo     = "CO2",
                    CompDescripcion = "Carbon Dioxide",
                    CompMolPorc     = Math.Round(Math.Round(vCompMolPorcCO2 / diaOperativo.Day, 5, MidpointRounding.AwayFromZero), 4, MidpointRounding.AwayFromZero),
                    CompUnna        = Math.Round(Math.Round(vCompMolPorcCO2 / diaOperativo.Day, 5, MidpointRounding.AwayFromZero), 4, MidpointRounding.AwayFromZero),
                    CompDif         = 0
                }
                );
               
                ComposicionGnaLIVDetComponente.Add(new ComposicionGnaLIVDetComponenteDto
                {
                    CompSimbolo     = "N2",
                    CompDescripcion = "Nitrogen",
                    CompMolPorc     = Math.Round(Math.Round(vCompMolPorcN2 / diaOperativo.Day, 5, MidpointRounding.AwayFromZero), 4, MidpointRounding.AwayFromZero),
                    CompUnna        = Math.Round(Math.Round(vCompMolPorcN2 / diaOperativo.Day, 5, MidpointRounding.AwayFromZero), 4, MidpointRounding.AwayFromZero),
                    CompDif = 0
                }
                );
                ComposicionGnaLIVDetComponente.Add(new ComposicionGnaLIVDetComponenteDto
                {
                    CompSimbolo = "C1",
                    CompDescripcion = "Methane",
                    CompMolPorc = Math.Round(Math.Round(vCompMolPorcC1 / diaOperativo.Day, 5, MidpointRounding.AwayFromZero), 4, MidpointRounding.AwayFromZero),
                    CompUnna    = Math.Round(Math.Round(vCompMolPorcC1 / diaOperativo.Day, 5, MidpointRounding.AwayFromZero), 4, MidpointRounding.AwayFromZero),
                    CompDif = 0
                }
                );
                ComposicionGnaLIVDetComponente.Add(new ComposicionGnaLIVDetComponenteDto
                {
                    CompSimbolo = "C2",
                    CompDescripcion = "Ethane",
                    CompMolPorc = Math.Round(Math.Round(vCompMolPorcC2 / diaOperativo.Day, 5, MidpointRounding.AwayFromZero), 4, MidpointRounding.ToPositiveInfinity),
                    CompUnna    = Math.Round(Math.Round(vCompMolPorcC2 / diaOperativo.Day, 5, MidpointRounding.AwayFromZero), 4, MidpointRounding.ToPositiveInfinity),
                    CompDif = 0
                }
                );
                ComposicionGnaLIVDetComponente.Add(new ComposicionGnaLIVDetComponenteDto
                {
                    CompSimbolo = "C3",
                    CompDescripcion = "Propane",
                    CompMolPorc     = Math.Round(Math.Round(vCompMolPorcC3 / diaOperativo.Day, 5, MidpointRounding.AwayFromZero), 4, MidpointRounding.AwayFromZero),
                    CompUnna        = Math.Round(Math.Round(vCompMolPorcC3 / diaOperativo.Day, 5, MidpointRounding.AwayFromZero), 4, MidpointRounding.AwayFromZero),
                    CompDif = 0
                }
                );
                ComposicionGnaLIVDetComponente.Add(new ComposicionGnaLIVDetComponenteDto
                {
                    CompSimbolo = "IC4",
                    CompDescripcion = "i-Butane",
                    CompMolPorc     = Math.Round(Math.Round(vCompMolPorcIC4 / diaOperativo.Day, 5, MidpointRounding.AwayFromZero), 4, MidpointRounding.AwayFromZero),
                    CompUnna        = Math.Round(Math.Round(vCompMolPorcIC4 / diaOperativo.Day, 5, MidpointRounding.AwayFromZero), 4, MidpointRounding.AwayFromZero),
                    CompDif = 0
                }
                );
                ComposicionGnaLIVDetComponente.Add(new ComposicionGnaLIVDetComponenteDto
                {
                    CompSimbolo = "NC4",
                    CompDescripcion = "n-Butane",
                    CompMolPorc = Math.Round(Math.Round(vCompMolPorcNC4 / diaOperativo.Day, 5, MidpointRounding.AwayFromZero), 4, MidpointRounding.AwayFromZero),
                    CompUnna    = Math.Round(Math.Round(vCompMolPorcNC4 / diaOperativo.Day, 5, MidpointRounding.AwayFromZero), 4, MidpointRounding.AwayFromZero),
                    CompDif = 0
                }
                );
                ComposicionGnaLIVDetComponente.Add(new ComposicionGnaLIVDetComponenteDto
                {
                    CompSimbolo = "IC5",
                    CompDescripcion = "i-Pentane",
                    CompMolPorc = Math.Round(Math.Round(vCompMolPorcIC5 / diaOperativo.Day, 5, MidpointRounding.AwayFromZero), 4, MidpointRounding.ToNegativeInfinity),
                    CompUnna    = Math.Round(Math.Round(vCompMolPorcIC5 / diaOperativo.Day, 5, MidpointRounding.AwayFromZero), 4, MidpointRounding.ToNegativeInfinity),
                    CompDif = 0
                }
                );
                ComposicionGnaLIVDetComponente.Add(new ComposicionGnaLIVDetComponenteDto
                {
                    CompSimbolo = "NC5",
                    CompDescripcion = "n-Pentane",
                    CompMolPorc = Math.Round(Math.Round(vCompMolPorcNC5 / diaOperativo.Day, 5, MidpointRounding.AwayFromZero), 4, MidpointRounding.AwayFromZero),
                    CompUnna    = Math.Round(Math.Round(vCompMolPorcNC5 / diaOperativo.Day, 5, MidpointRounding.AwayFromZero), 4, MidpointRounding.AwayFromZero),
                    CompDif = 0
                }
                );
                ComposicionGnaLIVDetComponente.Add(new ComposicionGnaLIVDetComponenteDto
                {
                    CompSimbolo = "NeoC5",
                    CompDescripcion = "NeoPentane",
                    CompMolPorc = Math.Round(Math.Round(vCompMolPorcNeoC5 / diaOperativo.Day, 5, MidpointRounding.AwayFromZero), 4, MidpointRounding.AwayFromZero),
                    CompUnna    = Math.Round(Math.Round(vCompMolPorcNeoC5 / diaOperativo.Day, 5, MidpointRounding.AwayFromZero), 4, MidpointRounding.AwayFromZero),
                    CompDif = 0
                }
                );
                ComposicionGnaLIVDetComponente.Add(new ComposicionGnaLIVDetComponenteDto
                {
                    CompSimbolo = "C6",
                    CompDescripcion = "Hexanes",
                    CompMolPorc = Math.Round(Math.Round(vCompMolPorcC6 / diaOperativo.Day, 5, MidpointRounding.AwayFromZero), 4, MidpointRounding.AwayFromZero),
                    CompUnna    = Math.Round(Math.Round(vCompMolPorcC6 / diaOperativo.Day, 5, MidpointRounding.AwayFromZero), 4, MidpointRounding.AwayFromZero),
                    CompDif = 0

                }
                );
                ComposicionGnaLIVDetComponente.Add(new ComposicionGnaLIVDetComponenteDto
                {
                    CompSimbolo = "C7",
                    CompDescripcion = "Heptanes",
                    CompMolPorc = 0,
                    CompUnna = 0,
                    CompDif = 0
                }
                );
                ComposicionGnaLIVDetComponente.Add(new ComposicionGnaLIVDetComponenteDto
                {
                    CompSimbolo = "C8",
                    CompDescripcion = "Octanes",
                    CompMolPorc = 0,
                    CompUnna = 0,
                    CompDif = 0
                }
                );
                ComposicionGnaLIVDetComponente.Add(new ComposicionGnaLIVDetComponenteDto
                {
                    CompSimbolo = "C9",
                    CompDescripcion = "Nonanes",
                    CompMolPorc = 0,
                    CompUnna = 0,
                    CompDif = 0
                }
                );
                ComposicionGnaLIVDetComponente.Add(new ComposicionGnaLIVDetComponenteDto
                {
                    CompSimbolo = "C10",
                    CompDescripcion = "Decanes",
                    CompMolPorc = 0,
                    CompUnna = 0,
                    CompDif = 0
                }
                );
                ComposicionGnaLIVDetComponente.Add(new ComposicionGnaLIVDetComponenteDto
                {
                    CompSimbolo = "C11",
                    CompDescripcion = "Undecanes",
                    CompMolPorc = 0,
                    CompUnna = 0,
                    CompDif = 0
                }
                );
                ComposicionGnaLIVDetComponente.Add(new ComposicionGnaLIVDetComponenteDto
                {
                    CompSimbolo = "C12+",
                    CompDescripcion = "Dodecanes plus",
                    CompMolPorc = 0,
                    CompUnna = 0,
                    CompDif = 0
                }
                );
            
            return ComposicionGnaLIVDetComponente;
        }
    }
}
