using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Registro.Entidades;
using Unna.OperationalReport.Data.Registro.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Registro.Repositorios.Implementaciones;
using Unna.OperationalReport.Data.Reporte.Enums;
using Unna.OperationalReport.Service.Registros.DiaOperativos.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.Generales.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.Impresiones.Dtos;
using Unna.OperationalReport.Service.Reportes.Impresiones.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletaSuministroGNSdelLoteIVaEnel.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletaSuministroGNSdelLoteIVaEnel.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ComposicionGnaLIV.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;

namespace Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletaSuministroGNSdelLoteIVaEnel.Servicios.Implementaciones
{
    public class BoletaSuministroGNSdelLoteIVaEnelServicio : IBoletaSuministroGNSdelLoteIVaEnelServicio
    {
        private readonly IRegistroRepositorio _registroRepositorio;
        private readonly IImpresionServicio _impresionServicio;
        private readonly IReporteServicio _reporteServicio;
        DateTime diaOperativo = DateTime.ParseExact("30/11/2023", "dd/MM/yyyy", CultureInfo.InvariantCulture);//FechasUtilitario.ObtenerDiaOperativo();
        double vTotalVolumenMPC=0;
        double vTotalPCBTUPC = 0;
        double vTotalEnergiaMMBTU = 0;
        public BoletaSuministroGNSdelLoteIVaEnelServicio
        (
            IRegistroRepositorio registroRepositorio,
            IImpresionServicio impresionServicio,
            IReporteServicio reporteServicio
        )
        {
            _registroRepositorio = registroRepositorio;
            _impresionServicio = impresionServicio;
            _reporteServicio = reporteServicio;
        }

        public async Task<OperacionDto<BoletaSuministroGNSdelLoteIVaEnelDto>> ObtenerAsync(long idUsuario)
        {
            var operacionGeneral = await _reporteServicio.ObtenerAsync((int)TiposReportes.BoletaSuministroGNSdelLoteIVaEnel, idUsuario);
            if (!operacionGeneral.Completado)
            {
                return new OperacionDto<BoletaSuministroGNSdelLoteIVaEnelDto>(CodigosOperacionDto.NoExiste, operacionGeneral.Mensajes);
            }

            string observacion = default(string);
            var operacionImpresion = await _impresionServicio.ObtenerAsync((int)TiposReportes.BoletaSuministroGNSdelLoteIVaEnel, diaOperativo);
            if (operacionImpresion != null && operacionImpresion.Completado && operacionImpresion.Resultado != null && !string.IsNullOrWhiteSpace(operacionImpresion.Resultado.Datos))
            {
                observacion = operacionImpresion.Resultado?.Comentario;
                if (new DateTime(diaOperativo.Year, diaOperativo.Month, 1) == new DateTime(operacionImpresion.Resultado.Fecha.Year, operacionImpresion.Resultado.Fecha.Month, 1))
                {
                    var rpta = JsonConvert.DeserializeObject<BoletaSuministroGNSdelLoteIVaEnelDto>(operacionImpresion.Resultado.Datos);
                    rpta.General = operacionGeneral.Resultado;
                    return new OperacionDto<BoletaSuministroGNSdelLoteIVaEnelDto>(rpta);
                }
            }

            var registrosVol = await _registroRepositorio.ObtenerValorMensualGNSAsync(1, 4, diaOperativo);
            var registrosPC = await _registroRepositorio.ObtenerValorMensualGNSAsync(2, 4, diaOperativo);
            for (int i = 0; i < registrosVol.Count; i++)
            {
                vTotalVolumenMPC = vTotalVolumenMPC + (double)registrosVol[i].Valor;
                vTotalPCBTUPC = vTotalPCBTUPC + (double)registrosPC[i].Valor;
                vTotalEnergiaMMBTU = Math.Round((vTotalEnergiaMMBTU + ((double)registrosVol[i].Valor * (double)registrosPC[i].Valor / 1000)), 4, MidpointRounding.AwayFromZero);
            }
                var dto = new BoletaSuministroGNSdelLoteIVaEnelDto
            {
                Periodo = diaOperativo.ToString("MMM - yyyy"),//"Noviembre-2023",//FechasUtilitario.ObtenerDiaOperativo().ToString("dd-MMMM-yyyy").Substring(3),
                TotalVolumenMPC = vTotalVolumenMPC,
                TotalPCBTUPC = Math.Round((vTotalPCBTUPC/diaOperativo.Day), 2, MidpointRounding.AwayFromZero),
                TotalEnergiaMMBTU = vTotalEnergiaMMBTU,

                TotalEnergiaVolTransferidoMMBTU = vTotalEnergiaMMBTU,

                Comentarios = "",


            };
            dto.BoletaSuministroGNSdelLoteIVaEnelDet = await BoletaSuministroGNSdelLoteIVaEnelDet();

            return new OperacionDto<BoletaSuministroGNSdelLoteIVaEnelDto>(dto);
        }

        private async Task<List<BoletaSuministroGNSdelLoteIVaEnelDetDto>> BoletaSuministroGNSdelLoteIVaEnelDet()
        {
            List<BoletaSuministroGNSdelLoteIVaEnelDetDto> BoletaSuministroGNSdelLoteIVaEnelDet = new List<BoletaSuministroGNSdelLoteIVaEnelDetDto>();
            var registrosVol = await _registroRepositorio.ObtenerValorMensualGNSAsync(1, 4, diaOperativo);
            var registrosPC = await _registroRepositorio.ObtenerValorMensualGNSAsync(2, 4, diaOperativo);
            for (int i = 0; i < registrosVol.Count; i++)
            {
                
            
                BoletaSuministroGNSdelLoteIVaEnelDet.Add(new BoletaSuministroGNSdelLoteIVaEnelDetDto
                {
                    Fecha = registrosVol[i].Fecha.ToString("dd/MM/yyyy"),
                    VolumneMPC = registrosVol[i].Valor,
                    PCBTUPC = (double)registrosPC[i].Valor,
                    EnergiaMMBTU = Math.Round(((double)registrosVol[i].Valor * (double)registrosPC[i].Valor / 1000), 4, MidpointRounding.AwayFromZero)///(VolumneMPC * PCBTUPC)/1000



                });
            }
            return BoletaSuministroGNSdelLoteIVaEnelDet;
            
        }

        public async Task<OperacionDto<RespuestaSimpleDto<string>>> GuardarAsync(BoletaSuministroGNSdelLoteIVaEnelDto peticion)
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
                IdConfiguracion = RijndaelUtilitario.EncryptRijndaelToUrl((int)TiposReportes.BoletaSuministroGNSdelLoteIVaEnel),
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
