using Autofac.Core.Registration;
using DocumentFormat.OpenXml.Office.CustomUI;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Propiedad.Enums;
using Unna.OperationalReport.Data.Propiedad.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Registro.Entidades;
using Unna.OperationalReport.Data.Registro.Enums;
using Unna.OperationalReport.Data.Reporte.Repositorios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaDeterminacionVolumenGna.Calculos.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaDeterminacionVolumenGna.Calculos.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaDeterminacionVolumenGna.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;

namespace Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaDeterminacionVolumenGna.Calculos.Servicios.Implementaciones
{
    public class CalculoServicio : ICalculoServicio
    {

        private readonly IFisicasRepositorio _fisicasRepositorio;
        private readonly IViewValoresIngresadosPorFechaRepositorio _viewValoresIngresadosPorFechaRepositorio;
        public CalculoServicio(
            IFisicasRepositorio fisicasRepositorio,
            IViewValoresIngresadosPorFechaRepositorio viewValoresIngresadosPorFechaRepositorio
            )
        {
            _fisicasRepositorio = fisicasRepositorio;
            _viewValoresIngresadosPorFechaRepositorio = viewValoresIngresadosPorFechaRepositorio;
        }

        public async Task<OperacionDto<CalculosLoteIvDto>> ObtenerPropiedadesFisicasAsync(DateTime diaOperativo)
        {
            // HOJA 1
            var gpa = await _fisicasRepositorio.ListarPropiedadesFisicasAsync(TiposPropiedadesFisicas.Gpa);
            var gpsa = await _fisicasRepositorio.ListarPropiedadesFisicasAsync(TiposPropiedadesFisicas.Gpsa);

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
            var dtoCalidad = await ObtenerCantidadCalidadAsync();
            dto.CantidadCalidad = dtoCalidad.Resultado;
            dto.ComponsicionGnaEntrada = new List<ComponsicionGnaEntradaDto>
{
                new ComponsicionGnaEntradaDto { Componente = "Metano", Mol = 80.5 },
                new ComponsicionGnaEntradaDto { Componente = "Etano", Mol = 10.3 },
                new ComponsicionGnaEntradaDto { Componente = "Propano", Mol = 5.2 },
                new ComponsicionGnaEntradaDto { Componente = "Butano", Mol = 2.0 },
                new ComponsicionGnaEntradaDto { Componente = "Pentano", Mol = 1.0 },
                new ComponsicionGnaEntradaDto { Componente = "Hexano", Mol = 0.5 }
            };

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
                GasCombustible = gasCombustible != null ? gasCombustible.Volumen ?? 0 : 0,
            };

            return new OperacionDto<CantidadCalidadDto>(dto);

        }

        //2_Factor
        private async Task<DeterminacionFactorConvertirVolumenLgnDto> DeterminacionFactorConvertirVolumenLgnDtoAsync(DateTime diaOperativo)
        {
            var dto = new DeterminacionFactorConvertirVolumenLgnDto();

            // Inicializar la lista ComponentesComposicionGna con datos ficticios
            dto.ComponentesComposicionGna = new List<ComponentesComposicionGnaDto>
            {
                new ComponentesComposicionGnaDto
                {
                    Simbolo = "CH4",
                    Componente = "Metano",
                    Molar = 80.5,
                    GnaComponente = 75.0,
                    GnaVolumen = 100.0,
                    EficienciaRecuperacion = 95.0,
                    LiquidoVolumenBl = 85.0,
                    LiquidoVolumenPcsd = 90.0,
                    ProductoGlpBl = 10.0,
                    ProductoGlpVol = 12.0,
                    ProductoCgnBl = 5.0,
                    ProductoCgnVol = 6.0
                },
                new ComponentesComposicionGnaDto
                {
                    Simbolo = "C2H6",
                    Componente = "Etano",
                    Molar = 10.3,
                    GnaComponente = 8.0,
                    GnaVolumen = 15.0,
                    EficienciaRecuperacion = 90.0,
                    LiquidoVolumenBl = 12.0,
                    LiquidoVolumenPcsd = 14.0,
                    ProductoGlpBl = 2.0,
                    ProductoGlpVol = 3.0,
                    ProductoCgnBl = 1.0,
                    ProductoCgnVol = 1.5
                },
                new ComponentesComposicionGnaDto
                {
                    Simbolo = "C3H8",
                    Componente = "Propano",
                    Molar = 5.2,
                    GnaComponente = 4.5,
                    GnaVolumen = 7.0,
                    EficienciaRecuperacion = 85.0,
                    LiquidoVolumenBl = 6.0,
                    LiquidoVolumenPcsd = 7.0,
                    ProductoGlpBl = 1.0,
                    ProductoGlpVol = 1.2,
                    ProductoCgnBl = 0.5,
                    ProductoCgnVol = 0.6
                },
                new ComponentesComposicionGnaDto
                {
                    Simbolo = "C4H10",
                    Componente = "Butano",
                    Molar = 2.0,
                    GnaComponente = 1.8,
                    GnaVolumen = 3.0,
                    EficienciaRecuperacion = 80.0,
                    LiquidoVolumenBl = 2.5,
                    LiquidoVolumenPcsd = 3.0,
                    ProductoGlpBl = 0.5,
                    ProductoGlpVol = 0.6,
                    ProductoCgnBl = 0.2,
                    ProductoCgnVol = 0.3
                },
                new ComponentesComposicionGnaDto
                {
                    Simbolo = "C5H12",
                    Componente = "Pentano",
                    Molar = 1.0,
                    GnaComponente = 0.9,
                    GnaVolumen = 1.5,
                    EficienciaRecuperacion = 75.0,
                    LiquidoVolumenBl = 1.2,
                    LiquidoVolumenPcsd = 1.4,
                    ProductoGlpBl = 0.2,
                    ProductoGlpVol = 0.3,
                    ProductoCgnBl = 0.1,
                    ProductoCgnVol = 0.15
                },
                new ComponentesComposicionGnaDto
                {
                    Simbolo = "C6H14",
                    Componente = "Hexano",
                    Molar = 0.5,
                    GnaComponente = 0.4,
                    GnaVolumen = 0.8,
                    EficienciaRecuperacion = 70.0,
                    LiquidoVolumenBl = 0.6,
                    LiquidoVolumenPcsd = 0.7,
                    ProductoGlpBl = 0.1,
                    ProductoGlpVol = 0.15,
                    ProductoCgnBl = 0.05,
                    ProductoCgnVol = 0.1
                }
            };

            return dto;
        }


    }
}
