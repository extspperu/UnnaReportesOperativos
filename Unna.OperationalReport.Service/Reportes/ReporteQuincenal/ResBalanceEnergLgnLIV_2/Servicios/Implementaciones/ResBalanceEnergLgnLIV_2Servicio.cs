using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ResBalanceEnergLgnLIV.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ResBalanceEnergLgnLIV_2.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ResBalanceEnergLgnLIV_2.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;

namespace Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ResBalanceEnergLgnLIV_2.Servicios.Implementaciones
{
    public class ResBalanceEnergLgnLIV_2Servicio : IResBalanceEnergLgnLIV_2Servicio
    {
        public async Task<OperacionDto<ResBalanceEnergLgnLIV_2Dto>> ObtenerAsync(long idUsuario)
        {
            var dto = new ResBalanceEnergLgnLIV_2Dto
            {
                Lote = "LOTE IV",
                Mes = "NOVIEMBRE",
                Anio = "2023",

                //Medicion de Gas Natural Lote IV - Acumulado Quincena UNNA
                AcumUnnaQ1MedGasGlpVolumen = 1398.84,
                AcumUnnaQ1MedGasGlpPoderCal = 4.2797,
                AcumUnnaQ1MedGasGlpEnergia = 5986.6137,
                AcumUnnaQ1MedGasGlpDensidad = 91.2698,
                AcumUnnaQ1MedGasCgnVolumen = 450.59,
                AcumUnnaQ1MedGasCgnPoderCal = 4.7089,
                AcumUnnaQ1MedGasCgnEnergia = 2121.771,
                AcumUnnaQ1MedGasLgnVolumen = 1849.43,
                AcumUnnaQ1MedGasLgnPoderCal = 4.467,
                AcumUnnaQ1MedGasLgnEnergia = 8261.3699,

                AcumUnnaQ2MedGasGlpVolumen = 0,
                AcumUnnaQ2MedGasGlpPoderCal = 4.0674,
                AcumUnnaQ2MedGasGlpEnergia = 0,
                AcumUnnaQ2MedGasGlpDensidad = 86.0282,
                AcumUnnaQ2MedGasCgnVolumen = 0,
                AcumUnnaQ2MedGasCgnPoderCal = 4.7097,
                AcumUnnaQ2MedGasCgnEnergia = 0,
                AcumUnnaQ2MedGasLgnVolumen = 0,
                AcumUnnaQ2MedGasLgnPoderCal = 4.2539,
                AcumUnnaQ2MedGasLgnEnergia = 0,

                //Medicion de Gas Natural Lote IV - Acumulado Quincena PERUPETRO
                AcumPeruPetroQ1MedGasGlpVolumen = 1398.84,
                AcumPeruPetroQ1MedGasGlpPoderCal = 4.2797,
                AcumPeruPetroQ1MedGasGlpEnergia = 5986.6137,
                AcumPeruPetroQ1MedGasGlpDensidad = 91.2698,
                AcumPeruPetroQ1MedGasCgnVolumen = 450.59,
                AcumPeruPetroQ1MedGasCgnPoderCal = 4.7089,
                AcumPeruPetroQ1MedGasCgnEnergia = 2121.771,
                AcumPeruPetroQ1MedGasLgnVolumen = 1849.43,
                AcumPeruPetroQ1MedGasLgnPoderCal = 4.467,
                AcumPeruPetroQ1MedGasLgnEnergia = 8261.3699,

                AcumPeruPetroQ2MedGasGlpVolumen = 0,
                AcumPeruPetroQ2MedGasGlpPoderCal = 0,
                AcumPeruPetroQ2MedGasGlpEnergia = 0,
                AcumPeruPetroQ2MedGasGlpDensidad = 0,
                AcumPeruPetroQ2MedGasCgnVolumen = 0,
                AcumPeruPetroQ2MedGasCgnPoderCal = 0,
                AcumPeruPetroQ2MedGasCgnEnergia = 0,
                AcumPeruPetroQ2MedGasLgnVolumen = 0,
                AcumPeruPetroQ2MedGasLgnPoderCal = 0,
                AcumPeruPetroQ2MedGasLgnEnergia = 0,

                //Medicion de Gas Natural Lote IV - Diff Unna-PeruPetro Quincena 1 y 2
                DifUPQ1MedGasGlpVolumen = 0,
                DifUPQ1MedGasGlpPoderCal = 0,
                DifUPQ1MedGasGlpEnergia = 0,
                DifUPQ1MedGasGlpDensidad = 0,
                DifUPQ1MedGasCgnVolumen = 0,
                DifUPQ1MedGasCgnPoderCal = 0,
                DifUPQ1MedGasCgnEnergia = 0,
                DifUPQ1MedGasLgnVolumen = 0,
                DifUPQ1MedGasLgnPoderCal = 0,
                DifUPQ1MedGasLgnEnergia = 0,

                DifUPQ2MedGasGlpVolumen = 0,
                DifUPQ2MedGasGlpPoderCal = -4.0674,
                DifUPQ2MedGasGlpEnergia = 0,
                DifUPQ2MedGasGlpDensidad = -86.0282,
                DifUPQ2MedGasCgnVolumen = 0,
                DifUPQ2MedGasCgnPoderCal = -4.7097,
                DifUPQ2MedGasCgnEnergia = 0,
                DifUPQ2MedGasLgnVolumen = 0,
                DifUPQ2MedGasLgnPoderCal = -4.2539,
                DifUPQ2MedGasLgnEnergia = 0,

                PromQ1MedGasGlpDensidad = 91.2697660497154,
                PromQ1MedGasGlpPoderCal = 4.27969860651788,
                PromQ1MedGasCgnPoderCal = 4.70887243494649,
                PromQ1MedGasLgnPoderCal = 4.46698162739498,
                Q1MedGasFactorConv = 34.25,
                TotalQ1MedGasGlpEnergia = 5986.6137,
                TotalQ1MedGasCgnEnergia = 2121.771,


                PromQ2MedGasGlpDensidad = 86.0282,
                PromQ2MedGasGlpPoderCal = 4.0674,
                PromQ2MedGasCgnPoderCal = 4.7097,
                PromQ2MedGasLgnPoderCal = 4.25389287176218,
                Q2MedGasFactorConv = 31.57,
                TotalQ2MedGasGlpEnergia = 0,
                TotalQ2MedGasCgnEnergia = 0

            };

            dto.ResBalanceEnergLgnLIV_2DetLgn = await ResBalanceEnergLgnLIV_2DetLgn();

            return new OperacionDto<ResBalanceEnergLgnLIV_2Dto>(dto);
        }

