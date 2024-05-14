using Microsoft.IdentityModel.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Registro.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Registro.Repositorios.Implementaciones;
using Unna.OperationalReport.Data.Reporte.Procedimientos;
using Unna.OperationalReport.Data.Reporte.Repositorios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ComposicionGnaLIV.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ResBalanceEnergLgnLIV_2.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ResBalanceEnergLIV.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ResBalanceEnergLIV.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Tools.Seguridad.Servicios.General.Dtos;

namespace Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ResBalanceEnergLIV.Servicios.Implementaciones
{
    public class ResBalanceEnergLIVServicio : IResBalanceEnergLIVServicio
    {
        private readonly IRegistroRepositorio _registroRepositorio;
        public ResBalanceEnergLIVServicio(IRegistroRepositorio registroRepositorio)
        {
            _registroRepositorio = registroRepositorio;
        }
        public async Task<OperacionDto<ResBalanceEnergLIVDto>> ObtenerAsync(long idUsuario)
        {
            var generalData = await _registroRepositorio.ObtenerMedicionesGasAsync();

            var FechaActuala = _registroRepositorio.ObtenerFechaActualAsync();
            var primeraQuincena = generalData.Where(d => d.Dia >= 1 && d.Dia <= 15);
            var sumaPrimeraQuincena = new
            {
                MedGasGasNatAsocMedVolumen = primeraQuincena.Sum(d => d.MedGasGasNatAsocMedVolumen),
                MedGasGasNatAsocMedPoderCal = primeraQuincena.Sum(d => d.MedGasGasNatAsocMedPoderCal),
                MedGasGasNatAsocMedEnergia = primeraQuincena.Sum(d => d.MedGasGasNatAsocMedEnergia),
                MedGasGasCombSecoMedVolumen = primeraQuincena.Sum(d => d.MedGasGasCombSecoMedVolumen),
                MedGasGasCombSecoMedPoderCal = primeraQuincena.Sum(d => d.MedGasGasCombSecoMedPoderCal),
                MedGasGasCombSecoMedEnergia = primeraQuincena.Sum(d => d.MedGasGasCombSecoMedEnergia),
                MedGasVolGasEquivLgnVolumen = primeraQuincena.Sum(d => d.MedGasVolGasEquivLgnVolumen),
                MedGasVolGasEquivLgnPoderCal = primeraQuincena.Sum(d => d.MedGasVolGasEquivLgnPoderCal),
                MedGasVolGasEquivLgnEnergia = primeraQuincena.Sum(d => d.MedGasVolGasEquivLgnEnergia),
                MedGasVolGasClienteVolumen = primeraQuincena.Sum(d => d.MedGasVolGasClienteVolumen),
                MedGasVolGasClientePoderCal = primeraQuincena.Sum(d => d.MedGasVolGasClientePoderCal),
                MedGasVolGasClienteEnergia = primeraQuincena.Sum(d => d.MedGasVolGasClienteEnergia),
                MedGasVolGasSaviaVolumen = primeraQuincena.Sum(d => d.MedGasVolGasSaviaVolumen),
                MedGasVolGasSaviaPoderCal = primeraQuincena.Sum(d => d.MedGasVolGasSaviaPoderCal),
                MedGasVolGasSaviaEnergia = primeraQuincena.Sum(d => d.MedGasVolGasSaviaEnergia),
                MedGasVolGasLimaGasVolumen = primeraQuincena.Sum(d => d.MedGasVolGasLimaGasVolumen),
                MedGasVolGasLimaGasPoderCal = primeraQuincena.Sum(d => d.MedGasVolGasLimaGasPoderCal),
                MedGasVolGasLimaGasEnergia = primeraQuincena.Sum(d => d.MedGasVolGasLimaGasEnergia),
                MedGasVolGasGasNorpVolumen = primeraQuincena.Sum(d => d.MedGasVolGasGasNorpVolumen),
                MedGasVolGasGasNorpPoderCal = primeraQuincena.Sum(d => d.MedGasVolGasGasNorpPoderCal),
                MedGasVolGasGasNorpEnergia = primeraQuincena.Sum(d => d.MedGasVolGasGasNorpEnergia),
                MedGasVolGasQuemadoVolumen = primeraQuincena.Sum(d => d.MedGasVolGasQuemadoVolumen),
                MedGasVolGasQuemadoPoderCal = primeraQuincena.Sum(d => d.MedGasVolGasQuemadoPoderCal),
                MedGasVolGasQuemadoEnergia = primeraQuincena.Sum(d => d.MedGasVolGasQuemadoEnergia)
            };
            var segundaQuincena = generalData.Where(d => d.Dia >= 16 && d.Dia <= DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));
            var sumaSegundaQuincena = new
            {
                MedGasGasNatAsocMedVolumen = segundaQuincena.Sum(d => d.MedGasGasNatAsocMedVolumen),
                MedGasGasNatAsocMedPoderCal = segundaQuincena.Sum(d => d.MedGasGasNatAsocMedPoderCal),
                MedGasGasNatAsocMedEnergia = segundaQuincena.Sum(d => d.MedGasGasNatAsocMedEnergia),
                MedGasGasCombSecoMedVolumen = segundaQuincena.Sum(d => d.MedGasGasCombSecoMedVolumen),
                MedGasGasCombSecoMedPoderCal = segundaQuincena.Sum(d => d.MedGasGasCombSecoMedPoderCal),
                MedGasGasCombSecoMedEnergia = segundaQuincena.Sum(d => d.MedGasGasCombSecoMedEnergia),
                MedGasVolGasEquivLgnVolumen = segundaQuincena.Sum(d => d.MedGasVolGasEquivLgnVolumen),
                MedGasVolGasEquivLgnPoderCal = segundaQuincena.Sum(d => d.MedGasVolGasEquivLgnPoderCal),
                MedGasVolGasEquivLgnEnergia = segundaQuincena.Sum(d => d.MedGasVolGasEquivLgnEnergia),
                MedGasVolGasClienteVolumen = segundaQuincena.Sum(d => d.MedGasVolGasClienteVolumen),
                MedGasVolGasClientePoderCal = segundaQuincena.Sum(d => d.MedGasVolGasClientePoderCal),
                MedGasVolGasClienteEnergia = segundaQuincena.Sum(d => d.MedGasVolGasClienteEnergia),
                MedGasVolGasSaviaVolumen = segundaQuincena.Sum(d => d.MedGasVolGasSaviaVolumen),
                MedGasVolGasSaviaPoderCal = segundaQuincena.Sum(d => d.MedGasVolGasSaviaPoderCal),
                MedGasVolGasSaviaEnergia = segundaQuincena.Sum(d => d.MedGasVolGasSaviaEnergia),
                MedGasVolGasLimaGasVolumen = segundaQuincena.Sum(d => d.MedGasVolGasLimaGasVolumen),
                MedGasVolGasLimaGasPoderCal = segundaQuincena.Sum(d => d.MedGasVolGasLimaGasPoderCal),
                MedGasVolGasLimaGasEnergia = segundaQuincena.Sum(d => d.MedGasVolGasLimaGasEnergia),
                MedGasVolGasGasNorpVolumen = segundaQuincena.Sum(d => d.MedGasVolGasGasNorpVolumen),
                MedGasVolGasGasNorpPoderCal = segundaQuincena.Sum(d => d.MedGasVolGasGasNorpPoderCal),
                MedGasVolGasGasNorpEnergia = segundaQuincena.Sum(d => d.MedGasVolGasGasNorpEnergia),
                MedGasVolGasQuemadoVolumen = segundaQuincena.Sum(d => d.MedGasVolGasQuemadoVolumen),
                MedGasVolGasQuemadoPoderCal = segundaQuincena.Sum(d => d.MedGasVolGasQuemadoPoderCal),
                MedGasVolGasQuemadoEnergia = segundaQuincena.Sum(d => d.MedGasVolGasQuemadoEnergia)
            };

