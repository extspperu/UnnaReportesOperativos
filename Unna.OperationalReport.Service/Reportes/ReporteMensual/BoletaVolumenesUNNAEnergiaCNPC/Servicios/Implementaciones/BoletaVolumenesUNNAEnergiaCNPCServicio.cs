using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Mantenimiento.Enums;
using Unna.OperationalReport.Data.Reporte.Enums;
using Unna.OperationalReport.Data.Reporte.Repositorios.Abstracciones;
using Unna.OperationalReport.Service.Mantenimientos.ValoresDefectoReportes.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.Generales.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.Impresiones.Dtos;
using Unna.OperationalReport.Service.Reportes.Impresiones.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.Impresiones.Servicios.Implementaciones;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaCnpc.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletaVolumenesUNNAEnergiaCNPC.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletaVolumenesUNNAEnergiaCNPC.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;

namespace Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletaVolumenesUNNAEnergiaCNPC.Servicios.Implementaciones
{
    public class BoletaVolumenesUNNAEnergiaCNPCServicio : IBoletaVolumenesUNNAEnergiaCNPCServicio
    {

        private readonly IBoletaCnpcRepositorio _boletaCnpcRepositorio;
        private readonly IImpresionServicio _impresionServicio;
        private readonly IReporteServicio _reporteServicio;
        private readonly IValoresDefectoReporteServicio _valoresDefectoReporteServicio;
        public BoletaVolumenesUNNAEnergiaCNPCServicio(
            IBoletaCnpcRepositorio boletaCnpcRepositorio,
            IImpresionServicio impresionServicio,
            IReporteServicio reporteServicio,
            IValoresDefectoReporteServicio valoresDefectoReporteServicio
            )
        {
            _boletaCnpcRepositorio = boletaCnpcRepositorio;
            _impresionServicio = impresionServicio;
            _reporteServicio = reporteServicio;
            _valoresDefectoReporteServicio = valoresDefectoReporteServicio;
        }

        public async Task<OperacionDto<BoletaVolumenesUNNAEnergiaCNPCDto>> ObtenerAsync(long idUsuario)
        {
            DateTime fecha = FechasUtilitario.ObtenerDiaOperativo();
            var operacionGeneral = await _reporteServicio.ObtenerAsync((int)TiposReportes.BoletaMensualVolumenGna, idUsuario);
            if (!operacionGeneral.Completado)
            {
                return new OperacionDto<BoletaVolumenesUNNAEnergiaCNPCDto>(CodigosOperacionDto.NoExiste, operacionGeneral.Mensajes);
            }

            string nota = default(string);
            var operacionImpresion = await _impresionServicio.ObtenerAsync((int)TiposReportes.BoletaMensualVolumenGna, fecha);
            if (operacionImpresion != null && operacionImpresion.Completado && operacionImpresion.Resultado != null && !string.IsNullOrWhiteSpace(operacionImpresion.Resultado.Datos))
            {
                nota = operacionImpresion.Resultado?.Comentario;
                if (new DateTime(fecha.Year,fecha.Month,1) == new DateTime(operacionImpresion.Resultado.Fecha.Year, operacionImpresion.Resultado.Fecha.Month, 1))
                {
                    var rpta = JsonConvert.DeserializeObject<BoletaVolumenesUNNAEnergiaCNPCDto>(operacionImpresion.Resultado.Datos);
                    rpta.General = operacionGeneral.Resultado;
                    return new OperacionDto<BoletaVolumenesUNNAEnergiaCNPCDto>(rpta);
                }                
            }
            
            DateTime Inicio = new DateTime(fecha.Year, fecha.Month, 1);

            var entidades = await _boletaCnpcRepositorio.ListarPorFechaAsync(Inicio,fecha);

            var datos = entidades.Select(e => new BoletaVolumenesUNNAEnergiaCNPCDetDto
            {
                GnsMpc = e.GnsMpc,
                Id = e.Id,
                CgnBls = e.CgnBls,
                GasMpcd = e.GnsMpc,
                GcMpc = e.GcMpc,
                GlpBls = e.GlpBls,
                Fecha = e.Fecha
            }).ToList();

            double? gravedadEspecifica = await _valoresDefectoReporteServicio.ObtenerValorAsync(LlaveValoresDefecto.GravedadEspecificaKgGlp);

            if (string.IsNullOrWhiteSpace(nota))
            {
                nota = "GNA: Gas Natural Asociado, GNS: Gas Natural Seco, GC: Gas Combustible, \t\t\t\t\t\r\nGLP: Gas Licuado de Petróleo, CGN: Condensado de Gas Natural.";
            }
            var dto = new BoletaVolumenesUNNAEnergiaCNPCDto
            {
                DiaOperativo = fecha,
                Anio = fecha.Year.ToString(),
                Mes = FechasUtilitario.ObtenerNombreMes(fecha),
                TotalGnsMpc = datos.Sum(e=>e.GnsMpc),
                TotalCgnBls = datos.Sum(e => e.CgnBls),
                TotalGasMpcd = datos.Sum(e => e.GasMpcd),
                TotalGcMpc = datos.Sum(e => e.GcMpc),
                TotalGlpBls = datos.Sum(e => e.GlpBls),
                VolumenGna = datos,
                Nota = nota,
                GravedadEspacificoGlp = gravedadEspecifica,
                General = operacionGeneral.Resultado
            };

            return new OperacionDto<BoletaVolumenesUNNAEnergiaCNPCDto>(dto);
        }


        public async Task<OperacionDto<RespuestaSimpleDto<string>>> GuardarAsync(BoletaVolumenesUNNAEnergiaCNPCDto peticion)
        {
            var operacionValidacion = ValidacionUtilitario.ValidarModelo<RespuestaSimpleDto<string>>(peticion);
            if (!operacionValidacion.Completado)
            {
                return operacionValidacion;
            }

            DateTime fecha = FechasUtilitario.ObtenerDiaOperativo();
            peticion.General = null;
            var dto = new ImpresionDto()
            {
                IdConfiguracion = RijndaelUtilitario.EncryptRijndaelToUrl((int)TiposReportes.BoletaMensualVolumenGna),
                Fecha = fecha,
                IdUsuario = peticion.IdUsuario,
                Datos = JsonConvert.SerializeObject(peticion),
                Comentario = peticion.Nota
            };

            DateTime Desde = new DateTime(fecha.Year, fecha.Month, 1);
            DateTime Hasta = new DateTime(fecha.Year, fecha.Month, 1).AddMonths(1).AddDays(-1);
            if (peticion.VolumenGna != null && peticion.VolumenGna.Count > 0)
            {
                await _boletaCnpcRepositorio.EliminarPorFechaAsync(Desde, Hasta);
                foreach (var item in peticion.VolumenGna)
                {
                    if (!item.Fecha.HasValue)
                    {
                        continue;
                    }
                    
                    var boletaCnpc = new Data.Reporte.Entidades.BoletaCnpc
                    {
                        Fecha = item.Fecha.Value,
                        CgnBls = item.CgnBls,
                        GasMpcd = item.GasMpcd,
                        GcMpc = item.GcMpc,
                        GlpBls = item.GlpBls,
                        GnsMpc = item.GnsMpc,
                        Actualizado = DateTime.UtcNow
                    };
                    await _boletaCnpcRepositorio.InsertarAsync(boletaCnpc);
                }
            }
                   
            return await _impresionServicio.GuardarAsync(dto);

        }


    }
}
