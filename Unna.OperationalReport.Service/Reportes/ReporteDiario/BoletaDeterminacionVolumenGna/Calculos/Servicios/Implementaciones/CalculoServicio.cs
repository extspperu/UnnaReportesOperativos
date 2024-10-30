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
            //var gpa = await _fisicasRepositorio.ListarPropiedadesFisicasAsync(TiposPropiedadesFisicas.Gpa, diaOperativo);
            //var gpsa = await _fisicasRepositorio.ListarPropiedadesFisicasAsync(TiposPropiedadesFisicas.Gpsa, diaOperativo);


            var gpa = await _fisicasRepositorio.FisicasPorGrupoAsync(TiposPropiedadesFisicas.Gpa);
            var gpsa = await _fisicasRepositorio.FisicasPorGrupoAsync(TiposPropiedadesFisicas.Gpsa);

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


            var suministradorComponente = await _fisicasRepositorio.ListarSuministradorComponenteAsync((int)TiposLote.LoteIv, diaOperativo);
            var componentes = suministradorComponente?.Select(e => new ComponsicionGnaEntradaDto
            {
                Componente = e.Suministrador,
                Mol = e.Porcentaje
            }).ToList();



            var dto = new CalculosLoteIvDto
            {
                PropiedadesGpa = entidadGpa,
                PropiedadesGpsa = entidadGpsa,
                ComponsicionGnaEntrada = componentes,
                CantidadCalidad = await ObtenerCantidadCalidadAsync(),
                DeterminacionFactorConvertirVolumenLgn = await ObtenerFactorAsync()
            };

            //2_Factor
            dto.DeterminacionFactorConvertirVolumenLgn = await DeterminacionFactorConvertirVolumenLgnDtoAsync(diaOperativo);
            var dfcvlgn = dto.DeterminacionFactorConvertirVolumenLgn.ComponentesComposicionGna;

            List<ComponsicionGnaEntradaDto> ComponsicionGnaEntrada = new List<ComponsicionGnaEntradaDto>();
            List<PropiedadesFisicasDto> PropiedadesCompVolLGNGpa = new List<PropiedadesFisicasDto>();
            List<PropiedadesFisicasDto> PropiedadesCompVolCGNGpa = new List<PropiedadesFisicasDto>();
            List<PropiedadesFisicasDto> PropiedadesCompVolGLPGpa = new List<PropiedadesFisicasDto>();
            //for (int i = 0; i < dfcvlgn.Count; i++)
            //{
            //    ComponsicionGnaEntrada.Add(new ComponsicionGnaEntradaDto
            //    {

            //        Componente = dfcvlgn[i].Componente, 
            //        Mol = dfcvlgn[i].Molar 

            //    });
            //    var totalLiqVolBl = dfcvlgn.Sum(e => e.LiquidoVolumenBl); 

            //    PropiedadesCompVolLGNGpa.Add(new PropiedadesFisicasDto
            //    {

            //        Componente = dfcvlgn[i].Componente,
            //        PoderCalorificoBruto = gpa[i].PoderCalorificoBruto,
            //        ComposicionVolumetricaLGN = dfcvlgn[i].LiquidoVolumenBl / totalLiqVolBl,
            //        PoderCalorificoLGN = (dfcvlgn[i].LiquidoVolumenBl / totalLiqVolBl) * gpa[i].PoderCalorificoBruto
            //    });

            //    PropiedadesCompVolCGNGpa.Add(new PropiedadesFisicasDto
            //    {

            //        Componente = dfcvlgn[i].Componente,
            //        PoderCalorificoBruto = gpa[i].PoderCalorificoBruto,
            //        ComposicionVolumetricaCGN = dfcvlgn[i].ProductoCgnVol / 100,
            //        PoderCalorificoCGN = (dfcvlgn[i].ProductoCgnVol / 100) * gpa[i].PoderCalorificoBruto

            //    });

            //    PropiedadesCompVolGLPGpa.Add(new PropiedadesFisicasDto
            //    {

            //        Componente = dfcvlgn[i].Componente,
            //        PoderCalorificoBruto = gpa[i].PoderCalorificoBruto,
            //        ComposicionVolumetricaGLP = dfcvlgn[i].ProductoGlpVol / 100,
            //        PoderCalorificoGLP = (dfcvlgn[i].ProductoGlpVol / 100) * gpa[i].PoderCalorificoBruto

            //    });

            //}

            dto.PropiedadesCompVolLGNGpa = PropiedadesCompVolLGNGpa;
            dto.PropiedadesCompVolCGNGpa = PropiedadesCompVolCGNGpa;
            dto.PropiedadesCompVolGLPGpa = PropiedadesCompVolGLPGpa.Where(e => e.Componente == "Methane" || e.Componente == "Ethane" || e.Componente == "Propane" || e.Componente == "i-Butane" || e.Componente == "n-Butane" || e.Componente == "C5+").ToList();
            return new OperacionDto<CalculosLoteIvDto>(dto);

        }


        //2_Cantidad y Calidad
        private async Task<CantidadCalidadDto> ObtenerCantidadCalidadAsync()
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
            return dto;
        }

        //3_Factor
        private async Task<DeterminacionFactorConvertirVolumenLgnDto> ObtenerFactorAsync()
        {

            DateTime diaOperativo = FechasUtilitario.ObtenerDiaOperativo();

            var valores = await _fisicasRepositorio.ListarFactorLoteIvAsync(diaOperativo);
            var componentesComposicionGna = valores.Select(e =>
             new ComponentesComposicionGnaDto
             {
                 Componente = e.Componente,
                 EficienciaRecuperacion = e.EficienciaRecuperacion,
                 GnaComponente = e.GnaComponente,
                 GnaVolumen = e.GnaVolumen,
                 LiquidoVolumenBl = e.LiquidoVolumenBl,
                 LiquidoVolumenPcsd = e.LiquidoVolumenPcsd,
                 ProductoCgnBl = e.ProductoCgnBl,
                 Molar = e.Molar,
                 ProductoCgnVol = e.ProductoCgnVol,
                 ProductoGlpBl = e.ProductoGlpBl,
                 ProductoGlpVol = e.ProductoGlpVol,
                 Simbolo = e.Simbolo,
             }).ToList();

            var dto = new DeterminacionFactorConvertirVolumenLgnDto
            {
                ComponentesComposicionGna = componentesComposicionGna,
                TotalMolar = componentesComposicionGna.Sum(e => e.Molar ?? 0),
                TotalProductoCgnVol = componentesComposicionGna.Sum(e => e.ProductoCgnVol ?? 0),
                TotalVolumenBl = componentesComposicionGna.Sum(e => e.GnaVolumen ?? 0),
                TotalLiquidoVolumenBl = componentesComposicionGna.Sum(e => e.LiquidoVolumenBl ?? 0),
                TotalLiquidoVolumenPcsd = componentesComposicionGna.Sum(e => e.LiquidoVolumenPcsd ?? 0),
                TotalProductoGlpBl = componentesComposicionGna.Sum(e => e.ProductoGlpBl ?? 0),
                TotalProductoGlpVol = componentesComposicionGna.Sum(e => e.ProductoGlpVol ?? 0)
            };
            return dto;
        }


        //2_Factor
        private async Task<DeterminacionFactorConvertirVolumenLgnDto> DeterminacionFactorConvertirVolumenLgnDtoAsync(DateTime diaOperativo)
        {

            var componentes = await _composicionUnnaEnergiaPromedioRepositorio.ObtenerComposicionUnnaEnergiaPromedioDiario(diaOperativo);
            var gpa = await _fisicasRepositorio.ListarPropiedadesFisicasAsync(TiposPropiedadesFisicas.Gpa, diaOperativo);
            var gpacomp = gpa.OrderBy(e => e.Id).ToList();
            var valores = await _fisicasRepositorio.ListarCantidadCalidadVolumenGnaLoteIvAsync(diaOperativo);
            var gnavolumenentrada = valores.Where(e => e.IdLote == 4).ToList();


            var entidades = await _boletaDiariaFiscalizacionRepositorio.ListarRegistroPorDiaOperativoFactorAsignacionAsync(diaOperativo, (int)TiposDatos.VolumenMpcd, (int)TiposDatos.Riqueza, (int)TiposDatos.PoderCalorifico, false);
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
            if (gnavolumenentrada.Count > 0)
            {
                dto.VolumenGasEntrada = gnavolumenentrada[0].Volumen;

            }
            List<ComponentesComposicionGnaDto> ComponentesComposicionGna = new List<ComponentesComposicionGnaDto>();

            double produccion = productoGlpCgn.Count > 2 ? productoGlpCgn[2].Produccion ?? 0 : 0;
            double gnavolumenentradaVolumen = gnavolumenentrada.Count > 0 ? gnavolumenentrada[0].Volumen : 0;

            if (componentes != null)
            {
                for (int i = 0; i < componentes?.Count; i++)
                {
                    if (componentes == null || componentes.Count == 0 || componentes[i] == null)
                    {
                        continue;
                    }
                    var a = new ComponentesComposicionGnaDto();
                    a.Simbolo = componentes?[i].Simbolo;
                    a.Componente = componentes?[i].Suministrador;
                    a.Molar = componentes[i].PromedioComponente;
                    a.EficienciaRecuperacion = produccion / (totalContenido / 42) * 100;
                    if (gpacomp.Count > i)
                    {
                        a.GnaComponente = componentes[i].PromedioComponente / gpacomp[i].RelacionVolumen * 10;
                        a.GnaVolumen = ((componentes[i].PromedioComponente / gpacomp[i].RelacionVolumen * 10) * gnavolumenentradaVolumen) / 42;
                        a.LiquidoVolumenBl = (((componentes[i].PromedioComponente / gpacomp[i].RelacionVolumen * 10) * gnavolumenentradaVolumen) / 42) * (produccion / (totalContenido / 42) * 100) / 100;
                        a.LiquidoVolumenPcsd = ((((componentes[i].PromedioComponente / gpacomp[i].RelacionVolumen * 10) * gnavolumenentradaVolumen) / 42) * (produccion / (totalContenido / 42) * 100) / 100) * gpacomp[i].RelacionVolumen * 42 / 1000;
                        a.ProductoGlpBl = (((componentes[i].PromedioComponente / gpacomp[i].RelacionVolumen * 10) * gnavolumenentradaVolumen) / 42) * (produccion / (totalContenido / 42) * 100) / 100;
                        a.ProductoGlpVol = ((((componentes[i].PromedioComponente / gpacomp[i].RelacionVolumen * 10) * gnavolumenentradaVolumen) / 42) * (produccion / (totalContenido / 42) * 100) / 100);
                        a.ProductoCgnBl = (((componentes[i].PromedioComponente / gpacomp[i].RelacionVolumen * 10) * gnavolumenentradaVolumen) / 42) * (produccion / (totalContenido / 42) * 100) / 100;
                        a.ProductoCgnVol = ((((componentes[i].PromedioComponente / gpacomp[i].RelacionVolumen * 10) * gnavolumenentradaVolumen) / 42) * (produccion / (totalContenido / 42) * 100) / 100);

                    }

                    ComponentesComposicionGna.Add(a);
                }
            }

            List<ComponentesComposicionGnaDto> ComponentesComposicionGna2 = new List<ComponentesComposicionGnaDto>();
            List<ComponentesComposicionGnaDto> ComponentesComposicionGna3 = new List<ComponentesComposicionGnaDto>();
            double eficiencia = 0;
            double liqvolBl = 0;
            double liqvolPcsd = 0;
            double prodglpBl = 0;
            double prodglpVol = 0;
            double prodcgnBl = 0;
            double prodcgnVol = 0;
            for (int i = 0; i < ComponentesComposicionGna.Count; i++)
            {
                if (ComponentesComposicionGna[i].Componente == "Propane" || ComponentesComposicionGna[i].Componente == "i-Butane" || ComponentesComposicionGna[i].Componente == "n-Butane" || ComponentesComposicionGna[i].Componente == "i-Pentane" || ComponentesComposicionGna[i].Componente == "n-Pentane" || ComponentesComposicionGna[i].Componente == "NeoPentane" || ComponentesComposicionGna[i].Componente == "Hexanes")
                {
                    eficiencia = ComponentesComposicionGna[i].EficienciaRecuperacion.HasValue ? ComponentesComposicionGna[i].EficienciaRecuperacion.Value : 0;
                }
                else
                {
                    eficiencia = 0;
                }

                if (ComponentesComposicionGna[i].Componente == "i-Butane" || ComponentesComposicionGna[i].Componente == "n-Butane" || ComponentesComposicionGna[i].Componente == "i-Pentane" || ComponentesComposicionGna[i].Componente == "n-Pentane" || ComponentesComposicionGna[i].Componente == "NeoPentane" || ComponentesComposicionGna[i].Componente == "Hexanes")
                {
                    liqvolBl = ComponentesComposicionGna[i].LiquidoVolumenBl.HasValue ? ComponentesComposicionGna[i].LiquidoVolumenBl.Value : 0;
                }
                else
                {
                    liqvolBl = 0;
                }

                if (ComponentesComposicionGna[i].Componente == "i-Butane" || ComponentesComposicionGna[i].Componente == "n-Butane" || ComponentesComposicionGna[i].Componente == "i-Pentane" || ComponentesComposicionGna[i].Componente == "n-Pentane" || ComponentesComposicionGna[i].Componente == "NeoPentane" || ComponentesComposicionGna[i].Componente == "Hexanes")
                {
                    liqvolPcsd = ComponentesComposicionGna[i].LiquidoVolumenPcsd.HasValue ? ComponentesComposicionGna[i].LiquidoVolumenPcsd.Value : 0;
                }
                else
                {
                    liqvolPcsd = 0;
                }

                if (ComponentesComposicionGna[i].Componente == "i-Butane" || ComponentesComposicionGna[i].Componente == "n-Butane")
                {
                    prodglpBl = ComponentesComposicionGna[i].ProductoGlpBl.HasValue ? ComponentesComposicionGna[i].ProductoGlpBl.Value : 0;
                    prodglpVol = ComponentesComposicionGna[i].ProductoGlpVol.HasValue ? ComponentesComposicionGna[i].ProductoGlpVol.Value : 0;
                }
                else
                {
                    prodglpBl = 0;
                    prodglpVol = 0;
                }

                if (ComponentesComposicionGna[i].Componente == "i-Pentane" || ComponentesComposicionGna[i].Componente == "n-Pentane" || ComponentesComposicionGna[i].Componente == "NeoPentane" || ComponentesComposicionGna[i].Componente == "Hexanes")
                {
                    prodcgnBl = ComponentesComposicionGna[i].ProductoCgnBl.HasValue ? ComponentesComposicionGna[i].ProductoCgnBl.Value : 0;
                    prodcgnVol = ComponentesComposicionGna[i].ProductoCgnVol.HasValue ? ComponentesComposicionGna[i].ProductoCgnVol.Value : 0;
                }
                else
                {
                    prodcgnBl = 0;
                    prodcgnVol = 0;
                }

                ComponentesComposicionGna2.Add(new ComponentesComposicionGnaDto
                {
                    Simbolo = ComponentesComposicionGna[i].Simbolo,
                    Componente = ComponentesComposicionGna[i].Componente,
                    Molar = ComponentesComposicionGna[i].Molar,
                    GnaComponente = ComponentesComposicionGna[i].GnaComponente,
                    GnaVolumen = ComponentesComposicionGna[i].GnaVolumen,
                    EficienciaRecuperacion = eficiencia,//ComponentesComposicionGna[i].EficienciaRecuperacion,
                    LiquidoVolumenBl = liqvolBl,//ComponentesComposicionGna[i].LiquidoVolumenBl,
                    LiquidoVolumenPcsd = liqvolPcsd,//ComponentesComposicionGna[i].LiquidoVolumenPcsd,
                    ProductoGlpBl = prodglpBl,//ComponentesComposicionGna[i].ProductoGlpBl,
                    ProductoGlpVol = prodglpVol, //ComponentesComposicionGna[i].ProductoGlpVol / totalProductoGlpBl * 100,
                    ProductoCgnBl = prodcgnBl,//ComponentesComposicionGna[i].ProductoCgnBl,
                    ProductoCgnVol = prodcgnVol//ComponentesComposicionGna[i].ProductoCgnVol / totalProductoCgnBl * 100


                });


            }

            var totalProductoGlpBl = ComponentesComposicionGna2.Sum(e => e.ProductoGlpBl);
            var totalProductoCgnBl = ComponentesComposicionGna2.Sum(e => e.ProductoCgnBl);
            for (int i = 0; i < ComponentesComposicionGna2.Count; i++)
            {
                ComponentesComposicionGna3.Add(new ComponentesComposicionGnaDto
                {
                    Simbolo = ComponentesComposicionGna2[i].Simbolo,
                    Componente = ComponentesComposicionGna2[i].Componente,
                    Molar = ComponentesComposicionGna2[i].Molar,
                    GnaComponente = ComponentesComposicionGna2[i].GnaComponente,
                    GnaVolumen = ComponentesComposicionGna2[i].GnaVolumen,
                    EficienciaRecuperacion = ComponentesComposicionGna2[i].EficienciaRecuperacion,
                    LiquidoVolumenBl = ComponentesComposicionGna2[i].LiquidoVolumenBl,
                    LiquidoVolumenPcsd = ComponentesComposicionGna2[i].LiquidoVolumenPcsd,
                    ProductoGlpBl = ComponentesComposicionGna2[i].ProductoGlpBl,
                    ProductoGlpVol = ComponentesComposicionGna2[i].ProductoGlpVol / totalProductoGlpBl * 100,
                    ProductoCgnBl = ComponentesComposicionGna2[i].ProductoCgnBl,
                    ProductoCgnVol = ComponentesComposicionGna2[i].ProductoCgnVol / totalProductoCgnBl * 100


                });

            }
            dto.ComponentesComposicionGna = ComponentesComposicionGna3;

            return dto;
        }


    }
}
