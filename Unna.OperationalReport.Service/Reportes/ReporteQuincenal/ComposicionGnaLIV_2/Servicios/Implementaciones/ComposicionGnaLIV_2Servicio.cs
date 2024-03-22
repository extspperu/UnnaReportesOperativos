using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ComposicionGnaLIV_2.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ComposicionGnaLIV_2.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;

namespace Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ComposicionGnaLIV_2.Servicios.Implementaciones
{
    public class ComposicionGnaLIV_2Servicio : IComposicionGnaLIV_2Servicio
    {
        public async Task<OperacionDto<ComposicionGnaLIV_2Dto>> ObtenerAsync(long idUsuario)
        {

            var dto = new ComposicionGnaLIV_2Dto
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
                TotalDifVol = 0.0000
            };

            dto.ComposicionGnaLIV_2DetComposicion = await ComposicionGnaLIV_2DetComposicion();
            dto.ComposicionGnaLIV_2DetComponente = await ComposicionGnaLIV_2DetComponente();

            return new OperacionDto<ComposicionGnaLIV_2Dto>(dto);
        }


        private async Task<List<ComposicionGnaLIV_2DetComposicionDto>?> ComposicionGnaLIV_2DetComposicion()
        {

            List<ComposicionGnaLIV_2DetComposicionDto> ComposicionGnaLIV_2DetComposicion = new List<ComposicionGnaLIV_2DetComposicionDto>();

            ComposicionGnaLIV_2DetComposicion.Add(new ComposicionGnaLIV_2DetComposicionDto
            {
                CompGnaDia = "16/11/2023",
                CompGnaC6 = 0.3718,
                CompGnaC3 = 2.1342,
                CompGnaIc4 = 0.8092,
                CompGnaNc4 = 1.0305,
                CompGnaNeoC5 = 0.0117,
                CompGnaIc5 = 0.3556,
                CompGnaNc5 = 0.4708,
                CompGnaNitrog = 0.3052,
                CompGnaC1 = 90.0996,
                CompGnaCo2 = 0.2447,
                CompGnaC2 = 4.1667,
                CompGnaTotal = 100.0000,
                CompGnaVol = 4193.7080,
                CompGnaPCalorifico = 1137.24,
                CompGnaObservacion = ""
            }
);

            ComposicionGnaLIV_2DetComposicion.Add(new ComposicionGnaLIV_2DetComposicionDto
            {
                CompGnaDia = "17/11/2023",
                CompGnaC6 = 0.3744,
                CompGnaC3 = 2.1217,
                CompGnaIc4 = 0.8039,
                CompGnaNc4 = 1.0228,
                CompGnaNeoC5 = 0.0115,
                CompGnaIc5 = 0.3511,
                CompGnaNc5 = 0.4624,
                CompGnaNitrog = 0.2999,
                CompGnaC1 = 90.1532,
                CompGnaCo2 = 0.2386,
                CompGnaC2 = 4.1604,
                CompGnaTotal = 100,
                CompGnaVol = 4193.708,
                CompGnaPCalorifico = 1137.24,
                CompGnaObservacion = ""
            }
            );

            ComposicionGnaLIV_2DetComposicion.Add(new ComposicionGnaLIV_2DetComposicionDto
            {
                CompGnaDia = "18/11/2023",
                CompGnaC6 = 0.3786,
                CompGnaC3 = 2.1459,
                CompGnaIc4 = 0.8143,
                CompGnaNc4 = 1.0360,
                CompGnaNeoC5 = 0.0117,
                CompGnaIc5 = 0.3557,
                CompGnaNc5 = 0.4688,
                CompGnaNitrog = 0.2935,
                CompGnaC1 = 90.0682,
                CompGnaCo2 = 0.2391,
                CompGnaC2 = 4.1881,
                CompGnaTotal = 100,
                CompGnaVol = 4155.188,
                CompGnaPCalorifico = 1151.49,
                CompGnaObservacion = ""
            }
            );

            ComposicionGnaLIV_2DetComposicion.Add(new ComposicionGnaLIV_2DetComposicionDto
            {
                CompGnaDia = "19/11/2023",
                CompGnaC6 = 0.3846,
                CompGnaC3 = 2.1146,
                CompGnaIc4 = 0.8013,
                CompGnaNc4 = 1.0225,
                CompGnaNeoC5 = 0.0118,
                CompGnaIc5 = 0.3561,
                CompGnaNc5 = 0.4753,
                CompGnaNitrog = 0.1896,
                CompGnaC1 = 90.2297,
                CompGnaCo2 = 0.2409,
                CompGnaC2 = 4.1737,
                CompGnaTotal = 100,
                CompGnaVol = 4084.943,
                CompGnaPCalorifico = 1152.83,
                CompGnaObservacion = ""
            }
            );

            ComposicionGnaLIV_2DetComposicion.Add(new ComposicionGnaLIV_2DetComposicionDto
            {
                CompGnaDia = "20/11/2023",
                CompGnaC6 = 0.3638,
                CompGnaC3 = 2.1579,
                CompGnaIc4 = 0.8151,
                CompGnaNc4 = 1.0430,
                CompGnaNeoC5 = 0.0114,
                CompGnaIc5 = 0.3573,
                CompGnaNc5 = 0.4740,
                CompGnaNitrog = 0.1522,
                CompGnaC1 = 90.1805,
                CompGnaCo2 = 0.2423,
                CompGnaC2 = 4.2025,
                CompGnaTotal = 100,
                CompGnaVol = 4005.724,
                CompGnaPCalorifico = 1153.72,
                CompGnaObservacion = ""
            }
            );

            ComposicionGnaLIV_2DetComposicion.Add(new ComposicionGnaLIV_2DetComposicionDto
            {
                CompGnaDia = "21/11/2023",
                CompGnaC6 = 0.3670,
                CompGnaC3 = 2.1221,
                CompGnaIc4 = 0.8080,
                CompGnaNc4 = 1.0305,
                CompGnaNeoC5 = 0.0117,
                CompGnaIc5 = 0.3591,
                CompGnaNc5 = 0.4781,
                CompGnaNitrog = 0.1593,
                CompGnaC1 = 90.2568,
                CompGnaCo2 = 0.2392,
                CompGnaC2 = 4.1681,
                CompGnaTotal = 100,
                CompGnaVol = 4100.4,
                CompGnaPCalorifico = 1152.81,
                CompGnaObservacion = ""
            }
            );

            ComposicionGnaLIV_2DetComposicion.Add(new ComposicionGnaLIV_2DetComposicionDto
            {
                CompGnaDia = "22/11/2023",
                CompGnaC6 = 0.3753,
                CompGnaC3 = 2.2314,
                CompGnaIc4 = 0.8451,
                CompGnaNc4 = 1.0788,
                CompGnaNeoC5 = 0.0116,
                CompGnaIc5 = 0.3718,
                CompGnaNc5 = 0.4954,
                CompGnaNitrog = 0.1448,
                CompGnaC1 = 89.8783,
                CompGnaCo2 = 0.2549,
                CompGnaC2 = 4.3127,
                CompGnaTotal = 100,
                CompGnaVol = 4082.561,
                CompGnaPCalorifico = 1152.5,
                CompGnaObservacion = ""
            }
            );

            ComposicionGnaLIV_2DetComposicion.Add(new ComposicionGnaLIV_2DetComposicionDto
            {
                CompGnaDia = "23/11/2023",
                CompGnaC6 = 0.3700,
                CompGnaC3 = 2.1595,
                CompGnaIc4 = 0.8170,
                CompGnaNc4 = 1.0429,
                CompGnaNeoC5 = 0.0113,
                CompGnaIc5 = 0.3593,
                CompGnaNc5 = 0.4754,
                CompGnaNitrog = 0.1456,
                CompGnaC1 = 90.1704,
                CompGnaCo2 = 0.2430,
                CompGnaC2 = 4.2057,
                CompGnaTotal = 100,
                CompGnaVol = 4166.253,
                CompGnaPCalorifico = 1152.95,
                CompGnaObservacion = ""
            }
            );


            ComposicionGnaLIV_2DetComposicion.Add(new ComposicionGnaLIV_2DetComposicionDto
            {
                CompGnaDia = "24/11/2023",
                CompGnaC6 = 0.3751,
                CompGnaC3 = 2.1451,
                CompGnaIc4 = 0.8169,
                CompGnaNc4 = 1.0422,
                CompGnaNeoC5 = 0.0114,
                CompGnaIc5 = 0.3605,
                CompGnaNc5 = 0.4765,
                CompGnaNitrog = 0.1966,
                CompGnaC1 = 90.1514,
                CompGnaCo2 = 0.2439,
                CompGnaC2 = 4.1805,
                CompGnaTotal = 100,
                CompGnaVol = 4134.603,
                CompGnaPCalorifico = 1151.55,
                CompGnaObservacion = ""
            }
            );

            ComposicionGnaLIV_2DetComposicion.Add(new ComposicionGnaLIV_2DetComposicionDto
            {
                CompGnaDia = "25/11/2023",
                CompGnaC6 = 0.3668,
                CompGnaC3 = 2.1847,
                CompGnaIc4 = 0.8345,
                CompGnaNc4 = 1.0686,
                CompGnaNeoC5 = 0.0114,
                CompGnaIc5 = 0.3698,
                CompGnaNc5 = 0.4901,
                CompGnaNitrog = 0.2492,
                CompGnaC1 = 89.9593,
                CompGnaCo2 = 0.2519,
                CompGnaC2 = 4.2137,
                CompGnaTotal = 100,
                CompGnaVol = 4023.96,
                CompGnaPCalorifico = 1152,
                CompGnaObservacion = ""
            }
            );

            ComposicionGnaLIV_2DetComposicion.Add(new ComposicionGnaLIV_2DetComposicionDto
            {
                CompGnaDia = "26/11/2023",
                CompGnaC6 = 0.3644,
                CompGnaC3 = 2.1553,
                CompGnaIc4 = 0.8170,
                CompGnaNc4 = 1.0409,
                CompGnaNeoC5 = 0.0116,
                CompGnaIc5 = 0.3621,
                CompGnaNc5 = 0.4799,
                CompGnaNitrog = 0.2499,
                CompGnaC1 = 90.0719,
                CompGnaCo2 = 0.2443,
                CompGnaC2 = 4.2026,
                CompGnaTotal = 100,
                CompGnaVol = 4045.171,
                CompGnaPCalorifico = 1153.34,
                CompGnaObservacion = ""
            }
            );

            ComposicionGnaLIV_2DetComposicion.Add(new ComposicionGnaLIV_2DetComposicionDto
            {
                CompGnaDia = "27/11/2023",
                CompGnaC6 = 0.3650,
                CompGnaC3 = 2.1413,
                CompGnaIc4 = 0.8081,
                CompGnaNc4 = 1.0365,
                CompGnaNeoC5 = 0.0112,
                CompGnaIc5 = 0.3608,
                CompGnaNc5 = 0.4845,
                CompGnaNitrog = 0.2491,
                CompGnaC1 = 90.1150,
                CompGnaCo2 = 0.2350,
                CompGnaC2 = 4.1935,
                CompGnaTotal = 100,
                CompGnaVol = 4133.973,
                CompGnaPCalorifico = 1156.21,
                CompGnaObservacion = ""
            }
            );

            ComposicionGnaLIV_2DetComposicion.Add(new ComposicionGnaLIV_2DetComposicionDto
            {
                CompGnaDia = "28/11/2023",
                CompGnaC6 = 0.3696,
                CompGnaC3 = 2.2379,
                CompGnaIc4 = 0.8510,
                CompGnaNc4 = 1.0881,
                CompGnaNeoC5 = 0.0118,
                CompGnaIc5 = 0.3773,
                CompGnaNc5 = 0.5006,
                CompGnaNitrog = 0.1795,
                CompGnaC1 = 89.8278,
                CompGnaCo2 = 0.2806,
                CompGnaC2 = 4.2759,
                CompGnaTotal = 100,
                CompGnaVol = 4201.688,
                CompGnaPCalorifico = 1153.67,
                CompGnaObservacion = ""
            }
            );

            ComposicionGnaLIV_2DetComposicion.Add(new ComposicionGnaLIV_2DetComposicionDto
            {
                CompGnaDia = "29/11/2023",
                CompGnaC6 = 0.3760,
                CompGnaC3 = 2.1459,
                CompGnaIc4 = 0.8172,
                CompGnaNc4 = 1.0461,
                CompGnaNeoC5 = 0.0120,
                CompGnaIc5 = 0.3673,
                CompGnaNc5 = 0.4904,
                CompGnaNitrog = 0.2909,
                CompGnaC1 = 90.0378,
                CompGnaCo2 = 0.2378,
                CompGnaC2 = 4.1786,
                CompGnaTotal = 100,
                CompGnaVol = 4222.862,
                CompGnaPCalorifico = 1154.01,
                CompGnaObservacion = ""
            }
            );

            ComposicionGnaLIV_2DetComposicion.Add(new ComposicionGnaLIV_2DetComposicionDto
            {
                CompGnaDia = "30/11/2023",
                CompGnaC6 = 0.3957,
                CompGnaC3 = 2.25611,
                CompGnaIc4 = 0.8564,
                CompGnaNc4 = 1.0990,
                CompGnaNeoC5 = 0.0121,
                CompGnaIc5 = 0.3815,
                CompGnaNc5 = 0.5098,
                CompGnaNitrog = 0.2995,
                CompGnaC1 = 89.6163,
                CompGnaCo2 = 0.2594,
                CompGnaC2 = 4.3142,
                CompGnaTotal = 100,//no tengo
                CompGnaVol = 4024.073,//no tengo
                CompGnaPCalorifico = 1152.04,//no tengo
                CompGnaObservacion = ""
            }
            );
            return ComposicionGnaLIV_2DetComposicion;
        }


