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

        public async Task<OperacionDto<CalculosLoteIvDto>> ObtenerPropiedadesFisicasAsync()
        {
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

    }
}