            var dto = new ResBalanceEnergLIVDto
            {
                Lote = "LOTE IV",
                Mes = FechaActuala.Result.MesActual,
                Anio = FechaActuala.Result.AnioActual.ToString(),

                // Asignar valores de la primera quincena
                AcumUnnaQ1MedGasGasNatAsocMedVolumen = sumaPrimeraQuincena.MedGasGasNatAsocMedVolumen,
                AcumUnnaQ1MedGasGasNatAsocMedPoderCal = sumaPrimeraQuincena.MedGasGasNatAsocMedPoderCal,
                AcumUnnaQ1MedGasGasNatAsocMedEnergia = sumaPrimeraQuincena.MedGasGasNatAsocMedEnergia,
                AcumUnnaQ1MedGasGasCombSecoMedVolumen = sumaPrimeraQuincena.MedGasGasCombSecoMedVolumen,
                AcumUnnaQ1MedGasGasCombSecoMedPoderCal = sumaPrimeraQuincena.MedGasGasCombSecoMedPoderCal,
                AcumUnnaQ1MedGasGasCombSecoMedEnergia = sumaPrimeraQuincena.MedGasGasCombSecoMedEnergia,
                AcumUnnaQ1MedGasVolGasEquivLgnVolumen = sumaPrimeraQuincena.MedGasVolGasEquivLgnVolumen,
                AcumUnnaQ1MedGasVolGasEquivLgnPoderCal = sumaPrimeraQuincena.MedGasVolGasEquivLgnPoderCal,
                AcumUnnaQ1MedGasVolGasEquivLgnEnergia = sumaPrimeraQuincena.MedGasVolGasEquivLgnEnergia,
                AcumUnnaQ1MedGasVolGasClienteVolumen = sumaPrimeraQuincena.MedGasVolGasClienteVolumen,
                AcumUnnaQ1MedGasVolGasClientePoderCal = sumaPrimeraQuincena.MedGasVolGasClientePoderCal,
                AcumUnnaQ1MedGasVolGasClienteEnergia = sumaPrimeraQuincena.MedGasVolGasClienteEnergia,
                AcumUnnaQ1MedGasVolGasSaviaVolumen = sumaPrimeraQuincena.MedGasVolGasSaviaVolumen,
                AcumUnnaQ1MedGasVolGasSaviaPoderCal = sumaPrimeraQuincena.MedGasVolGasSaviaPoderCal,
                AcumUnnaQ1MedGasVolGasSaviaEnergia = sumaPrimeraQuincena.MedGasVolGasSaviaEnergia,
                AcumUnnaQ1MedGasVolGasLimaGasVolumen = sumaPrimeraQuincena.MedGasVolGasLimaGasVolumen,
                AcumUnnaQ1MedGasVolGasLimaGasPoderCal = sumaPrimeraQuincena.MedGasVolGasLimaGasPoderCal,
                AcumUnnaQ1MedGasVolGasLimaGasEnergia = sumaPrimeraQuincena.MedGasVolGasLimaGasEnergia,
                AcumUnnaQ1MedGasVolGasGasNorpVolumen = sumaPrimeraQuincena.MedGasVolGasGasNorpVolumen,
                AcumUnnaQ1MedGasVolGasGasNorpPoderCal = sumaPrimeraQuincena.MedGasVolGasGasNorpPoderCal,
                AcumUnnaQ1MedGasVolGasGasNorpEnergia = sumaPrimeraQuincena.MedGasVolGasGasNorpEnergia,
                AcumUnnaQ1MedGasVolGasQuemadoVolumen = sumaPrimeraQuincena.MedGasVolGasQuemadoVolumen,
                AcumUnnaQ1MedGasVolGasQuemadoPoderCal = sumaPrimeraQuincena.MedGasVolGasQuemadoPoderCal,
                AcumUnnaQ1MedGasVolGasQuemadoEnergia = sumaPrimeraQuincena.MedGasVolGasQuemadoEnergia,

                // Asignar valores de la segunda quincena
                AcumUnnaQ2MedGasGasNatAsocMedVolumen = sumaSegundaQuincena.MedGasGasNatAsocMedVolumen,
                AcumUnnaQ2MedGasGasNatAsocMedPoderCal = sumaSegundaQuincena.MedGasGasNatAsocMedPoderCal,
                AcumUnnaQ2MedGasGasNatAsocMedEnergia = sumaSegundaQuincena.MedGasGasNatAsocMedEnergia,
                AcumUnnaQ2MedGasGasCombSecoMedVolumen = sumaSegundaQuincena.MedGasGasCombSecoMedVolumen,
                AcumUnnaQ2MedGasGasCombSecoMedPoderCal = sumaSegundaQuincena.MedGasGasCombSecoMedPoderCal,
                AcumUnnaQ2MedGasGasCombSecoMedEnergia = sumaSegundaQuincena.MedGasGasCombSecoMedEnergia,
                AcumUnnaQ2MedGasVolGasEquivLgnVolumen = sumaSegundaQuincena.MedGasVolGasEquivLgnVolumen,
                AcumUnnaQ2MedGasVolGasEquivLgnPoderCal = sumaSegundaQuincena.MedGasVolGasEquivLgnPoderCal,
                AcumUnnaQ2MedGasVolGasEquivLgnEnergia = sumaSegundaQuincena.MedGasVolGasEquivLgnEnergia,
                AcumUnnaQ2MedGasVolGasClienteVolumen = sumaSegundaQuincena.MedGasVolGasClienteVolumen,
                AcumUnnaQ2MedGasVolGasClientePoderCal = sumaSegundaQuincena.MedGasVolGasClientePoderCal,
                AcumUnnaQ2MedGasVolGasClienteEnergia = sumaSegundaQuincena.MedGasVolGasClienteEnergia,
                AcumUnnaQ2MedGasVolGasSaviaVolumen = sumaSegundaQuincena.MedGasVolGasSaviaVolumen,
                AcumUnnaQ2MedGasVolGasSaviaPoderCal = sumaSegundaQuincena.MedGasVolGasSaviaPoderCal,
                AcumUnnaQ2MedGasVolGasSaviaEnergia = sumaSegundaQuincena.MedGasVolGasSaviaEnergia,
                AcumUnnaQ2MedGasVolGasLimaGasVolumen = sumaSegundaQuincena.MedGasVolGasLimaGasVolumen,
                AcumUnnaQ2MedGasVolGasLimaGasPoderCal = sumaSegundaQuincena.MedGasVolGasLimaGasPoderCal,
                AcumUnnaQ2MedGasVolGasLimaGasEnergia = sumaSegundaQuincena.MedGasVolGasLimaGasEnergia,
                AcumUnnaQ2MedGasVolGasGasNorpVolumen = sumaSegundaQuincena.MedGasVolGasGasNorpVolumen,
                AcumUnnaQ2MedGasVolGasGasNorpPoderCal = sumaSegundaQuincena.MedGasVolGasGasNorpPoderCal,
                AcumUnnaQ2MedGasVolGasGasNorpEnergia = sumaSegundaQuincena.MedGasVolGasGasNorpEnergia,
                AcumUnnaQ2MedGasVolGasQuemadoVolumen = sumaSegundaQuincena.MedGasVolGasQuemadoVolumen,
                AcumUnnaQ2MedGasVolGasQuemadoPoderCal = sumaSegundaQuincena.MedGasVolGasQuemadoPoderCal,
                AcumUnnaQ2MedGasVolGasQuemadoEnergia = sumaSegundaQuincena.MedGasVolGasQuemadoEnergia
            };

