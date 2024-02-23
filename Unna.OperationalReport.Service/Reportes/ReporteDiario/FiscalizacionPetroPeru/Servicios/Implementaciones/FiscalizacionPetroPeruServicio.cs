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
using Unna.OperationalReport.Service.Reportes.Generales.Servicios.Abstracciones;
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
        public FiscalizacionPetroPeruServicio(
            IReporteServicio reporteServicio,
            IBoletaDiariaFiscalizacionRepositorio boletaDiariaFiscalizacionRepositorio
            )
        {
            _reporteServicio = reporteServicio;
            _boletaDiariaFiscalizacionRepositorio = boletaDiariaFiscalizacionRepositorio;
        }

        public async Task<OperacionDto<FiscalizacionPetroPeruDto>> ObtenerAsync(long idUsuario)
        {
            var dto = new FiscalizacionPetroPeruDto
            {
                Fecha = FechasUtilitario.ObtenerDiaOperativo().ToString("dd/MM/yyyy")
            };
            var operacionGeneral = await _reporteServicio.ObtenerAsync((int)TiposReportes.BoletaCnpc, idUsuario);
            if (!operacionGeneral.Completado)
            {
                return new OperacionDto<FiscalizacionPetroPeruDto>(CodigosOperacionDto.NoExiste, operacionGeneral.Mensajes);
            }
            dto.General = operacionGeneral.Resultado;

            // Cuadro N° 1
            dto.VolumenTotalProduccion = 999.9;
            dto.ContenidoLgn = 1113.51;
            dto.Eficiencia = (dto.VolumenTotalProduccion / dto.ContenidoLgn) * 100;
            dto.FactorAsignacionLiquidoGasNatural = await ObtenerFactorAsignacionLiquidoGasNatural(dto.VolumenTotalProduccion??0, dto.ContenidoLgn??0);

            dto.FactorConversionI = new double?();
            dto.FactorConversionZ69 = new double?();
            dto.FactorConversionVi = new double?();

            // Cuadro N° 2
            dto.DistribucionGasNaturalSeco = await ObtenerDistribucionGasNaturalSecoAsync(dto.VolumenTotalProduccion ?? 0, dto.ContenidoLgn ?? 0);

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


        private async Task<List<DistribucionGasNaturalSecoDto>> ObtenerDistribucionGasNaturalSecoAsync(double volumenTotalProduccion, double contenidoLgn)
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

            //lista.ForEach(e => e.Factor = ((e.Contenido / contenidoLgn) / 42) * 100);// 42 es fijo

            //lista.ForEach(e => e.Asignacion = Math.Round((volumenTotalProduccion * e.Factor), 2));

            return lista;
        }

    }
}
