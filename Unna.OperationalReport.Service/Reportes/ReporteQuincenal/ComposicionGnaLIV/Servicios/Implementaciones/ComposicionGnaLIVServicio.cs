using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.FacturacionGnsLIV.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ComposicionGnaLIV.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ComposicionGnaLIV.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;

namespace Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ComposicionGnaLIV.Servicios.Implementaciones
{
    public class ComposicionGnaLIVServicio : IComposicionGnaLIVServicio

    {
        public async Task<OperacionDto<ComposicionGnaLIVDto>> ObtenerAsync(long idUsuario)
        {
            var dto = new ComposicionGnaLIVDto
            {
                Fecha = "NOVIEMBRE 2023",
                TotalPromedioPeruPetroC6 = 0.3505,
                TotalPromedioPeruPetroC3 = 2.1349,
                TotalPromedioPeruPetroIc4 = 0.8098,
                TotalPromedioPeruPetroNc4 = 1.0339,
                TotalPromedioPeruPetroNeoC5 = 0.0115,
                TotalPromedioPeruPetroIc5 = 0.3554,
                TotalPromedioPeruPetroNc5 = 0.4700,
                TotalPromedioPeruPetroNitrog = 0.2672,
                TotalPromedioPeruPetroC1 = 90.1558,
                TotalPromedioPeruPetroCo2 = 0.2560,
                TotalPromedioPeruPetroC2 = 4.1549,
                TotalPromedioPeruPetroVol = 61623.4940,
                TotalPromedioUnnaC6 = 0.3505,
                TotalPromedioUnnaC3 = 2.1349,
                TotalPromedioUnnaIc4 = 0.8098,
                TotalPromedioUnnaNc4 = 1.0339,
                TotalPromedioUnnaNeoC5 = 0.0115,
                TotalPromedioUnnaIc5 = 0.3554,
                TotalPromedioUnnaNc5 = 0.4700,
                TotalPromedioUnnaNitrog = 0.2672,
                TotalPromedioUnnaC1 = 90.1558,
                TotalPromedioUnnaCo2 = 0.2560,
                TotalPromedioUnnaC2 = 4.1549,
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
                TotalDifVol = 0.0000,
            };

            dto.ComposicionGnaLIVDetComposicion = await ComposicionGnaLIVDetComposicion();
            dto.ComposicionGnaLIVDetComponente = await ComposicionGnaLIVDetComponente();

            return new OperacionDto<ComposicionGnaLIVDto>(dto);
        }

