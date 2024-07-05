using Autofac.Core.Registration;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Office.CustomUI;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Propiedad.Enums;
using Unna.OperationalReport.Data.Propiedad.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Registro.Entidades;
using Unna.OperationalReport.Data.Registro.Enums;
using Unna.OperationalReport.Data.Registro.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Reporte.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Reporte.Repositorios.Implementaciones;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaDeterminacionVolumenGna.Calculos.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaDeterminacionVolumenGna.Calculos.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaDeterminacionVolumenGna.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaDeterminacionVolumenGna.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.FiscalizacionProductos.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ComposicionGnaLIV.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;
using Unna.OperationalReport.Tools.WebComunes.ApiWeb.Auth.Atributos;
using Unna.OperationalReport.Tools.WebComunes.WebSite.Base;

namespace Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaDeterminacionVolumenGna.Calculos.Servicios.Implementaciones
{
    public class CalculoServicio : ICalculoServicio 
    {

        private readonly IFisicasRepositorio _fisicasRepositorio;
        private readonly IViewValoresIngresadosPorFechaRepositorio _viewValoresIngresadosPorFechaRepositorio;
        private readonly IComposicionUnnaEnergiaPromedioRepositorio _composicionUnnaEnergiaPromedioRepositorio;
        private readonly IBoletaDeterminacionVolumenGnaServicio _boletaDeterminacionVolumenGnaServicio;
        private readonly IBoletaDiariaFiscalizacionRepositorio _boletaDiariaFiscalizacionRepositorio;
        private readonly IFiscalizacionProductoProduccionRepositorio _fiscalizacionProductoProduccionRepositorio;
        
        public CalculoServicio(
            IFisicasRepositorio fisicasRepositorio,
            IViewValoresIngresadosPorFechaRepositorio viewValoresIngresadosPorFechaRepositorio,
            IComposicionUnnaEnergiaPromedioRepositorio composicionUnnaEnergiaPromedioRepositorio,
            IBoletaDeterminacionVolumenGnaServicio boletaDeterminacionVolumenGnaServicio,
            IBoletaDiariaFiscalizacionRepositorio boletaDiariaFiscalizacionRepositorio,
            IFiscalizacionProductoProduccionRepositorio fiscalizacionProductoProduccionRepositorio


            )
        {
            _fisicasRepositorio = fisicasRepositorio;
            _viewValoresIngresadosPorFechaRepositorio = viewValoresIngresadosPorFechaRepositorio;
            _composicionUnnaEnergiaPromedioRepositorio = composicionUnnaEnergiaPromedioRepositorio;
            _boletaDeterminacionVolumenGnaServicio = boletaDeterminacionVolumenGnaServicio;
            _boletaDiariaFiscalizacionRepositorio = boletaDiariaFiscalizacionRepositorio;
            _fiscalizacionProductoProduccionRepositorio = fiscalizacionProductoProduccionRepositorio;
        }

