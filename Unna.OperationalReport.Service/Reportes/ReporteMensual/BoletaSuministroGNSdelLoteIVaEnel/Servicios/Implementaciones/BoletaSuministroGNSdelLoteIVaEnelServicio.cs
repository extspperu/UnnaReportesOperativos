﻿using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Registro.Entidades;
using Unna.OperationalReport.Data.Registro.Repositorios.Abstracciones;
using Unna.OperationalReport.Service.Registros.DiaOperativos.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletaSuministroGNSdelLoteIVaEnel.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletaSuministroGNSdelLoteIVaEnel.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;

namespace Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletaSuministroGNSdelLoteIVaEnel.Servicios.Implementaciones
{
    public class BoletaSuministroGNSdelLoteIVaEnelServicio : IBoletaSuministroGNSdelLoteIVaEnelServicio
    {
        private readonly IRegistroRepositorio _registroRepositorio;
        DateTime diaOperativo = DateTime.ParseExact("30/11/2023", "dd/MM/yyyy", CultureInfo.InvariantCulture);//FechasUtilitario.ObtenerDiaOperativo();
        double vTotalVolumenMPC=0;
        double vTotalPCBTUPC = 0;
        double vTotalEnergiaMMBTU = 0;
        public BoletaSuministroGNSdelLoteIVaEnelServicio
        (
            IRegistroRepositorio registroRepositorio
        )
        {
            _registroRepositorio = registroRepositorio;
        }

        public async Task<OperacionDto<BoletaSuministroGNSdelLoteIVaEnelDto>> ObtenerAsync(long idUsuario)
        {
            var registrosVol = await _registroRepositorio.ObtenerValorVolumenMensualAsync(1, 4, diaOperativo);
            var registrosPC = await _registroRepositorio.ObtenerValorPoderCalorificoMensualAsync(2, 4, diaOperativo);
            for (int i = 0; i < registrosVol.Count; i++)
            {
                vTotalVolumenMPC = vTotalVolumenMPC + (double)registrosVol[i].Valor;
                vTotalPCBTUPC = vTotalPCBTUPC + (double)registrosPC[i].Valor;
                vTotalEnergiaMMBTU = vTotalEnergiaMMBTU + ((double)registrosVol[i].Valor * (double)registrosPC[i].Valor / 1000);
            }
                var dto = new BoletaSuministroGNSdelLoteIVaEnelDto
            {
                Periodo = diaOperativo.ToString("Y - yyyy"),//"Noviembre-2023",//FechasUtilitario.ObtenerDiaOperativo().ToString("dd-MMMM-yyyy").Substring(3),
                TotalVolumenMPC = vTotalVolumenMPC,
                TotalPCBTUPC = vTotalPCBTUPC/diaOperativo.Day,
                TotalEnergiaMMBTU = vTotalEnergiaMMBTU,

                TotalEnergiaVolTransferidoMMBTU = 0.00,

                Comentarios = "",


            };
            dto.BoletaSuministroGNSdelLoteIVaEnelDet = await BoletaSuministroGNSdelLoteIVaEnelDet();

            return new OperacionDto<BoletaSuministroGNSdelLoteIVaEnelDto>(dto);
        }

        private async Task<List<BoletaSuministroGNSdelLoteIVaEnelDetDto>> BoletaSuministroGNSdelLoteIVaEnelDet()
        {
            List<BoletaSuministroGNSdelLoteIVaEnelDetDto> BoletaSuministroGNSdelLoteIVaEnelDet = new List<BoletaSuministroGNSdelLoteIVaEnelDetDto>();
            var registrosVol = await _registroRepositorio.ObtenerValorVolumenMensualAsync(1, 4, diaOperativo);
            var registrosPC = await _registroRepositorio.ObtenerValorPoderCalorificoMensualAsync(2, 4, diaOperativo);
            for (int i = 0; i < registrosVol.Count; i++)
            {
                
            
                BoletaSuministroGNSdelLoteIVaEnelDet.Add(new BoletaSuministroGNSdelLoteIVaEnelDetDto
                {
                    Fecha = registrosVol[i].Fecha.ToString("dd/MM/yyyy"),
                    VolumneMPC = registrosVol[i].Valor,
                    PCBTUPC = (double)registrosPC[i].Valor,
                    EnergiaMMBTU = (double)registrosVol[i].Valor * (double)registrosPC[i].Valor / 1000///(VolumneMPC * PCBTUPC)/1000



                });
            }
            return BoletaSuministroGNSdelLoteIVaEnelDet;
            
        }
    }
}
