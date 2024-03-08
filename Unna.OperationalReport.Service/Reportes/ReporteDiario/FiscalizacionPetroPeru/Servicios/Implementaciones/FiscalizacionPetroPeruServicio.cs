using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Auth.Enums;
using Unna.OperationalReport.Data.Registro.Entidades;
using Unna.OperationalReport.Data.Registro.Enums;
using Unna.OperationalReport.Data.Reporte.Enums;
using Unna.OperationalReport.Data.Reporte.Repositorios.Abstracciones;
using Unna.OperationalReport.Service.Registros.DiaOperativos.Dtos;
using Unna.OperationalReport.Service.Reportes.Generales.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.Impresiones.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaCnpc.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.FiscalizacionPetroPeru.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.FiscalizacionPetroPeru.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;

namespace Unna.OperationalReport.Service.Reportes.ReporteDiario.FiscalizacionPetroPeru.Servicios.Implementaciones
{
    public class FiscalizacionPetroPeruServicio: IFiscalizacionPetroPeruServicio
    {

        private readonly IReporteServicio _reporteServicio;
        private readonly IBoletaDiariaFiscalizacionRepositorio _boletaDiariaFiscalizacionRepositorio;
        private readonly IImpresionServicio _impresionServicio;
        private readonly IReporteDiariaDatosRepositorio _reporteDiariaDatosRepositorio;
        public FiscalizacionPetroPeruServicio(
            IReporteServicio reporteServicio,
            IBoletaDiariaFiscalizacionRepositorio boletaDiariaFiscalizacionRepositorio,
            IImpresionServicio impresionServicio,
            IReporteDiariaDatosRepositorio reporteDiariaDatosRepositorio
            )
        {
            _reporteServicio = reporteServicio;
            _boletaDiariaFiscalizacionRepositorio = boletaDiariaFiscalizacionRepositorio;
            _impresionServicio = impresionServicio;
            _reporteDiariaDatosRepositorio = reporteDiariaDatosRepositorio;
        }

        public async Task<OperacionDto<FiscalizacionPetroPeruDto>> ObtenerAsync(long idUsuario)
        {
            DateTime diaOperativo = FechasUtilitario.ObtenerDiaOperativo();
            var dto = new FiscalizacionPetroPeruDto
            {
                Fecha = diaOperativo.ToString("dd/MM/yyyy")
            };
            var operacionGeneral = await _reporteServicio.ObtenerAsync((int)TiposReportes.BoletaCnpc, idUsuario);
            if (!operacionGeneral.Completado)
            {
                return new OperacionDto<FiscalizacionPetroPeruDto>(CodigosOperacionDto.NoExiste, operacionGeneral.Mensajes);
            }
            dto.General = operacionGeneral.Resultado;

            //var operacionReporte = await _impresionServicio.ObtenerAsync((int)TiposReportes.BoletaBalanceEnergiaDiaria, FechasUtilitario.ObtenerDiaOperativo());

            // Cuadro N° 1
            dto.VolumenTotalProduccion = 999.9;
            dto.ContenidoLgn = 1113.51;
            dto.Eficiencia = (dto.VolumenTotalProduccion / dto.ContenidoLgn) * 100;
            dto.FactorAsignacionLiquidoGasNatural = await ObtenerFactorAsignacionLiquidoGasNatural(dto.VolumenTotalProduccion??0,dto.ContenidoLgn??0);

            var factorConversionZ69 = await _reporteDiariaDatosRepositorio.ObtenerFactorConversionPorLotePetroperuAsync(diaOperativo, (int)TiposLote.LoteZ69,(int)TiposDatos.VolumenMpcd, dto.Eficiencia);
            if (factorConversionZ69 != null)
            {
                dto.FactorConversionZ69 = factorConversionZ69.Value;
            }

            var factorConversionVi = await _reporteDiariaDatosRepositorio.ObtenerFactorConversionPorLotePetroperuAsync(diaOperativo, (int)TiposLote.LoteVI, (int)TiposDatos.VolumenMpcd, dto.Eficiencia);
            if (factorConversionVi != null)
            {
                dto.FactorConversionVi = factorConversionVi.Value;
            }

            var factorConversionI = await _reporteDiariaDatosRepositorio.ObtenerFactorConversionPorLotePetroperuAsync(diaOperativo, (int)TiposLote.LoteI, (int)TiposDatos.VolumenMpcd, dto.Eficiencia);
            if (factorConversionI != null)
            {
                dto.FactorConversionI = factorConversionI.Value;
            }
           
            

            // Cuadro N° 2
            dto.DistribucionGasNaturalSeco = await ObtenerDistribucionGasNaturalSecoAsync(dto);


            // Cuadro N° 3
            dto.VolumenTotalGns = 0;
            dto.VolumenTotalGnsFlare = 0;
            dto.VolumenTransferidoRefineriaPorLote = VolumenTransferidoRefineriaPorLoteAsync(dto);

            return new OperacionDto<FiscalizacionPetroPeruDto>(dto);
        }