        public async Task<OperacionDto<CalculosLoteIvDto>> ObtenerPropiedadesFisicasAsync(DateTime diaOperativo)
        {
            // HOJA 1
            var gpa = await _fisicasRepositorio.ListarPropiedadesFisicasAsync(TiposPropiedadesFisicas.Gpa, diaOperativo);
            var gpsa = await _fisicasRepositorio.ListarPropiedadesFisicasAsync(TiposPropiedadesFisicas.Gpsa, diaOperativo);
           
            var entidadGpa = gpa.Select(e =>
             new PropiedadesFisicasDto
             {
                 Id = e.Id,
                 Componente = e.Componente,
                 DensidadLiquido = e.DensidadLiquido,
                 PesoMolecular = e.PesoMolecular,
                 PoderCalorifico = e.PoderCalorifico,
                 RelacionVolumen = e.RelacionVolumen
             }).ToList();

            var entidadGpsa = gpsa.Select(e =>
             new PropiedadesFisicasDto
             {
                 Id = e.Id,
                 Componente = e.Componente,
                 DensidadLiquido = e.DensidadLiquido,
                 PesoMolecular = e.PesoMolecular,
                 PoderCalorifico = e.PoderCalorifico,
                 RelacionVolumen = e.RelacionVolumen
             }).ToList();           
           
            var dto = new CalculosLoteIvDto
            {
                PropiedadesGpa = entidadGpa,
                PropiedadesGpsa = entidadGpsa
                
            };

            //2_Factor
            dto.DeterminacionFactorConvertirVolumenLgn = await DeterminacionFactorConvertirVolumenLgnDtoAsync(diaOperativo);
            var dfcvlgn = dto.DeterminacionFactorConvertirVolumenLgn.ComponentesComposicionGna;
            var dtoCalidad = await ObtenerCantidadCalidadAsync();
            dto.CantidadCalidad = dtoCalidad.Resultado;
            List<ComponsicionGnaEntradaDto> ComponsicionGnaEntrada = new List<ComponsicionGnaEntradaDto>();
            List<PropiedadesFisicasDto> PropiedadesCompVolLGNGpa = new List<PropiedadesFisicasDto>();
            List<PropiedadesFisicasDto> PropiedadesCompVolCGNGpa = new List<PropiedadesFisicasDto>();
            List<PropiedadesFisicasDto> PropiedadesCompVolGLPGpa = new List<PropiedadesFisicasDto>();
            for (int i = 0; i < dfcvlgn.Count; i++)
            {
                ComponsicionGnaEntrada.Add(new ComponsicionGnaEntradaDto
                {

                    Componente = dfcvlgn[i].Componente, 
                    Mol = dfcvlgn[i].Molar 
                
                });
            
                PropiedadesCompVolLGNGpa.Add(new PropiedadesFisicasDto
                {

                    Componente = dfcvlgn[i].Componente,
                    PoderCalorificoBruto = gpa[i].PoderCalorificoBruto,
                    ComposicionVolumetricaLGN = dfcvlgn[i].LiquidoVolumenBl ,
                    PoderCalorificoLGN = dfcvlgn[i].LiquidoVolumenBl * gpa[i].PoderCalorificoBruto
                });

                PropiedadesCompVolCGNGpa.Add(new PropiedadesFisicasDto
                {

                    Componente = dfcvlgn[i].Componente,
                    PoderCalorificoBruto = gpa[i].PoderCalorificoBruto,
                    ComposicionVolumetricaCGN = dfcvlgn[i].ProductoCgnVol,
                    PoderCalorificoCGN = dfcvlgn[i].ProductoCgnVol * gpa[i].PoderCalorificoBruto

                });

                PropiedadesCompVolGLPGpa.Add(new PropiedadesFisicasDto
                {

                    Componente = dfcvlgn[i].Componente,
                    PoderCalorificoBruto = gpa[i].PoderCalorificoBruto,
                    ComposicionVolumetricaGLP = dfcvlgn[i].ProductoGlpVol,
                    PoderCalorificoGLP = dfcvlgn[i].ProductoGlpVol * gpa[i].PoderCalorificoBruto

                });
               
            }
            dto.ComponsicionGnaEntrada = ComponsicionGnaEntrada;
            dto.PropiedadesCompVolLGNGpa = PropiedadesCompVolLGNGpa;
            dto.PropiedadesCompVolCGNGpa = PropiedadesCompVolCGNGpa;
            dto.PropiedadesCompVolGLPGpa = PropiedadesCompVolGLPGpa.Where(e=> e.Componente == "Methane" || e.Componente == "Ethane" || e.Componente == "Propane" || e.Componente == "i-Butane" || e.Componente == "n-Butane" || e.Componente == "C5+").ToList();
            return new OperacionDto<CalculosLoteIvDto>(dto);

        }