        private async Task<List<ComposicionGnaLIVDetComposicionDto>> ComposicionGnaLIVDetComposicion()
        {

            List<ComposicionGnaLIVDetComposicionDto> ComposicionGnaLIVDetComposicion = new List<ComposicionGnaLIVDetComposicionDto>();

            ComposicionGnaLIVDetComposicion.Add(new ComposicionGnaLIVDetComposicionDto
            {
                CompGnaDia = "1/11/2023",
                CompGnaC6 = 0.3491040,
                CompGnaC3 = 2.140200,
                CompGnaIc4 = 0.811257,
                CompGnaNc4 = 1.036110,
                CompGnaNeoC5 = 0.0114588,
                CompGnaIc5 = 0.355811,
                CompGnaNc5 = 0.472289,
                CompGnaNitrog = 0.320038,
                CompGnaC1 = 90.075000,
                CompGnaCo2 = 0.252294,
                CompGnaC2 = 4.176440,
                CompGnaTotal = 100.0000,
                CompGnaVol = 4193.7080,
                CompGnaPCalorifico = 1137.24,
                CompGnaObservacion = ""
            }
            );

            ComposicionGnaLIVDetComposicion.Add(new ComposicionGnaLIVDetComposicionDto
            {
                CompGnaDia = "2/11/2023",
                CompGnaC6 = 0.321542,
                CompGnaC3 = 2.14769,
                CompGnaIc4 = 0.80578,
                CompGnaNc4 = 1.02471,
                CompGnaNeoC5 = 0.0113471,
                CompGnaIc5 = 0.344008,
                CompGnaNc5 = 0.45158,
                CompGnaNitrog = 0.23286,
                CompGnaC1 = 90.2176,
                CompGnaCo2 = 0.251835,
                CompGnaC2 = 4.19106,
                CompGnaTotal = 100,
                CompGnaVol = 4193.708,
                CompGnaPCalorifico = 1137.24,
                CompGnaObservacion = "",
            }
);

            ComposicionGnaLIVDetComposicion.Add(new ComposicionGnaLIVDetComposicionDto
            {
                CompGnaDia = "3/11/2023",
                CompGnaC6 = 0.344981,
                CompGnaC3 = 2.14365,
                CompGnaIc4 = 0.813408,
                CompGnaNc4 = 1.04111,
                CompGnaNeoC5 = 0.011685,
                CompGnaIc5 = 0.356614,
                CompGnaNc5 = 0.473803,
                CompGnaNitrog = 0.191665,
                CompGnaC1 = 90.1951,
                CompGnaCo2 = 0.260966,
                CompGnaC2 = 4.16704,
                CompGnaTotal = 100,
                CompGnaVol = 4155.188,
                CompGnaPCalorifico = 1151.49,
                CompGnaObservacion = "",
            }
            );

            ComposicionGnaLIVDetComposicion.Add(new ComposicionGnaLIVDetComposicionDto
            {
                CompGnaDia = "4/11/2023",
                CompGnaC6 = 0.339296,
                CompGnaC3 = 2.12872,
                CompGnaIc4 = 0.80709,
                CompGnaNc4 = 1.03028,
                CompGnaNeoC5 = 0.0116232,
                CompGnaIc5 = 0.352623,
                CompGnaNc5 = 0.466086,
                CompGnaNitrog = 0.202292,
                CompGnaC1 = 90.2521,
                CompGnaCo2 = 0.255135,
                CompGnaC2 = 4.15472,
                CompGnaTotal = 100,
                CompGnaVol = 4084.943,
                CompGnaPCalorifico = 1152.83,
                CompGnaObservacion = "",
            }
            );

            ComposicionGnaLIVDetComposicion.Add(new ComposicionGnaLIVDetComposicionDto
            {
                CompGnaDia = "5/11/2023",
                CompGnaC6 = 0.3421,
                CompGnaC3 = 2.14818,
                CompGnaIc4 = 0.815068,
                CompGnaNc4 = 1.04617,
                CompGnaNeoC5 = 0.0120231,
                CompGnaIc5 = 0.359785,
                CompGnaNc5 = 0.478935,
                CompGnaNitrog = 0.275756,
                CompGnaC1 = 90.0993,
                CompGnaCo2 = 0.257281,
                CompGnaC2 = 4.16538,
                CompGnaTotal = 100,
                CompGnaVol = 4005.724,
                CompGnaPCalorifico = 1153.72,
                CompGnaObservacion = "",
            }
            );

            ComposicionGnaLIVDetComposicion.Add(new ComposicionGnaLIVDetComposicionDto
            {
                CompGnaDia = "6/11/2023",
                CompGnaC6 = 0.350776,
                CompGnaC3 = 2.12966,
                CompGnaIc4 = 0.806753,
                CompGnaNc4 = 1.03197,
                CompGnaNeoC5 = 0.0115137,
                CompGnaIc5 = 0.355071,
                CompGnaNc5 = 0.471699,
                CompGnaNitrog = 0.282786,
                CompGnaC1 = 90.1477,
                CompGnaCo2 = 0.255344,
                CompGnaC2 = 4.15669,
                CompGnaTotal = 100,
                CompGnaVol = 4100.4,
                CompGnaPCalorifico = 1152.81,
                CompGnaObservacion = "",
            }
            );

            ComposicionGnaLIVDetComposicion.Add(new ComposicionGnaLIVDetComposicionDto
            {
                CompGnaDia = "7/11/2023",
                CompGnaC6 = 0.351809,
                CompGnaC3 = 2.12033,
                CompGnaIc4 = 0.804936,
                CompGnaNc4 = 1.02834,
                CompGnaNeoC5 = 0.0113193,
                CompGnaIc5 = 0.355024,
                CompGnaNc5 = 0.471607,
                CompGnaNitrog = 0.277289,
                CompGnaC1 = 90.1826,
                CompGnaCo2 = 0.25628,
                CompGnaC2 = 4.14044,
                CompGnaTotal = 100,
                CompGnaVol = 4082.561,
                CompGnaPCalorifico = 1152.5,
                CompGnaObservacion = "",
            }
            );

            ComposicionGnaLIVDetComposicion.Add(new ComposicionGnaLIVDetComposicionDto
            {
                CompGnaDia = "8/11/2023",
                CompGnaC6 = 0.355665,
                CompGnaC3 = 2.12841,
                CompGnaIc4 = 0.80612,
                CompGnaNc4 = 1.02782,
                CompGnaNeoC5 = 0.0116597,
                CompGnaIc5 = 0.354486,
                CompGnaNc5 = 0.469457,
                CompGnaNitrog = 0.273591,
                CompGnaC1 = 90.1694,
                CompGnaCo2 = 0.248786,
                CompGnaC2 = 4.15459,
                CompGnaTotal = 100,
                CompGnaVol = 4166.253,
                CompGnaPCalorifico = 1152.95,
                CompGnaObservacion = "",
            }
            );


            ComposicionGnaLIVDetComposicion.Add(new ComposicionGnaLIVDetComposicionDto
            {
                CompGnaDia = "9/11/2023",
                CompGnaC6 = 0.351493,
                CompGnaC3 = 2.1117,
                CompGnaIc4 = 0.798096,
                CompGnaNc4 = 1.01612,
                CompGnaNeoC5 = 0.0109648,
                CompGnaIc5 = 0.348204,
                CompGnaNc5 = 0.458526,
                CompGnaNitrog = 0.237913,
                CompGnaC1 = 90.2747,
                CompGnaCo2 = 0.263345,
                CompGnaC2 = 4.12898,
                CompGnaTotal = 100,
                CompGnaVol = 4134.603,
                CompGnaPCalorifico = 1151.55,
                CompGnaObservacion = "",
            }
            );

            ComposicionGnaLIVDetComposicion.Add(new ComposicionGnaLIVDetComposicionDto
            {
                CompGnaDia = "10/11/2023",
                CompGnaC6 = 0.352943,
                CompGnaC3 = 2.09986,
                CompGnaIc4 = 0.799486,
                CompGnaNc4 = 1.02075,
                CompGnaNeoC5 = 0.0112877,
                CompGnaIc5 = 0.353977,
                CompGnaNc5 = 0.469013,
                CompGnaNitrog = 0.226973,
                CompGnaC1 = 90.3083,
                CompGnaCo2 = 0.257978,
                CompGnaC2 = 4.09944,
                CompGnaTotal = 100,
                CompGnaVol = 4023.96,
                CompGnaPCalorifico = 1152,
                CompGnaObservacion = "",
            }
            );

            ComposicionGnaLIVDetComposicion.Add(new ComposicionGnaLIVDetComposicionDto
            {
                CompGnaDia = "11/11/2023",
                CompGnaC6 = 0.362821,
                CompGnaC3 = 2.12771,
                CompGnaIc4 = 0.809116,
                CompGnaNc4 = 1.0332,
                CompGnaNeoC5 = 0.011306,
                CompGnaIc5 = 0.357391,
                CompGnaNc5 = 0.471925,
                CompGnaNitrog = 0.273686,
                CompGnaC1 = 90.1613,
                CompGnaCo2 = 0.25769,
                CompGnaC2 = 4.13382,
                CompGnaTotal = 100,
                CompGnaVol = 4045.171,
                CompGnaPCalorifico = 1153.34,
                CompGnaObservacion = "",
            }
            );

            ComposicionGnaLIVDetComposicion.Add(new ComposicionGnaLIVDetComposicionDto
            {
                CompGnaDia = "12/11/2023",
                CompGnaC6 = 0.370436,
                CompGnaC3 = 2.16225,
                CompGnaIc4 = 0.827624,
                CompGnaNc4 = 1.06133,
                CompGnaNeoC5 = 0.0118119,
                CompGnaIc5 = 0.368898,
                CompGnaNc5 = 0.491083,
                CompGnaNitrog = 0.29001,
                CompGnaC1 = 89.9782,
                CompGnaCo2 = 0.267697,
                CompGnaC2 = 4.1707,
                CompGnaTotal = 100,
                CompGnaVol = 4133.973,
                CompGnaPCalorifico = 1156.21,
                CompGnaObservacion = "",
            }
            );

            ComposicionGnaLIVDetComposicion.Add(new ComposicionGnaLIVDetComposicionDto
            {
                CompGnaDia = "13/11/2023",
                CompGnaC6 = 0.363971,
                CompGnaC3 = 2.13667,
                CompGnaIc4 = 0.811505,
                CompGnaNc4 = 1.03421,
                CompGnaNeoC5 = 0.0115248,
                CompGnaIc5 = 0.357835,
                CompGnaNc5 = 0.471959,
                CompGnaNitrog = 0.287436,
                CompGnaC1 = 90.1123,
                CompGnaCo2 = 0.25633,
                CompGnaC2 = 4.15622,
                CompGnaTotal = 100,
                CompGnaVol = 4201.688,
                CompGnaPCalorifico = 1153.67,
                CompGnaObservacion = "",
            }
            );

            ComposicionGnaLIVDetComposicion.Add(new ComposicionGnaLIVDetComposicionDto
            {
                CompGnaDia = "14/11/2023",
                CompGnaC6 = 0.359437,
                CompGnaC3 = 2.14381,
                CompGnaIc4 = 0.817117,
                CompGnaNc4 = 1.04264,
                CompGnaNeoC5 = 0.0118774,
                CompGnaIc5 = 0.359971,
                CompGnaNc5 = 0.475536,
                CompGnaNitrog = 0.299503,
                CompGnaC1 = 90.0739,
                CompGnaCo2 = 0.255047,
                CompGnaC2 = 4.16115,
                CompGnaTotal = 100,
                CompGnaVol = 4222.862,
                CompGnaPCalorifico = 1154.01,
                CompGnaObservacion = "",
            }
            );

            ComposicionGnaLIVDetComposicion.Add(new ComposicionGnaLIVDetComposicionDto
            {
                CompGnaDia = "15/11/2023",
                CompGnaC6 = 0.341452,
                CompGnaC3 = 2.1549,
                CompGnaIc4 = 0.813522,
                CompGnaNc4 = 1.03359,
                CompGnaNeoC5 = 0.0115564,
                CompGnaIc5 = 0.350775,
                CompGnaNc5 = 0.45695,
                CompGnaNitrog = 0.336936,
                CompGnaC1 = 90.089,
                CompGnaCo2 = 0.24465,
                CompGnaC2 = 4.1667,
                CompGnaTotal = 100,
                CompGnaVol = 4024.073,
                CompGnaPCalorifico = 1152.04,
                CompGnaObservacion = "",
            }
            );

            return ComposicionGnaLIVDetComposicion;
        }