            dto.ResBalanceEnergLIVDetMedGas = await ResBalanceEnergLIVDetMedGas();
            dto.ResBalanceEnergLIVDetGnaFisc = await ResBalanceEnergLIVDetGnaFisc();
            dto.ResBalanceEnergLgnLIV_2DetLgnDto = await ResBalanceEnergLIVDetMedGasLGN();
            return new OperacionDto<ResBalanceEnergLIVDto>(dto);
        }

        private async Task<List<ResBalanceEnergLIVDetMedGasDto>> ResBalanceEnergLIVDetMedGas()
        {
            List<ResBalanceEnergLIVDetMedGasDto> ResBalanceEnergLIVDetMedGas = new List<ResBalanceEnergLIVDetMedGasDto>();

            // Obtener los datos generales
            var generalData = await _registroRepositorio.ObtenerMedicionesGasAsync();

            // Obtener el año y mes actual
            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;

            // Crear una lista de todos los días del mes actual
            var allDaysInMonth = Enumerable.Range(1, DateTime.DaysInMonth(year, month)).ToList();

            // Iterar sobre cada día del mes actual
            foreach (var day in allDaysInMonth)
            {
                // Buscar los datos para el día actual
                var dataForDay = generalData.FirstOrDefault(d => d.Dia == day);

                // Si no hay datos para el día actual, crear un objeto con valores 0
                if (dataForDay == null)
                {
                    ResBalanceEnergLIVDetMedGas.Add(new ResBalanceEnergLIVDetMedGasDto
                    {
                        Dia = day,
                        MedGasGasNatAsocMedVolumen = 0,
                        MedGasGasNatAsocMedPoderCal = 0,
                        MedGasGasNatAsocMedEnergia = 0,

                        MedGasGasCombSecoMedVolumen = 0,
                        MedGasGasCombSecoMedPoderCal = 0,
                        MedGasGasCombSecoMedEnergia = 0,

                        MedGasVolGasEquivLgnVolumen = 0,
                        MedGasVolGasEquivLgnPoderCal = 0,
                        MedGasVolGasEquivLgnEnergia = 0,

                        MedGasVolGasClienteVolumen = 0,
                        MedGasVolGasClientePoderCal = 0,
                        MedGasVolGasClienteEnergia = 0,

                        MedGasVolGasSaviaVolumen = 0,
                        MedGasVolGasSaviaPoderCal = 0,
                        MedGasVolGasSaviaEnergia = 0,

                        MedGasVolGasLimaGasVolumen = 0,
                        MedGasVolGasLimaGasPoderCal = 0,
                        MedGasVolGasLimaGasEnergia = 0,

                        MedGasVolGasGasNorpVolumen = 0,
                        MedGasVolGasGasNorpPoderCal = 0,
                        MedGasVolGasGasNorpEnergia = 0,

                        MedGasVolGasQuemadoVolumen = 0,
                        MedGasVolGasQuemadoPoderCal = 0,
                        MedGasVolGasQuemadoEnergia = 0
                    });
                }
                else
                {
                    // Si hay datos para el día actual, agregar el objeto correspondiente
                    ResBalanceEnergLIVDetMedGas.Add(new ResBalanceEnergLIVDetMedGasDto
                    {
                        Dia = dataForDay.Dia,
                        MedGasGasNatAsocMedVolumen = dataForDay.MedGasGasNatAsocMedVolumen,
                        MedGasGasNatAsocMedPoderCal = dataForDay.MedGasGasNatAsocMedPoderCal,
                        MedGasGasNatAsocMedEnergia = dataForDay.MedGasGasNatAsocMedEnergia,

                        MedGasGasCombSecoMedVolumen = dataForDay.MedGasGasCombSecoMedVolumen,
                        MedGasGasCombSecoMedPoderCal = dataForDay.MedGasGasCombSecoMedPoderCal,
                        MedGasGasCombSecoMedEnergia = dataForDay.MedGasGasCombSecoMedEnergia,

                        MedGasVolGasEquivLgnVolumen = dataForDay.MedGasVolGasEquivLgnVolumen,
                        MedGasVolGasEquivLgnPoderCal = dataForDay.MedGasVolGasEquivLgnPoderCal,
                        MedGasVolGasEquivLgnEnergia = dataForDay.MedGasVolGasEquivLgnEnergia,

                        MedGasVolGasClienteVolumen = dataForDay.MedGasVolGasClienteVolumen,
                        MedGasVolGasClientePoderCal = dataForDay.MedGasVolGasClientePoderCal,
                        MedGasVolGasClienteEnergia = dataForDay.MedGasVolGasClienteEnergia,

                        MedGasVolGasSaviaVolumen = dataForDay.MedGasVolGasSaviaVolumen,
                        MedGasVolGasSaviaPoderCal = dataForDay.MedGasVolGasSaviaPoderCal,
                        MedGasVolGasSaviaEnergia = dataForDay.MedGasVolGasSaviaEnergia,

                        MedGasVolGasLimaGasVolumen = dataForDay.MedGasVolGasLimaGasVolumen,
                        MedGasVolGasLimaGasPoderCal = dataForDay.MedGasVolGasLimaGasPoderCal,
                        MedGasVolGasLimaGasEnergia = dataForDay.MedGasVolGasLimaGasEnergia,

                        MedGasVolGasGasNorpVolumen = dataForDay.MedGasVolGasGasNorpVolumen,
                        MedGasVolGasGasNorpPoderCal = dataForDay.MedGasVolGasGasNorpPoderCal,
                        MedGasVolGasGasNorpEnergia = dataForDay.MedGasVolGasGasNorpEnergia,

                        MedGasVolGasQuemadoVolumen = dataForDay.MedGasVolGasQuemadoVolumen,
                        MedGasVolGasQuemadoPoderCal = dataForDay.MedGasVolGasQuemadoPoderCal,
                        MedGasVolGasQuemadoEnergia = dataForDay.MedGasVolGasQuemadoEnergia
                    });
                }
            }
            ResBalanceEnergLIVDetMedGas.Add(new ResBalanceEnergLIVDetMedGasDto
            {
                Dia = 11,
                MedGasGasNatAsocMedVolumen = 4023.96,
                MedGasGasNatAsocMedPoderCal = 1152,
                MedGasGasNatAsocMedEnergia = 4635.6019,
                MedGasGasCombSecoMedVolumen = 156.7593,
                MedGasGasCombSecoMedPoderCal = 1056.69,
                MedGasGasCombSecoMedEnergia = 165.646,
                MedGasVolGasEquivLgnVolumen = 199.1084,
                MedGasVolGasEquivLgnPoderCal = 3105.31,
                MedGasVolGasEquivLgnEnergia = 618.2933,
                MedGasVolGasClienteVolumen = 0,
                MedGasVolGasClientePoderCal = 1056.59,
                MedGasVolGasClienteEnergia = 0,
                MedGasVolGasSaviaVolumen = 117.81,
                MedGasVolGasSaviaPoderCal = 1056.56,
                MedGasVolGasSaviaEnergia = 124.4733,
                MedGasVolGasLimaGasVolumen = 1200,
                MedGasVolGasLimaGasPoderCal = 1057.12,
                MedGasVolGasLimaGasEnergia = 1268.544,
                MedGasVolGasGasNorpVolumen = 2350.09,
                MedGasVolGasGasNorpPoderCal = 1055.92,
                MedGasVolGasGasNorpEnergia = 2481.507,
                MedGasVolGasQuemadoVolumen = 0.192299999999705,
                MedGasVolGasQuemadoPoderCal = 1056.59,
                MedGasVolGasQuemadoEnergia = 0.2032
            }
            );
            ResBalanceEnergLIVDetMedGas.Add(new ResBalanceEnergLIVDetMedGasDto
            {
                Dia = 12,
                MedGasGasNatAsocMedVolumen = 4023.96,
                MedGasGasNatAsocMedPoderCal = 1152,
                MedGasGasNatAsocMedEnergia = 4635.6019,
                MedGasGasCombSecoMedVolumen = 156.7593,
                MedGasGasCombSecoMedPoderCal = 1056.69,
                MedGasGasCombSecoMedEnergia = 165.646,
                MedGasVolGasEquivLgnVolumen = 199.1084,
                MedGasVolGasEquivLgnPoderCal = 3105.31,
                MedGasVolGasEquivLgnEnergia = 618.2933,
                MedGasVolGasClienteVolumen = 0,
                MedGasVolGasClientePoderCal = 1056.59,
                MedGasVolGasClienteEnergia = 0,
                MedGasVolGasSaviaVolumen = 117.81,
                MedGasVolGasSaviaPoderCal = 1056.56,
                MedGasVolGasSaviaEnergia = 124.4733,
                MedGasVolGasLimaGasVolumen = 1200,
                MedGasVolGasLimaGasPoderCal = 1057.12,
                MedGasVolGasLimaGasEnergia = 1268.544,
                MedGasVolGasGasNorpVolumen = 2350.09,
                MedGasVolGasGasNorpPoderCal = 1055.92,
                MedGasVolGasGasNorpEnergia = 2481.507,
                MedGasVolGasQuemadoVolumen = 0.192299999999705,
                MedGasVolGasQuemadoPoderCal = 1056.59,
                MedGasVolGasQuemadoEnergia = 0.2032
            }
            );

            ResBalanceEnergLIVDetMedGas.Add(new ResBalanceEnergLIVDetMedGasDto
            {
                Dia = 13,
                MedGasGasNatAsocMedVolumen = 4023.96,
                MedGasGasNatAsocMedPoderCal = 1152,
                MedGasGasNatAsocMedEnergia = 4635.6019,
                MedGasGasCombSecoMedVolumen = 156.7593,
                MedGasGasCombSecoMedPoderCal = 1056.69,
                MedGasGasCombSecoMedEnergia = 165.646,
                MedGasVolGasEquivLgnVolumen = 199.1084,
                MedGasVolGasEquivLgnPoderCal = 3105.31,
                MedGasVolGasEquivLgnEnergia = 618.2933,
                MedGasVolGasClienteVolumen = 0,
                MedGasVolGasClientePoderCal = 1056.59,
                MedGasVolGasClienteEnergia = 0,
                MedGasVolGasSaviaVolumen = 117.81,
                MedGasVolGasSaviaPoderCal = 1056.56,
                MedGasVolGasSaviaEnergia = 124.4733,
                MedGasVolGasLimaGasVolumen = 1200,
                MedGasVolGasLimaGasPoderCal = 1057.12,
                MedGasVolGasLimaGasEnergia = 1268.544,
                MedGasVolGasGasNorpVolumen = 2350.09,
                MedGasVolGasGasNorpPoderCal = 1055.92,
                MedGasVolGasGasNorpEnergia = 2481.507,
                MedGasVolGasQuemadoVolumen = 0.192299999999705,
                MedGasVolGasQuemadoPoderCal = 1056.59,
                MedGasVolGasQuemadoEnergia = 0.2032
            }
            );

            ResBalanceEnergLIVDetMedGas.Add(new ResBalanceEnergLIVDetMedGasDto
            {
                Dia = 14,
                MedGasGasNatAsocMedVolumen = 4023.96,
                MedGasGasNatAsocMedPoderCal = 1152,
                MedGasGasNatAsocMedEnergia = 4635.6019,
                MedGasGasCombSecoMedVolumen = 156.7593,
                MedGasGasCombSecoMedPoderCal = 1056.69,
                MedGasGasCombSecoMedEnergia = 165.646,
                MedGasVolGasEquivLgnVolumen = 199.1084,
                MedGasVolGasEquivLgnPoderCal = 3105.31,
                MedGasVolGasEquivLgnEnergia = 618.2933,
                MedGasVolGasClienteVolumen = 0,
                MedGasVolGasClientePoderCal = 1056.59,
                MedGasVolGasClienteEnergia = 0,
                MedGasVolGasSaviaVolumen = 117.81,
                MedGasVolGasSaviaPoderCal = 1056.56,
                MedGasVolGasSaviaEnergia = 124.4733,
                MedGasVolGasLimaGasVolumen = 1200,
                MedGasVolGasLimaGasPoderCal = 1057.12,
                MedGasVolGasLimaGasEnergia = 1268.544,
                MedGasVolGasGasNorpVolumen = 2350.09,
                MedGasVolGasGasNorpPoderCal = 1055.92,
                MedGasVolGasGasNorpEnergia = 2481.507,
                MedGasVolGasQuemadoVolumen = 0.192299999999705,
                MedGasVolGasQuemadoPoderCal = 1056.59,
                MedGasVolGasQuemadoEnergia = 0.2032
            }
            );

            ResBalanceEnergLIVDetMedGas.Add(new ResBalanceEnergLIVDetMedGasDto
            {
                Dia = 15,
                MedGasGasNatAsocMedVolumen = 4023.96,
                MedGasGasNatAsocMedPoderCal = 1152,
                MedGasGasNatAsocMedEnergia = 4635.6019,
                MedGasGasCombSecoMedVolumen = 156.7593,
                MedGasGasCombSecoMedPoderCal = 1056.69,
                MedGasGasCombSecoMedEnergia = 165.646,
                MedGasVolGasEquivLgnVolumen = 199.1084,
                MedGasVolGasEquivLgnPoderCal = 3105.31,
                MedGasVolGasEquivLgnEnergia = 618.2933,
                MedGasVolGasClienteVolumen = 0,
                MedGasVolGasClientePoderCal = 1056.59,
                MedGasVolGasClienteEnergia = 0,
                MedGasVolGasSaviaVolumen = 117.81,
                MedGasVolGasSaviaPoderCal = 1056.56,
                MedGasVolGasSaviaEnergia = 124.4733,
                MedGasVolGasLimaGasVolumen = 1200,
                MedGasVolGasLimaGasPoderCal = 1057.12,
                MedGasVolGasLimaGasEnergia = 1268.544,
                MedGasVolGasGasNorpVolumen = 2350.09,
                MedGasVolGasGasNorpPoderCal = 1055.92,
                MedGasVolGasGasNorpEnergia = 2481.507,
                MedGasVolGasQuemadoVolumen = 0.192299999999705,
                MedGasVolGasQuemadoPoderCal = 1056.59,
                MedGasVolGasQuemadoEnergia = 0.2032
            }
            );

            return ResBalanceEnergLIVDetMedGas;
        }