        //2_Cantidad y Calidad
        public async Task<OperacionDto<CantidadCalidadDto>> ObtenerCantidadCalidadAsync()
        {

            DateTime diaOperativo = FechasUtilitario.ObtenerDiaOperativo();

            var valores = await _fisicasRepositorio.ListarCantidadCalidadVolumenGnaLoteIvAsync(diaOperativo);
            var gnaEntradaPlantaParinias = valores.Select(e =>
             new GnaEntradaPlantaPariniasDto
             {
                 Corriente = e.Corriente,
                 Medidor = e.Medidor,
                 VolumenFiscalizado = e.Volumen,
                 PoderCalorifico = e.Calorifico,
                 Riqueza = e.Riqueza
             }).ToList();

            var volumenGasNaturalPorTipoLoteIv = await _fisicasRepositorio.ListarVolumenGasNaturalPorTipoLoteIvAsync(diaOperativo);

            var gasCombustible = volumenGasNaturalPorTipoLoteIv.Where(e => e.Tipo.Equals("I")).FirstOrDefault();
            var glp = volumenGasNaturalPorTipoLoteIv.Where(e => e.Tipo.Equals("II")).FirstOrDefault();
            var c5 = volumenGasNaturalPorTipoLoteIv.Where(e => e.Tipo.Equals("III")).FirstOrDefault();
            var volumenGasVendidoPorCliente = volumenGasNaturalPorTipoLoteIv.Where(e => e.Tipo.Equals("IV")).ToList().Select(e =>
            new VolumenGasNaturalVendidoPorClienteDto
            {
                Cliente = e.Nombre,
                Volumen = e.Volumen ?? 0
            }).ToList();

            var dto = new CantidadCalidadDto
            {
                Fecha = diaOperativo.ToString("dd/MM/yyyy"),
                GnaEntradaPlantaParinias = gnaEntradaPlantaParinias,
                VolumenGasVendidoPorCliente = volumenGasVendidoPorCliente,
                Glp = glp != null ? glp.Volumen ?? 0 : 0,
                C5 = c5 != null ? c5.Volumen ?? 0 : 0,
                GasCombustible = gasCombustible != null ? gasCombustible.Volumen ?? 0 : 0
            };

            return new OperacionDto<CantidadCalidadDto>(dto);

        }