        private async Task<List<ComposicionGnaLIVDetComponenteDto>> ComposicionGnaLIVDetComponente()
        {

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
                CompSimbolo = "CO2",
                CompDescripcion = "Carbon Dioxide",
                CompMolPorc = 0.2560,
                CompUnna = 0.2560,
                CompDif = 0
            }
            );
            ComposicionGnaLIVDetComponente.Add(new ComposicionGnaLIVDetComponenteDto
            {
                CompSimbolo = "N2",
                CompDescripcion = "Nitrogen",
                CompMolPorc = 0.2672,
                CompUnna = 0.2672,
                CompDif = 0
            }
            );
            ComposicionGnaLIVDetComponente.Add(new ComposicionGnaLIVDetComponenteDto
            {
                CompSimbolo = "C1",
                CompDescripcion = "Methane",
                CompMolPorc = 90.1558,
                CompUnna = 90.1558,
                CompDif = 0
            }
            );
            ComposicionGnaLIVDetComponente.Add(new ComposicionGnaLIVDetComponenteDto
            {
                CompSimbolo = "C2",
                CompDescripcion = "Ethane",
                CompMolPorc = 4.1549,
                CompUnna = 4.1549,
                CompDif = 0
            }
            );
            ComposicionGnaLIVDetComponente.Add(new ComposicionGnaLIVDetComponenteDto
            {
                CompSimbolo = "C3",
                CompDescripcion = "Propane",
                CompMolPorc = 2.1349,
                CompUnna = 2.1349,
                CompDif = 0
            }
            );
            ComposicionGnaLIVDetComponente.Add(new ComposicionGnaLIVDetComponenteDto
            {
                CompSimbolo = "IC5",
                CompDescripcion = "i-Butane",
                CompMolPorc = 0.8098,
                CompUnna = 0.8098,
                CompDif = 0
            }
            );
            ComposicionGnaLIVDetComponente.Add(new ComposicionGnaLIVDetComponenteDto
            {
                CompSimbolo = "NC4",
                CompDescripcion = "n-Butane",
                CompMolPorc = 1.0339,
                CompUnna = 1.0339,
                CompDif = 0
            }
            );
            ComposicionGnaLIVDetComponente.Add(new ComposicionGnaLIVDetComponenteDto
            {
                CompSimbolo = "IC5",
                CompDescripcion = "i-Pentane",
                CompMolPorc = 0.3554,
                CompUnna = 0.3554,
                CompDif = 0
            }
            );
            ComposicionGnaLIVDetComponente.Add(new ComposicionGnaLIVDetComponenteDto
            {
                CompSimbolo = "NC5",
                CompDescripcion = "n-Pentane",
                CompMolPorc = 0.4700,
                CompUnna = 0.4700,
                CompDif = 0
            }
            );
            ComposicionGnaLIVDetComponente.Add(new ComposicionGnaLIVDetComponenteDto
            {
                CompSimbolo = "NeoC5",
                CompDescripcion = "NeoPentane",
                CompMolPorc = 0.0115,
                CompUnna = 0.0115,
                CompDif = 0
            }
            );
            ComposicionGnaLIVDetComponente.Add(new ComposicionGnaLIVDetComponenteDto
            {
                CompSimbolo = "C6",
                CompDescripcion = "Hexanes",
                CompMolPorc = 0.3505,
                CompUnna = 0.3505,
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