        private async Task<List<ResBalanceEnergLgnLIV_2DetLgnDto>?> ResBalanceEnergLgnLIV_2DetLgn()
        {
            List<ResBalanceEnergLgnLIV_2DetLgnDto> ResBalanceEnergLgnLIV_2DetLgn = new List<ResBalanceEnergLgnLIV_2DetLgnDto>();

            ResBalanceEnergLgnLIV_2DetLgn.Add(new ResBalanceEnergLgnLIV_2DetLgnDto
            {
                Dia = 16,
                MedGasGlpVolumen = 4193.708,
                MedGasGlpPoderCal = 1152.93,
                MedGasGlpEnergia = 4835.0518,
                MedGasGlpDensidad = 160.95,
                MedGasCgnVolumen = 1055.08,
                MedGasCgnPoderCal = 169.8151,
                MedGasCgnEnergia = 213.097,
                MedGasLgnVolumen = 3105.31,
                MedGasLgnPoderCal = 661.7322,
                MedGasLgnEnergia = 0,
            }
            );

            ResBalanceEnergLgnLIV_2DetLgn.Add(new ResBalanceEnergLgnLIV_2DetLgnDto
            {
                Dia = 17,
                MedGasGlpVolumen = 4193.708,
                MedGasGlpPoderCal = 1152.93,
                MedGasGlpEnergia = 4835.0518,
                MedGasGlpDensidad = 160.95,
                MedGasCgnVolumen = 1055.08,
                MedGasCgnPoderCal = 169.8151,
                MedGasCgnEnergia = 213.097,
                MedGasLgnVolumen = 3105.31,
                MedGasLgnPoderCal = 661.7322,
                MedGasLgnEnergia = 0,
            }
            );

            ResBalanceEnergLgnLIV_2DetLgn.Add(new ResBalanceEnergLgnLIV_2DetLgnDto
            {
                Dia = 18,
                MedGasGlpVolumen = 4193.708,
                MedGasGlpPoderCal = 1152.93,
                MedGasGlpEnergia = 4835.0518,
                MedGasGlpDensidad = 160.95,
                MedGasCgnVolumen = 1055.08,
                MedGasCgnPoderCal = 169.8151,
                MedGasCgnEnergia = 213.097,
                MedGasLgnVolumen = 3105.31,
                MedGasLgnPoderCal = 661.7322,
                MedGasLgnEnergia = 0,
            }
            );

            ResBalanceEnergLgnLIV_2DetLgn.Add(new ResBalanceEnergLgnLIV_2DetLgnDto
            {
                Dia = 19,
                MedGasGlpVolumen = 4193.708,
                MedGasGlpPoderCal = 1152.93,
                MedGasGlpEnergia = 4835.0518,
                MedGasGlpDensidad = 160.95,
                MedGasCgnVolumen = 1055.08,
                MedGasCgnPoderCal = 169.8151,
                MedGasCgnEnergia = 213.097,
                MedGasLgnVolumen = 3105.31,
                MedGasLgnPoderCal = 661.7322,
                MedGasLgnEnergia = 0,
            }
            );

            ResBalanceEnergLgnLIV_2DetLgn.Add(new ResBalanceEnergLgnLIV_2DetLgnDto
            {
                Dia = 20,
                MedGasGlpVolumen = 4193.708,
                MedGasGlpPoderCal = 1152.93,
                MedGasGlpEnergia = 4835.0518,
                MedGasGlpDensidad = 160.95,
                MedGasCgnVolumen = 1055.08,
                MedGasCgnPoderCal = 169.8151,
                MedGasCgnEnergia = 213.097,
                MedGasLgnVolumen = 3105.31,
                MedGasLgnPoderCal = 661.7322,
                MedGasLgnEnergia = 0,
            }
            );

            ResBalanceEnergLgnLIV_2DetLgn.Add(new ResBalanceEnergLgnLIV_2DetLgnDto
            {
                Dia = 21,
                MedGasGlpVolumen = 4193.708,
                MedGasGlpPoderCal = 1152.93,
                MedGasGlpEnergia = 4835.0518,
                MedGasGlpDensidad = 160.95,
                MedGasCgnVolumen = 1055.08,
                MedGasCgnPoderCal = 169.8151,
                MedGasCgnEnergia = 213.097,
                MedGasLgnVolumen = 3105.31,
                MedGasLgnPoderCal = 661.7322,
                MedGasLgnEnergia = 0,
            }
            );

            ResBalanceEnergLgnLIV_2DetLgn.Add(new ResBalanceEnergLgnLIV_2DetLgnDto
            {
                Dia = 22,
                MedGasGlpVolumen = 4193.708,
                MedGasGlpPoderCal = 1152.93,
                MedGasGlpEnergia = 4835.0518,
                MedGasGlpDensidad = 160.95,
                MedGasCgnVolumen = 1055.08,
                MedGasCgnPoderCal = 169.8151,
                MedGasCgnEnergia = 213.097,
                MedGasLgnVolumen = 3105.31,
                MedGasLgnPoderCal = 661.7322,
                MedGasLgnEnergia = 0,
            }
            );

            ResBalanceEnergLgnLIV_2DetLgn.Add(new ResBalanceEnergLgnLIV_2DetLgnDto
            {
                Dia = 23,
                MedGasGlpVolumen = 4193.708,
                MedGasGlpPoderCal = 1152.93,
                MedGasGlpEnergia = 4835.0518,
                MedGasGlpDensidad = 160.95,
                MedGasCgnVolumen = 1055.08,
                MedGasCgnPoderCal = 169.8151,
                MedGasCgnEnergia = 213.097,
                MedGasLgnVolumen = 3105.31,
                MedGasLgnPoderCal = 661.7322,
                MedGasLgnEnergia = 0,
            }
            );

            ResBalanceEnergLgnLIV_2DetLgn.Add(new ResBalanceEnergLgnLIV_2DetLgnDto
            {
                Dia = 24,
                MedGasGlpVolumen = 4193.708,
                MedGasGlpPoderCal = 1152.93,
                MedGasGlpEnergia = 4835.0518,
                MedGasGlpDensidad = 160.95,
                MedGasCgnVolumen = 1055.08,
                MedGasCgnPoderCal = 169.8151,
                MedGasCgnEnergia = 213.097,
                MedGasLgnVolumen = 3105.31,
                MedGasLgnPoderCal = 661.7322,
                MedGasLgnEnergia = 0,
            }
            );

            ResBalanceEnergLgnLIV_2DetLgn.Add(new ResBalanceEnergLgnLIV_2DetLgnDto
            {
                Dia = 25,
                MedGasGlpVolumen = 4193.708,
                MedGasGlpPoderCal = 1152.93,
                MedGasGlpEnergia = 4835.0518,
                MedGasGlpDensidad = 160.95,
                MedGasCgnVolumen = 1055.08,
                MedGasCgnPoderCal = 169.8151,
                MedGasCgnEnergia = 213.097,
                MedGasLgnVolumen = 3105.31,
                MedGasLgnPoderCal = 661.7322,
                MedGasLgnEnergia = 0,
            }
            );

            ResBalanceEnergLgnLIV_2DetLgn.Add(new ResBalanceEnergLgnLIV_2DetLgnDto
            {
                Dia = 26,
                MedGasGlpVolumen = 4193.708,
                MedGasGlpPoderCal = 1152.93,
                MedGasGlpEnergia = 4835.0518,
                MedGasGlpDensidad = 160.95,
                MedGasCgnVolumen = 1055.08,
                MedGasCgnPoderCal = 169.8151,
                MedGasCgnEnergia = 213.097,
                MedGasLgnVolumen = 3105.31,
                MedGasLgnPoderCal = 661.7322,
                MedGasLgnEnergia = 0,
            }
            );

            ResBalanceEnergLgnLIV_2DetLgn.Add(new ResBalanceEnergLgnLIV_2DetLgnDto
            {
                Dia = 27,
                MedGasGlpVolumen = 4193.708,
                MedGasGlpPoderCal = 1152.93,
                MedGasGlpEnergia = 4835.0518,
                MedGasGlpDensidad = 160.95,
                MedGasCgnVolumen = 1055.08,
                MedGasCgnPoderCal = 169.8151,
                MedGasCgnEnergia = 213.097,
                MedGasLgnVolumen = 3105.31,
                MedGasLgnPoderCal = 661.7322,
                MedGasLgnEnergia = 0,
            }
            );

            ResBalanceEnergLgnLIV_2DetLgn.Add(new ResBalanceEnergLgnLIV_2DetLgnDto
            {
                Dia = 28,
                MedGasGlpVolumen = 4193.708,
                MedGasGlpPoderCal = 1152.93,
                MedGasGlpEnergia = 4835.0518,
                MedGasGlpDensidad = 160.95,
                MedGasCgnVolumen = 1055.08,
                MedGasCgnPoderCal = 169.8151,
                MedGasCgnEnergia = 213.097,
                MedGasLgnVolumen = 3105.31,
                MedGasLgnPoderCal = 661.7322,
                MedGasLgnEnergia = 0,
            }
            );

            ResBalanceEnergLgnLIV_2DetLgn.Add(new ResBalanceEnergLgnLIV_2DetLgnDto
            {
                Dia = 29,
                MedGasGlpVolumen = 4193.708,
                MedGasGlpPoderCal = 1152.93,
                MedGasGlpEnergia = 4835.0518,
                MedGasGlpDensidad = 160.95,
                MedGasCgnVolumen = 1055.08,
                MedGasCgnPoderCal = 169.8151,
                MedGasCgnEnergia = 213.097,
                MedGasLgnVolumen = 3105.31,
                MedGasLgnPoderCal = 661.7322,
                MedGasLgnEnergia = 0,
            }
            );

            ResBalanceEnergLgnLIV_2DetLgn.Add(new ResBalanceEnergLgnLIV_2DetLgnDto
            {
                Dia = 30,
                MedGasGlpVolumen = 4193.708,
                MedGasGlpPoderCal = 1152.93,
                MedGasGlpEnergia = 4835.0518,
                MedGasGlpDensidad = 160.95,
                MedGasCgnVolumen = 1055.08,
                MedGasCgnPoderCal = 169.8151,
                MedGasCgnEnergia = 213.097,
                MedGasLgnVolumen = 3105.31,
                MedGasLgnPoderCal = 661.7322,
                MedGasLgnEnergia = 0,
            }
            );

            return ResBalanceEnergLgnLIV_2DetLgn;
        }

       
        

    }
}
