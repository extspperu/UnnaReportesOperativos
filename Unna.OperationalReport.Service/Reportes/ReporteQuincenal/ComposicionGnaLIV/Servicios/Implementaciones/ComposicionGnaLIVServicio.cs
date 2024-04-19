
using Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ComposicionGnaLIV.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ComposicionGnaLIV.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;

namespace Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ComposicionGnaLIV.Servicios.Implementaciones
{
    public class ComposicionGnaLIVServicio : IComposicionGnaLIVServicio
    {
        public async Task<OperacionDto<ComposicionGnaLIVDto>> ObtenerAsync(long idUsuario)
        {

            var dto = new ComposicionGnaLIVDto();
            dto.Composicion = await ComposicionUnnaEnergiaLIVAsync();
            dto.Componente = await ComposicionGnaLIVDetComponente();

            return new OperacionDto<ComposicionGnaLIVDto>(dto);
        }

        private async Task<List<ComposicionUnnaEnergiaLIVDto>> ComposicionUnnaEnergiaLIVAsync()
        {

            await Task.Delay(0);

            List<ComposicionUnnaEnergiaLIVDto> ComposicionGnaLIVDetComposicion = new List<ComposicionUnnaEnergiaLIVDto>();
            ComposicionGnaLIVDetComposicion.Add(new ComposicionUnnaEnergiaLIVDto
            {
                Fecha = "1/11/2023",
                C6 = 0.3491040,
                C3 = 2.140200,
                Ic4 = 0.811257,
                Nc4 = 1.036110,
                NeoC5 = 0.0114588,
                Ic5 = 0.355811,
                Nc5 = 0.472289,
                Nitrog = 0.320038,
                C1 = 90.075000,
                Co2 = 0.252294,
                C2 = 4.176440,
                Observacion = ""
            }
                );
            ComposicionGnaLIVDetComposicion.Add(new ComposicionUnnaEnergiaLIVDto
            {
                Fecha = "2/11/2023",
                C6 = 0.321542,
                C3 = 2.14769,
                Ic4 = 0.80578,
                Nc4 = 1.02471,
                NeoC5 = 0.0113471,
                Ic5 = 0.344008,
                Nc5 = 0.45158,
                Nitrog = 0.23286,
                C1 = 90.2176,
                Co2 = 0.251835,
                C2 = 4.19106
            }
);

            ComposicionGnaLIVDetComposicion.Add(new ComposicionUnnaEnergiaLIVDto
            {
                Fecha = "3/11/2023",
                C6 = 0.344981,
                C3 = 2.14365,
                Ic4 = 0.813408,
                Nc4 = 1.04111,
                NeoC5 = 0.011685,
                Ic5 = 0.356614,
                Nc5 = 0.473803,
                Nitrog = 0.191665,
                C1 = 90.1951,
                Co2 = 0.260966,
                C2 = 4.16704
            }
            );

            ComposicionGnaLIVDetComposicion.Add(new ComposicionUnnaEnergiaLIVDto
            {
                Fecha = "4/11/2023",
                C6 = 0.339296,
                C3 = 2.12872,
                Ic4 = 0.80709,
                Nc4 = 1.03028,
                NeoC5 = 0.0116232,
                Ic5 = 0.352623,
                Nc5 = 0.466086,
                Nitrog = 0.202292,
                C1 = 90.2521,
                Co2 = 0.255135,
                C2 = 4.15472,
            }
            );

            ComposicionGnaLIVDetComposicion.Add(new ComposicionUnnaEnergiaLIVDto
            {
                Fecha = "5/11/2023",
                C6 = 0.3421,
                C3 = 2.14818,
                Ic4 = 0.815068,
                Nc4 = 1.04617,
                NeoC5 = 0.0120231,
                Ic5 = 0.359785,
                Nc5 = 0.478935,
                Nitrog = 0.275756,
                C1 = 90.0993,
                Co2 = 0.257281,
                C2 = 4.16538
            }
            );

            ComposicionGnaLIVDetComposicion.Add(new ComposicionUnnaEnergiaLIVDto
            {
                Fecha = "6/11/2023",
                C6 = 0.350776,
                C3 = 2.12966,
                Ic4 = 0.806753,
                Nc4 = 1.03197,
                NeoC5 = 0.0115137,
                Ic5 = 0.355071,
                Nc5 = 0.471699,
                Nitrog = 0.282786,
                C1 = 90.1477,
                Co2 = 0.255344,
                C2 = 4.15669,
            }
            );

            ComposicionGnaLIVDetComposicion.Add(new ComposicionUnnaEnergiaLIVDto
            {
                Fecha = "7/11/2023",
                C6 = 0.351809,
                C3 = 2.12033,
                Ic4 = 0.804936,
                Nc4 = 1.02834,
                NeoC5 = 0.0113193,
                Ic5 = 0.355024,
                Nc5 = 0.471607,
                Nitrog = 0.277289,
                C1 = 90.1826,
                Co2 = 0.25628,
                C2 = 4.14044,
            }
            );

            ComposicionGnaLIVDetComposicion.Add(new ComposicionUnnaEnergiaLIVDto
            {
                Fecha = "8/11/2023",
                C6 = 0.355665,
                C3 = 2.12841,
                Ic4 = 0.80612,
                Nc4 = 1.02782,
                NeoC5 = 0.0116597,
                Ic5 = 0.354486,
                Nc5 = 0.469457,
                Nitrog = 0.273591,
                C1 = 90.1694,
                Co2 = 0.248786,
                C2 = 4.15459,
            }
            );


            ComposicionGnaLIVDetComposicion.Add(new ComposicionUnnaEnergiaLIVDto
            {
                Fecha = "9/11/2023",
                C6 = 0.351493,
                C3 = 2.1117,
                Ic4 = 0.798096,
                Nc4 = 1.01612,
                NeoC5 = 0.0109648,
                Ic5 = 0.348204,
                Nc5 = 0.458526,
                Nitrog = 0.237913,
                C1 = 90.2747,
                Co2 = 0.263345,
            }
            );

            ComposicionGnaLIVDetComposicion.Add(new ComposicionUnnaEnergiaLIVDto
            {
                Fecha = "10/11/2023",
                C6 = 0.352943,
                C3 = 2.09986,
                Ic4 = 0.799486,
                Nc4 = 1.02075,
                NeoC5 = 0.0112877,
                Ic5 = 0.353977,
                Nc5 = 0.469013,
                Nitrog = 0.226973,
                C1 = 90.3083,
                Co2 = 0.257978,
                C2 = 4.09944
            }
            );

            ComposicionGnaLIVDetComposicion.Add(new ComposicionUnnaEnergiaLIVDto
            {
                Fecha = "11/11/2023",
                C6 = 0.362821,
                C3 = 2.12771,
                Ic4 = 0.809116,
                Nc4 = 1.0332,
                NeoC5 = 0.011306,
                Ic5 = 0.357391,
                Nc5 = 0.471925,
                Nitrog = 0.273686,
                C1 = 90.1613,
                Co2 = 0.25769,
                C2 = 4.13382,
            }
            );

            ComposicionGnaLIVDetComposicion.Add(new ComposicionUnnaEnergiaLIVDto
            {
                Fecha = "12/11/2023",
                C6 = 0.370436,
                C3 = 2.16225,
                Ic4 = 0.827624,
                Nc4 = 1.06133,
                NeoC5 = 0.0118119,
                Ic5 = 0.368898,
                Nc5 = 0.491083,
                Nitrog = 0.29001,
                C1 = 89.9782,
                Co2 = 0.267697,
                C2 = 4.1707
            }
            );

            ComposicionGnaLIVDetComposicion.Add(new ComposicionUnnaEnergiaLIVDto
            {
                Fecha = "13/11/2023",
                C6 = 0.363971,
                C3 = 2.13667,
                Ic4 = 0.811505,
                Nc4 = 1.03421,
                NeoC5 = 0.0115248,
                Ic5 = 0.357835,
                Nc5 = 0.471959,
                Nitrog = 0.287436,
                C1 = 90.1123,
                Co2 = 0.25633,
                C2 = 4.15622
            }
            );

            ComposicionGnaLIVDetComposicion.Add(new ComposicionUnnaEnergiaLIVDto
            {
                Fecha = "14/11/2023",
                C6 = 0.359437,
                C3 = 2.14381,
                Ic4 = 0.817117,
                Nc4 = 1.04264,
                NeoC5 = 0.0118774,
                Ic5 = 0.359971,
                Nc5 = 0.475536,
                Nitrog = 0.299503,
                C1 = 90.0739,
                Co2 = 0.255047,
                C2 = 4.16115
            }
            );

            ComposicionGnaLIVDetComposicion.Add(new ComposicionUnnaEnergiaLIVDto
            {
                Fecha = "15/11/2023",
                C6 = 0.341452,
                C3 = 2.1549,
                Ic4 = 0.813522,
                Nc4 = 1.03359,
                NeoC5 = 0.0115564,
                Ic5 = 0.350775,
                Nc5 = 0.45695,
                Nitrog = 0.336936,
                C1 = 90.089,
                Co2 = 0.24465,
                C2 = 4.1667
            }
            );

            return ComposicionGnaLIVDetComposicion;
        }