        //2_Factor
        private async Task<DeterminacionFactorConvertirVolumenLgnDto> DeterminacionFactorConvertirVolumenLgnDtoAsync(DateTime diaOperativo)
        {
           
            var componentes = await _composicionUnnaEnergiaPromedioRepositorio.ObtenerComposicionUnnaEnergiaPromedioDiario(diaOperativo);
            var gpa = await _fisicasRepositorio.ListarPropiedadesFisicasAsync(TiposPropiedadesFisicas.Gpa, diaOperativo);
            var gpacomp = gpa.OrderBy(e =>e.Id).ToList();
            var valores = await _fisicasRepositorio.ListarCantidadCalidadVolumenGnaLoteIvAsync(diaOperativo);
            var gnavolumenentrada = valores.Where(e => e.IdLote == 4).ToList();
            

            var entidades = await _boletaDiariaFiscalizacionRepositorio.ListarRegistroPorDiaOperativoFactorAsignacionAsync(diaOperativo, (int)TiposDatos.VolumenMpcd, (int)TiposDatos.Riqueza, (int)TiposDatos.PoderCalorifico);
            var lista = entidades.Select(e => new FactorAsignacionLiquidosGasNaturalDto()
            {
                Item = e.Item,
                Suministrador = e.Suministrador,
                Volumen = e.Volumen,
                Riqueza = e.Riqueza,
                Contenido = e.Volumen * e.Riqueza
            }).ToList();

            double totalContenido = lista.Sum(e => e.Contenido);
            
            var productoGlpCgn = new List<FiscalizacionProductoGlpCgnDto>();
            var fiscalizacionGlpCgn = await _fiscalizacionProductoProduccionRepositorio.FiscalizacionProductosGlpCgnAsycn(diaOperativo);


            productoGlpCgn = fiscalizacionGlpCgn.Select(e => new FiscalizacionProductoGlpCgnDto
            {
                Producto = e.Producto,
                Produccion = e.Produccion,
                Despacho = e.Despacho
            }).ToList();

            productoGlpCgn.Add(new FiscalizacionProductoGlpCgnDto
            {
                Producto = "TOTAL",
                Produccion = Math.Round(productoGlpCgn.Sum(e => e.Produccion ?? 0), 2),
                Despacho = Math.Round(productoGlpCgn.Sum(e => e.Despacho ?? 0), 2),
                Inventario = Math.Round(productoGlpCgn.Sum(e => e.Inventario ?? 0), 2)
            });

                     
             var dto = new DeterminacionFactorConvertirVolumenLgnDto();
            List<ComponentesComposicionGnaDto> ComponentesComposicionGna = new List<ComponentesComposicionGnaDto>();
            for (int i = 0; i < componentes.Count; i++)
            {
                
                ComponentesComposicionGna.Add(new ComponentesComposicionGnaDto
                {

                    Simbolo = componentes[i].Simbolo,
                    Componente = componentes[i].Suministrador,
                    Molar = componentes[i].PromedioComponente,
                    GnaComponente = componentes[i].PromedioComponente / gpacomp[i].RelacionVolumen * 10,
                    GnaVolumen = ((componentes[i].PromedioComponente / gpacomp[i].RelacionVolumen * 10) * gnavolumenentrada[0].Volumen) / 42,
                    EficienciaRecuperacion = productoGlpCgn[2].Produccion / (totalContenido / 42) *  100,
                    LiquidoVolumenBl = (((componentes[i].PromedioComponente / gpacomp[i].RelacionVolumen * 10) * gnavolumenentrada[0].Volumen) / 42) * (productoGlpCgn[2].Produccion / (totalContenido / 42) * 100) / 100,
                    LiquidoVolumenPcsd = ((((componentes[i].PromedioComponente / gpacomp[i].RelacionVolumen * 10) * gnavolumenentrada[0].Volumen) / 42) * (productoGlpCgn[2].Produccion / (totalContenido / 42) * 100) / 100) * gpacomp[i].RelacionVolumen * 42 / 1000,
                    ProductoGlpBl = (((componentes[i].PromedioComponente / gpacomp[i].RelacionVolumen * 10) * gnavolumenentrada[0].Volumen) / 42) * (productoGlpCgn[2].Produccion / (totalContenido / 42) * 100) / 100,
                    ProductoGlpVol = ((((componentes[i].PromedioComponente / gpacomp[i].RelacionVolumen * 10) * gnavolumenentrada[0].Volumen) / 42) * (productoGlpCgn[2].Produccion / (totalContenido / 42) * 100) / 100),
                    ProductoCgnBl = (((componentes[i].PromedioComponente / gpacomp[i].RelacionVolumen * 10) * gnavolumenentrada[0].Volumen) / 42) * (productoGlpCgn[2].Produccion / (totalContenido / 42) * 100) / 100,
                    ProductoCgnVol = ((((componentes[i].PromedioComponente / gpacomp[i].RelacionVolumen * 10) * gnavolumenentrada[0].Volumen) / 42) * (productoGlpCgn[2].Produccion / (totalContenido / 42) * 100) / 100)

                });
            }
            var totalProductoGlpBl = ComponentesComposicionGna.Sum(e => e.ProductoGlpBl);
            var totalProductoCgnBl = ComponentesComposicionGna.Sum(e => e.ProductoCgnBl);
            List<ComponentesComposicionGnaDto> ComponentesComposicionGna2 = new List<ComponentesComposicionGnaDto>();

            for (int i = 0; i < ComponentesComposicionGna.Count; i++)
            {
                ComponentesComposicionGna2.Add( new ComponentesComposicionGnaDto
                {
                    Simbolo = ComponentesComposicionGna[i].Simbolo,
                    Componente = ComponentesComposicionGna[i].Componente,
                    Molar = ComponentesComposicionGna[i].Molar,
                    GnaComponente = ComponentesComposicionGna[i].GnaComponente,
                    GnaVolumen = ComponentesComposicionGna[i].GnaVolumen,
                    EficienciaRecuperacion = ComponentesComposicionGna[i].EficienciaRecuperacion,
                    LiquidoVolumenBl = ComponentesComposicionGna[i].LiquidoVolumenBl,
                    LiquidoVolumenPcsd = ComponentesComposicionGna[i].LiquidoVolumenPcsd,
                    ProductoGlpBl = ComponentesComposicionGna[i].ProductoGlpBl,
                    ProductoGlpVol = ComponentesComposicionGna[i].ProductoGlpVol / totalProductoGlpBl * 100,
                    ProductoCgnBl = ComponentesComposicionGna[i].ProductoCgnBl,
                    ProductoCgnVol = ComponentesComposicionGna[i].ProductoCgnVol / totalProductoCgnBl * 100


                });
            }
            dto.ComponentesComposicionGna = ComponentesComposicionGna2;
                  
            return dto;
        }


    }
}