        private async Task<List<ResBalanceEnergLIVDetGnaFiscDto>> ResBalanceEnergLIVDetGnaFisc()
        {

            List<ResBalanceEnergLIVDetGnaFiscDto> ResBalanceEnergLIVDetGnaFisc = new List<ResBalanceEnergLIVDetGnaFiscDto>();

            ResBalanceEnergLIVDetGnaFisc.Add(new ResBalanceEnergLIVDetGnaFiscDto
            {
                Dia = 1,
                GnaFiscVtaRefVolumen = 507.7,
                GnaFiscVtaRefPoderCal = 1152.93,
                GnaFiscVtaRefEnergia = 585.3426,
                GnaFiscVtaLimaGasVolumen = 0,
                GnaFiscVtaLimaGasPoderCal = 1152.93,
                GnaFiscVtaLimaGasEnergia = 0,
                GnaFiscGasNorpVolumen = 3311.15,
                GnaFiscGasNorpPoderCal = 1152.93,
                GnaFiscGasNorpEnergia = 3817.5242,
                GnaFiscVtaEnelVolumen = 0,
                GnaFiscVtaEnelPoderCal = 1152.93,
                GnaFiscVtaEnelEnergia = 0,
                GnaFiscGcyLgnVolumen = 374.047,
                GnaFiscGcyLgnPoderCal = 1152.93,
                GnaFiscGcyLgnEnergia = 431.25,
                GnaFiscGnafVolumen = 4192.897,
                GnaFiscGnafPoderCal = 1152.93,
                GnaFiscGnafEnergia = 4834.1168,
                GnaFiscTotalVolumen = 4192.897,
                GnaFiscTotalEnergia = 4834.1168
            }
            );

            ResBalanceEnergLIVDetGnaFisc.Add(new ResBalanceEnergLIVDetGnaFiscDto
            {
                Dia = 2,
                GnaFiscVtaRefVolumen = 290.46,
                GnaFiscVtaRefPoderCal = 1151.49,
                GnaFiscVtaRefEnergia = 334.4618,
                GnaFiscVtaLimaGasVolumen = 483.77,
                GnaFiscVtaLimaGasPoderCal = 1151.49,
                GnaFiscVtaLimaGasEnergia = 557.0563,
                GnaFiscGasNorpVolumen = 3002.77,
                GnaFiscGasNorpPoderCal = 1151.49,
                GnaFiscGasNorpEnergia = 3457.6596,
                GnaFiscVtaEnelVolumen = 0,
                GnaFiscVtaEnelPoderCal = 1151.49,
                GnaFiscVtaEnelEnergia = 0,
                GnaFiscGcyLgnVolumen = 365.2188,
                GnaFiscGcyLgnPoderCal = 1151.49,
                GnaFiscGcyLgnEnergia = 420.5458,
                GnaFiscGnafVolumen = 4142.2188,
                GnaFiscGnafPoderCal = 1151.49,
                GnaFiscGnafEnergia = 4769.7235,
                GnaFiscTotalVolumen = 8335.1158,
                GnaFiscTotalEnergia = 9603.8403
            }
            );
            ResBalanceEnergLIVDetGnaFisc.Add(new ResBalanceEnergLIVDetGnaFiscDto
            {
                Dia = 3,
                GnaFiscVtaRefVolumen = 212.15,
                GnaFiscVtaRefPoderCal = 1154.19,
                GnaFiscVtaRefEnergia = 244.8614,
                GnaFiscVtaLimaGasVolumen = 459.49,
                GnaFiscVtaLimaGasPoderCal = 1154.19,
                GnaFiscVtaLimaGasEnergia = 530.3388,
                GnaFiscGasNorpVolumen = 2994.11,
                GnaFiscGasNorpPoderCal = 1154.19,
                GnaFiscGasNorpEnergia = 3455.7718,
                GnaFiscVtaEnelVolumen = 0,
                GnaFiscVtaEnelPoderCal = 1154.19,
                GnaFiscVtaEnelEnergia = 0,
                GnaFiscGcyLgnVolumen = 361.5267,
                GnaFiscGcyLgnPoderCal = 1154.19,
                GnaFiscGcyLgnEnergia = 417.2705,
                GnaFiscGnafVolumen = 4027.2767,
                GnaFiscGnafPoderCal = 1154.19,
                GnaFiscGnafEnergia = 4648.2425,
                GnaFiscTotalVolumen = 12362.3925,
                GnaFiscTotalEnergia = 14252.0828

            }
            );
            ResBalanceEnergLIVDetGnaFisc.Add(new ResBalanceEnergLIVDetGnaFiscDto
            {
                Dia = 4,
                GnaFiscVtaRefVolumen = 268.12,
                GnaFiscVtaRefPoderCal = 1152.83,
                GnaFiscVtaRefEnergia = 309.0968,
                GnaFiscVtaLimaGasVolumen = 500.74,
                GnaFiscVtaLimaGasPoderCal = 1152.83,
                GnaFiscVtaLimaGasEnergia = 577.2681,
                GnaFiscGasNorpVolumen = 2920.57,
                GnaFiscGasNorpPoderCal = 1152.83,
                GnaFiscGasNorpEnergia = 3366.9207,
                GnaFiscVtaEnelVolumen = 0,
                GnaFiscVtaEnelPoderCal = 1152.83,
                GnaFiscVtaEnelEnergia = 0,
                GnaFiscGcyLgnVolumen = 358.3209,
                GnaFiscGcyLgnPoderCal = 1152.83,
                GnaFiscGcyLgnEnergia = 413.0831,
                GnaFiscGnafVolumen = 4047.7509,
                GnaFiscGnafPoderCal = 1152.83,
                GnaFiscGnafEnergia = 4666.3687,
                GnaFiscTotalVolumen = 16410.1434,
                GnaFiscTotalEnergia = 18918.4515

            }
            );
            ResBalanceEnergLIVDetGnaFisc.Add(new ResBalanceEnergLIVDetGnaFiscDto
            {
                Dia = 5,
                GnaFiscVtaRefVolumen = 192.04,
                GnaFiscVtaRefPoderCal = 1153.72,
                GnaFiscVtaRefEnergia = 221.5604,
                GnaFiscVtaLimaGasVolumen = 432.51,
                GnaFiscVtaLimaGasPoderCal = 1153.72,
                GnaFiscVtaLimaGasEnergia = 498.9954,
                GnaFiscGasNorpVolumen = 3000.7,
                GnaFiscGasNorpPoderCal = 1153.72,
                GnaFiscGasNorpEnergia = 3461.9676,
                GnaFiscVtaEnelVolumen = 0,
                GnaFiscVtaEnelPoderCal = 1153.72,
                GnaFiscVtaEnelEnergia = 0,
                GnaFiscGcyLgnVolumen = 351.6959,
                GnaFiscGcyLgnPoderCal = 1153.72,
                GnaFiscGcyLgnEnergia = 405.7586,
                GnaFiscGnafVolumen = 3976.9459,
                GnaFiscGnafPoderCal = 1153.72,
                GnaFiscGnafEnergia = 4588.282,
                GnaFiscTotalVolumen = 20387.0893,
                GnaFiscTotalEnergia = 23506.7335

            }
            );
            ResBalanceEnergLIVDetGnaFisc.Add(new ResBalanceEnergLIVDetGnaFiscDto
            {
                Dia = 6,
                GnaFiscVtaRefVolumen = 580.55,
                GnaFiscVtaRefPoderCal = 1152.81,
                GnaFiscVtaRefEnergia = 669.2638,
                GnaFiscVtaLimaGasVolumen = 0,
                GnaFiscVtaLimaGasPoderCal = 1152.81,
                GnaFiscVtaLimaGasEnergia = 0,
                GnaFiscGasNorpVolumen = 3008.36,
                GnaFiscGasNorpPoderCal = 1152.81,
                GnaFiscGasNorpEnergia = 3468.0675,
                GnaFiscVtaEnelVolumen = 0,
                GnaFiscVtaEnelPoderCal = 1152.81,
                GnaFiscVtaEnelEnergia = 0,
                GnaFiscGcyLgnVolumen = 361.6158,
                GnaFiscGcyLgnPoderCal = 1152.81,
                GnaFiscGcyLgnEnergia = 416.8743,
                GnaFiscGnafVolumen = 3950.5258,
                GnaFiscGnafPoderCal = 1152.81,
                GnaFiscGnafEnergia = 4554.2056,
                GnaFiscTotalVolumen = 24337.6151,
                GnaFiscTotalEnergia = 28060.9391

            }
            );
            ResBalanceEnergLIVDetGnaFisc.Add(new ResBalanceEnergLIVDetGnaFiscDto
            {
                Dia = 7,
                GnaFiscVtaRefVolumen = 672.12,
                GnaFiscVtaRefPoderCal = 1152.5,
                GnaFiscVtaRefEnergia = 774.6183,
                GnaFiscVtaLimaGasVolumen = 0,
                GnaFiscVtaLimaGasPoderCal = 1152.5,
                GnaFiscVtaLimaGasEnergia = 0,
                GnaFiscGasNorpVolumen = 2997.07,
                GnaFiscGasNorpPoderCal = 1152.5,
                GnaFiscGasNorpEnergia = 3454.1232,
                GnaFiscVtaEnelVolumen = 0,
                GnaFiscVtaEnelPoderCal = 1152.5,
                GnaFiscVtaEnelEnergia = 0,
                GnaFiscGcyLgnVolumen = 352.3006,
                GnaFiscGcyLgnPoderCal = 1152.5,
                GnaFiscGcyLgnEnergia = 406.0264,
                GnaFiscGnafVolumen = 4021.4906,
                GnaFiscGnafPoderCal = 1152.5,
                GnaFiscGnafEnergia = 4634.7679,
                GnaFiscTotalVolumen = 28359.1057,
                GnaFiscTotalEnergia = 32695.707

            }
            );
            ResBalanceEnergLIVDetGnaFisc.Add(new ResBalanceEnergLIVDetGnaFiscDto
            {
                Dia = 8,
                GnaFiscVtaRefVolumen = 610.57,
                GnaFiscVtaRefPoderCal = 1152.95,
                GnaFiscVtaRefEnergia = 703.9567,
                GnaFiscVtaLimaGasVolumen = 166.82,
                GnaFiscVtaLimaGasPoderCal = 1152.95,
                GnaFiscVtaLimaGasEnergia = 192.3351,
                GnaFiscGasNorpVolumen = 3015.29,
                GnaFiscGasNorpPoderCal = 1152.95,
                GnaFiscGasNorpEnergia = 3476.4786,
                GnaFiscVtaEnelVolumen = 0,
                GnaFiscVtaEnelPoderCal = 1152.95,
                GnaFiscVtaEnelEnergia = 0,
                GnaFiscGcyLgnVolumen = 363.5728,
                GnaFiscGcyLgnPoderCal = 1152.95,
                GnaFiscGcyLgnEnergia = 419.1813,
                GnaFiscGnafVolumen = 4156.2528,
                GnaFiscGnafPoderCal = 1152.95,
                GnaFiscGnafEnergia = 4791.9517,
                GnaFiscTotalVolumen = 32515.3585,
                GnaFiscTotalEnergia = 37487.6587

            }
            );
            ResBalanceEnergLIVDetGnaFisc.Add(new ResBalanceEnergLIVDetGnaFiscDto
            {
                Dia = 9,
                GnaFiscVtaRefVolumen = 466.43,
                GnaFiscVtaRefPoderCal = 1151.55,
                GnaFiscVtaRefEnergia = 537.1175,
                GnaFiscVtaLimaGasVolumen = 214.78,
                GnaFiscVtaLimaGasPoderCal = 1151.55,
                GnaFiscVtaLimaGasEnergia = 247.3299,
                GnaFiscGasNorpVolumen = 3063.71,
                GnaFiscGasNorpPoderCal = 1151.55,
                GnaFiscGasNorpEnergia = 3528.0153,
                GnaFiscVtaEnelVolumen = 0,
                GnaFiscVtaEnelPoderCal = 1151.55,
                GnaFiscVtaEnelEnergia = 0,
                GnaFiscGcyLgnVolumen = 359.3022,
                GnaFiscGcyLgnPoderCal = 1151.55,
                GnaFiscGcyLgnEnergia = 413.7544,
                GnaFiscGnafVolumen = 4104.2222,
                GnaFiscGnafPoderCal = 1151.55,
                GnaFiscGnafEnergia = 4726.2171,
                GnaFiscTotalVolumen = 36619.5807,
                GnaFiscTotalEnergia = 42213.8758

            }
            );
            ResBalanceEnergLIVDetGnaFisc.Add(new ResBalanceEnergLIVDetGnaFiscDto
            {
                Dia = 10,
                GnaFiscVtaRefVolumen = 117.81,
                GnaFiscVtaRefPoderCal = 1152,
                GnaFiscVtaRefEnergia = 135.7171,
                GnaFiscVtaLimaGasVolumen = 1200,
                GnaFiscVtaLimaGasPoderCal = 1152,
                GnaFiscVtaLimaGasEnergia = 1382.4,
                GnaFiscGasNorpVolumen = 2350.09,
                GnaFiscGasNorpPoderCal = 1152,
                GnaFiscGasNorpEnergia = 2707.3037,
                GnaFiscVtaEnelVolumen = 0,
                GnaFiscVtaEnelPoderCal = 1152,
                GnaFiscVtaEnelEnergia = 0,
                GnaFiscGcyLgnVolumen = 355.8677,
                GnaFiscGcyLgnPoderCal = 1152,
                GnaFiscGcyLgnEnergia = 409.9596,
                GnaFiscGnafVolumen = 4023.7677,
                GnaFiscGnafPoderCal = 1152,
                GnaFiscGnafEnergia = 4635.3804,
                GnaFiscTotalVolumen = 40643.3484,
                GnaFiscTotalEnergia = 46849.2562

            }
            );
            ResBalanceEnergLIVDetGnaFisc.Add(new ResBalanceEnergLIVDetGnaFiscDto
            {
                Dia = 11,
                GnaFiscVtaRefVolumen = 117.81,
                GnaFiscVtaRefPoderCal = 1152,
                GnaFiscVtaRefEnergia = 135.7171,
                GnaFiscVtaLimaGasVolumen = 1200,
                GnaFiscVtaLimaGasPoderCal = 1152,
                GnaFiscVtaLimaGasEnergia = 1382.4,
                GnaFiscGasNorpVolumen = 2350.09,
                GnaFiscGasNorpPoderCal = 1152,
                GnaFiscGasNorpEnergia = 2707.3037,
                GnaFiscVtaEnelVolumen = 0,
                GnaFiscVtaEnelPoderCal = 1152,
                GnaFiscVtaEnelEnergia = 0,
                GnaFiscGcyLgnVolumen = 355.8677,
                GnaFiscGcyLgnPoderCal = 1152,
                GnaFiscGcyLgnEnergia = 409.9596,
                GnaFiscGnafVolumen = 4023.7677,
                GnaFiscGnafPoderCal = 1152,
                GnaFiscGnafEnergia = 4635.3804,
                GnaFiscTotalVolumen = 40643.3484,
                GnaFiscTotalEnergia = 46849.2562

            }
           );
            ResBalanceEnergLIVDetGnaFisc.Add(new ResBalanceEnergLIVDetGnaFiscDto
            {
                Dia = 12,
                GnaFiscVtaRefVolumen = 117.81,
                GnaFiscVtaRefPoderCal = 1152,
                GnaFiscVtaRefEnergia = 135.7171,
                GnaFiscVtaLimaGasVolumen = 1200,
                GnaFiscVtaLimaGasPoderCal = 1152,
                GnaFiscVtaLimaGasEnergia = 1382.4,
                GnaFiscGasNorpVolumen = 2350.09,
                GnaFiscGasNorpPoderCal = 1152,
                GnaFiscGasNorpEnergia = 2707.3037,
                GnaFiscVtaEnelVolumen = 0,
                GnaFiscVtaEnelPoderCal = 1152,
                GnaFiscVtaEnelEnergia = 0,
                GnaFiscGcyLgnVolumen = 355.8677,
                GnaFiscGcyLgnPoderCal = 1152,
                GnaFiscGcyLgnEnergia = 409.9596,
                GnaFiscGnafVolumen = 4023.7677,
                GnaFiscGnafPoderCal = 1152,
                GnaFiscGnafEnergia = 4635.3804,
                GnaFiscTotalVolumen = 40643.3484,
                GnaFiscTotalEnergia = 46849.2562

            }
           );
            ResBalanceEnergLIVDetGnaFisc.Add(new ResBalanceEnergLIVDetGnaFiscDto
            {
                Dia = 13,
                GnaFiscVtaRefVolumen = 117.81,
                GnaFiscVtaRefPoderCal = 1152,
                GnaFiscVtaRefEnergia = 135.7171,
                GnaFiscVtaLimaGasVolumen = 1200,
                GnaFiscVtaLimaGasPoderCal = 1152,
                GnaFiscVtaLimaGasEnergia = 1382.4,
                GnaFiscGasNorpVolumen = 2350.09,
                GnaFiscGasNorpPoderCal = 1152,
                GnaFiscGasNorpEnergia = 2707.3037,
                GnaFiscVtaEnelVolumen = 0,
                GnaFiscVtaEnelPoderCal = 1152,
                GnaFiscVtaEnelEnergia = 0,
                GnaFiscGcyLgnVolumen = 355.8677,
                GnaFiscGcyLgnPoderCal = 1152,
                GnaFiscGcyLgnEnergia = 409.9596,
                GnaFiscGnafVolumen = 4023.7677,
                GnaFiscGnafPoderCal = 1152,
                GnaFiscGnafEnergia = 4635.3804,
                GnaFiscTotalVolumen = 40643.3484,
                GnaFiscTotalEnergia = 46849.2562

            }
           );
            ResBalanceEnergLIVDetGnaFisc.Add(new ResBalanceEnergLIVDetGnaFiscDto
            {
                Dia = 14,
                GnaFiscVtaRefVolumen = 117.81,
                GnaFiscVtaRefPoderCal = 1152,
                GnaFiscVtaRefEnergia = 135.7171,
                GnaFiscVtaLimaGasVolumen = 1200,
                GnaFiscVtaLimaGasPoderCal = 1152,
                GnaFiscVtaLimaGasEnergia = 1382.4,
                GnaFiscGasNorpVolumen = 2350.09,
                GnaFiscGasNorpPoderCal = 1152,
                GnaFiscGasNorpEnergia = 2707.3037,
                GnaFiscVtaEnelVolumen = 0,
                GnaFiscVtaEnelPoderCal = 1152,
                GnaFiscVtaEnelEnergia = 0,
                GnaFiscGcyLgnVolumen = 355.8677,
                GnaFiscGcyLgnPoderCal = 1152,
                GnaFiscGcyLgnEnergia = 409.9596,
                GnaFiscGnafVolumen = 4023.7677,
                GnaFiscGnafPoderCal = 1152,
                GnaFiscGnafEnergia = 4635.3804,
                GnaFiscTotalVolumen = 40643.3484,
                GnaFiscTotalEnergia = 46849.2562

            }
           );
            ResBalanceEnergLIVDetGnaFisc.Add(new ResBalanceEnergLIVDetGnaFiscDto
            {
                Dia = 15,
                GnaFiscVtaRefVolumen = 117.81,
                GnaFiscVtaRefPoderCal = 1152,
                GnaFiscVtaRefEnergia = 135.7171,
                GnaFiscVtaLimaGasVolumen = 1200,
                GnaFiscVtaLimaGasPoderCal = 1152,
                GnaFiscVtaLimaGasEnergia = 1382.4,
                GnaFiscGasNorpVolumen = 2350.09,
                GnaFiscGasNorpPoderCal = 1152,
                GnaFiscGasNorpEnergia = 2707.3037,
                GnaFiscVtaEnelVolumen = 0,
                GnaFiscVtaEnelPoderCal = 1152,
                GnaFiscVtaEnelEnergia = 0,
                GnaFiscGcyLgnVolumen = 355.8677,
                GnaFiscGcyLgnPoderCal = 1152,
                GnaFiscGcyLgnEnergia = 409.9596,
                GnaFiscGnafVolumen = 4023.7677,
                GnaFiscGnafPoderCal = 1152,
                GnaFiscGnafEnergia = 4635.3804,
                GnaFiscTotalVolumen = 40643.3484,
                GnaFiscTotalEnergia = 46849.2562

            }
           );

            return ResBalanceEnergLIVDetGnaFisc;
        }