        private async Task<List<ComposicionGnaLIV_2DetComponenteDto>?> ComposicionGnaLIV_2DetComponente()
        {
            List<ComposicionGnaLIV_2DetComponenteDto> ComposicionGnaLIV_2DetComponente = new List<ComposicionGnaLIV_2DetComponenteDto>();

            ComposicionGnaLIV_2DetComponente.Add(new ComposicionGnaLIV_2DetComponenteDto
            {
                CompSimbolo = "H2",
                CompDescripcion = "Hidrogen",
                CompMolPorc = 0,
                CompUnna = 0,
                CompDif = 0
            }
           );
            ComposicionGnaLIV_2DetComponente.Add(new ComposicionGnaLIV_2DetComponenteDto
            {
                CompSimbolo = "H2S",
                CompDescripcion = "Hidrogen Sulphide",
                CompMolPorc = 0,
                CompUnna = 0,
                CompDif = 0
            }
            );
            ComposicionGnaLIV_2DetComponente.Add(new ComposicionGnaLIV_2DetComponenteDto
            {
                CompSimbolo = "CO2",
                CompDescripcion = "Carbon Dioxide",
                CompMolPorc = 0.2464,
                CompUnna = 0,
                CompDif = 0
            }
            );
            ComposicionGnaLIV_2DetComponente.Add(new ComposicionGnaLIV_2DetComponenteDto
            {
                CompSimbolo = "N2",
                CompDescripcion = "Nitrogen",
                CompMolPorc = 0.2270,
                CompUnna = 0,
                CompDif = 0
            }
            );
            ComposicionGnaLIV_2DetComponente.Add(new ComposicionGnaLIV_2DetComponenteDto
            {
                CompSimbolo = "C1",
                CompDescripcion = "Methane",
                CompMolPorc = 90.0544,
                CompUnna = 0,
                CompDif = 0
            }
            );
            ComposicionGnaLIV_2DetComponente.Add(new ComposicionGnaLIV_2DetComponenteDto
            {
                CompSimbolo = "C2",
                CompDescripcion = "Ethane",
                CompMolPorc = 4.2091,
                CompUnna = 0,
                CompDif = 0
            }
            );
            ComposicionGnaLIV_2DetComponente.Add(new ComposicionGnaLIV_2DetComponenteDto
            {
                CompSimbolo = "C3",
                CompDescripcion = "Propane",
                CompMolPorc = 2.1636,
                CompUnna = 0,
                CompDif = 0
            }
            );
            ComposicionGnaLIV_2DetComponente.Add(new ComposicionGnaLIV_2DetComponenteDto
            {
                CompSimbolo = "IC4",
                CompDescripcion = "i-Butane",
                CompMolPorc = 0.8210,
                CompUnna = 0,
                CompDif = 0
            }
            );
            ComposicionGnaLIV_2DetComponente.Add(new ComposicionGnaLIV_2DetComponenteDto
            {
                CompSimbolo = "NC4",
                CompDescripcion = "n-Butane",
                CompMolPorc = 1.0486,
                CompUnna = 0,
                CompDif = 0
            }
            );
            ComposicionGnaLIV_2DetComponente.Add(new ComposicionGnaLIV_2DetComponenteDto
            {
                CompSimbolo = "IC5",
                CompDescripcion = "i-Pentane",
                CompMolPorc = 0.3630,
                CompUnna = 0,
                CompDif = 0
            }
            );
            ComposicionGnaLIV_2DetComponente.Add(new ComposicionGnaLIV_2DetComponenteDto
            {
                CompSimbolo = "NC5",
                CompDescripcion = "n-Pentane",
                CompMolPorc = 0.4821,
                CompUnna = 0,
                CompDif = 0
            }
            );
            ComposicionGnaLIV_2DetComponente.Add(new ComposicionGnaLIV_2DetComponenteDto
            {
                CompSimbolo = "NeoC5",
                CompDescripcion = "NeoPentane",
                CompMolPorc = 0.0116,
                CompUnna = 0,
                CompDif = 0
            }
            );
            ComposicionGnaLIV_2DetComponente.Add(new ComposicionGnaLIV_2DetComponenteDto
            {
                CompSimbolo = "C6",
                CompDescripcion = "Hexanes",
                CompMolPorc = 0.3732,
                CompUnna = 0,
                CompDif = 0
            }
            );
            ComposicionGnaLIV_2DetComponente.Add(new ComposicionGnaLIV_2DetComponenteDto
            {
                CompSimbolo = "C7",
                CompDescripcion = "Heptanes",
                CompMolPorc = 0,
                CompUnna = 0,
                CompDif = 0
            }
            );
            ComposicionGnaLIV_2DetComponente.Add(new ComposicionGnaLIV_2DetComponenteDto
            {
                CompSimbolo = "C8",
                CompDescripcion = "Octanes",
                CompMolPorc = 0,
                CompUnna = 0,
                CompDif = 0
            }
            );
            ComposicionGnaLIV_2DetComponente.Add(new ComposicionGnaLIV_2DetComponenteDto
            {
                CompSimbolo = "C9",
                CompDescripcion = "Nonanes",
                CompMolPorc = 0,
                CompUnna = 0,
                CompDif = 0
            }
            );
            ComposicionGnaLIV_2DetComponente.Add(new ComposicionGnaLIV_2DetComponenteDto
            {
                CompSimbolo = "C10",
                CompDescripcion = "Decanes",
                CompMolPorc = 0,
                CompUnna = 0,
                CompDif = 0
            }
            );
            ComposicionGnaLIV_2DetComponente.Add(new ComposicionGnaLIV_2DetComponenteDto
            {
                CompSimbolo = "C11",
                CompDescripcion = "Undecanes",
                CompMolPorc = 0,
                CompUnna = 0,
                CompDif = 0
            }
            );
            ComposicionGnaLIV_2DetComponente.Add(new ComposicionGnaLIV_2DetComponenteDto
            {
                CompSimbolo = "C12+",
                CompDescripcion = "Dodecanes plus",
                CompMolPorc = 0,
                CompUnna = 0,
                CompDif = 0
            }
            );
            return ComposicionGnaLIV_2DetComponente;
        }

        
    }
}