        private async Task<List<FactorAsignacionLiquidoGasNaturalDto>> ObtenerFactorAsignacionLiquidoGasNatural(double volumenTotalProduccion, double contenidoLgn)
        {
            var lista = new List<FactorAsignacionLiquidoGasNaturalDto>();
            var entidades = await _boletaDiariaFiscalizacionRepositorio.ListarFactorAsignacionLiquidoGasNaturalAsync(FechasUtilitario.ObtenerDiaOperativo(),(int)TiposDatos.VolumenMpcd,(int)TiposDatos.Riqueza, (int)TiposDatos.PoderCalorifico);
            lista = entidades.Select(e => new FactorAsignacionLiquidoGasNaturalDto()
            {
                Item = e.Item,
                Suministrador = e.Suministrador,
                Volumen = e.Volumen,
                Riqueza = e.Riqueza,                
            }).ToList();

            lista.ForEach(e => e.Factor = ((e.Contenido / contenidoLgn) / 42) * 100);// 42 es fijo

            lista.ForEach(e => e.Asignacion = Math.Round((volumenTotalProduccion * e.Factor),2));
                        
            return lista;
        }

        // Cuadro N° 2
        private async Task<List<DistribucionGasNaturalSecoDto>> ObtenerDistribucionGasNaturalSecoAsync(FiscalizacionPetroPeruDto parametros)
        {
            var lista = new List<DistribucionGasNaturalSecoDto>();
            var entidades = await _boletaDiariaFiscalizacionRepositorio.ListarFactorAsignacionLiquidoGasNaturalAsync(FechasUtilitario.ObtenerDiaOperativo(), (int)TiposDatos.VolumenMpcd, (int)TiposDatos.Riqueza, (int)TiposDatos.PoderCalorifico);
            lista = entidades.Select(e => new DistribucionGasNaturalSecoDto()
            {
                Item = e.Item,
                Suministrador = e.Suministrador,
                VolumenGna = e.Volumen,
                PoderCalorifico = e.Calorifico,
            }).ToList();

            if (parametros.FactorAsignacionLiquidoGasNatural != null  && parametros.FactorAsignacionLiquidoGasNatural.Where(e => e.Item == 1).FirstOrDefault() != null)
            {
               double volumenGns =  Math.Round((parametros.FactorAsignacionLiquidoGasNatural.Where(e => e.Item == 1).First().Asignacion * 42 * parametros.FactorConversionZ69 / 1000), 4);                
               lista.Where(w => w.Item == 1).ToList().ForEach(s => s.VolumenGns = volumenGns);
            }

            if (parametros.FactorAsignacionLiquidoGasNatural != null && parametros.FactorAsignacionLiquidoGasNatural.Where(e => e.Item == 2).FirstOrDefault() != null)
            {
                double volumenGns = Math.Round((parametros.FactorAsignacionLiquidoGasNatural.Where(e => e.Item == 2).First().Asignacion * 42 * parametros.FactorConversionVi / 1000), 4);
                lista.Where(w => w.Item == 2).ToList().ForEach(s => s.VolumenGns = volumenGns);
            }

            if (parametros.FactorAsignacionLiquidoGasNatural != null && parametros.FactorAsignacionLiquidoGasNatural.Where(e => e.Item ==3).FirstOrDefault() != null)
            {
                double volumenGns = Math.Round((parametros.FactorAsignacionLiquidoGasNatural.Where(e => e.Item == 3).First().Asignacion * 42 * parametros.FactorConversionI / 1000), 4);
                lista.Where(w => w.Item == 3).ToList().ForEach(s => s.VolumenGns = volumenGns);
            }

            lista.ForEach(e => e.VolumenGnsd = e.VolumenGna - e.VolumenGns);
                        
            return lista;
        }


        // Cuadro N° 3
        private List<VolumenTransferidoRefineriaPorLoteDto> VolumenTransferidoRefineriaPorLoteAsync(FiscalizacionPetroPeruDto parametros)
        {
            var lista = new List<VolumenTransferidoRefineriaPorLoteDto>();

            if (parametros.DistribucionGasNaturalSeco == null || parametros.DistribucionGasNaturalSeco.Count == 0)
            {
                return lista;
            }

            lista = parametros.DistribucionGasNaturalSeco.Select(e => new VolumenTransferidoRefineriaPorLoteDto()
            {
                Item = e.Item,
                Suministrador = e.Suministrador,
                VolumenGns = e.VolumenGnsd
            }).ToList();
            
            lista.ForEach(e => e.VolumenGnsTransferido = e.VolumenGns - e.VolumenFlare);

            return lista;
        }




        public async Task<OperacionDto<RespuestaSimpleDto<bool>>> GuardarAsync(FiscalizacionPetroPeruDto peticion)
        {
            await Task.Delay(0);

            return new OperacionDto<RespuestaSimpleDto<bool>>(
                new RespuestaSimpleDto<bool>()
                {
                    Id = true,
                    Mensaje = "Se guardo correctamente"
                }
                );

        }



    }
}