        private async Task<List<ResBalanceEnergLgnLIV_2DetLgnDto>> ResBalanceEnergLIVDetMedGasLGN()
        {

            List<ResBalanceEnergLgnLIV_2DetLgnDto> ResBalanceEnergLgnLIV_2DetLgn = new List<ResBalanceEnergLgnLIV_2DetLgnDto>();

            var test = await _registroRepositorio.ObtenerMedicionesGasAsync();
            foreach (var item in test)
            {
                ResBalanceEnergLgnLIV_2DetLgn.Add(new ResBalanceEnergLgnLIV_2DetLgnDto
                {
                    Dia = item.Dia,
                    MedGasGlpVolumen = 4193.708,
                    MedGasGlpPoderCal = 1152.93,
                    MedGasGlpEnergia = 4835.0518,
                    MedGasGlpDensidad = item.MedGasGasCombSecoMedVolumen, 

                    MedGasCgnVolumen = item.MedGasGasCombSecoMedPoderCal, 
                    MedGasCgnPoderCal = item.MedGasGasCombSecoMedEnergia, 
                    MedGasCgnEnergia = item.MedGasGasCombSecoMedEnergia, 

                    MedGasLgnVolumen = item.MedGasGasCombSecoMedEnergia, 
                    MedGasLgnPoderCal = item.MedGasGasCombSecoMedEnergia, 
                    MedGasLgnEnergia = item.MedGasGasCombSecoMedEnergia, 
                }
                );
            }
            return ResBalanceEnergLgnLIV_2DetLgn;
        }
    }
}