        private async Task<List<ComposicionComponenteDto>> ComposicionGnaLIVDetComponente()
        {
            await Task.Delay(0);
            List<ComposicionComponenteDto> ComposicionGnaLIVDetComponente = new List<ComposicionComponenteDto>();

            ComposicionGnaLIVDetComponente.Add(new ComposicionComponenteDto
            {
                Simbolo = "H2",
                Componente = "Hidrogen",
                Molecula = 0
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
                Molecula = 0.2560
            }
            );
            ComposicionGnaLIVDetComponente.Add(new ComposicionComponenteDto
            {
                Simbolo = "N2",
                Componente = "Nitrogen",
                Molecula = 0.2672,
            }
            );
            ComposicionGnaLIVDetComponente.Add(new ComposicionComponenteDto
            {
                Simbolo = "C1",
                Componente = "Methane",
                Molecula = 90.1558,
            }
            );
            ComposicionGnaLIVDetComponente.Add(new ComposicionComponenteDto
            {
                Simbolo = "C2",
                Componente = "Ethane",
                Molecula = 4.1549
            }
            );
            ComposicionGnaLIVDetComponente.Add(new ComposicionComponenteDto
            {
                Simbolo = "C3",
                Componente = "Propane",
                Molecula = 2.1349,
            }
            );
            ComposicionGnaLIVDetComponente.Add(new ComposicionComponenteDto
            {
                Simbolo = "IC5",
                Componente = "i-Butane",
                Molecula = 0.8098,
            }
            );
            ComposicionGnaLIVDetComponente.Add(new ComposicionComponenteDto
            {
                Simbolo = "NC4",
                Componente = "n-Butane",
                Molecula = 1.0339,
            }
            );
            ComposicionGnaLIVDetComponente.Add(new ComposicionComponenteDto
            {
                Simbolo = "IC5",
                Componente = "i-Pentane",
                Molecula = 0.3554
            }
            );
            ComposicionGnaLIVDetComponente.Add(new ComposicionComponenteDto
            {
                Simbolo = "NC5",
                Componente = "n-Pentane",
                Molecula = 0.4700,
            }
            );
            ComposicionGnaLIVDetComponente.Add(new ComposicionComponenteDto
            {
                Simbolo = "NeoC5",
                Componente = "NeoPentane",
                Molecula = 0.0115,
            }
            );
            ComposicionGnaLIVDetComponente.Add(new ComposicionComponenteDto
            {
                Simbolo = "C6",
                Componente = "Hexanes",
                Molecula = 0.3505,
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
