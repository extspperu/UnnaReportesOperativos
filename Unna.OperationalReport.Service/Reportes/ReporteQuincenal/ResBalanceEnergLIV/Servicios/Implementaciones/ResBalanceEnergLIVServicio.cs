using Microsoft.IdentityModel.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Registro.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Registro.Repositorios.Implementaciones;
using Unna.OperationalReport.Data.Reporte.Enums;
using Unna.OperationalReport.Data.Reporte.Procedimientos;
using Unna.OperationalReport.Data.Reporte.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Reporte.Repositorios.Implementaciones;
using Unna.OperationalReport.Service.Reportes.Impresiones.Dtos;
using Unna.OperationalReport.Service.Reportes.Impresiones.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletaVolumenesUNNAEnergiaCNPC.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ComposicionGnaLIV.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ResBalanceEnergLgnLIV_2.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ResBalanceEnergLIV.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ResBalanceEnergLIV.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ValorizacionVtaGns.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;
using Unna.OperationalReport.Tools.Seguridad.Servicios.General.Dtos;

namespace Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ResBalanceEnergLIV.Servicios.Implementaciones
{
    public class ResBalanceEnergLIVServicio : IResBalanceEnergLIVServicio
    {
        private readonly IRegistroRepositorio _registroRepositorio;
        private readonly IImpresionServicio _impresionServicio;
        private readonly IImprimirRepositorio _imprimirRepositorio;

        public ResBalanceEnergLIVServicio(IRegistroRepositorio registroRepositorio, IImpresionServicio impresionServicio, IImprimirRepositorio imprimirRepositorio)
        {
            _registroRepositorio = registroRepositorio;
            _impresionServicio = impresionServicio;
            _imprimirRepositorio = imprimirRepositorio;
        }
        public class RootObject
        {
            public long IdUsuario { get; set; }
            public string Mes { get; set; }
            public string Anio { get; set; }
            public List<DiaMedicion> DatosDiarios { get; set; }
            public ParametrosLGN ParametrosLGN { get; set; }
        }

        public class DiaMedicion
        {
            public string Dia { get; set; }
            public List<Medicion> Mediciones { get; set; }
        }

        public class Medicion
        {
            public string ID { get; set; }
            public string Valor { get; set; }
        }

        public class ParametrosLGN
        {
            public double? DensidadGLPKgBl { get; set; }
            public double? PCGLPMMBtuBl60F { get; set; }
            public double? PCCGNMMBtuBl60F { get; set; }
            public double? PCLGNMMBtuBl60F { get; set; }
            public double? FactorConversionSCFDGal { get; set; }
            public double? EnergiaMMBTUQ1GLP { get; set; }
            public double? EnergiaMMBTUQ1CGN { get; set; }
            public double? DensidadGLPKgBlQ2 { get; set; }
            public double? PCGLPMMBtuBl60FQ2 { get; set; }
            public double? PCCGNMMBtuBl60FQ2 { get; set; }
            public double? PCLGNMMBtuBl60FQ2 { get; set; }
            public double? FactorConversionSCFDGalQ2 { get; set; }
            public double? EnergiaMMBTUQ2GLP { get; set; }
            public double? EnergiaMMBTUQ2CGN { get; set; }
        }


        public async Task<OperacionDto<ResBalanceEnergLIVDto>> ObtenerAsync(long idUsuario)
        {
            var imprimir = await _imprimirRepositorio.BuscarPorIdConfiguracionYFechaAsync(15, DateTime.UtcNow.Date);
            ResBalanceEnergLIVDto dto = null;

            if (imprimir is null)
            {
                var generalData = await _registroRepositorio.ObtenerMedicionesGasAsync();

                //var FechaActuala = _registroRepositorio.ObtenerFechaActualAsync();
                var cultureInfo = new CultureInfo("es-ES");
                //var fechaActual = DateTime.Now;
                var fechaActual = new DateTime(DateTime.Now.Year, 4, 30);
                string mesActual = fechaActual.ToString("MMMM", cultureInfo);
                string anioActual = fechaActual.Year.ToString();
                var primeraQuincena = generalData.Where(d => d.Dia >= 1 && d.Dia <= 15);
                var sumaPrimeraQuincena = new
                {
                    MedGasGasNatAsocMedVolumen = primeraQuincena.Sum(d => d.MedGasGasNatAsocMedVolumen),
                    MedGasGasNatAsocMedPoderCal = primeraQuincena.Sum(d => d.MedGasGasNatAsocMedPoderCal),
                    MedGasGasNatAsocMedEnergia = primeraQuincena.Sum(d => d.MedGasGasNatAsocMedEnergia),
                    MedGasGasCombSecoMedVolumen = primeraQuincena.Sum(d => d.MedGasGasCombSecoMedVolumen),
                    MedGasGasCombSecoMedPoderCal = primeraQuincena.Sum(d => d.MedGasGasCombSecoMedPoderCal),
                    MedGasGasCombSecoMedEnergia = primeraQuincena.Sum(d => d.MedGasGasCombSecoMedEnergia),
                    MedGasVolGasEquivLgnVolumen = primeraQuincena.Sum(d => d.MedGasVolGasEquivLgnVolumen),
                    MedGasVolGasEquivLgnPoderCal = primeraQuincena.Sum(d => d.MedGasVolGasEquivLgnPoderCal),
                    MedGasVolGasEquivLgnEnergia = primeraQuincena.Sum(d => d.MedGasVolGasEquivLgnEnergia),
                    MedGasVolGasClienteVolumen = primeraQuincena.Sum(d => d.MedGasVolGasClienteVolumen),
                    MedGasVolGasClientePoderCal = primeraQuincena.Sum(d => d.MedGasVolGasClientePoderCal),
                    MedGasVolGasClienteEnergia = primeraQuincena.Sum(d => d.MedGasVolGasClienteEnergia),
                    MedGasVolGasSaviaVolumen = primeraQuincena.Sum(d => d.MedGasVolGasSaviaVolumen),
                    MedGasVolGasSaviaPoderCal = primeraQuincena.Sum(d => d.MedGasVolGasSaviaPoderCal),
                    MedGasVolGasSaviaEnergia = primeraQuincena.Sum(d => d.MedGasVolGasSaviaEnergia),
                    MedGasVolGasLimaGasVolumen = primeraQuincena.Sum(d => d.MedGasVolGasLimaGasVolumen),
                    MedGasVolGasLimaGasPoderCal = primeraQuincena.Sum(d => d.MedGasVolGasLimaGasPoderCal),
                    MedGasVolGasLimaGasEnergia = primeraQuincena.Sum(d => d.MedGasVolGasLimaGasEnergia),
                    MedGasVolGasGasNorpVolumen = primeraQuincena.Sum(d => d.MedGasVolGasGasNorpVolumen),
                    MedGasVolGasGasNorpPoderCal = primeraQuincena.Sum(d => d.MedGasVolGasGasNorpPoderCal),
                    MedGasVolGasGasNorpEnergia = primeraQuincena.Sum(d => d.MedGasVolGasGasNorpEnergia),
                    MedGasVolGasQuemadoVolumen = primeraQuincena.Sum(d => d.MedGasVolGasQuemadoVolumen),
                    MedGasVolGasQuemadoPoderCal = primeraQuincena.Sum(d => d.MedGasVolGasQuemadoPoderCal),
                    MedGasVolGasQuemadoEnergia = primeraQuincena.Sum(d => d.MedGasVolGasQuemadoEnergia)
                };
                var segundaQuincena = generalData.Where(d => d.Dia >= 16 && d.Dia <= DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));
                var sumaSegundaQuincena = new
                {
                    MedGasGasNatAsocMedVolumen = segundaQuincena.Sum(d => d.MedGasGasNatAsocMedVolumen),
                    MedGasGasNatAsocMedPoderCal = segundaQuincena.Sum(d => d.MedGasGasNatAsocMedPoderCal),
                    MedGasGasNatAsocMedEnergia = segundaQuincena.Sum(d => d.MedGasGasNatAsocMedEnergia),
                    MedGasGasCombSecoMedVolumen = segundaQuincena.Sum(d => d.MedGasGasCombSecoMedVolumen),
                    MedGasGasCombSecoMedPoderCal = segundaQuincena.Sum(d => d.MedGasGasCombSecoMedPoderCal),
                    MedGasGasCombSecoMedEnergia = segundaQuincena.Sum(d => d.MedGasGasCombSecoMedEnergia),
                    MedGasVolGasEquivLgnVolumen = segundaQuincena.Sum(d => d.MedGasVolGasEquivLgnVolumen),
                    MedGasVolGasEquivLgnPoderCal = segundaQuincena.Sum(d => d.MedGasVolGasEquivLgnPoderCal),
                    MedGasVolGasEquivLgnEnergia = segundaQuincena.Sum(d => d.MedGasVolGasEquivLgnEnergia),
                    MedGasVolGasClienteVolumen = segundaQuincena.Sum(d => d.MedGasVolGasClienteVolumen),
                    MedGasVolGasClientePoderCal = segundaQuincena.Sum(d => d.MedGasVolGasClientePoderCal),
                    MedGasVolGasClienteEnergia = segundaQuincena.Sum(d => d.MedGasVolGasClienteEnergia),
                    MedGasVolGasSaviaVolumen = segundaQuincena.Sum(d => d.MedGasVolGasSaviaVolumen),
                    MedGasVolGasSaviaPoderCal = segundaQuincena.Sum(d => d.MedGasVolGasSaviaPoderCal),
                    MedGasVolGasSaviaEnergia = segundaQuincena.Sum(d => d.MedGasVolGasSaviaEnergia),
                    MedGasVolGasLimaGasVolumen = segundaQuincena.Sum(d => d.MedGasVolGasLimaGasVolumen),
                    MedGasVolGasLimaGasPoderCal = segundaQuincena.Sum(d => d.MedGasVolGasLimaGasPoderCal),
                    MedGasVolGasLimaGasEnergia = segundaQuincena.Sum(d => d.MedGasVolGasLimaGasEnergia),
                    MedGasVolGasGasNorpVolumen = segundaQuincena.Sum(d => d.MedGasVolGasGasNorpVolumen),
                    MedGasVolGasGasNorpPoderCal = segundaQuincena.Sum(d => d.MedGasVolGasGasNorpPoderCal),
                    MedGasVolGasGasNorpEnergia = segundaQuincena.Sum(d => d.MedGasVolGasGasNorpEnergia),
                    MedGasVolGasQuemadoVolumen = segundaQuincena.Sum(d => d.MedGasVolGasQuemadoVolumen),
                    MedGasVolGasQuemadoPoderCal = segundaQuincena.Sum(d => d.MedGasVolGasQuemadoPoderCal),
                    MedGasVolGasQuemadoEnergia = segundaQuincena.Sum(d => d.MedGasVolGasQuemadoEnergia)
                };

                dto = new ResBalanceEnergLIVDto
                {
                    Lote = "LOTE IV",
                    Mes = mesActual.ToUpper(),
                    Anio = anioActual,
                    // Primer cuadro
                    // Asignar valores de la primera quincena
                    AcumUnnaQ1MedGasGasNatAsocMedVolumen = sumaPrimeraQuincena.MedGasGasNatAsocMedVolumen,
                    AcumUnnaQ1MedGasGasNatAsocMedPoderCal = sumaPrimeraQuincena.MedGasGasNatAsocMedPoderCal,
                    AcumUnnaQ1MedGasGasNatAsocMedEnergia = sumaPrimeraQuincena.MedGasGasNatAsocMedEnergia,
                    AcumUnnaQ1MedGasGasCombSecoMedVolumen = sumaPrimeraQuincena.MedGasGasCombSecoMedVolumen,
                    AcumUnnaQ1MedGasGasCombSecoMedPoderCal = sumaPrimeraQuincena.MedGasGasCombSecoMedPoderCal,
                    AcumUnnaQ1MedGasGasCombSecoMedEnergia = sumaPrimeraQuincena.MedGasGasCombSecoMedEnergia,
                    AcumUnnaQ1MedGasVolGasEquivLgnVolumen = sumaPrimeraQuincena.MedGasVolGasEquivLgnVolumen,
                    AcumUnnaQ1MedGasVolGasEquivLgnPoderCal = sumaPrimeraQuincena.MedGasVolGasEquivLgnPoderCal,
                    AcumUnnaQ1MedGasVolGasEquivLgnEnergia = sumaPrimeraQuincena.MedGasVolGasEquivLgnEnergia,
                    AcumUnnaQ1MedGasVolGasClienteVolumen = sumaPrimeraQuincena.MedGasVolGasClienteVolumen,
                    AcumUnnaQ1MedGasVolGasClientePoderCal = sumaPrimeraQuincena.MedGasVolGasClientePoderCal,
                    AcumUnnaQ1MedGasVolGasClienteEnergia = sumaPrimeraQuincena.MedGasVolGasClienteEnergia,
                    AcumUnnaQ1MedGasVolGasSaviaVolumen = sumaPrimeraQuincena.MedGasVolGasSaviaVolumen,
                    AcumUnnaQ1MedGasVolGasSaviaPoderCal = sumaPrimeraQuincena.MedGasVolGasSaviaPoderCal,
                    AcumUnnaQ1MedGasVolGasSaviaEnergia = sumaPrimeraQuincena.MedGasVolGasSaviaEnergia,
                    AcumUnnaQ1MedGasVolGasLimaGasVolumen = sumaPrimeraQuincena.MedGasVolGasLimaGasVolumen,
                    AcumUnnaQ1MedGasVolGasLimaGasPoderCal = sumaPrimeraQuincena.MedGasVolGasLimaGasPoderCal,
                    AcumUnnaQ1MedGasVolGasLimaGasEnergia = sumaPrimeraQuincena.MedGasVolGasLimaGasEnergia,
                    AcumUnnaQ1MedGasVolGasGasNorpVolumen = sumaPrimeraQuincena.MedGasVolGasGasNorpVolumen,
                    AcumUnnaQ1MedGasVolGasGasNorpPoderCal = sumaPrimeraQuincena.MedGasVolGasGasNorpPoderCal,
                    AcumUnnaQ1MedGasVolGasGasNorpEnergia = sumaPrimeraQuincena.MedGasVolGasGasNorpEnergia,
                    AcumUnnaQ1MedGasVolGasQuemadoVolumen = sumaPrimeraQuincena.MedGasVolGasQuemadoVolumen,
                    AcumUnnaQ1MedGasVolGasQuemadoPoderCal = sumaPrimeraQuincena.MedGasVolGasQuemadoPoderCal,
                    AcumUnnaQ1MedGasVolGasQuemadoEnergia = sumaPrimeraQuincena.MedGasVolGasQuemadoEnergia,

                    // Primer cuadro
                    // Asignar valores de la segunda quincena
                    AcumUnnaQ2MedGasGasNatAsocMedVolumen = sumaSegundaQuincena.MedGasGasNatAsocMedVolumen,
                    AcumUnnaQ2MedGasGasNatAsocMedPoderCal = sumaSegundaQuincena.MedGasGasNatAsocMedPoderCal,
                    AcumUnnaQ2MedGasGasNatAsocMedEnergia = sumaSegundaQuincena.MedGasGasNatAsocMedEnergia,
                    AcumUnnaQ2MedGasGasCombSecoMedVolumen = sumaSegundaQuincena.MedGasGasCombSecoMedVolumen,
                    AcumUnnaQ2MedGasGasCombSecoMedPoderCal = sumaSegundaQuincena.MedGasGasCombSecoMedPoderCal,
                    AcumUnnaQ2MedGasGasCombSecoMedEnergia = sumaSegundaQuincena.MedGasGasCombSecoMedEnergia,
                    AcumUnnaQ2MedGasVolGasEquivLgnVolumen = sumaSegundaQuincena.MedGasVolGasEquivLgnVolumen,
                    AcumUnnaQ2MedGasVolGasEquivLgnPoderCal = sumaSegundaQuincena.MedGasVolGasEquivLgnPoderCal,
                    AcumUnnaQ2MedGasVolGasEquivLgnEnergia = sumaSegundaQuincena.MedGasVolGasEquivLgnEnergia,
                    AcumUnnaQ2MedGasVolGasClienteVolumen = sumaSegundaQuincena.MedGasVolGasClienteVolumen,
                    AcumUnnaQ2MedGasVolGasClientePoderCal = sumaSegundaQuincena.MedGasVolGasClientePoderCal,
                    AcumUnnaQ2MedGasVolGasClienteEnergia = sumaSegundaQuincena.MedGasVolGasClienteEnergia,
                    AcumUnnaQ2MedGasVolGasSaviaVolumen = sumaSegundaQuincena.MedGasVolGasSaviaVolumen,
                    AcumUnnaQ2MedGasVolGasSaviaPoderCal = sumaSegundaQuincena.MedGasVolGasSaviaPoderCal,
                    AcumUnnaQ2MedGasVolGasSaviaEnergia = sumaSegundaQuincena.MedGasVolGasSaviaEnergia,
                    AcumUnnaQ2MedGasVolGasLimaGasVolumen = sumaSegundaQuincena.MedGasVolGasLimaGasVolumen,
                    AcumUnnaQ2MedGasVolGasLimaGasPoderCal = sumaSegundaQuincena.MedGasVolGasLimaGasPoderCal,
                    AcumUnnaQ2MedGasVolGasLimaGasEnergia = sumaSegundaQuincena.MedGasVolGasLimaGasEnergia,
                    AcumUnnaQ2MedGasVolGasGasNorpVolumen = sumaSegundaQuincena.MedGasVolGasGasNorpVolumen,
                    AcumUnnaQ2MedGasVolGasGasNorpPoderCal = sumaSegundaQuincena.MedGasVolGasGasNorpPoderCal,
                    AcumUnnaQ2MedGasVolGasGasNorpEnergia = sumaSegundaQuincena.MedGasVolGasGasNorpEnergia,
                    AcumUnnaQ2MedGasVolGasQuemadoVolumen = sumaSegundaQuincena.MedGasVolGasQuemadoVolumen,
                    AcumUnnaQ2MedGasVolGasQuemadoPoderCal = sumaSegundaQuincena.MedGasVolGasQuemadoPoderCal,
                    AcumUnnaQ2MedGasVolGasQuemadoEnergia = sumaSegundaQuincena.MedGasVolGasQuemadoEnergia,

                    // Segundo cuadro
                    // ACUMULADO QUINCENAL PERUPETRO	
                    AcumPeruPQ1MedGasGasNatAsocMedVolumen = 0,
                    AcumPeruPQ1MedGasGasNatAsocMedPoderCal = 0,
                    AcumPeruPQ1MedGasGasNatAsocMedEnergia = 0,
                    AcumPeruPQ1MedGasGasCombSecoMedVolumen = 0,
                    AcumPeruPQ1MedGasGasCombSecoMedPoderCal = 0,
                    AcumPeruPQ1MedGasGasCombSecoMedEnergia = 0,
                    AcumPeruPQ1MedGasVolGasEquivLgnVolumen = 0,
                    AcumPeruPQ1MedGasVolGasEquivLgnPoderCal = 0,
                    AcumPeruPQ1MedGasVolGasEquivLgnEnergia = 0,
                    AcumPeruPQ1MedGasVolGasClienteVolumen = 0,
                    AcumPeruPQ1MedGasVolGasClientePoderCal = 0,
                    AcumPeruPQ1MedGasVolGasClienteEnergia = 0,
                    AcumPeruPQ1MedGasVolGasSaviaVolumen = 0,
                    AcumPeruPQ1MedGasVolGasSaviaPoderCal = 0,
                    AcumPeruPQ1MedGasVolGasSaviaEnergia = 0,
                    AcumPeruPQ1MedGasVolGasLimaGasVolumen = 0,
                    AcumPeruPQ1MedGasVolGasLimaGasPoderCal = 0,
                    AcumPeruPQ1MedGasVolGasLimaGasEnergia = 0,
                    AcumPeruPQ1MedGasVolGasGasNorpVolumen = 0,
                    AcumPeruPQ1MedGasVolGasGasNorpPoderCal = 0,
                    AcumPeruPQ1MedGasVolGasGasNorpEnergia = 0,
                    AcumPeruPQ1MedGasVolGasQuemadoVolumen = 0,
                    AcumPeruPQ1MedGasVolGasQuemadoPoderCal = 0,
                    AcumPeruPQ1MedGasVolGasQuemadoEnergia = 0,

                    // Segundo cuadro
                    // ACUMULADO QUINCENAL PERUPETRO

                    AcumPeruPQ2MedGasGasNatAsocMedVolumen = 0,
                    AcumPeruPQ2MedGasGasNatAsocMedPoderCal = 0,
                    AcumPeruPQ2MedGasGasNatAsocMedEnergia = 0,
                    AcumPeruPQ2MedGasGasCombSecoMedVolumen = 0,
                    AcumPeruPQ2MedGasGasCombSecoMedPoderCal = 0,
                    AcumPeruPQ2MedGasGasCombSecoMedEnergia = 0,
                    AcumPeruPQ2MedGasVolGasEquivLgnVolumen = 0,
                    AcumPeruPQ2MedGasVolGasEquivLgnPoderCal = 0,
                    AcumPeruPQ2MedGasVolGasEquivLgnEnergia = 0,
                    AcumPeruPQ2MedGasVolGasClienteVolumen = 0,
                    AcumPeruPQ2MedGasVolGasClientePoderCal = 0,
                    AcumPeruPQ2MedGasVolGasClienteEnergia = 0,
                    AcumPeruPQ2MedGasVolGasSaviaVolumen = 0,
                    AcumPeruPQ2MedGasVolGasSaviaPoderCal = 0,
                    AcumPeruPQ2MedGasVolGasSaviaEnergia = 0,
                    AcumPeruPQ2MedGasVolGasLimaGasVolumen = 0,
                    AcumPeruPQ2MedGasVolGasLimaGasPoderCal = 0,
                    AcumPeruPQ2MedGasVolGasLimaGasEnergia = 0,
                    AcumPeruPQ2MedGasVolGasGasNorpVolumen = 0,
                    AcumPeruPQ2MedGasVolGasGasNorpPoderCal = 0,
                    AcumPeruPQ2MedGasVolGasGasNorpEnergia = 0,
                    AcumPeruPQ2MedGasVolGasQuemadoVolumen = 0,
                    AcumPeruPQ2MedGasVolGasQuemadoPoderCal = 0,
                    AcumPeruPQ2MedGasVolGasQuemadoEnergia = 0,

                    // Tercer Cuadro
                    DifUPQ1MedGasGasNatAsocMedVolumen =0,
                    DifUPQ1MedGasGasNatAsocMedPoderCal = 0,
                    DifUPQ1MedGasGasNatAsocMedEnergia = 0,
                    DifUPQ1MedGasGasCombSecoMedVolumen = 0,
                    DifUPQ1MedGasGasCombSecoMedPoderCal = 0,
                    DifUPQ1MedGasGasCombSecoMedEnergia = 0,
                    DifUPQ1MedGasVolGasEquivLgnVolumen = 0,
                    DifUPQ1MedGasVolGasEquivLgnPoderCal = 0,
                    DifUPQ1MedGasVolGasEquivLgnEnergia = 0,
                    DifUPQ1MedGasVolGasClienteVolumen = 0,
                    DifUPQ1MedGasVolGasClientePoderCal = 0,
                    DifUPQ1MedGasVolGasClienteEnergia = 0,
                    DifUPQ1MedGasVolGasSaviaVolumen = 0,
                    DifUPQ1MedGasVolGasSaviaPoderCal = 0,
                    DifUPQ1MedGasVolGasSaviaEnergia = 0,
                    DifUPQ1MedGasVolGasLimaGasVolumen = 0,
                    DifUPQ1MedGasVolGasLimaGasPoderCal = 0,
                    DifUPQ1MedGasVolGasLimaGasEnergia = 0,
                    DifUPQ1MedGasVolGasGasNorpVolumen = 0,
                    DifUPQ1MedGasVolGasGasNorpPoderCal = 0,
                    DifUPQ1MedGasVolGasGasNorpEnergia = 0,
                    DifUPQ1MedGasVolGasQuemadoVolumen = 0,
                    DifUPQ1MedGasVolGasQuemadoPoderCal = 0,
                    DifUPQ1MedGasVolGasQuemadoEnergia = 0,

                    //Tercer Cuadro
                    DifUPQ2MedGasGasNatAsocMedVolumen = 0,
                    DifUPQ2MedGasGasNatAsocMedPoderCal = 0,
                    DifUPQ2MedGasGasNatAsocMedEnergia = 0,
                    DifUPQ2MedGasGasCombSecoMedVolumen = 0,
                    DifUPQ2MedGasGasCombSecoMedPoderCal = 0,
                    DifUPQ2MedGasGasCombSecoMedEnergia = 0,
                    DifUPQ2MedGasVolGasEquivLgnVolumen = 0,
                    DifUPQ2MedGasVolGasEquivLgnPoderCal = 0,
                    DifUPQ2MedGasVolGasEquivLgnEnergia = 0,
                    DifUPQ2MedGasVolGasClienteVolumen = 0,
                    DifUPQ2MedGasVolGasClientePoderCal = 0,
                    DifUPQ2MedGasVolGasClienteEnergia = 0,
                    DifUPQ2MedGasVolGasSaviaVolumen = 0,
                    DifUPQ2MedGasVolGasSaviaPoderCal = 0,
                    DifUPQ2MedGasVolGasSaviaEnergia = 0,
                    DifUPQ2MedGasVolGasLimaGasVolumen = 0,
                    DifUPQ2MedGasVolGasLimaGasPoderCal = 0,
                    DifUPQ2MedGasVolGasLimaGasEnergia = 0,
                    DifUPQ2MedGasVolGasGasNorpVolumen = 0,
                    DifUPQ2MedGasVolGasGasNorpPoderCal = 0,
                    DifUPQ2MedGasVolGasGasNorpEnergia = 0,
                    DifUPQ2MedGasVolGasQuemadoVolumen = 0,
                    DifUPQ2MedGasVolGasQuemadoPoderCal = 0,
                    DifUPQ2MedGasVolGasQuemadoEnergia = 0,

                    // Cuarto cuadro
                    AcumUnnaQ1GnaFiscVtaRefVolumen = 0,
                    AcumUnnaQ1GnaFiscVtaRefPoderCal = 0,
                    AcumUnnaQ1GnaFiscVtaRefEnergia = 0,
                    AcumUnnaQ1GnaFiscVtaLimaGasVolumen = 0,
                    AcumUnnaQ1GnaFiscVtaLimaGasPoderCal = 0,
                    AcumUnnaQ1GnaFiscVtaLimaGasEnergia = 0,
                    AcumUnnaQ1GnaFiscGasNorpVolumen = 0,
                    AcumUnnaQ1GnaFiscGasNorpPoderCal = 0,
                    AcumUnnaQ1GnaFiscGasNorpEnergia = 0,
                    AcumUnnaQ1GnaFiscVtaEnelVolumen = 0,
                    AcumUnnaQ1GnaFiscVtaEnelPoderCal = 0,
                    AcumUnnaQ1GnaFiscVtaEnelEnergia = 0,
                    AcumUnnaQ1GnaFiscGcyLgnVolumen = 0,
                    AcumUnnaQ1GnaFiscGcyLgnPoderCal = 0,
                    AcumUnnaQ1GnaFiscGcyLgnEnergia = 0,
                    AcumUnnaQ1GnaFiscGnafVolumen = 0,
                    AcumUnnaQ1GnaFiscGnafPoderCal = 0,
                    AcumUnnaQ1GnaFiscGnafEnergia = 0,

                    // Cuarto Cuadro
                    //AcumUnnaQ1GnaFiscVtaRefVolumen = 0,
                    AcumUnnaQ2GnaFiscVtaRefPoderCal = 0,
                    AcumUnnaQ2GnaFiscVtaRefEnergia = 0,
                    AcumUnnaQ2GnaFiscVtaLimaGasVolumen = 0,
                    AcumUnnaQ2GnaFiscVtaLimaGasPoderCal = 0,
                    AcumUnnaQ2GnaFiscVtaLimaGasEnergia = 0,
                    AcumUnnaQ2GnaFiscGasNorpVolumen = 0,
                    AcumUnnaQ2GnaFiscGasNorpPoderCal = 0,
                    AcumUnnaQ2GnaFiscGasNorpEnergia = 0,
                    AcumUnnaQ2GnaFiscVtaEnelVolumen = 0,
                    AcumUnnaQ2GnaFiscVtaEnelPoderCal = 0,
                    AcumUnnaQ2GnaFiscVtaEnelEnergia = 0,
                    AcumUnnaQ2GnaFiscGcyLgnVolumen = 0,
                    AcumUnnaQ2GnaFiscGcyLgnPoderCal = 0,
                    AcumUnnaQ2GnaFiscGcyLgnEnergia = 0,
                    AcumUnnaQ2GnaFiscGnafVolumen = 0,
                    AcumUnnaQ2GnaFiscGnafPoderCal = 0,
                    AcumUnnaQ2GnaFiscGnafEnergia = 0,

                    AcumPeruPTotalGnaFiscVtaRefVolumen = 0,
                    AcumPeruPTotalGnaFiscVtaRefPoderCal = 0,
                    AcumPeruPTotalGnaFiscVtaRefEnergia = 0,
                    AcumPeruPTotalGnaFiscVtaLimaGasVolumen = 0,
                    AcumPeruPTotalGnaFiscVtaLimaGasPoderCal = 0,
                    AcumPeruPTotalGnaFiscVtaLimaGasEnergia = 0,
                    AcumPeruPTotalGnaFiscGasNorpVolumen = 0,
                    AcumPeruPTotalGnaFiscGasNorpPoderCal = 0,
                    AcumPeruPTotalGnaFiscGasNorpEnergia = 0,
                    AcumPeruPTotalGnaFiscVtaEnelVolumen = 0,
                    AcumPeruPTotalGnaFiscVtaEnelPoderCal = 0,
                    AcumPeruPTotalGnaFiscVtaEnelEnergia = 0,
                    AcumPeruPTotalGnaFiscGcyLgnVolumen = 0,
                    AcumPeruPTotalGnaFiscGcyLgnPoderCal = 0,
                    AcumPeruPTotalGnaFiscGcyLgnEnergia = 0,
                    AcumPeruPTotalGnaFiscGnafVolumen = 0,
                    AcumPeruPTotalGnaFiscGnafPoderCal = 0,
                    AcumPeruPTotalGnaFiscGnafEnergia = 0,

                    // Quinto Cuadro
                    AcumPeruPQ1GnaFiscVtaRefVolumen = 0,
                    AcumPeruPQ1GnaFiscVtaRefPoderCal = 0,
                    AcumPeruPQ1GnaFiscVtaRefEnergia = 0,
                    AcumPeruPQ1GnaFiscVtaLimaGasVolumen = 0,
                    AcumPeruPQ1GnaFiscVtaLimaGasPoderCal = 0,
                    AcumPeruPQ1GnaFiscVtaLimaGasEnergia = 0,
                    AcumPeruPQ1GnaFiscGasNorpVolumen = 0,
                    AcumPeruPQ1GnaFiscGasNorpPoderCal = 0,
                    AcumPeruPQ1GnaFiscGasNorpEnergia = 0,
                    AcumPeruPQ1GnaFiscVtaEnelVolumen = 0,
                    AcumPeruPQ1GnaFiscVtaEnelPoderCal = 0,
                    AcumPeruPQ1GnaFiscVtaEnelEnergia = 0,
                    AcumPeruPQ1GnaFiscGcyLgnVolumen = 0,
                    AcumPeruPQ1GnaFiscGcyLgnPoderCal = 0,
                    AcumPeruPQ1GnaFiscGcyLgnEnergia = 0,
                    AcumPeruPQ1GnaFiscGnafVolumen = 0,
                    AcumPeruPQ1GnaFiscGnafPoderCal = 0,
                    AcumPeruPQ1GnaFiscGnafEnergia = 0,

                    // Quinto Cuadro
                    AcumPeruPQ2GnaFiscVtaRefVolumen = 0,
                    AcumPeruPQ2GnaFiscVtaRefPoderCal = 0,
                    AcumPeruPQ2GnaFiscVtaRefEnergia = 0,
                    AcumPeruPQ2GnaFiscVtaLimaGasVolumen = 0,
                    AcumPeruPQ2GnaFiscVtaLimaGasPoderCal = 0,
                    AcumPeruPQ2GnaFiscVtaLimaGasEnergia = 0,
                    AcumPeruPQ2GnaFiscGasNorpVolumen = 0,
                    AcumPeruPQ2GnaFiscGasNorpPoderCal = 0,
                    AcumPeruPQ2GnaFiscGasNorpEnergia = 0,
                    AcumPeruPQ2GnaFiscVtaEnelVolumen = 0,
                    AcumPeruPQ2GnaFiscVtaEnelPoderCal = 0,
                    AcumPeruPQ2GnaFiscVtaEnelEnergia = 0,
                    AcumPeruPQ2GnaFiscGcyLgnVolumen = 0,
                    AcumPeruPQ2GnaFiscGcyLgnPoderCal = 0,
                    AcumPeruPQ2GnaFiscGcyLgnEnergia = 0,
                    AcumPeruPQ2GnaFiscGnafVolumen = 0,
                    AcumPeruPQ2GnaFiscGnafPoderCal = 0,
                    AcumPeruPQ2GnaFiscGnafEnergia = 0,

                    // Quinto Cuadro
                    AcumUnnaTotalGnaFiscVtaRefVolumen = 0,
                    AcumUnnaTotalGnaFiscVtaRefPoderCal = 0,
                    AcumUnnaTotalGnaFiscVtaRefEnergia = 0,
                    AcumUnnaTotalGnaFiscVtaLimaGasVolumen = 0,
                    AcumUnnaTotalGnaFiscVtaLimaGasPoderCal = 0,
                    AcumUnnaTotalGnaFiscVtaLimaGasEnergia = 0,
                    AcumUnnaTotalGnaFiscGasNorpVolumen = 0,
                    AcumUnnaTotalGnaFiscGasNorpPoderCal = 0,
                    AcumUnnaTotalGnaFiscGasNorpEnergia = 0,
                    AcumUnnaTotalGnaFiscVtaEnelVolumen = 0,
                    AcumUnnaTotalGnaFiscVtaEnelPoderCal = 0,
                    AcumUnnaTotalGnaFiscVtaEnelEnergia = 0,
                    AcumUnnaTotalGnaFiscGcyLgnVolumen = 0,
                    AcumUnnaTotalGnaFiscGcyLgnPoderCal = 0,
                    AcumUnnaTotalGnaFiscGcyLgnEnergia = 0,
                    AcumUnnaTotalGnaFiscGnafVolumen = 0,
                    AcumUnnaTotalGnaFiscGnafPoderCal = 0,
                    AcumUnnaTotalGnaFiscGnafEnergia = 0,

                    // Sexto Cuadro 
                    DifUPQ1GnaFiscVtaRefVolumen = 0,
                    DifUPQ1GnaFiscVtaRefPoderCal = 0,
                    DifUPQ1GnaFiscVtaRefEnergia = 0,
                    DifUPQ1GnaFiscVtaLimaGasVolumen = 0,
                    DifUPQ1GnaFiscVtaLimaGasPoderCal = 0,
                    DifUPQ1GnaFiscVtaLimaGasEnergia = 0,
                    DifUPQ1GnaFiscGasNorpVolumen = 0,
                    DifUPQ1GnaFiscGasNorpPoderCal = 0,
                    DifUPQ1GnaFiscGasNorpEnergia = 0,
                    DifUPQ1GnaFiscVtaEnelVolumen = 0,
                    DifUPQ1GnaFiscVtaEnelPoderCal = 0,
                    DifUPQ1GnaFiscVtaEnelEnergia = 0,
                    DifUPQ1GnaFiscGcyLgnVolumen = 0,
                    DifUPQ1GnaFiscGcyLgnPoderCal = 0,
                    DifUPQ1GnaFiscGcyLgnEnergia = 0,
                    DifUPQ1GnaFiscGnafVolumen = 0,
                    DifUPQ1GnaFiscGnafPoderCal = 0,
                    DifUPQ1GnaFiscGnafEnergia = 0,

                    // Sexto Cuadro 
                    DifUPQ2GnaFiscVtaRefVolumen = 0,
                    DifUPQ2GnaFiscVtaRefPoderCal = 0,
                    DifUPQ2GnaFiscVtaRefEnergia = 0,
                    DifUPQ2GnaFiscVtaLimaGasVolumen = 0,
                    DifUPQ2GnaFiscVtaLimaGasPoderCal = 0,
                    DifUPQ2GnaFiscVtaLimaGasEnergia = 0,
                    DifUPQ2GnaFiscGasNorpVolumen = 0,
                    DifUPQ2GnaFiscGasNorpPoderCal = 0,
                    DifUPQ2GnaFiscGasNorpEnergia = 0,
                    DifUPQ2GnaFiscVtaEnelVolumen = 0,
                    DifUPQ2GnaFiscVtaEnelPoderCal = 0,
                    DifUPQ2GnaFiscVtaEnelEnergia = 0,
                    DifUPQ2GnaFiscGcyLgnVolumen = 0,
                    DifUPQ2GnaFiscGcyLgnPoderCal = 0,
                    DifUPQ2GnaFiscGcyLgnEnergia = 0,
                    DifUPQ2GnaFiscGnafVolumen = 0,
                    DifUPQ2GnaFiscGnafPoderCal = 0,
                    DifUPQ2GnaFiscGnafEnergia = 0,

                    //LGN
                    MedGasGlpVolumenQ1 = 0,
                    MedGasGlpPoderCalQ1 = 0,
                    MedGasGlpEnergiaQ1 = 0,
                    MedGasGlpDensidadQ1 = 0,
                    MedGasCgnVolumenQ1 = 0,
                    MedGasCgnPoderCalQ1 = 0,
                    MedGasCgnEnergiaQ1 = 0,
                    MedGasLgnVolumenQ1 = 0,
                    MedGasLgnPoderCalQ1 = 0,
                    MedGasLgnEnergiaQ1 = 0,

                    MedGasGlpVolumenQ2 = 0,
                    MedGasGlpPoderCalQ2 = 0,
                    MedGasGlpEnergiaQ2 = 0,
                    MedGasGlpDensidadQ2 = 0,
                    MedGasCgnVolumenQ2 = 0,
                    MedGasCgnPoderCalQ2 = 0,
                    MedGasCgnEnergiaQ2 = 0,
                    MedGasLgnVolumenQ2 = 0,
                    MedGasLgnPoderCalQ2 = 0,
                    MedGasLgnEnergiaQ2 = 0,

                };

                // TERCER CUADRO
                dto.DifUPQ1MedGasGasNatAsocMedVolumen = dto.AcumPeruPQ1MedGasGasNatAsocMedVolumen - sumaPrimeraQuincena.MedGasGasNatAsocMedVolumen;
                dto.DifUPQ1MedGasGasNatAsocMedPoderCal = dto.AcumPeruPQ1MedGasGasNatAsocMedPoderCal - sumaPrimeraQuincena.MedGasGasNatAsocMedPoderCal;
                dto.DifUPQ1MedGasGasNatAsocMedEnergia = dto.AcumPeruPQ1MedGasGasNatAsocMedEnergia - sumaPrimeraQuincena.MedGasGasNatAsocMedEnergia;
                dto.DifUPQ1MedGasGasCombSecoMedVolumen = dto.AcumPeruPQ1MedGasGasCombSecoMedVolumen - sumaPrimeraQuincena.MedGasGasCombSecoMedVolumen;
                dto.DifUPQ1MedGasGasCombSecoMedPoderCal = dto.AcumPeruPQ1MedGasGasCombSecoMedPoderCal - sumaPrimeraQuincena.MedGasGasCombSecoMedPoderCal;
                dto.DifUPQ1MedGasGasCombSecoMedEnergia = dto.AcumPeruPQ1MedGasGasCombSecoMedEnergia - sumaPrimeraQuincena.MedGasGasCombSecoMedEnergia;
                dto.DifUPQ1MedGasVolGasEquivLgnVolumen = dto.AcumPeruPQ1MedGasVolGasEquivLgnVolumen - sumaPrimeraQuincena.MedGasVolGasEquivLgnVolumen;
                dto.DifUPQ1MedGasVolGasEquivLgnPoderCal = dto.AcumPeruPQ1MedGasVolGasEquivLgnPoderCal - sumaPrimeraQuincena.MedGasVolGasEquivLgnPoderCal;
                dto.DifUPQ1MedGasVolGasEquivLgnEnergia = dto.AcumPeruPQ1MedGasVolGasEquivLgnEnergia - sumaPrimeraQuincena.MedGasVolGasEquivLgnEnergia;
                dto.DifUPQ1MedGasVolGasClienteVolumen = dto.AcumPeruPQ1MedGasVolGasClienteVolumen - sumaPrimeraQuincena.MedGasVolGasClienteVolumen;
                dto.DifUPQ1MedGasVolGasClientePoderCal = dto.AcumPeruPQ1MedGasVolGasClientePoderCal - sumaPrimeraQuincena.MedGasVolGasClientePoderCal;
                dto.DifUPQ1MedGasVolGasClienteEnergia = dto.AcumPeruPQ1MedGasVolGasClienteEnergia - sumaPrimeraQuincena.MedGasVolGasClienteEnergia;
                dto.DifUPQ1MedGasVolGasSaviaVolumen = dto.AcumPeruPQ1MedGasVolGasSaviaVolumen - sumaPrimeraQuincena.MedGasVolGasSaviaVolumen;
                dto.DifUPQ1MedGasVolGasSaviaPoderCal = dto.AcumPeruPQ1MedGasVolGasSaviaPoderCal - sumaPrimeraQuincena.MedGasVolGasSaviaPoderCal;
                dto.DifUPQ1MedGasVolGasSaviaEnergia = dto.AcumPeruPQ1MedGasVolGasSaviaEnergia - sumaPrimeraQuincena.MedGasVolGasSaviaEnergia;
                dto.DifUPQ1MedGasVolGasLimaGasVolumen = dto.AcumPeruPQ1MedGasVolGasLimaGasVolumen - sumaPrimeraQuincena.MedGasVolGasLimaGasVolumen;
                dto.DifUPQ1MedGasVolGasLimaGasPoderCal = dto.AcumPeruPQ1MedGasVolGasLimaGasPoderCal - sumaPrimeraQuincena.MedGasVolGasLimaGasPoderCal;
                dto.DifUPQ1MedGasVolGasLimaGasEnergia = dto.AcumPeruPQ1MedGasVolGasLimaGasEnergia - sumaPrimeraQuincena.MedGasVolGasLimaGasEnergia;
                dto.DifUPQ1MedGasVolGasGasNorpVolumen = dto.AcumPeruPQ1MedGasVolGasGasNorpVolumen - sumaPrimeraQuincena.MedGasVolGasGasNorpVolumen;
                dto.DifUPQ1MedGasVolGasGasNorpPoderCal = dto.AcumPeruPQ1MedGasVolGasGasNorpPoderCal - sumaPrimeraQuincena.MedGasVolGasGasNorpPoderCal;
                dto.DifUPQ1MedGasVolGasGasNorpEnergia = dto.AcumPeruPQ1MedGasVolGasGasNorpEnergia - sumaPrimeraQuincena.MedGasVolGasGasNorpEnergia;
                dto.DifUPQ1MedGasVolGasQuemadoVolumen = dto.AcumPeruPQ1MedGasVolGasQuemadoVolumen - sumaPrimeraQuincena.MedGasVolGasQuemadoVolumen;
                dto.DifUPQ1MedGasVolGasQuemadoPoderCal = dto.AcumPeruPQ1MedGasVolGasQuemadoPoderCal - sumaPrimeraQuincena.MedGasVolGasQuemadoPoderCal;
                dto.DifUPQ1MedGasVolGasQuemadoEnergia = dto.AcumPeruPQ1MedGasVolGasQuemadoEnergia - sumaPrimeraQuincena.MedGasVolGasQuemadoEnergia;
                // TERCER CUADRO
                dto.DifUPQ2MedGasGasNatAsocMedVolumen = dto.AcumPeruPQ2MedGasGasNatAsocMedVolumen - sumaSegundaQuincena.MedGasGasNatAsocMedVolumen;
                dto.DifUPQ2MedGasGasNatAsocMedPoderCal = dto.AcumPeruPQ2MedGasGasNatAsocMedPoderCal - sumaSegundaQuincena.MedGasGasNatAsocMedPoderCal;
                dto.DifUPQ2MedGasGasNatAsocMedEnergia = dto.AcumPeruPQ2MedGasGasNatAsocMedEnergia - sumaSegundaQuincena.MedGasGasNatAsocMedEnergia;
                dto.DifUPQ2MedGasGasCombSecoMedVolumen = dto.AcumPeruPQ2MedGasGasCombSecoMedVolumen - sumaSegundaQuincena.MedGasGasCombSecoMedVolumen;
                dto.DifUPQ2MedGasGasCombSecoMedPoderCal = dto.AcumPeruPQ2MedGasGasCombSecoMedPoderCal - sumaSegundaQuincena.MedGasGasCombSecoMedPoderCal;
                dto.DifUPQ2MedGasGasCombSecoMedEnergia = dto.AcumPeruPQ2MedGasGasCombSecoMedEnergia - sumaSegundaQuincena.MedGasGasCombSecoMedEnergia;
                dto.DifUPQ2MedGasVolGasEquivLgnVolumen = dto.AcumPeruPQ2MedGasVolGasEquivLgnVolumen - sumaSegundaQuincena.MedGasVolGasEquivLgnVolumen;
                dto.DifUPQ2MedGasVolGasEquivLgnPoderCal = dto.AcumPeruPQ2MedGasVolGasEquivLgnPoderCal - sumaSegundaQuincena.MedGasVolGasEquivLgnPoderCal;
                dto.DifUPQ2MedGasVolGasEquivLgnEnergia = dto.AcumPeruPQ2MedGasVolGasEquivLgnEnergia - sumaSegundaQuincena.MedGasVolGasEquivLgnEnergia;
                dto.DifUPQ2MedGasVolGasClienteVolumen = dto.AcumPeruPQ2MedGasVolGasClienteVolumen - sumaSegundaQuincena.MedGasVolGasClienteVolumen;
                dto.DifUPQ2MedGasVolGasClientePoderCal = dto.AcumPeruPQ2MedGasVolGasClientePoderCal - sumaSegundaQuincena.MedGasVolGasClientePoderCal;
                dto.DifUPQ2MedGasVolGasClienteEnergia = dto.AcumPeruPQ2MedGasVolGasClienteEnergia - sumaSegundaQuincena.MedGasVolGasClienteEnergia;
                dto.DifUPQ2MedGasVolGasSaviaVolumen = dto.AcumPeruPQ2MedGasVolGasSaviaVolumen - sumaSegundaQuincena.MedGasVolGasSaviaVolumen;
                dto.DifUPQ2MedGasVolGasSaviaPoderCal = dto.AcumPeruPQ2MedGasVolGasSaviaPoderCal - sumaSegundaQuincena.MedGasVolGasSaviaPoderCal;
                dto.DifUPQ2MedGasVolGasSaviaEnergia = dto.AcumPeruPQ2MedGasVolGasSaviaEnergia - sumaSegundaQuincena.MedGasVolGasSaviaEnergia;
                dto.DifUPQ2MedGasVolGasLimaGasVolumen = dto.AcumPeruPQ2MedGasVolGasLimaGasVolumen - sumaSegundaQuincena.MedGasVolGasLimaGasVolumen;
                dto.DifUPQ2MedGasVolGasLimaGasPoderCal = dto.AcumPeruPQ2MedGasVolGasLimaGasPoderCal - sumaSegundaQuincena.MedGasVolGasLimaGasPoderCal;
                dto.DifUPQ2MedGasVolGasLimaGasEnergia = dto.AcumPeruPQ2MedGasVolGasLimaGasEnergia - sumaSegundaQuincena.MedGasVolGasLimaGasEnergia;
                dto.DifUPQ2MedGasVolGasGasNorpVolumen = dto.AcumPeruPQ2MedGasVolGasGasNorpVolumen - sumaSegundaQuincena.MedGasVolGasGasNorpVolumen;
                dto.DifUPQ2MedGasVolGasGasNorpPoderCal = dto.AcumPeruPQ2MedGasVolGasGasNorpPoderCal - sumaSegundaQuincena.MedGasVolGasGasNorpPoderCal;
                dto.DifUPQ2MedGasVolGasGasNorpEnergia = dto.AcumPeruPQ2MedGasVolGasGasNorpEnergia - sumaSegundaQuincena.MedGasVolGasGasNorpEnergia;
                dto.DifUPQ2MedGasVolGasQuemadoVolumen = dto.AcumPeruPQ2MedGasVolGasQuemadoVolumen - sumaSegundaQuincena.MedGasVolGasQuemadoVolumen;
                dto.DifUPQ2MedGasVolGasQuemadoPoderCal = dto.AcumPeruPQ2MedGasVolGasQuemadoPoderCal - sumaSegundaQuincena.MedGasVolGasQuemadoPoderCal;
                dto.DifUPQ2MedGasVolGasQuemadoEnergia = dto.AcumPeruPQ2MedGasVolGasQuemadoEnergia - sumaSegundaQuincena.MedGasVolGasQuemadoEnergia;

                //SEXTO CUADRO
                dto.DifUPQ1GnaFiscVtaRefVolumen = dto.AcumUnnaQ1GnaFiscVtaRefVolumen - dto.AcumPeruPQ1GnaFiscVtaRefVolumen;
                dto.DifUPQ1GnaFiscVtaRefPoderCal = dto.AcumUnnaQ1GnaFiscVtaRefPoderCal - dto.AcumPeruPQ1GnaFiscVtaRefPoderCal;
                dto.DifUPQ1GnaFiscVtaRefEnergia = dto.AcumUnnaQ1GnaFiscVtaRefEnergia - dto.AcumPeruPQ1GnaFiscVtaRefEnergia;
                dto.DifUPQ1GnaFiscVtaLimaGasVolumen = dto.AcumUnnaQ1GnaFiscVtaLimaGasVolumen - dto.AcumPeruPQ1GnaFiscVtaLimaGasVolumen;
                dto.DifUPQ1GnaFiscVtaLimaGasPoderCal = dto.AcumUnnaQ1GnaFiscVtaLimaGasPoderCal - dto.AcumPeruPQ1GnaFiscVtaLimaGasPoderCal;
                dto.DifUPQ1GnaFiscVtaLimaGasEnergia = dto.AcumUnnaQ1GnaFiscVtaLimaGasEnergia - dto.AcumPeruPQ1GnaFiscVtaLimaGasEnergia;
                dto.DifUPQ1GnaFiscGasNorpVolumen = dto.AcumUnnaQ1GnaFiscGasNorpVolumen - dto.AcumPeruPQ1GnaFiscGasNorpVolumen;
                dto.DifUPQ1GnaFiscGasNorpPoderCal = dto.AcumUnnaQ1GnaFiscGasNorpPoderCal - dto.AcumPeruPQ1GnaFiscGasNorpPoderCal;
                dto.DifUPQ1GnaFiscGasNorpEnergia = dto.AcumUnnaQ1GnaFiscGasNorpEnergia - dto.AcumPeruPQ1GnaFiscGasNorpEnergia;
                dto.DifUPQ1GnaFiscVtaEnelVolumen = dto.AcumUnnaQ1GnaFiscVtaEnelVolumen - dto.AcumPeruPQ1GnaFiscVtaEnelVolumen;
                dto.DifUPQ1GnaFiscVtaEnelPoderCal = dto.AcumUnnaQ1GnaFiscVtaEnelPoderCal - dto.AcumPeruPQ1GnaFiscVtaEnelPoderCal;
                dto.DifUPQ1GnaFiscVtaEnelEnergia = dto.AcumUnnaQ1GnaFiscVtaEnelEnergia - dto.AcumPeruPQ1GnaFiscVtaEnelEnergia;
                dto.DifUPQ1GnaFiscGcyLgnVolumen = dto.AcumUnnaQ1GnaFiscGcyLgnVolumen - dto.AcumPeruPQ1GnaFiscGcyLgnVolumen;
                dto.DifUPQ1GnaFiscGcyLgnPoderCal = dto.AcumUnnaQ1GnaFiscGcyLgnPoderCal - dto.AcumPeruPQ1GnaFiscGcyLgnPoderCal;
                dto.DifUPQ1GnaFiscGcyLgnEnergia = dto.AcumUnnaQ1GnaFiscGcyLgnEnergia - dto.AcumPeruPQ1GnaFiscGcyLgnEnergia;
                dto.DifUPQ1GnaFiscGnafVolumen = dto.AcumUnnaQ1GnaFiscGnafVolumen - dto.AcumPeruPQ1GnaFiscGnafVolumen;
                dto.DifUPQ1GnaFiscGnafPoderCal = dto.AcumUnnaQ1GnaFiscGnafPoderCal - dto.AcumPeruPQ1GnaFiscGnafPoderCal;
                dto.DifUPQ1GnaFiscGnafEnergia = dto.AcumUnnaQ1GnaFiscGnafEnergia - dto.AcumPeruPQ1GnaFiscGnafEnergia;
                //SEXTO CUADRO
                dto.DifUPQ2GnaFiscVtaRefVolumen = dto.AcumUnnaQ2GnaFiscVtaRefVolumen - dto.AcumPeruPQ2GnaFiscVtaRefVolumen;
                dto.DifUPQ2GnaFiscVtaRefPoderCal = dto.AcumUnnaQ2GnaFiscVtaRefPoderCal - dto.AcumPeruPQ2GnaFiscVtaRefPoderCal;
                dto.DifUPQ2GnaFiscVtaRefEnergia = dto.AcumUnnaQ2GnaFiscVtaRefEnergia - dto.AcumPeruPQ2GnaFiscVtaRefEnergia;
                dto.DifUPQ2GnaFiscVtaLimaGasVolumen = dto.AcumUnnaQ2GnaFiscVtaLimaGasVolumen - dto.AcumPeruPQ2GnaFiscVtaLimaGasVolumen;
                dto.DifUPQ2GnaFiscVtaLimaGasPoderCal = dto.AcumUnnaQ2GnaFiscVtaLimaGasPoderCal - dto.AcumPeruPQ2GnaFiscVtaLimaGasPoderCal;
                dto.DifUPQ2GnaFiscVtaLimaGasEnergia = dto.AcumUnnaQ2GnaFiscVtaLimaGasEnergia - dto.AcumPeruPQ2GnaFiscVtaLimaGasEnergia;
                dto.DifUPQ2GnaFiscGasNorpVolumen = dto.AcumUnnaQ2GnaFiscGasNorpVolumen - dto.AcumPeruPQ2GnaFiscGasNorpVolumen;
                dto.DifUPQ2GnaFiscGasNorpPoderCal = dto.AcumUnnaQ2GnaFiscGasNorpPoderCal - dto.AcumPeruPQ2GnaFiscGasNorpPoderCal;
                dto.DifUPQ2GnaFiscGasNorpEnergia = dto.AcumUnnaQ2GnaFiscGasNorpEnergia - dto.AcumPeruPQ2GnaFiscGasNorpEnergia;
                dto.DifUPQ2GnaFiscVtaEnelVolumen = dto.AcumUnnaQ2GnaFiscVtaEnelVolumen - dto.AcumPeruPQ2GnaFiscVtaEnelVolumen;
                dto.DifUPQ2GnaFiscVtaEnelPoderCal = dto.AcumUnnaQ2GnaFiscVtaEnelPoderCal - dto.AcumPeruPQ2GnaFiscVtaEnelPoderCal;
                dto.DifUPQ2GnaFiscVtaEnelEnergia = dto.AcumUnnaQ2GnaFiscVtaEnelEnergia - dto.AcumPeruPQ2GnaFiscVtaEnelEnergia;
                dto.DifUPQ2GnaFiscGcyLgnVolumen = dto.AcumUnnaQ2GnaFiscGcyLgnVolumen - dto.AcumPeruPQ2GnaFiscGcyLgnVolumen;
                dto.DifUPQ2GnaFiscGcyLgnPoderCal = dto.AcumUnnaQ2GnaFiscGcyLgnPoderCal - dto.AcumPeruPQ2GnaFiscGcyLgnPoderCal;
                dto.DifUPQ2GnaFiscGcyLgnEnergia = dto.AcumUnnaQ2GnaFiscGcyLgnEnergia - dto.AcumPeruPQ2GnaFiscGcyLgnEnergia;
                dto.DifUPQ2GnaFiscGnafVolumen = dto.AcumUnnaQ2GnaFiscGnafVolumen - dto.AcumPeruPQ2GnaFiscGnafVolumen;
                dto.DifUPQ2GnaFiscGnafPoderCal = dto.AcumUnnaQ2GnaFiscGnafPoderCal - dto.AcumPeruPQ2GnaFiscGnafPoderCal;
                dto.DifUPQ2GnaFiscGnafEnergia = dto.AcumUnnaQ2GnaFiscGnafEnergia - dto.AcumPeruPQ2GnaFiscGnafEnergia;

                
                dto.ResBalanceEnergLIVDetMedGas = await ResBalanceEnergLIVDetMedGas();
                dto.ResBalanceEnergLIVDetGnaFisc = await ResBalanceEnergLIVDetGnaFisc();
                dto.ResBalanceEnergLgnLIV_2DetLgnDto = await ResBalanceEnergLIVDetMedGasLGN();

                var primeraQuincenaLGN = dto.ResBalanceEnergLgnLIV_2DetLgnDto.Where(d => d.Dia >= 1 && d.Dia <= 15);
                var sumaPrimeraQuincenaLGN = new
                {
                    MedGasGlpVolumenQ1 = primeraQuincenaLGN.Sum(d => d.MedGasGlpVolumen),
                    MedGasGlpPoderCalQ1 = primeraQuincenaLGN.Sum(d => d.MedGasGlpPoderCal),
                    MedGasGlpEnergiaQ1 = primeraQuincenaLGN.Sum(d => d.MedGasGlpEnergia),
                    MedGasGlpDensidadQ1 = primeraQuincenaLGN.Sum(d => d.MedGasGlpDensidad),
                    MedGasCgnVolumenQ1 = primeraQuincenaLGN.Sum(d => d.MedGasCgnVolumen),
                    MedGasCgnPoderCalQ1 = primeraQuincenaLGN.Sum(d => d.MedGasCgnPoderCal),
                    MedGasCgnEnergiaQ1 = primeraQuincenaLGN.Sum(d => d.MedGasCgnEnergia),
                    MedGasLgnVolumenQ1 = primeraQuincenaLGN.Sum(d => d.MedGasLgnVolumen),
                    MedGasLgnPoderCalQ1 = primeraQuincenaLGN.Sum(d => d.MedGasLgnPoderCal),
                    MedGasLgnEnergiaQ1 = primeraQuincenaLGN.Sum(d => d.MedGasLgnEnergia),
                };
                //LGN 
                dto.MedGasGlpVolumenQ1 = sumaPrimeraQuincenaLGN.MedGasGlpVolumenQ1;
                dto.MedGasGlpPoderCalQ1 = sumaPrimeraQuincenaLGN.MedGasGlpPoderCalQ1;
                dto.MedGasGlpEnergiaQ1 = sumaPrimeraQuincenaLGN.MedGasGlpEnergiaQ1;
                dto.MedGasGlpDensidadQ1 = sumaPrimeraQuincenaLGN.MedGasGlpDensidadQ1;
                dto.MedGasCgnVolumenQ1 = sumaPrimeraQuincenaLGN.MedGasCgnVolumenQ1;
                dto.MedGasCgnPoderCalQ1 = sumaPrimeraQuincenaLGN.MedGasCgnPoderCalQ1;
                dto.MedGasCgnEnergiaQ1 = sumaPrimeraQuincenaLGN.MedGasCgnEnergiaQ1;
                dto.MedGasLgnVolumenQ1 = sumaPrimeraQuincenaLGN.MedGasLgnVolumenQ1;
                dto.MedGasLgnPoderCalQ1 = sumaPrimeraQuincenaLGN.MedGasLgnPoderCalQ1;
                dto.MedGasLgnEnergiaQ1 = sumaPrimeraQuincenaLGN.MedGasLgnEnergiaQ1;

                var segundaQuincenaLGN = dto.ResBalanceEnergLgnLIV_2DetLgnDto.Where(d => d.Dia >= 16 && d.Dia <= DateTime.DaysInMonth(2024, 4));
                var sumaSegundaQuincenaLGN = new
                {
                    MedGasGlpVolumenQ2 = segundaQuincenaLGN.Sum(d => d.MedGasGlpVolumen),
                    MedGasGlpPoderCalQ2 = segundaQuincenaLGN.Sum(d => d.MedGasGlpPoderCal),
                    MedGasGlpEnergiaQ2 = segundaQuincenaLGN.Sum(d => d.MedGasGlpEnergia),
                    MedGasGlpDensidadQ2 = segundaQuincenaLGN.Sum(d => d.MedGasGlpDensidad),
                    MedGasCgnVolumenQ2 = segundaQuincenaLGN.Sum(d => d.MedGasCgnVolumen),
                    MedGasCgnPoderCalQ2 = segundaQuincenaLGN.Sum(d => d.MedGasCgnPoderCal),
                    MedGasCgnEnergiaQ2 = segundaQuincenaLGN.Sum(d => d.MedGasCgnEnergia),
                    MedGasLgnVolumenQ2 = segundaQuincenaLGN.Sum(d => d.MedGasLgnVolumen),
                    MedGasLgnPoderCalQ2 = segundaQuincenaLGN.Sum(d => d.MedGasLgnPoderCal),
                    MedGasLgnEnergiaQ2 = segundaQuincenaLGN.Sum(d => d.MedGasLgnEnergia),
                };
                dto.MedGasGlpVolumenQ2 = sumaSegundaQuincenaLGN.MedGasGlpVolumenQ2;
                dto.MedGasGlpPoderCalQ2 = sumaSegundaQuincenaLGN.MedGasGlpPoderCalQ2;
                dto.MedGasGlpEnergiaQ2 = sumaSegundaQuincenaLGN.MedGasGlpEnergiaQ2;
                dto.MedGasGlpDensidadQ2 = sumaSegundaQuincenaLGN.MedGasGlpDensidadQ2;
                dto.MedGasCgnVolumenQ2 = sumaSegundaQuincenaLGN.MedGasCgnVolumenQ2;
                dto.MedGasCgnPoderCalQ2 = sumaSegundaQuincenaLGN.MedGasCgnPoderCalQ2;
                dto.MedGasCgnEnergiaQ2 = sumaSegundaQuincenaLGN.MedGasCgnEnergiaQ2;
                dto.MedGasLgnVolumenQ2 = sumaSegundaQuincenaLGN.MedGasLgnVolumenQ2;
                dto.MedGasLgnPoderCalQ2 = sumaSegundaQuincenaLGN.MedGasLgnPoderCalQ2;
                dto.MedGasLgnEnergiaQ2 = sumaSegundaQuincenaLGN.MedGasLgnEnergiaQ2;

                // LGN SEGUNDO CUADRO
                dto.MedGasGlpVolumenQ1S2 = 0;
                dto.MedGasGlpPoderCalQ1S2 = 0;
                dto.MedGasGlpEnergiaQ1S2 = 0;
                dto.MedGasGlpDensidadQ1S2 = 0;
                dto.MedGasCgnVolumenQ1S2 = 0;
                dto.MedGasCgnPoderCalQ1S2 = 0;
                dto.MedGasCgnEnergiaQ1S2 = 0;
                dto.MedGasLgnVolumenQ1S2 = 0;
                dto.MedGasLgnPoderCalQ1S2 = 0;
                dto.MedGasLgnEnergiaQ1S2 = 0;

                dto.MedGasGlpVolumenQ2S2 = 0;
                dto.MedGasGlpPoderCalQ2S2 = 0;
                dto.MedGasGlpEnergiaQ2S2 = 0;
                dto.MedGasGlpDensidadQ2S2 = 0;
                dto.MedGasCgnVolumenQ2S2 = 0;
                dto.MedGasCgnPoderCalQ2S2 = 0;
                dto.MedGasCgnEnergiaQ2S2 = 0;
                dto.MedGasLgnVolumenQ2S2 = 0;
                dto.MedGasLgnPoderCalQ2S2 = 0;
                dto.MedGasLgnEnergiaQ2S2 = 0;

                // LGN TERCER CUADRO
                dto.AcumMedGasGlpVolumenQ1S2 = dto.MedGasGlpVolumenQ1S2 - dto.MedGasGlpVolumenQ1;
                dto.AcumMedGasGlpPoderCalQ1S2 = dto.MedGasGlpPoderCalQ1S2 - dto.MedGasGlpPoderCalQ1;
                dto.AcumMedGasGlpEnergiaQ1S2 = dto.MedGasGlpEnergiaQ1S2 - dto.MedGasGlpEnergiaQ1;
                dto.AcumMedGasGlpDensidadQ1S2 = dto.MedGasGlpDensidadQ1S2 - dto.MedGasGlpDensidadQ1;
                dto.AcumMedGasCgnVolumenQ1S2 = dto.MedGasCgnVolumenQ1S2 - dto.MedGasCgnVolumenQ1;
                dto.AcumMedGasCgnPoderCalQ1S2 = dto.MedGasCgnPoderCalQ1S2 - dto.MedGasCgnPoderCalQ1;
                dto.AcumMedGasCgnEnergiaQ1S2 = dto.MedGasCgnEnergiaQ1S2 - dto.MedGasCgnEnergiaQ1;
                dto.AcumMedGasLgnVolumenQ1S2 = dto.MedGasLgnVolumenQ1S2 - dto.MedGasLgnVolumenQ1;
                dto.AcumMedGasLgnPoderCalQ1S2 = dto.MedGasLgnPoderCalQ1S2 - dto.MedGasLgnPoderCalQ1;
                dto.AcumMedGasLgnEnergiaQ1S2 = dto.MedGasLgnEnergiaQ1S2 - dto.MedGasLgnEnergiaQ1;

                dto.AcumMedGasGlpVolumenQ2S2 = dto.MedGasGlpVolumenQ2S2 - dto.MedGasGlpVolumenQ2;
                dto.AcumMedGasGlpPoderCalQ2S2 = dto.MedGasGlpPoderCalQ2S2 - dto.MedGasGlpPoderCalQ2;
                dto.AcumMedGasGlpEnergiaQ2S2 = dto.MedGasGlpEnergiaQ2S2 - dto.MedGasGlpEnergiaQ2;
                dto.AcumMedGasGlpDensidadQ2S2 = dto.MedGasGlpDensidadQ2S2 - dto.MedGasGlpDensidadQ2;
                dto.AcumMedGasCgnVolumenQ2S2 = dto.MedGasCgnVolumenQ2S2 - dto.MedGasCgnVolumenQ2;
                dto.AcumMedGasCgnPoderCalQ2S2 = dto.MedGasCgnPoderCalQ2S2 - dto.MedGasCgnPoderCalQ2;
                dto.AcumMedGasCgnEnergiaQ2S2 = dto.MedGasCgnEnergiaQ2S2 - dto.MedGasCgnEnergiaQ2;
                dto.AcumMedGasLgnVolumenQ2S2 = dto.MedGasLgnVolumenQ2S2 - dto.MedGasLgnVolumenQ2;
                dto.AcumMedGasLgnPoderCalQ2S2 = dto.MedGasLgnPoderCalQ2S2 - dto.MedGasLgnPoderCalQ2;
                dto.AcumMedGasLgnEnergiaQ2S2 = dto.MedGasLgnEnergiaQ2S2 - dto.MedGasLgnEnergiaQ2;

                var parametrosLGNQ = await _registroRepositorio.ObtenerResumenBalanceEnergiaLGNParametrosAsync();
                var parametroQ1 = parametrosLGNQ?.FirstOrDefault(p => p.Quincena == 1);
                var parametroQ2 = parametrosLGNQ?.FirstOrDefault(p => p.Quincena == 2);

                dto.Id = parametroQ1.Id;
                dto.DensidadGLPKgBl = parametroQ1.DensidadGLPKgBl;
                dto.PCGLPMMBtuBl60F = parametroQ1.PCGLPMMBtuBl60F;
                dto.PCCGNMMBtuBl60F = parametroQ1.PCCGNMMBtuBl60F;
                dto.PCLGNMMBtuBl60F = parametroQ1.PCLGNMMBtuBl60F;
                dto.FactorConversionSCFDGal = parametroQ1.FactorConversionSCFDGal;
                dto.Quincena = parametroQ1.Quincena;
                dto.Fecha = parametroQ1.Fecha;

                dto.IdQ2 = parametroQ2.Id;
                dto.DensidadGLPKgBlQ2 = parametroQ2.DensidadGLPKgBl;
                dto.PCGLPMMBtuBl60FQ2 = parametroQ2.PCGLPMMBtuBl60F;
                dto.PCCGNMMBtuBl60FQ2 = parametroQ2.PCCGNMMBtuBl60F;
                dto.PCLGNMMBtuBl60FQ2 = parametroQ2.PCLGNMMBtuBl60F;
                dto.FactorConversionSCFDGalQ2 = parametroQ2.FactorConversionSCFDGal;
                dto.QuincenaQ2 = parametroQ2.Quincena;
                dto.FechaQ2 = parametroQ2.Fecha;

                // Cuadro LGN Q1
                dto.EnergiaMMBTUQ1GLP = dto.MedGasGlpEnergiaQ1;
                dto.EnergiaMMBTUQ1CGN = dto.MedGasCgnEnergiaQ1;
                // Cuadro LGN Q2       
                dto.EnergiaMMBTUQ2GLP = dto.MedGasGlpEnergiaQ2;
                dto.EnergiaMMBTUQ2CGN = dto.MedGasCgnEnergiaQ2;
            }
            else
            {
                string jsonData = imprimir.Datos.Replace("\\", "");
                RootObject rootObject = JsonConvert.DeserializeObject<RootObject>(jsonData);

                if (rootObject != null)
                {
                    //string Mes = rootObject.Mes ?? string.Empty; 
                    
                    dto = new ResBalanceEnergLIVDto
                    {
                        Lote = "LOTE IV",
                        Mes = "ABRIL",
                        Anio = rootObject.Anio
                    };

                    // Crear un diccionario de mapeo
                    var propertyMap = new Dictionary<string, Action<string>>
                    {
                        // Acumulado Quincena UNNA - Q1
                        {"AcumUnnaQ1MedGasGasNatAsocMedVolumen", value => dto.AcumUnnaQ1MedGasGasNatAsocMedVolumen = Convert.ToDouble(value)},
                        {"AcumUnnaQ1MedGasGasNatAsocMedPoderCal", value => dto.AcumUnnaQ1MedGasGasNatAsocMedPoderCal = Convert.ToDouble(value)},
                        {"AcumUnnaQ1MedGasGasNatAsocMedEnergia", value => dto.AcumUnnaQ1MedGasGasNatAsocMedEnergia = Convert.ToDouble(value)},
                        {"AcumUnnaQ1MedGasGasCombSecoMedVolumen", value => dto.AcumUnnaQ1MedGasGasCombSecoMedVolumen = Convert.ToDouble(value)},
                        {"AcumUnnaQ1MedGasGasCombSecoMedPoderCal", value => dto.AcumUnnaQ1MedGasGasCombSecoMedPoderCal = Convert.ToDouble(value)},
                        {"AcumUnnaQ1MedGasGasCombSecoMedEnergia", value => dto.AcumUnnaQ1MedGasGasCombSecoMedEnergia = Convert.ToDouble(value)},
                        {"AcumUnnaQ1MedGasVolGasEquivLgnVolumen", value => dto.AcumUnnaQ1MedGasVolGasEquivLgnVolumen = Convert.ToDouble(value)},
                        {"AcumUnnaQ1MedGasVolGasEquivLgnPoderCal", value => dto.AcumUnnaQ1MedGasVolGasEquivLgnPoderCal = Convert.ToDouble(value)},
                        {"AcumUnnaQ1MedGasVolGasEquivLgnEnergia", value => dto.AcumUnnaQ1MedGasVolGasEquivLgnEnergia = Convert.ToDouble(value)},
                        {"AcumUnnaQ1MedGasVolGasClienteVolumen", value => dto.AcumUnnaQ1MedGasVolGasClienteVolumen = Convert.ToDouble(value)},
                        {"AcumUnnaQ1MedGasVolGasClientePoderCal", value => dto.AcumUnnaQ1MedGasVolGasClientePoderCal = Convert.ToDouble(value)},
                        {"AcumUnnaQ1MedGasVolGasClienteEnergia", value => dto.AcumUnnaQ1MedGasVolGasClienteEnergia = Convert.ToDouble(value)},
                        {"AcumUnnaQ1MedGasVolGasSaviaVolumen", value => dto.AcumUnnaQ1MedGasVolGasSaviaVolumen = Convert.ToDouble(value)},
                        {"AcumUnnaQ1MedGasVolGasSaviaPoderCal", value => dto.AcumUnnaQ1MedGasVolGasSaviaPoderCal = Convert.ToDouble(value)},
                        {"AcumUnnaQ1MedGasVolGasSaviaEnergia", value => dto.AcumUnnaQ1MedGasVolGasSaviaEnergia = Convert.ToDouble(value)},
                        {"AcumUnnaQ1MedGasVolGasLimaGasVolumen", value => dto.AcumUnnaQ1MedGasVolGasLimaGasVolumen = Convert.ToDouble(value)},
                        {"AcumUnnaQ1MedGasVolGasLimaGasPoderCal", value => dto.AcumUnnaQ1MedGasVolGasLimaGasPoderCal = Convert.ToDouble(value)},
                        {"AcumUnnaQ1MedGasVolGasLimaGasEnergia", value => dto.AcumUnnaQ1MedGasVolGasLimaGasEnergia = Convert.ToDouble(value)},
                        {"AcumUnnaQ1MedGasVolGasGasNorpVolumen", value => dto.AcumUnnaQ1MedGasVolGasGasNorpVolumen = Convert.ToDouble(value)},
                        {"AcumUnnaQ1MedGasVolGasGasNorpPoderCal", value => dto.AcumUnnaQ1MedGasVolGasGasNorpPoderCal = Convert.ToDouble(value)},
                        {"AcumUnnaQ1MedGasVolGasGasNorpEnergia", value => dto.AcumUnnaQ1MedGasVolGasGasNorpEnergia = Convert.ToDouble(value)},
                        {"AcumUnnaQ1MedGasVolGasQuemadoVolumen", value => dto.AcumUnnaQ1MedGasVolGasQuemadoVolumen = Convert.ToDouble(value)},
                        {"AcumUnnaQ1MedGasVolGasQuemadoPoderCal", value => dto.AcumUnnaQ1MedGasVolGasQuemadoPoderCal = Convert.ToDouble(value)},
                        {"AcumUnnaQ1MedGasVolGasQuemadoEnergia", value => dto.AcumUnnaQ1MedGasVolGasQuemadoEnergia = Convert.ToDouble(value)},
                        // Acumulado Quincena UNNA - Q2
                        {"AcumUnnaQ2MedGasGasNatAsocMedVolumen", value => dto.AcumUnnaQ2MedGasGasNatAsocMedVolumen = Convert.ToDouble(value)},
                        {"AcumUnnaQ2MedGasGasNatAsocMedPoderCal", value => dto.AcumUnnaQ2MedGasGasNatAsocMedPoderCal = Convert.ToDouble(value)},
                        {"AcumUnnaQ2MedGasGasNatAsocMedEnergia", value => dto.AcumUnnaQ2MedGasGasNatAsocMedEnergia = Convert.ToDouble(value)},
                        {"AcumUnnaQ2MedGasGasCombSecoMedVolumen", value => dto.AcumUnnaQ2MedGasGasCombSecoMedVolumen = Convert.ToDouble(value)},
                        {"AcumUnnaQ2MedGasGasCombSecoMedPoderCal", value => dto.AcumUnnaQ2MedGasGasCombSecoMedPoderCal = Convert.ToDouble(value)},
                        {"AcumUnnaQ2MedGasGasCombSecoMedEnergia", value => dto.AcumUnnaQ2MedGasGasCombSecoMedEnergia = Convert.ToDouble(value)},
                        {"AcumUnnaQ2MedGasVolGasEquivLgnVolumen", value => dto.AcumUnnaQ2MedGasVolGasEquivLgnVolumen = Convert.ToDouble(value)},
                        {"AcumUnnaQ2MedGasVolGasEquivLgnPoderCal", value => dto.AcumUnnaQ2MedGasVolGasEquivLgnPoderCal = Convert.ToDouble(value)},
                        {"AcumUnnaQ2MedGasVolGasEquivLgnEnergia", value => dto.AcumUnnaQ2MedGasVolGasEquivLgnEnergia = Convert.ToDouble(value)},
                        {"AcumUnnaQ2MedGasVolGasClienteVolumen", value => dto.AcumUnnaQ2MedGasVolGasClienteVolumen = Convert.ToDouble(value)},
                        {"AcumUnnaQ2MedGasVolGasClientePoderCal", value => dto.AcumUnnaQ2MedGasVolGasClientePoderCal = Convert.ToDouble(value)},
                        {"AcumUnnaQ2MedGasVolGasClienteEnergia", value => dto.AcumUnnaQ2MedGasVolGasClienteEnergia = Convert.ToDouble(value)},
                        {"AcumUnnaQ2MedGasVolGasSaviaVolumen", value => dto.AcumUnnaQ2MedGasVolGasSaviaVolumen = Convert.ToDouble(value)},
                        {"AcumUnnaQ2MedGasVolGasSaviaPoderCal", value => dto.AcumUnnaQ2MedGasVolGasSaviaPoderCal = Convert.ToDouble(value)},
                        {"AcumUnnaQ2MedGasVolGasSaviaEnergia", value => dto.AcumUnnaQ2MedGasVolGasSaviaEnergia = Convert.ToDouble(value)},
                        {"AcumUnnaQ2MedGasVolGasLimaGasVolumen", value => dto.AcumUnnaQ2MedGasVolGasLimaGasVolumen = Convert.ToDouble(value)},
                        {"AcumUnnaQ2MedGasVolGasLimaGasPoderCal", value => dto.AcumUnnaQ2MedGasVolGasLimaGasPoderCal = Convert.ToDouble(value)},
                        {"AcumUnnaQ2MedGasVolGasLimaGasEnergia", value => dto.AcumUnnaQ2MedGasVolGasLimaGasEnergia = Convert.ToDouble(value)},
                        {"AcumUnnaQ2MedGasVolGasGasNorpVolumen", value => dto.AcumUnnaQ2MedGasVolGasGasNorpVolumen = Convert.ToDouble(value)},
                        {"AcumUnnaQ2MedGasVolGasGasNorpPoderCal", value => dto.AcumUnnaQ2MedGasVolGasGasNorpPoderCal = Convert.ToDouble(value)},
                        {"AcumUnnaQ2MedGasVolGasGasNorpEnergia", value => dto.AcumUnnaQ2MedGasVolGasGasNorpEnergia = Convert.ToDouble(value)},
                        {"AcumUnnaQ2MedGasVolGasQuemadoVolumen", value => dto.AcumUnnaQ2MedGasVolGasQuemadoVolumen = Convert.ToDouble(value)},
                        {"AcumUnnaQ2MedGasVolGasQuemadoPoderCal", value => dto.AcumUnnaQ2MedGasVolGasQuemadoPoderCal = Convert.ToDouble(value)},
                        {"AcumUnnaQ2MedGasVolGasQuemadoEnergia", value => dto.AcumUnnaQ2MedGasVolGasQuemadoEnergia = Convert.ToDouble(value)},
                        // Acumulado Quincena PERUPETRO - Q1
                        {"AcumPeruPQ1MedGasGasNatAsocMedVolumen", value => dto.AcumPeruPQ1MedGasGasNatAsocMedVolumen = Convert.ToDouble(value)},
                        {"AcumPeruPQ1MedGasGasNatAsocMedPoderCal", value => dto.AcumPeruPQ1MedGasGasNatAsocMedPoderCal = Convert.ToDouble(value)},
                        {"AcumPeruPQ1MedGasGasNatAsocMedEnergia", value => dto.AcumPeruPQ1MedGasGasNatAsocMedEnergia = Convert.ToDouble(value)},
                        {"AcumPeruPQ1MedGasGasCombSecoMedVolumen", value => dto.AcumPeruPQ1MedGasGasCombSecoMedVolumen = Convert.ToDouble(value)},
                        {"AcumPeruPQ1MedGasGasCombSecoMedPoderCal", value => dto.AcumPeruPQ1MedGasGasCombSecoMedPoderCal = Convert.ToDouble(value)},
                        {"AcumPeruPQ1MedGasGasCombSecoMedEnergia", value => dto.AcumPeruPQ1MedGasGasCombSecoMedEnergia = Convert.ToDouble(value)},
                        {"AcumPeruPQ1MedGasVolGasEquivLgnVolumen", value => dto.AcumPeruPQ1MedGasVolGasEquivLgnVolumen = Convert.ToDouble(value)},
                        {"AcumPeruPQ1MedGasVolGasEquivLgnPoderCal", value => dto.AcumPeruPQ1MedGasVolGasEquivLgnPoderCal = Convert.ToDouble(value)},
                        {"AcumPeruPQ1MedGasVolGasEquivLgnEnergia", value => dto.AcumPeruPQ1MedGasVolGasEquivLgnEnergia = Convert.ToDouble(value)},
                        {"AcumPeruPQ1MedGasVolGasClienteVolumen", value => dto.AcumPeruPQ1MedGasVolGasClienteVolumen = Convert.ToDouble(value)},
                        {"AcumPeruPQ1MedGasVolGasClientePoderCal", value => dto.AcumPeruPQ1MedGasVolGasClientePoderCal = Convert.ToDouble(value)},
                        {"AcumPeruPQ1MedGasVolGasClienteEnergia", value => dto.AcumPeruPQ1MedGasVolGasClienteEnergia = Convert.ToDouble(value)},
                        {"AcumPeruPQ1MedGasVolGasSaviaVolumen", value => dto.AcumPeruPQ1MedGasVolGasSaviaVolumen = Convert.ToDouble(value)},
                        {"AcumPeruPQ1MedGasVolGasSaviaPoderCal", value => dto.AcumPeruPQ1MedGasVolGasSaviaPoderCal = Convert.ToDouble(value)},
                        {"AcumPeruPQ1MedGasVolGasSaviaEnergia", value => dto.AcumPeruPQ1MedGasVolGasSaviaEnergia = Convert.ToDouble(value)},
                        {"AcumPeruPQ1MedGasVolGasLimaGasVolumen", value => dto.AcumPeruPQ1MedGasVolGasLimaGasVolumen = Convert.ToDouble(value)},
                        {"AcumPeruPQ1MedGasVolGasLimaGasPoderCal", value => dto.AcumPeruPQ1MedGasVolGasLimaGasPoderCal = Convert.ToDouble(value)},
                        {"AcumPeruPQ1MedGasVolGasLimaGasEnergia", value => dto.AcumPeruPQ1MedGasVolGasLimaGasEnergia = Convert.ToDouble(value)},
                        {"AcumPeruPQ1MedGasVolGasGasNorpVolumen", value => dto.AcumPeruPQ1MedGasVolGasGasNorpVolumen = Convert.ToDouble(value)},
                        {"AcumPeruPQ1MedGasVolGasGasNorpPoderCal", value => dto.AcumPeruPQ1MedGasVolGasGasNorpPoderCal = Convert.ToDouble(value)},
                        {"AcumPeruPQ1MedGasVolGasGasNorpEnergia", value => dto.AcumPeruPQ1MedGasVolGasGasNorpEnergia = Convert.ToDouble(value)},
                        {"AcumPeruPQ1MedGasVolGasQuemadoVolumen", value => dto.AcumPeruPQ1MedGasVolGasQuemadoVolumen = Convert.ToDouble(value)},
                        {"AcumPeruPQ1MedGasVolGasQuemadoPoderCal", value => dto.AcumPeruPQ1MedGasVolGasQuemadoPoderCal = Convert.ToDouble(value)},
                        {"AcumPeruPQ1MedGasVolGasQuemadoEnergia", value => dto.AcumPeruPQ1MedGasVolGasQuemadoEnergia = Convert.ToDouble(value)},
                        // Acumulado Quincena PERUPETRO - Q2
                        {"AcumPeruPQ2MedGasGasNatAsocMedVolumen", value => dto.AcumPeruPQ2MedGasGasNatAsocMedVolumen = Convert.ToDouble(value)},
                        {"AcumPeruPQ2MedGasGasNatAsocMedPoderCal", value => dto.AcumPeruPQ2MedGasGasNatAsocMedPoderCal = Convert.ToDouble(value)},
                        {"AcumPeruPQ2MedGasGasNatAsocMedEnergia", value => dto.AcumPeruPQ2MedGasGasNatAsocMedEnergia = Convert.ToDouble(value)},
                        {"AcumPeruPQ2MedGasGasCombSecoMedVolumen", value => dto.AcumPeruPQ2MedGasGasCombSecoMedVolumen = Convert.ToDouble(value)},
                        {"AcumPeruPQ2MedGasGasCombSecoMedPoderCal", value => dto.AcumPeruPQ2MedGasGasCombSecoMedPoderCal = Convert.ToDouble(value)},
                        {"AcumPeruPQ2MedGasGasCombSecoMedEnergia", value => dto.AcumPeruPQ2MedGasGasCombSecoMedEnergia = Convert.ToDouble(value)},
                        {"AcumPeruPQ2MedGasVolGasEquivLgnVolumen", value => dto.AcumPeruPQ2MedGasVolGasEquivLgnVolumen = Convert.ToDouble(value)},
                        {"AcumPeruPQ2MedGasVolGasEquivLgnPoderCal", value => dto.AcumPeruPQ2MedGasVolGasEquivLgnPoderCal = Convert.ToDouble(value)},
                        {"AcumPeruPQ2MedGasVolGasEquivLgnEnergia", value => dto.AcumPeruPQ2MedGasVolGasEquivLgnEnergia = Convert.ToDouble(value)},
                        {"AcumPeruPQ2MedGasVolGasClienteVolumen", value => dto.AcumPeruPQ2MedGasVolGasClienteVolumen = Convert.ToDouble(value)},
                        {"AcumPeruPQ2MedGasVolGasClientePoderCal", value => dto.AcumPeruPQ2MedGasVolGasClientePoderCal = Convert.ToDouble(value)},
                        {"AcumPeruPQ2MedGasVolGasClienteEnergia", value => dto.AcumPeruPQ2MedGasVolGasClienteEnergia = Convert.ToDouble(value)},
                        {"AcumPeruPQ2MedGasVolGasSaviaVolumen", value => dto.AcumPeruPQ2MedGasVolGasSaviaVolumen = Convert.ToDouble(value)},
                        {"AcumPeruPQ2MedGasVolGasSaviaPoderCal", value => dto.AcumPeruPQ2MedGasVolGasSaviaPoderCal = Convert.ToDouble(value)},
                        {"AcumPeruPQ2MedGasVolGasSaviaEnergia", value => dto.AcumPeruPQ2MedGasVolGasSaviaEnergia = Convert.ToDouble(value)},
                        {"AcumPeruPQ2MedGasVolGasLimaGasVolumen", value => dto.AcumPeruPQ2MedGasVolGasLimaGasVolumen = Convert.ToDouble(value)},
                        {"AcumPeruPQ2MedGasVolGasLimaGasPoderCal", value => dto.AcumPeruPQ2MedGasVolGasLimaGasPoderCal = Convert.ToDouble(value)},
                        {"AcumPeruPQ2MedGasVolGasLimaGasEnergia", value => dto.AcumPeruPQ2MedGasVolGasLimaGasEnergia = Convert.ToDouble(value)},
                        {"AcumPeruPQ2MedGasVolGasGasNorpVolumen", value => dto.AcumPeruPQ2MedGasVolGasGasNorpVolumen = Convert.ToDouble(value)},
                        {"AcumPeruPQ2MedGasVolGasGasNorpPoderCal", value => dto.AcumPeruPQ2MedGasVolGasGasNorpPoderCal = Convert.ToDouble(value)},
                        {"AcumPeruPQ2MedGasVolGasGasNorpEnergia", value => dto.AcumPeruPQ2MedGasVolGasGasNorpEnergia = Convert.ToDouble(value)},
                        {"AcumPeruPQ2MedGasVolGasQuemadoVolumen", value => dto.AcumPeruPQ2MedGasVolGasQuemadoVolumen = Convert.ToDouble(value)},
                        {"AcumPeruPQ2MedGasVolGasQuemadoPoderCal", value => dto.AcumPeruPQ2MedGasVolGasQuemadoPoderCal = Convert.ToDouble(value)},
                        {"AcumPeruPQ2MedGasVolGasQuemadoEnergia", value => dto.AcumPeruPQ2MedGasVolGasQuemadoEnergia = Convert.ToDouble(value)},

                        {"DifUPQ1MedGasGasNatAsocMedVolumen", value => dto.DifUPQ1MedGasGasNatAsocMedVolumen = Convert.ToDouble(value)},
                        {"DifUPQ1MedGasGasNatAsocMedPoderCal", value => dto.DifUPQ1MedGasGasNatAsocMedPoderCal = Convert.ToDouble(value)},
                        {"DifUPQ1MedGasGasNatAsocMedEnergia", value => dto.DifUPQ1MedGasGasNatAsocMedEnergia = Convert.ToDouble(value)},
                        {"DifUPQ1MedGasGasCombSecoMedVolumen", value => dto.DifUPQ1MedGasGasCombSecoMedVolumen = Convert.ToDouble(value)},
                        {"DifUPQ1MedGasGasCombSecoMedPoderCal", value => dto.DifUPQ1MedGasGasCombSecoMedPoderCal = Convert.ToDouble(value)},
                        {"DifUPQ1MedGasGasCombSecoMedEnergia", value => dto.DifUPQ1MedGasGasCombSecoMedEnergia = Convert.ToDouble(value)},
                        {"DifUPQ1MedGasVolGasEquivLgnVolumen", value => dto.DifUPQ1MedGasVolGasEquivLgnVolumen = Convert.ToDouble(value)},
                        {"DifUPQ1MedGasVolGasEquivLgnPoderCal", value => dto.DifUPQ1MedGasVolGasEquivLgnPoderCal = Convert.ToDouble(value)},
                        {"DifUPQ1MedGasVolGasEquivLgnEnergia", value => dto.DifUPQ1MedGasVolGasEquivLgnEnergia = Convert.ToDouble(value)},
                        {"DifUPQ1MedGasVolGasClienteVolumen", value => dto.DifUPQ1MedGasVolGasClienteVolumen = Convert.ToDouble(value)},
                        {"DifUPQ1MedGasVolGasClientePoderCal", value => dto.DifUPQ1MedGasVolGasClientePoderCal = Convert.ToDouble(value)},
                        {"DifUPQ1MedGasVolGasClienteEnergia", value => dto.DifUPQ1MedGasVolGasClienteEnergia = Convert.ToDouble(value)},
                        {"DifUPQ1MedGasVolGasSaviaVolumen", value => dto.DifUPQ1MedGasVolGasSaviaVolumen = Convert.ToDouble(value)},
                        {"DifUPQ1MedGasVolGasSaviaPoderCal", value => dto.DifUPQ1MedGasVolGasSaviaPoderCal = Convert.ToDouble(value)},
                        {"DifUPQ1MedGasVolGasSaviaEnergia", value => dto.DifUPQ1MedGasVolGasSaviaEnergia = Convert.ToDouble(value)},
                        {"DifUPQ1MedGasVolGasLimaGasVolumen", value => dto.DifUPQ1MedGasVolGasLimaGasVolumen = Convert.ToDouble(value)},
                        {"DifUPQ1MedGasVolGasLimaGasPoderCal", value => dto.DifUPQ1MedGasVolGasLimaGasPoderCal = Convert.ToDouble(value)},
                        {"DifUPQ1MedGasVolGasLimaGasEnergia", value => dto.DifUPQ1MedGasVolGasLimaGasEnergia = Convert.ToDouble(value)},
                        {"DifUPQ1MedGasVolGasGasNorpVolumen", value => dto.DifUPQ1MedGasVolGasGasNorpVolumen = Convert.ToDouble(value)},
                        {"DifUPQ1MedGasVolGasGasNorpPoderCal", value => dto.DifUPQ1MedGasVolGasGasNorpPoderCal = Convert.ToDouble(value)},
                        {"DifUPQ1MedGasVolGasGasNorpEnergia", value => dto.DifUPQ1MedGasVolGasGasNorpEnergia = Convert.ToDouble(value)},
                        {"DifUPQ1MedGasVolGasQuemadoVolumen", value => dto.DifUPQ1MedGasVolGasQuemadoVolumen = Convert.ToDouble(value)},
                        {"DifUPQ1MedGasVolGasQuemadoPoderCal", value => dto.DifUPQ1MedGasVolGasQuemadoPoderCal = Convert.ToDouble(value)},
                        {"DifUPQ1MedGasVolGasQuemadoEnergia", value => dto.DifUPQ1MedGasVolGasQuemadoEnergia = Convert.ToDouble(value)},
    
                        // Diff Unna-PeruPetro Quincena 2
                        {"DifUPQ2MedGasGasNatAsocMedVolumen", value => dto.DifUPQ2MedGasGasNatAsocMedVolumen = Convert.ToDouble(value)},
                        {"DifUPQ2MedGasGasNatAsocMedPoderCal", value => dto.DifUPQ2MedGasGasNatAsocMedPoderCal = Convert.ToDouble(value)},
                        {"DifUPQ2MedGasGasNatAsocMedEnergia", value => dto.DifUPQ2MedGasGasNatAsocMedEnergia = Convert.ToDouble(value)},
                        {"DifUPQ2MedGasGasCombSecoMedVolumen", value => dto.DifUPQ2MedGasGasCombSecoMedVolumen = Convert.ToDouble(value)},
                        {"DifUPQ2MedGasGasCombSecoMedPoderCal", value => dto.DifUPQ2MedGasGasCombSecoMedPoderCal = Convert.ToDouble(value)},
                        {"DifUPQ2MedGasGasCombSecoMedEnergia", value => dto.DifUPQ2MedGasGasCombSecoMedEnergia = Convert.ToDouble(value)},
                        {"DifUPQ2MedGasVolGasEquivLgnVolumen", value => dto.DifUPQ2MedGasVolGasEquivLgnVolumen = Convert.ToDouble(value)},
                        {"DifUPQ2MedGasVolGasEquivLgnPoderCal", value => dto.DifUPQ2MedGasVolGasEquivLgnPoderCal = Convert.ToDouble(value)},
                        {"DifUPQ2MedGasVolGasEquivLgnEnergia", value => dto.DifUPQ2MedGasVolGasEquivLgnEnergia = Convert.ToDouble(value)},
                        {"DifUPQ2MedGasVolGasClienteVolumen", value => dto.DifUPQ2MedGasVolGasClienteVolumen = Convert.ToDouble(value)},
                        {"DifUPQ2MedGasVolGasClientePoderCal", value => dto.DifUPQ2MedGasVolGasClientePoderCal = Convert.ToDouble(value)},
                        {"DifUPQ2MedGasVolGasClienteEnergia", value => dto.DifUPQ2MedGasVolGasClienteEnergia = Convert.ToDouble(value)},
                        {"DifUPQ2MedGasVolGasSaviaVolumen", value => dto.DifUPQ2MedGasVolGasSaviaVolumen = Convert.ToDouble(value)},
                        {"DifUPQ2MedGasVolGasSaviaPoderCal", value => dto.DifUPQ2MedGasVolGasSaviaPoderCal = Convert.ToDouble(value)},
                        {"DifUPQ2MedGasVolGasSaviaEnergia", value => dto.DifUPQ2MedGasVolGasSaviaEnergia = Convert.ToDouble(value)},
                        {"DifUPQ2MedGasVolGasLimaGasVolumen", value => dto.DifUPQ2MedGasVolGasLimaGasVolumen = Convert.ToDouble(value)},
                        {"DifUPQ2MedGasVolGasLimaGasPoderCal", value => dto.DifUPQ2MedGasVolGasLimaGasPoderCal = Convert.ToDouble(value)},
                        {"DifUPQ2MedGasVolGasLimaGasEnergia", value => dto.DifUPQ2MedGasVolGasLimaGasEnergia = Convert.ToDouble(value)},
                        {"DifUPQ2MedGasVolGasGasNorpVolumen", value => dto.DifUPQ2MedGasVolGasGasNorpVolumen = Convert.ToDouble(value)},
                        {"DifUPQ2MedGasVolGasGasNorpPoderCal", value => dto.DifUPQ2MedGasVolGasGasNorpPoderCal = Convert.ToDouble(value)},
                        {"DifUPQ2MedGasVolGasGasNorpEnergia", value => dto.DifUPQ2MedGasVolGasGasNorpEnergia = Convert.ToDouble(value)},
                        {"DifUPQ2MedGasVolGasQuemadoVolumen", value => dto.DifUPQ2MedGasVolGasQuemadoVolumen = Convert.ToDouble(value)},
                        {"DifUPQ2MedGasVolGasQuemadoPoderCal", value => dto.DifUPQ2MedGasVolGasQuemadoPoderCal = Convert.ToDouble(value)},
                        {"DifUPQ2MedGasVolGasQuemadoEnergia", value => dto.DifUPQ2MedGasVolGasQuemadoEnergia = Convert.ToDouble(value)},

                        // GNA Fiscalizado - Acumulado Quincenal UNNA
                        {"AcumUnnaQ1GnaFiscVtaRefVolumen", value => dto.AcumUnnaQ1GnaFiscVtaRefVolumen = Convert.ToDouble(value)},
                        {"AcumUnnaQ1GnaFiscVtaRefPoderCal", value => dto.AcumUnnaQ1GnaFiscVtaRefPoderCal = Convert.ToDouble(value)},
                        {"AcumUnnaQ1GnaFiscVtaRefEnergia", value => dto.AcumUnnaQ1GnaFiscVtaRefEnergia = Convert.ToDouble(value)},
                        {"AcumUnnaQ1GnaFiscVtaLimaGasVolumen", value => dto.AcumUnnaQ1GnaFiscVtaLimaGasVolumen = Convert.ToDouble(value)},
                        {"AcumUnnaQ1GnaFiscVtaLimaGasPoderCal", value => dto.AcumUnnaQ1GnaFiscVtaLimaGasPoderCal = Convert.ToDouble(value)},
                        {"AcumUnnaQ1GnaFiscVtaLimaGasEnergia", value => dto.AcumUnnaQ1GnaFiscVtaLimaGasEnergia = Convert.ToDouble(value)},
                        {"AcumUnnaQ1GnaFiscGasNorpVolumen", value => dto.AcumUnnaQ1GnaFiscGasNorpVolumen = Convert.ToDouble(value)},
                        {"AcumUnnaQ1GnaFiscGasNorpPoderCal", value => dto.AcumUnnaQ1GnaFiscGasNorpPoderCal = Convert.ToDouble(value)},
                        {"AcumUnnaQ1GnaFiscGasNorpEnergia", value => dto.AcumUnnaQ1GnaFiscGasNorpEnergia = Convert.ToDouble(value)},
                        {"AcumUnnaQ1GnaFiscVtaEnelVolumen", value => dto.AcumUnnaQ1GnaFiscVtaEnelVolumen = Convert.ToDouble(value)},
                        {"AcumUnnaQ1GnaFiscVtaEnelPoderCal", value => dto.AcumUnnaQ1GnaFiscVtaEnelPoderCal = Convert.ToDouble(value)},
                        {"AcumUnnaQ1GnaFiscVtaEnelEnergia", value => dto.AcumUnnaQ1GnaFiscVtaEnelEnergia = Convert.ToDouble(value)},
                        {"AcumUnnaQ1GnaFiscGcyLgnVolumen", value => dto.AcumUnnaQ1GnaFiscGcyLgnVolumen = Convert.ToDouble(value)},
                        {"AcumUnnaQ1GnaFiscGcyLgnPoderCal", value => dto.AcumUnnaQ1GnaFiscGcyLgnPoderCal = Convert.ToDouble(value)},
                        {"AcumUnnaQ1GnaFiscGcyLgnEnergia", value => dto.AcumUnnaQ1GnaFiscGcyLgnEnergia = Convert.ToDouble(value)},
                        {"AcumUnnaQ1GnaFiscGnafVolumen", value => dto.AcumUnnaQ1GnaFiscGnafVolumen = Convert.ToDouble(value)},
                        {"AcumUnnaQ1GnaFiscGnafPoderCal", value => dto.AcumUnnaQ1GnaFiscGnafPoderCal = Convert.ToDouble(value)},
                        {"AcumUnnaQ1GnaFiscGnafEnergia", value => dto.AcumUnnaQ1GnaFiscGnafEnergia = Convert.ToDouble(value)},
                        {"AcumUnnaQ2GnaFiscVtaRefVolumen", value => dto.AcumUnnaQ2GnaFiscVtaRefVolumen = Convert.ToDouble(value)},
                        {"AcumUnnaQ2GnaFiscVtaRefPoderCal", value => dto.AcumUnnaQ2GnaFiscVtaRefPoderCal = Convert.ToDouble(value)},
                        {"AcumUnnaQ2GnaFiscVtaRefEnergia", value => dto.AcumUnnaQ2GnaFiscVtaRefEnergia = Convert.ToDouble(value)},
                        {"AcumUnnaQ2GnaFiscVtaLimaGasVolumen", value => dto.AcumUnnaQ2GnaFiscVtaLimaGasVolumen = Convert.ToDouble(value)},
                        {"AcumUnnaQ2GnaFiscVtaLimaGasPoderCal", value => dto.AcumUnnaQ2GnaFiscVtaLimaGasPoderCal = Convert.ToDouble(value)},
                        {"AcumUnnaQ2GnaFiscVtaLimaGasEnergia", value => dto.AcumUnnaQ2GnaFiscVtaLimaGasEnergia = Convert.ToDouble(value)},
                        {"AcumUnnaQ2GnaFiscGasNorpVolumen", value => dto.AcumUnnaQ2GnaFiscGasNorpVolumen = Convert.ToDouble(value)},
                        {"AcumUnnaQ2GnaFiscGasNorpPoderCal", value => dto.AcumUnnaQ2GnaFiscGasNorpPoderCal = Convert.ToDouble(value)},
                        {"AcumUnnaQ2GnaFiscGasNorpEnergia", value => dto.AcumUnnaQ2GnaFiscGasNorpEnergia = Convert.ToDouble(value)},
                        {"AcumUnnaQ2GnaFiscVtaEnelVolumen", value => dto.AcumUnnaQ2GnaFiscVtaEnelVolumen = Convert.ToDouble(value)},
                        {"AcumUnnaQ2GnaFiscVtaEnelPoderCal", value => dto.AcumUnnaQ2GnaFiscVtaEnelPoderCal = Convert.ToDouble(value)},
                        {"AcumUnnaQ2GnaFiscVtaEnelEnergia", value => dto.AcumUnnaQ2GnaFiscVtaEnelEnergia = Convert.ToDouble(value)},
                        {"AcumUnnaQ2GnaFiscGcyLgnVolumen", value => dto.AcumUnnaQ2GnaFiscGcyLgnVolumen = Convert.ToDouble(value)},
                        {"AcumUnnaQ2GnaFiscGcyLgnPoderCal", value => dto.AcumUnnaQ2GnaFiscGcyLgnPoderCal = Convert.ToDouble(value)},
                        {"AcumUnnaQ2GnaFiscGcyLgnEnergia", value => dto.AcumUnnaQ2GnaFiscGcyLgnEnergia = Convert.ToDouble(value)},
                        {"AcumUnnaQ2GnaFiscGnafVolumen", value => dto.AcumUnnaQ2GnaFiscGnafVolumen = Convert.ToDouble(value)},
                        {"AcumUnnaQ2GnaFiscGnafPoderCal", value => dto.AcumUnnaQ2GnaFiscGnafPoderCal = Convert.ToDouble(value)},
                        {"AcumUnnaQ2GnaFiscGnafEnergia", value => dto.AcumUnnaQ2GnaFiscGnafEnergia = Convert.ToDouble(value)},
    
                        // Acumulado Quincenal PERUPETRO
                        {"AcumPeruPQ1GnaFiscVtaRefVolumen", value => dto.AcumPeruPQ1GnaFiscVtaRefVolumen = Convert.ToDouble(value)},
                        {"AcumPeruPQ1GnaFiscVtaRefPoderCal", value => dto.AcumPeruPQ1GnaFiscVtaRefPoderCal = Convert.ToDouble(value)},
                        {"AcumPeruPQ1GnaFiscVtaRefEnergia", value => dto.AcumPeruPQ1GnaFiscVtaRefEnergia = Convert.ToDouble(value)},
                        {"AcumPeruPQ1GnaFiscVtaLimaGasVolumen", value => dto.AcumPeruPQ1GnaFiscVtaLimaGasVolumen = Convert.ToDouble(value)},
                        {"AcumPeruPQ1GnaFiscVtaLimaGasPoderCal", value => dto.AcumPeruPQ1GnaFiscVtaLimaGasPoderCal = Convert.ToDouble(value)},
                        {"AcumPeruPQ1GnaFiscVtaLimaGasEnergia", value => dto.AcumPeruPQ1GnaFiscVtaLimaGasEnergia = Convert.ToDouble(value)},
                        {"AcumPeruPQ1GnaFiscGasNorpVolumen", value => dto.AcumPeruPQ1GnaFiscGasNorpVolumen = Convert.ToDouble(value)},
                        {"AcumPeruPQ1GnaFiscGasNorpPoderCal", value => dto.AcumPeruPQ1GnaFiscGasNorpPoderCal = Convert.ToDouble(value)},
                        {"AcumPeruPQ1GnaFiscGasNorpEnergia", value => dto.AcumPeruPQ1GnaFiscGasNorpEnergia = Convert.ToDouble(value)},
                        {"AcumPeruPQ1GnaFiscVtaEnelVolumen", value => dto.AcumPeruPQ1GnaFiscVtaEnelVolumen = Convert.ToDouble(value)},
                        {"AcumPeruPQ1GnaFiscVtaEnelPoderCal", value => dto.AcumPeruPQ1GnaFiscVtaEnelPoderCal = Convert.ToDouble(value)},
                        {"AcumPeruPQ1GnaFiscVtaEnelEnergia", value => dto.AcumPeruPQ1GnaFiscVtaEnelEnergia = Convert.ToDouble(value)},
                        {"AcumPeruPQ1GnaFiscGcyLgnVolumen", value => dto.AcumPeruPQ1GnaFiscGcyLgnVolumen = Convert.ToDouble(value)},
                        {"AcumPeruPQ1GnaFiscGcyLgnPoderCal", value => dto.AcumPeruPQ1GnaFiscGcyLgnPoderCal = Convert.ToDouble(value)},
                        {"AcumPeruPQ1GnaFiscGcyLgnEnergia", value => dto.AcumPeruPQ1GnaFiscGcyLgnEnergia = Convert.ToDouble(value)},
                        {"AcumPeruPQ1GnaFiscGnafVolumen", value => dto.AcumPeruPQ1GnaFiscGnafVolumen = Convert.ToDouble(value)},
                        {"AcumPeruPQ1GnaFiscGnafPoderCal", value => dto.AcumPeruPQ1GnaFiscGnafPoderCal = Convert.ToDouble(value)},
                        {"AcumPeruPQ1GnaFiscGnafEnergia", value => dto.AcumPeruPQ1GnaFiscGnafEnergia = Convert.ToDouble(value)},
                        {"AcumPeruPQ2GnaFiscVtaRefVolumen", value => dto.AcumPeruPQ2GnaFiscVtaRefVolumen = Convert.ToDouble(value)},
                        {"AcumPeruPQ2GnaFiscVtaRefPoderCal", value => dto.AcumPeruPQ2GnaFiscVtaRefPoderCal = Convert.ToDouble(value)},
                        {"AcumPeruPQ2GnaFiscVtaRefEnergia", value => dto.AcumPeruPQ2GnaFiscVtaRefEnergia = Convert.ToDouble(value)},
                        {"AcumPeruPQ2GnaFiscVtaLimaGasVolumen", value => dto.AcumPeruPQ2GnaFiscVtaLimaGasVolumen = Convert.ToDouble(value)},
                        {"AcumPeruPQ2GnaFiscVtaLimaGasPoderCal", value => dto.AcumPeruPQ2GnaFiscVtaLimaGasPoderCal = Convert.ToDouble(value)},
                        {"AcumPeruPQ2GnaFiscVtaLimaGasEnergia", value => dto.AcumPeruPQ2GnaFiscVtaLimaGasEnergia = Convert.ToDouble(value)},
                        {"AcumPeruPQ2GnaFiscGasNorpVolumen", value => dto.AcumPeruPQ2GnaFiscGasNorpVolumen = Convert.ToDouble(value)},
                        {"AcumPeruPQ2GnaFiscGasNorpPoderCal", value => dto.AcumPeruPQ2GnaFiscGasNorpPoderCal = Convert.ToDouble(value)},
                        {"AcumPeruPQ2GnaFiscGasNorpEnergia", value => dto.AcumPeruPQ2GnaFiscGasNorpEnergia = Convert.ToDouble(value)},
                        {"AcumPeruPQ2GnaFiscVtaEnelVolumen", value => dto.AcumPeruPQ2GnaFiscVtaEnelVolumen = Convert.ToDouble(value)},
                        {"AcumPeruPQ2GnaFiscVtaEnelPoderCal", value => dto.AcumPeruPQ2GnaFiscVtaEnelPoderCal = Convert.ToDouble(value)},
                        {"AcumPeruPQ2GnaFiscVtaEnelEnergia", value => dto.AcumPeruPQ2GnaFiscVtaEnelEnergia = Convert.ToDouble(value)},
                        {"AcumPeruPQ2GnaFiscGcyLgnVolumen", value => dto.AcumPeruPQ2GnaFiscGcyLgnVolumen = Convert.ToDouble(value)},
                        {"AcumPeruPQ2GnaFiscGcyLgnPoderCal", value => dto.AcumPeruPQ2GnaFiscGcyLgnPoderCal = Convert.ToDouble(value)},
                        {"AcumPeruPQ2GnaFiscGcyLgnEnergia", value => dto.AcumPeruPQ2GnaFiscGcyLgnEnergia = Convert.ToDouble(value)},
                        {"AcumPeruPQ2GnaFiscGnafVolumen", value => dto.AcumPeruPQ2GnaFiscGnafVolumen = Convert.ToDouble(value)},
                        {"AcumPeruPQ2GnaFiscGnafPoderCal", value => dto.AcumPeruPQ2GnaFiscGnafPoderCal = Convert.ToDouble(value)},
                        {"AcumPeruPQ2GnaFiscGnafEnergia", value => dto.AcumPeruPQ2GnaFiscGnafEnergia = Convert.ToDouble(value)},

                        // Acumulado Total PERUPETRO
                        {"AcumPeruPTotalGnaFiscVtaRefVolumen", value => dto.AcumPeruPTotalGnaFiscVtaRefVolumen = Convert.ToDouble(value)},
                        {"AcumPeruPTotalGnaFiscVtaRefPoderCal", value => dto.AcumPeruPTotalGnaFiscVtaRefPoderCal = Convert.ToDouble(value)},
                        {"AcumPeruPTotalGnaFiscVtaRefEnergia", value => dto.AcumPeruPTotalGnaFiscVtaRefEnergia = Convert.ToDouble(value)},
                        {"AcumPeruPTotalGnaFiscVtaLimaGasVolumen", value => dto.AcumPeruPTotalGnaFiscVtaLimaGasVolumen = Convert.ToDouble(value)},
                        {"AcumPeruPTotalGnaFiscVtaLimaGasPoderCal", value => dto.AcumPeruPTotalGnaFiscVtaLimaGasPoderCal = Convert.ToDouble(value)},
                        {"AcumPeruPTotalGnaFiscVtaLimaGasEnergia", value => dto.AcumPeruPTotalGnaFiscVtaLimaGasEnergia = Convert.ToDouble(value)},
                        {"AcumPeruPTotalGnaFiscGasNorpVolumen", value => dto.AcumPeruPTotalGnaFiscGasNorpVolumen = Convert.ToDouble(value)},
                        {"AcumPeruPTotalGnaFiscGasNorpPoderCal", value => dto.AcumPeruPTotalGnaFiscGasNorpPoderCal = Convert.ToDouble(value)},
                        {"AcumPeruPTotalGnaFiscGasNorpEnergia", value => dto.AcumPeruPTotalGnaFiscGasNorpEnergia = Convert.ToDouble(value)},
                        {"AcumPeruPTotalGnaFiscVtaEnelVolumen", value => dto.AcumPeruPTotalGnaFiscVtaEnelVolumen = Convert.ToDouble(value)},
                        {"AcumPeruPTotalGnaFiscVtaEnelPoderCal", value => dto.AcumPeruPTotalGnaFiscVtaEnelPoderCal = Convert.ToDouble(value)},
                        {"AcumPeruPTotalGnaFiscVtaEnelEnergia", value => dto.AcumPeruPTotalGnaFiscVtaEnelEnergia = Convert.ToDouble(value)},
                        {"AcumPeruPTotalGnaFiscGcyLgnVolumen", value => dto.AcumPeruPTotalGnaFiscGcyLgnVolumen = Convert.ToDouble(value)},
                        {"AcumPeruPTotalGnaFiscGcyLgnPoderCal", value => dto.AcumPeruPTotalGnaFiscGcyLgnPoderCal = Convert.ToDouble(value)},
                        {"AcumPeruPTotalGnaFiscGcyLgnEnergia", value => dto.AcumPeruPTotalGnaFiscGcyLgnEnergia = Convert.ToDouble(value)},
                        {"AcumPeruPTotalGnaFiscGnafVolumen", value => dto.AcumPeruPTotalGnaFiscGnafVolumen = Convert.ToDouble(value)},
                        {"AcumPeruPTotalGnaFiscGnafPoderCal", value => dto.AcumPeruPTotalGnaFiscGnafPoderCal = Convert.ToDouble(value)},
                        {"AcumPeruPTotalGnaFiscGnafEnergia", value => dto.AcumPeruPTotalGnaFiscGnafEnergia = Convert.ToDouble(value)},
    
                        // Acumulado Total UNNA
                        {"AcumUnnaTotalGnaFiscVtaRefVolumen", value => dto.AcumUnnaTotalGnaFiscVtaRefVolumen = Convert.ToDouble(value)},
                        {"AcumUnnaTotalGnaFiscVtaRefPoderCal", value => dto.AcumUnnaTotalGnaFiscVtaRefPoderCal = Convert.ToDouble(value)},
                        {"AcumUnnaTotalGnaFiscVtaRefEnergia", value => dto.AcumUnnaTotalGnaFiscVtaRefEnergia = Convert.ToDouble(value)},
                        {"AcumUnnaTotalGnaFiscVtaLimaGasVolumen", value => dto.AcumUnnaTotalGnaFiscVtaLimaGasVolumen = Convert.ToDouble(value)},
                        {"AcumUnnaTotalGnaFiscVtaLimaGasPoderCal", value => dto.AcumUnnaTotalGnaFiscVtaLimaGasPoderCal = Convert.ToDouble(value)},
                        {"AcumUnnaTotalGnaFiscVtaLimaGasEnergia", value => dto.AcumUnnaTotalGnaFiscVtaLimaGasEnergia = Convert.ToDouble(value)},
                        {"AcumUnnaTotalGnaFiscGasNorpVolumen", value => dto.AcumUnnaTotalGnaFiscGasNorpVolumen = Convert.ToDouble(value)},
                        {"AcumUnnaTotalGnaFiscGasNorpPoderCal", value => dto.AcumUnnaTotalGnaFiscGasNorpPoderCal = Convert.ToDouble(value)},
                        {"AcumUnnaTotalGnaFiscGasNorpEnergia", value => dto.AcumUnnaTotalGnaFiscGasNorpEnergia = Convert.ToDouble(value)},
                        {"AcumUnnaTotalGnaFiscVtaEnelVolumen", value => dto.AcumUnnaTotalGnaFiscVtaEnelVolumen = Convert.ToDouble(value)},
                        {"AcumUnnaTotalGnaFiscVtaEnelPoderCal", value => dto.AcumUnnaTotalGnaFiscVtaEnelPoderCal = Convert.ToDouble(value)},
                        {"AcumUnnaTotalGnaFiscVtaEnelEnergia", value => dto.AcumUnnaTotalGnaFiscVtaEnelEnergia = Convert.ToDouble(value)},
                        {"AcumUnnaTotalGnaFiscGcyLgnVolumen", value => dto.AcumUnnaTotalGnaFiscGcyLgnVolumen = Convert.ToDouble(value)},
                        {"AcumUnnaTotalGnaFiscGcyLgnPoderCal", value => dto.AcumUnnaTotalGnaFiscGcyLgnPoderCal = Convert.ToDouble(value)},
                        {"AcumUnnaTotalGnaFiscGcyLgnEnergia", value => dto.AcumUnnaTotalGnaFiscGcyLgnEnergia = Convert.ToDouble(value)},
                        {"AcumUnnaTotalGnaFiscGnafVolumen", value => dto.AcumUnnaTotalGnaFiscGnafVolumen = Convert.ToDouble(value)},
                        {"AcumUnnaTotalGnaFiscGnafPoderCal", value => dto.AcumUnnaTotalGnaFiscGnafPoderCal = Convert.ToDouble(value)},
                        {"AcumUnnaTotalGnaFiscGnafEnergia", value => dto.AcumUnnaTotalGnaFiscGnafEnergia = Convert.ToDouble(value)},

                        // Diff Unna-PeruPetro Quincena 1
                        {"DifUPQ1GnaFiscVtaRefVolumen", value => dto.DifUPQ1GnaFiscVtaRefVolumen = Convert.ToDouble(value)},
                        {"DifUPQ1GnaFiscVtaRefPoderCal", value => dto.DifUPQ1GnaFiscVtaRefPoderCal = Convert.ToDouble(value)},
                        {"DifUPQ1GnaFiscVtaRefEnergia", value => dto.DifUPQ1GnaFiscVtaRefEnergia = Convert.ToDouble(value)},
                        {"DifUPQ1GnaFiscVtaLimaGasVolumen", value => dto.DifUPQ1GnaFiscVtaLimaGasVolumen = Convert.ToDouble(value)},
                        {"DifUPQ1GnaFiscVtaLimaGasPoderCal", value => dto.DifUPQ1GnaFiscVtaLimaGasPoderCal = Convert.ToDouble(value)},
                        {"DifUPQ1GnaFiscVtaLimaGasEnergia", value => dto.DifUPQ1GnaFiscVtaLimaGasEnergia = Convert.ToDouble(value)},
                        {"DifUPQ1GnaFiscGasNorpVolumen", value => dto.DifUPQ1GnaFiscGasNorpVolumen = Convert.ToDouble(value)},
                        {"DifUPQ1GnaFiscGasNorpPoderCal", value => dto.DifUPQ1GnaFiscGasNorpPoderCal = Convert.ToDouble(value)},
                        {"DifUPQ1GnaFiscGasNorpEnergia", value => dto.DifUPQ1GnaFiscGasNorpEnergia = Convert.ToDouble(value)},
                        {"DifUPQ1GnaFiscVtaEnelVolumen", value => dto.DifUPQ1GnaFiscVtaEnelVolumen = Convert.ToDouble(value)},
                        {"DifUPQ1GnaFiscVtaEnelPoderCal", value => dto.DifUPQ1GnaFiscVtaEnelPoderCal = Convert.ToDouble(value)},
                        {"DifUPQ1GnaFiscVtaEnelEnergia", value => dto.DifUPQ1GnaFiscVtaEnelEnergia = Convert.ToDouble(value)},
                        {"DifUPQ1GnaFiscGcyLgnVolumen", value => dto.DifUPQ1GnaFiscGcyLgnVolumen = Convert.ToDouble(value)},
                        {"DifUPQ1GnaFiscGcyLgnPoderCal", value => dto.DifUPQ1GnaFiscGcyLgnPoderCal = Convert.ToDouble(value)},
                        {"DifUPQ1GnaFiscGcyLgnEnergia", value => dto.DifUPQ1GnaFiscGcyLgnEnergia = Convert.ToDouble(value)},
                        {"DifUPQ1GnaFiscGnafVolumen", value => dto.DifUPQ1GnaFiscGnafVolumen = Convert.ToDouble(value)},
                        {"DifUPQ1GnaFiscGnafPoderCal", value => dto.DifUPQ1GnaFiscGnafPoderCal = Convert.ToDouble(value)},
                        {"DifUPQ1GnaFiscGnafEnergia", value => dto.DifUPQ1GnaFiscGnafEnergia = Convert.ToDouble(value)},

                        // Diff Unna-PeruPetro Quincena 2
                        {"DifUPQ2GnaFiscVtaRefVolumen", value => dto.DifUPQ2GnaFiscVtaRefVolumen = Convert.ToDouble(value)},
                        {"DifUPQ2GnaFiscVtaRefPoderCal", value => dto.DifUPQ2GnaFiscVtaRefPoderCal = Convert.ToDouble(value)},
                        {"DifUPQ2GnaFiscVtaRefEnergia", value => dto.DifUPQ2GnaFiscVtaRefEnergia = Convert.ToDouble(value)},
                        {"DifUPQ2GnaFiscVtaLimaGasVolumen", value => dto.DifUPQ2GnaFiscVtaLimaGasVolumen = Convert.ToDouble(value)},
                        {"DifUPQ2GnaFiscVtaLimaGasPoderCal", value => dto.DifUPQ2GnaFiscVtaLimaGasPoderCal = Convert.ToDouble(value)},
                        {"DifUPQ2GnaFiscVtaLimaGasEnergia", value => dto.DifUPQ2GnaFiscVtaLimaGasEnergia = Convert.ToDouble(value)},
                        {"DifUPQ2GnaFiscGasNorpVolumen", value => dto.DifUPQ2GnaFiscGasNorpVolumen = Convert.ToDouble(value)},
                        {"DifUPQ2GnaFiscGasNorpPoderCal", value => dto.DifUPQ2GnaFiscGasNorpPoderCal = Convert.ToDouble(value)},
                        {"DifUPQ2GnaFiscGasNorpEnergia", value => dto.DifUPQ2GnaFiscGasNorpEnergia = Convert.ToDouble(value)},
                        {"DifUPQ2GnaFiscVtaEnelVolumen", value => dto.DifUPQ2GnaFiscVtaEnelVolumen = Convert.ToDouble(value)},
                        {"DifUPQ2GnaFiscVtaEnelPoderCal", value => dto.DifUPQ2GnaFiscVtaEnelPoderCal = Convert.ToDouble(value)},
                        {"DifUPQ2GnaFiscVtaEnelEnergia", value => dto.DifUPQ2GnaFiscVtaEnelEnergia = Convert.ToDouble(value)},
                        {"DifUPQ2GnaFiscGcyLgnVolumen", value => dto.DifUPQ2GnaFiscGcyLgnVolumen = Convert.ToDouble(value)},
                        {"DifUPQ2GnaFiscGcyLgnPoderCal", value => dto.DifUPQ2GnaFiscGcyLgnPoderCal = Convert.ToDouble(value)},
                        {"DifUPQ2GnaFiscGcyLgnEnergia", value => dto.DifUPQ2GnaFiscGcyLgnEnergia = Convert.ToDouble(value)},
                        {"DifUPQ2GnaFiscGnafVolumen", value => dto.DifUPQ2GnaFiscGnafVolumen = Convert.ToDouble(value)},
                        {"DifUPQ2GnaFiscGnafPoderCal", value => dto.DifUPQ2GnaFiscGnafPoderCal = Convert.ToDouble(value)},
                        {"DifUPQ2GnaFiscGnafEnergia", value => dto.DifUPQ2GnaFiscGnafEnergia = Convert.ToDouble(value)},


                        // LGN CUADRO1 
                        {"MedGasGlpVolumenQ1", value => dto.MedGasGlpVolumenQ1 = Convert.ToDouble(value)},
                        {"MedGasGlpPoderCalQ1", value => dto.MedGasGlpPoderCalQ1 = Convert.ToDouble(value)},
                        {"MedGasGlpEnergiaQ1", value => dto.MedGasGlpEnergiaQ1 = Convert.ToDouble(value)},
                        {"MedGasGlpDensidadQ1", value => dto.MedGasGlpDensidadQ1 = Convert.ToDouble(value)},
                        {"MedGasCgnVolumenQ1", value => dto.MedGasCgnVolumenQ1 = Convert.ToDouble(value)},
                        {"MedGasCgnPoderCalQ1", value => dto.MedGasCgnPoderCalQ1 = Convert.ToDouble(value)},
                        {"MedGasCgnEnergiaQ1", value => dto.MedGasCgnEnergiaQ1 = Convert.ToDouble(value)},
                        {"MedGasLgnVolumenQ1", value => dto.MedGasLgnVolumenQ1 = Convert.ToDouble(value)},
                        {"MedGasLgnPoderCalQ1", value => dto.MedGasLgnPoderCalQ1 = Convert.ToDouble(value)},
                        {"MedGasLgnEnergiaQ1", value => dto.MedGasLgnEnergiaQ1 = Convert.ToDouble(value)},

                        {"MedGasGlpVolumenQ2", value => dto.MedGasGlpVolumenQ2 = Convert.ToDouble(value)},
                        {"MedGasGlpPoderCalQ2", value => dto.MedGasGlpPoderCalQ2 = Convert.ToDouble(value)},
                        {"MedGasGlpEnergiaQ2", value => dto.MedGasGlpEnergiaQ2 = Convert.ToDouble(value)},
                        {"MedGasGlpDensidadQ2", value => dto.MedGasGlpDensidadQ2 = Convert.ToDouble(value)},
                        {"MedGasCgnVolumenQ2", value => dto.MedGasCgnVolumenQ2 = Convert.ToDouble(value)},
                        {"MedGasCgnPoderCalQ2", value => dto.MedGasCgnPoderCalQ2 = Convert.ToDouble(value)},
                        {"MedGasCgnEnergiaQ2", value => dto.MedGasCgnEnergiaQ2 = Convert.ToDouble(value)},
                        {"MedGasLgnVolumenQ2", value => dto.MedGasLgnVolumenQ2 = Convert.ToDouble(value)},
                        {"MedGasLgnPoderCalQ2", value => dto.MedGasLgnPoderCalQ2 = Convert.ToDouble(value)},
                        {"MedGasLgnEnergiaQ2", value => dto.MedGasLgnEnergiaQ2 = Convert.ToDouble(value)},



                        {"MedGasGlpVolumenQ1S2", value => dto.MedGasGlpVolumenQ1S2 = Convert.ToDouble(value)},
                        {"MedGasGlpPoderCalQ1S2", value => dto.MedGasGlpPoderCalQ1S2 = Convert.ToDouble(value)},
                        {"MedGasGlpEnergiaQ1S2", value => dto.MedGasGlpEnergiaQ1S2 = Convert.ToDouble(value)},
                        {"MedGasGlpDensidadQ1S2", value => dto.MedGasGlpDensidadQ1S2 = Convert.ToDouble(value)},
                        {"MedGasCgnVolumenQ1S2", value => dto.MedGasCgnVolumenQ1S2 = Convert.ToDouble(value)},
                        {"MedGasCgnPoderCalQ1S2", value => dto.MedGasCgnPoderCalQ1S2 = Convert.ToDouble(value)},
                        {"MedGasCgnEnergiaQ1S2", value => dto.MedGasCgnEnergiaQ1S2 = Convert.ToDouble(value)},
                        {"MedGasLgnVolumenQ1S2", value => dto.MedGasLgnVolumenQ1S2 = Convert.ToDouble(value)},
                        {"MedGasLgnPoderCalQ1S2", value => dto.MedGasLgnPoderCalQ1S2 = Convert.ToDouble(value)},
                        {"MedGasLgnEnergiaQ1S2", value => dto.MedGasLgnEnergiaQ1S2 = Convert.ToDouble(value)},

                        {"MedGasGlpVolumenQ2S2", value => dto.MedGasGlpVolumenQ2S2 = Convert.ToDouble(value)},
                        {"MedGasGlpPoderCalQ2S2", value => dto.MedGasGlpPoderCalQ2S2 = Convert.ToDouble(value)},
                        {"MedGasGlpEnergiaQ2S2", value => dto.MedGasGlpEnergiaQ2S2 = Convert.ToDouble(value)},
                        {"MedGasGlpDensidadQ2S2", value => dto.MedGasGlpDensidadQ2S2 = Convert.ToDouble(value)},
                        {"MedGasCgnVolumenQ2S2", value => dto.MedGasCgnVolumenQ2S2 = Convert.ToDouble(value)},
                        {"MedGasCgnPoderCalQ2S2", value => dto.MedGasCgnPoderCalQ2S2 = Convert.ToDouble(value)},
                        {"MedGasCgnEnergiaQ2S2", value => dto.MedGasCgnEnergiaQ2S2 = Convert.ToDouble(value)},
                        {"MedGasLgnVolumenQ2S2", value => dto.MedGasLgnVolumenQ2S2 = Convert.ToDouble(value)},
                        {"MedGasLgnPoderCalQ2S2", value => dto.MedGasLgnPoderCalQ2S2 = Convert.ToDouble(value)},
                        {"MedGasLgnEnergiaQ2S2", value => dto.MedGasLgnEnergiaQ2S2 = Convert.ToDouble(value)},



                        {"AcumMedGasGlpVolumenQ1S2", value => dto.AcumMedGasGlpVolumenQ1S2 = Convert.ToDouble(value)},
                        {"AcumMedGasGlpPoderCalQ1S2", value => dto.AcumMedGasGlpPoderCalQ1S2 = Convert.ToDouble(value)},
                        {"AcumMedGasGlpEnergiaQ1S2", value => dto.AcumMedGasGlpEnergiaQ1S2 = Convert.ToDouble(value)},
                        {"AcumMedGasGlpDensidadQ1S2", value => dto.AcumMedGasGlpDensidadQ1S2 = Convert.ToDouble(value)},
                        {"AcumMedGasCgnVolumenQ1S2", value => dto.AcumMedGasCgnVolumenQ1S2 = Convert.ToDouble(value)},
                        {"AcumMedGasCgnPoderCalQ1S2", value => dto.AcumMedGasCgnPoderCalQ1S2 = Convert.ToDouble(value)},
                        {"AcumMedGasCgnEnergiaQ1S2", value => dto.AcumMedGasCgnEnergiaQ1S2 = Convert.ToDouble(value)},
                        {"AcumMedGasLgnVolumenQ1S2", value => dto.AcumMedGasLgnVolumenQ1S2 = Convert.ToDouble(value)},
                        {"AcumMedGasLgnPoderCalQ1S2", value => dto.AcumMedGasLgnPoderCalQ1S2 = Convert.ToDouble(value)},
                        {"AcumMedGasLgnEnergiaQ1S2", value => dto.AcumMedGasLgnEnergiaQ1S2 = Convert.ToDouble(value)},

                        {"AcumMedGasGlpVolumenQ2S2", value => dto.AcumMedGasGlpVolumenQ2S2 = Convert.ToDouble(value)},
                        {"AcumMedGasGlpPoderCalQ2S2", value => dto.AcumMedGasGlpPoderCalQ2S2 = Convert.ToDouble(value)},
                        {"AcumMedGasGlpEnergiaQ2S2", value => dto.AcumMedGasGlpEnergiaQ2S2 = Convert.ToDouble(value)},
                        {"AcumMedGasGlpDensidadQ2S2", value => dto.AcumMedGasGlpDensidadQ2S2 = Convert.ToDouble(value)},
                        {"AcumMedGasCgnVolumenQ2S2", value => dto.AcumMedGasCgnVolumenQ2S2 = Convert.ToDouble(value)},
                        {"AcumMedGasCgnPoderCalQ2S2", value => dto.AcumMedGasCgnPoderCalQ2S2 = Convert.ToDouble(value)},
                        {"AcumMedGasCgnEnergiaQ2S2", value => dto.AcumMedGasCgnEnergiaQ2S2 = Convert.ToDouble(value)},
                        {"AcumMedGasLgnVolumenQ2S2", value => dto.AcumMedGasLgnVolumenQ2S2 = Convert.ToDouble(value)},
                        {"AcumMedGasLgnPoderCalQ2S2", value => dto.AcumMedGasLgnPoderCalQ2S2 = Convert.ToDouble(value)},
                        {"AcumMedGasLgnEnergiaQ2S2", value => dto.AcumMedGasLgnEnergiaQ2S2 = Convert.ToDouble(value)},
                    };

                    dto.DensidadGLPKgBl = rootObject.ParametrosLGN.DensidadGLPKgBl;
                    dto.PCGLPMMBtuBl60F = rootObject.ParametrosLGN.PCGLPMMBtuBl60F;
                    dto.PCCGNMMBtuBl60F = rootObject.ParametrosLGN.PCCGNMMBtuBl60F;
                    dto.PCLGNMMBtuBl60F = rootObject.ParametrosLGN.PCLGNMMBtuBl60F;
                    dto.FactorConversionSCFDGal = rootObject.ParametrosLGN.FactorConversionSCFDGal;

                    dto.EnergiaMMBTUQ1GLP = rootObject.ParametrosLGN.EnergiaMMBTUQ1GLP;
                    dto.EnergiaMMBTUQ1CGN = rootObject.ParametrosLGN.EnergiaMMBTUQ1CGN;

                    dto.DensidadGLPKgBlQ2 = rootObject.ParametrosLGN.DensidadGLPKgBlQ2;
                    dto.PCGLPMMBtuBl60FQ2 = rootObject.ParametrosLGN.PCGLPMMBtuBl60FQ2;
                    dto.PCCGNMMBtuBl60FQ2 = rootObject.ParametrosLGN.PCCGNMMBtuBl60FQ2;
                    dto.PCLGNMMBtuBl60FQ2 = rootObject.ParametrosLGN.PCLGNMMBtuBl60FQ2;
                    dto.FactorConversionSCFDGalQ2 = rootObject.ParametrosLGN.FactorConversionSCFDGalQ2;

                    dto.EnergiaMMBTUQ2GLP = rootObject.ParametrosLGN.EnergiaMMBTUQ2GLP;
                    dto.EnergiaMMBTUQ2CGN = rootObject.ParametrosLGN.EnergiaMMBTUQ2CGN;


                    try
                    {
                        foreach (var diaMedicion in rootObject.DatosDiarios)
                        {
                            foreach (var medicion in diaMedicion.Mediciones)
                            {
                                if (propertyMap.ContainsKey(medicion.ID))
                                {
                                    try
                                    {                                        double valorConvertido;
                                        if (double.TryParse(medicion.Valor, out valorConvertido))
                                        {
                                            propertyMap[medicion.ID](medicion.Valor);
                                        }
                                        else
                                        {
                                            // Manejar el caso en que la conversión falle
                                            Console.WriteLine($"Error al convertir el valor: {medicion.Valor} para la medición: {medicion.ID}");
                                        }
                                    }
                                    catch (FormatException ex)
                                    {
                                        Console.WriteLine($"Error de formato al convertir el valor: {medicion.Valor} para la medición: {medicion.ID} - {ex.Message}");
                                    }
                                    catch (OverflowException ex)
                                    {
                                        Console.WriteLine($"Desbordamiento al convertir el valor: {medicion.Valor} para la medición: {medicion.ID} - {ex.Message}");
                                    }
                                }
                            }
                        }

                    }
                    catch (Exception ex)
                    {

                       Console.WriteLine(ex);
                    }

                   

                    dto.ResBalanceEnergLIVDetMedGas = rootObject.DatosDiarios
                        .SelectMany(d => d.Mediciones
                            .Where(m => m.ID.StartsWith("tbResBalanceEnergLIVDetMedGas_"))
                            .GroupBy(m => GetDayFromID(m.ID))
                            .Select(g => new ResBalanceEnergLIVDetMedGasDto
                            {
                                Dia = g.Key,
                                MedGasGasNatAsocMedVolumen = g.FirstOrDefault(m => m.ID.Contains("MedGasGasNatAsocMedVolumen"))?.Valor != null ? Convert.ToDouble(g.FirstOrDefault(m => m.ID.Contains("MedGasGasNatAsocMedVolumen")).Valor) : (double?)null,
                                MedGasGasNatAsocMedPoderCal = g.FirstOrDefault(m => m.ID.Contains("MedGasGasNatAsocMedPoderCal"))?.Valor != null ? Convert.ToDouble(g.FirstOrDefault(m => m.ID.Contains("MedGasGasNatAsocMedPoderCal")).Valor) : (double?)null,
                                MedGasGasNatAsocMedEnergia = g.FirstOrDefault(m => m.ID.Contains("MedGasGasNatAsocMedEnergia"))?.Valor != null ? Convert.ToDouble(g.FirstOrDefault(m => m.ID.Contains("MedGasGasNatAsocMedEnergia")).Valor) : (double?)null,
                                MedGasGasCombSecoMedVolumen = g.FirstOrDefault(m => m.ID.Contains("MedGasGasCombSecoMedVolumen"))?.Valor != null ? Convert.ToDouble(g.FirstOrDefault(m => m.ID.Contains("MedGasGasCombSecoMedVolumen")).Valor) : (double?)null,
                                MedGasGasCombSecoMedPoderCal = g.FirstOrDefault(m => m.ID.Contains("MedGasGasCombSecoMedPoderCal"))?.Valor != null ? Convert.ToDouble(g.FirstOrDefault(m => m.ID.Contains("MedGasGasCombSecoMedPoderCal")).Valor) : (double?)null,
                                MedGasGasCombSecoMedEnergia = g.FirstOrDefault(m => m.ID.Contains("MedGasGasCombSecoMedEnergia"))?.Valor != null ? Convert.ToDouble(g.FirstOrDefault(m => m.ID.Contains("MedGasGasCombSecoMedEnergia")).Valor) : (double?)null,
                                MedGasVolGasEquivLgnVolumen = g.FirstOrDefault(m => m.ID.Contains("MedGasVolGasEquivLgnVolumen"))?.Valor != null ? Convert.ToDouble(g.FirstOrDefault(m => m.ID.Contains("MedGasVolGasEquivLgnVolumen")).Valor) : (double?)null,
                                MedGasVolGasEquivLgnPoderCal = g.FirstOrDefault(m => m.ID.Contains("MedGasVolGasEquivLgnPoderCal"))?.Valor != null ? Convert.ToDouble(g.FirstOrDefault(m => m.ID.Contains("MedGasVolGasEquivLgnPoderCal")).Valor) : (double?)null,
                                MedGasVolGasEquivLgnEnergia = g.FirstOrDefault(m => m.ID.Contains("MedGasVolGasEquivLgnEnergia"))?.Valor != null ? Convert.ToDouble(g.FirstOrDefault(m => m.ID.Contains("MedGasVolGasEquivLgnEnergia")).Valor) : (double?)null,
                                MedGasVolGasClienteVolumen = g.FirstOrDefault(m => m.ID.Contains("MedGasVolGasClienteVolumen"))?.Valor != null ? Convert.ToDouble(g.FirstOrDefault(m => m.ID.Contains("MedGasVolGasClienteVolumen")).Valor) : (double?)null,
                                MedGasVolGasClientePoderCal = g.FirstOrDefault(m => m.ID.Contains("MedGasVolGasClientePoderCal"))?.Valor != null ? Convert.ToDouble(g.FirstOrDefault(m => m.ID.Contains("MedGasVolGasClientePoderCal")).Valor) : (double?)null,
                                MedGasVolGasClienteEnergia = g.FirstOrDefault(m => m.ID.Contains("MedGasVolGasClienteEnergia"))?.Valor != null ? Convert.ToDouble(g.FirstOrDefault(m => m.ID.Contains("MedGasVolGasClienteEnergia")).Valor) : (double?)null,
                                MedGasVolGasSaviaVolumen = g.FirstOrDefault(m => m.ID.Contains("MedGasVolGasSaviaVolumen"))?.Valor != null ? Convert.ToDouble(g.FirstOrDefault(m => m.ID.Contains("MedGasVolGasSaviaVolumen")).Valor) : (double?)null,
                                MedGasVolGasSaviaPoderCal = g.FirstOrDefault(m => m.ID.Contains("MedGasVolGasSaviaPoderCal"))?.Valor != null ? Convert.ToDouble(g.FirstOrDefault(m => m.ID.Contains("MedGasVolGasSaviaPoderCal")).Valor) : (double?)null,
                                MedGasVolGasSaviaEnergia = g.FirstOrDefault(m => m.ID.Contains("MedGasVolGasSaviaEnergia"))?.Valor != null ? Convert.ToDouble(g.FirstOrDefault(m => m.ID.Contains("MedGasVolGasSaviaEnergia")).Valor) : (double?)null,
                                MedGasVolGasLimaGasVolumen = g.FirstOrDefault(m => m.ID.Contains("MedGasVolGasLimaGasVolumen"))?.Valor != null ? Convert.ToDouble(g.FirstOrDefault(m => m.ID.Contains("MedGasVolGasLimaGasVolumen")).Valor) : (double?)null,
                                MedGasVolGasLimaGasPoderCal = g.FirstOrDefault(m => m.ID.Contains("MedGasVolGasLimaGasPoderCal"))?.Valor != null ? Convert.ToDouble(g.FirstOrDefault(m => m.ID.Contains("MedGasVolGasLimaGasPoderCal")).Valor) : (double?)null,
                                MedGasVolGasLimaGasEnergia = g.FirstOrDefault(m => m.ID.Contains("MedGasVolGasLimaGasEnergia"))?.Valor != null ? Convert.ToDouble(g.FirstOrDefault(m => m.ID.Contains("MedGasVolGasLimaGasEnergia")).Valor) : (double?)null,
                                MedGasVolGasGasNorpVolumen = g.FirstOrDefault(m => m.ID.Contains("MedGasVolGasGasNorpVolumen"))?.Valor != null ? Convert.ToDouble(g.FirstOrDefault(m => m.ID.Contains("MedGasVolGasGasNorpVolumen")).Valor) : (double?)null,
                                MedGasVolGasGasNorpPoderCal = g.FirstOrDefault(m => m.ID.Contains("MedGasVolGasGasNorpPoderCal"))?.Valor != null ? Convert.ToDouble(g.FirstOrDefault(m => m.ID.Contains("MedGasVolGasGasNorpPoderCal")).Valor) : (double?)null,
                                MedGasVolGasGasNorpEnergia = g.FirstOrDefault(m => m.ID.Contains("MedGasVolGasGasNorpEnergia"))?.Valor != null ? Convert.ToDouble(g.FirstOrDefault(m => m.ID.Contains("MedGasVolGasGasNorpEnergia")).Valor) : (double?)null,
                                MedGasVolGasQuemadoVolumen = g.FirstOrDefault(m => m.ID.Contains("MedGasVolGasQuemadoVolumen"))?.Valor != null ? Convert.ToDouble(g.FirstOrDefault(m => m.ID.Contains("MedGasVolGasQuemadoVolumen")).Valor) : (double?)null,
                                MedGasVolGasQuemadoPoderCal = g.FirstOrDefault(m => m.ID.Contains("MedGasVolGasQuemadoPoderCal"))?.Valor != null ? Convert.ToDouble(g.FirstOrDefault(m => m.ID.Contains("MedGasVolGasQuemadoPoderCal")).Valor) : (double?)null,
                                MedGasVolGasQuemadoEnergia = g.FirstOrDefault(m => m.ID.Contains("MedGasVolGasQuemadoEnergia"))?.Valor != null ? Convert.ToDouble(g.FirstOrDefault(m => m.ID.Contains("MedGasVolGasQuemadoEnergia")).Valor) : (double?)null
                            })
                        )
                        .ToList();

                    dto.ResBalanceEnergLIVDetGnaFisc = rootObject.DatosDiarios
                        .SelectMany(d => d.Mediciones
                            .Where(m => m.ID.StartsWith("tbResBalanceEnergLIVDetGnaFisc_"))
                            .GroupBy(m => GetDayFromID(m.ID))
                            .Select(g => new ResBalanceEnergLIVDetGnaFiscDto
                            {
                                Dia = g.Key,
                                GnaFiscVtaRefVolumen = g.FirstOrDefault(m => m.ID.Contains("GnaFiscVtaRefVolumen"))?.Valor != null ? Convert.ToDouble(g.FirstOrDefault(m => m.ID.Contains("GnaFiscVtaRefVolumen")).Valor) : (double?)null,
                                GnaFiscVtaRefPoderCal = g.FirstOrDefault(m => m.ID.Contains("GnaFiscVtaRefPoderCal"))?.Valor != null ? Convert.ToDouble(g.FirstOrDefault(m => m.ID.Contains("GnaFiscVtaRefPoderCal")).Valor) : (double?)null,
                                GnaFiscVtaRefEnergia = g.FirstOrDefault(m => m.ID.Contains("GnaFiscVtaRefEnergia"))?.Valor != null ? Convert.ToDouble(g.FirstOrDefault(m => m.ID.Contains("GnaFiscVtaRefEnergia")).Valor) : (double?)null,
                                GnaFiscVtaLimaGasVolumen = g.FirstOrDefault(m => m.ID.Contains("GnaFiscVtaLimaGasVolumen"))?.Valor != null ? Convert.ToDouble(g.FirstOrDefault(m => m.ID.Contains("GnaFiscVtaLimaGasVolumen")).Valor) : (double?)null,
                                GnaFiscVtaLimaGasPoderCal = g.FirstOrDefault(m => m.ID.Contains("GnaFiscVtaLimaGasPoderCal"))?.Valor != null ? Convert.ToDouble(g.FirstOrDefault(m => m.ID.Contains("GnaFiscVtaLimaGasPoderCal")).Valor) : (double?)null,
                                GnaFiscVtaLimaGasEnergia = g.FirstOrDefault(m => m.ID.Contains("GnaFiscVtaLimaGasEnergia"))?.Valor != null ? Convert.ToDouble(g.FirstOrDefault(m => m.ID.Contains("GnaFiscVtaLimaGasEnergia")).Valor) : (double?)null,
                                GnaFiscGasNorpVolumen = g.FirstOrDefault(m => m.ID.Contains("GnaFiscGasNorpVolumen"))?.Valor != null ? Convert.ToDouble(g.FirstOrDefault(m => m.ID.Contains("GnaFiscGasNorpVolumen")).Valor) : (double?)null,
                                GnaFiscGasNorpPoderCal = g.FirstOrDefault(m => m.ID.Contains("GnaFiscGasNorpPoderCal"))?.Valor != null ? Convert.ToDouble(g.FirstOrDefault(m => m.ID.Contains("GnaFiscGasNorpPoderCal")).Valor) : (double?)null,
                                GnaFiscGasNorpEnergia = g.FirstOrDefault(m => m.ID.Contains("GnaFiscGasNorpEnergia"))?.Valor != null ? Convert.ToDouble(g.FirstOrDefault(m => m.ID.Contains("GnaFiscGasNorpEnergia")).Valor) : (double?)null,
                                GnaFiscVtaEnelVolumen = g.FirstOrDefault(m => m.ID.Contains("GnaFiscVtaEnelVolumen"))?.Valor != null ? Convert.ToDouble(g.FirstOrDefault(m => m.ID.Contains("GnaFiscVtaEnelVolumen")).Valor) : (double?)null,
                                GnaFiscVtaEnelPoderCal = g.FirstOrDefault(m => m.ID.Contains("GnaFiscVtaEnelPoderCal"))?.Valor != null ? Convert.ToDouble(g.FirstOrDefault(m => m.ID.Contains("GnaFiscVtaEnelPoderCal")).Valor) : (double?)null,
                                GnaFiscVtaEnelEnergia = g.FirstOrDefault(m => m.ID.Contains("GnaFiscVtaEnelEnergia"))?.Valor != null ? Convert.ToDouble(g.FirstOrDefault(m => m.ID.Contains("GnaFiscVtaEnelEnergia")).Valor) : (double?)null,
                                GnaFiscGcyLgnVolumen = g.FirstOrDefault(m => m.ID.Contains("GnaFiscGcyLgnVolumen"))?.Valor != null ? Convert.ToDouble(g.FirstOrDefault(m => m.ID.Contains("GnaFiscGcyLgnVolumen")).Valor) : (double?)null,
                                GnaFiscGcyLgnPoderCal = g.FirstOrDefault(m => m.ID.Contains("GnaFiscGcyLgnPoderCal"))?.Valor != null ? Convert.ToDouble(g.FirstOrDefault(m => m.ID.Contains("GnaFiscGcyLgnPoderCal")).Valor) : (double?)null,
                                GnaFiscGcyLgnEnergia = g.FirstOrDefault(m => m.ID.Contains("GnaFiscGcyLgnEnergia"))?.Valor != null ? Convert.ToDouble(g.FirstOrDefault(m => m.ID.Contains("GnaFiscGcyLgnEnergia")).Valor) : (double?)null,
                                GnaFiscGnafVolumen = g.FirstOrDefault(m => m.ID.Contains("GnaFiscGnafVolumen"))?.Valor != null ? Convert.ToDouble(g.FirstOrDefault(m => m.ID.Contains("GnaFiscGnafVolumen")).Valor) : (double?)null,
                                GnaFiscGnafPoderCal = g.FirstOrDefault(m => m.ID.Contains("GnaFiscGnafPoderCal"))?.Valor != null ? Convert.ToDouble(g.FirstOrDefault(m => m.ID.Contains("GnaFiscGnafPoderCal")).Valor) : (double?)null,
                                GnaFiscGnafEnergia = g.FirstOrDefault(m => m.ID.Contains("GnaFiscGnafEnergia"))?.Valor != null ? Convert.ToDouble(g.FirstOrDefault(m => m.ID.Contains("GnaFiscGnafEnergia")).Valor) : (double?)null
                            })
                        )
                        .ToList();

                    dto.ResBalanceEnergLgnLIV_2DetLgnDto = rootObject.DatosDiarios
                        .SelectMany(d => d.Mediciones
                            .Where(m => m.ID.StartsWith("tbResBalanceEnergLgnLIV_2DetLgnDto_"))
                            .GroupBy(m => GetDayFromID(m.ID))
                            .Select(g => new ResBalanceEnergLgnLIV_2DetLgnDto
                            {
                                Dia = g.Key,
                                MedGasGlpVolumen = g.FirstOrDefault(m => m.ID.Contains("MedGasGlpVolumen"))?.Valor != null ? Convert.ToDouble(g.FirstOrDefault(m => m.ID.Contains("MedGasGlpVolumen")).Valor) : (double?)null,
                                MedGasGlpPoderCal = g.FirstOrDefault(m => m.ID.Contains("MedGasGlpPoderCal"))?.Valor != null ? Convert.ToDouble(g.FirstOrDefault(m => m.ID.Contains("MedGasGlpPoderCal")).Valor) : (double?)null,
                                MedGasGlpEnergia = g.FirstOrDefault(m => m.ID.Contains("MedGasGlpEnergia"))?.Valor != null ? Convert.ToDouble(g.FirstOrDefault(m => m.ID.Contains("MedGasGlpEnergia")).Valor) : (double?)null,
                                MedGasGlpDensidad = g.FirstOrDefault(m => m.ID.Contains("MedGasGlpDensidad"))?.Valor != null ? Convert.ToDouble(g.FirstOrDefault(m => m.ID.Contains("MedGasGlpDensidad")).Valor) : (double?)null,
                                MedGasCgnVolumen = g.FirstOrDefault(m => m.ID.Contains("MedGasCgnVolumen"))?.Valor != null ? Convert.ToDouble(g.FirstOrDefault(m => m.ID.Contains("MedGasCgnVolumen")).Valor) : (double?)null,
                                MedGasCgnPoderCal = g.FirstOrDefault(m => m.ID.Contains("MedGasCgnPoderCal"))?.Valor != null ? Convert.ToDouble(g.FirstOrDefault(m => m.ID.Contains("MedGasCgnPoderCal")).Valor) : (double?)null,
                                MedGasCgnEnergia = g.FirstOrDefault(m => m.ID.Contains("MedGasCgnEnergia"))?.Valor != null ? Convert.ToDouble(g.FirstOrDefault(m => m.ID.Contains("MedGasCgnEnergia")).Valor) : (double?)null,
                                MedGasLgnVolumen = g.FirstOrDefault(m => m.ID.Contains("MedGasLgnVolumen"))?.Valor != null ? Convert.ToDouble(g.FirstOrDefault(m => m.ID.Contains("MedGasLgnVolumen")).Valor) : (double?)null,
                                MedGasLgnPoderCal = g.FirstOrDefault(m => m.ID.Contains("MedGasLgnPoderCal"))?.Valor != null ? Convert.ToDouble(g.FirstOrDefault(m => m.ID.Contains("MedGasLgnPoderCal")).Valor) : (double?)null,
                                MedGasLgnEnergia = g.FirstOrDefault(m => m.ID.Contains("MedGasLgnEnergia"))?.Valor != null ? Convert.ToDouble(g.FirstOrDefault(m => m.ID.Contains("MedGasLgnEnergia")).Valor) : (double?)null,
                                
                            })
                        )
                        .ToList();

                    if (dto.AcumUnnaQ2GnaFiscVtaRefVolumen is null)
                    {
                        dto.AcumUnnaQ2GnaFiscVtaRefVolumen = 0;
                    }
                    int GetDayFromID(string id)
                    {
                        var parts = id.Split('_');
                        return int.TryParse(parts.Last(), out var day) ? day : 0;
                    }
                }
            }
            //dto.ResBalanceEnergLIVDetMedGas = await ResBalanceEnergLIVDetMedGas();
            //dto.ResBalanceEnergLIVDetGnaFisc = await ResBalanceEnergLIVDetGnaFisc();
            //dto.ResBalanceEnergLgnLIV_2DetLgnDto = await ResBalanceEnergLIVDetMedGasLGN();
            return new OperacionDto<ResBalanceEnergLIVDto>(dto);
        }

        // 1 Cuadro Principal
        // MEDICIÓN DE GAS NATURAL DEL LOTE IV - BALANCE ENERGETICO DE PLANTA DE UNNA-PGT
        private async Task<List<ResBalanceEnergLIVDetMedGasDto>> ResBalanceEnergLIVDetMedGas()
        {
            List<ResBalanceEnergLIVDetMedGasDto> ResBalanceEnergLIVDetMedGas = new List<ResBalanceEnergLIVDetMedGasDto>();

            var generalData = await _registroRepositorio.ObtenerMedicionesGasAsync();

            int year = DateTime.Now.Year;
            //int month = DateTime.Now.Month;
            int month = 4;

            var allDaysInMonth = Enumerable.Range(1, DateTime.DaysInMonth(year, month)).ToList();

            foreach (var day in allDaysInMonth)
            {
                var dataForDay = generalData.FirstOrDefault(d => d.Dia == day);

                if (dataForDay == null)
                {
                    ResBalanceEnergLIVDetMedGas.Add(new ResBalanceEnergLIVDetMedGasDto
                    {
                        Dia = day,
                        MedGasGasNatAsocMedVolumen = 0,
                        MedGasGasNatAsocMedPoderCal = 0,
                        MedGasGasNatAsocMedEnergia = 0,

                        MedGasGasCombSecoMedVolumen = 0,
                        MedGasGasCombSecoMedPoderCal = 0,
                        MedGasGasCombSecoMedEnergia = 0,

                        MedGasVolGasEquivLgnVolumen = 0,
                        MedGasVolGasEquivLgnPoderCal = 0,
                        MedGasVolGasEquivLgnEnergia = 0,

                        MedGasVolGasClienteVolumen = 0,
                        MedGasVolGasClientePoderCal = 0,
                        MedGasVolGasClienteEnergia = 0,

                        MedGasVolGasSaviaVolumen = 0,
                        MedGasVolGasSaviaPoderCal = 0,
                        MedGasVolGasSaviaEnergia = 0,

                        MedGasVolGasLimaGasVolumen = 0,
                        MedGasVolGasLimaGasPoderCal = 0,
                        MedGasVolGasLimaGasEnergia = 0,

                        MedGasVolGasGasNorpVolumen = 0,
                        MedGasVolGasGasNorpPoderCal = 0,
                        MedGasVolGasGasNorpEnergia = 0,

                        MedGasVolGasQuemadoVolumen = 0,
                        MedGasVolGasQuemadoPoderCal = 0,
                        MedGasVolGasQuemadoEnergia = 0
                    });
                }
                else
                {
                    ResBalanceEnergLIVDetMedGas.Add(new ResBalanceEnergLIVDetMedGasDto
                    {
                        Dia = dataForDay.Dia,
                        MedGasGasNatAsocMedVolumen = dataForDay.MedGasGasNatAsocMedVolumen,
                        MedGasGasNatAsocMedPoderCal = dataForDay.MedGasGasNatAsocMedPoderCal,
                        MedGasGasNatAsocMedEnergia = dataForDay.MedGasGasNatAsocMedEnergia,

                        MedGasGasCombSecoMedVolumen = dataForDay.MedGasGasCombSecoMedVolumen,
                        MedGasGasCombSecoMedPoderCal = dataForDay.MedGasGasCombSecoMedPoderCal,
                        MedGasGasCombSecoMedEnergia = dataForDay.MedGasGasCombSecoMedEnergia,

                        MedGasVolGasEquivLgnVolumen = dataForDay.MedGasVolGasEquivLgnVolumen,
                        MedGasVolGasEquivLgnPoderCal = dataForDay.MedGasVolGasEquivLgnPoderCal,
                        MedGasVolGasEquivLgnEnergia = dataForDay.MedGasVolGasEquivLgnEnergia,

                        MedGasVolGasClienteVolumen = dataForDay.MedGasVolGasClienteVolumen,
                        MedGasVolGasClientePoderCal = dataForDay.MedGasVolGasClientePoderCal,
                        MedGasVolGasClienteEnergia = dataForDay.MedGasVolGasClienteEnergia,

                        MedGasVolGasSaviaVolumen = dataForDay.MedGasVolGasSaviaVolumen,
                        MedGasVolGasSaviaPoderCal = dataForDay.MedGasVolGasSaviaPoderCal,
                        MedGasVolGasSaviaEnergia = dataForDay.MedGasVolGasSaviaEnergia,

                        MedGasVolGasLimaGasVolumen = dataForDay.MedGasVolGasLimaGasVolumen,
                        MedGasVolGasLimaGasPoderCal = dataForDay.MedGasVolGasLimaGasPoderCal,
                        MedGasVolGasLimaGasEnergia = dataForDay.MedGasVolGasLimaGasEnergia,

                        MedGasVolGasGasNorpVolumen = dataForDay.MedGasVolGasGasNorpVolumen,
                        MedGasVolGasGasNorpPoderCal = dataForDay.MedGasVolGasGasNorpPoderCal,
                        MedGasVolGasGasNorpEnergia = dataForDay.MedGasVolGasGasNorpEnergia,

                        MedGasVolGasQuemadoVolumen = dataForDay.MedGasVolGasQuemadoVolumen,
                        MedGasVolGasQuemadoPoderCal = dataForDay.MedGasVolGasQuemadoPoderCal,
                        MedGasVolGasQuemadoEnergia = dataForDay.MedGasVolGasQuemadoEnergia
                    });
                }
            }
            

            return ResBalanceEnergLIVDetMedGas;
        }

        // 2 GNA FISCALIZADO
        // MEDICIÓN DE GAS NATURAL DEL LOTE IV - BALANCE ENERGETICO DE PLANTA DE UNNA-PGT
        private async Task<List<ResBalanceEnergLIVDetGnaFiscDto>> ResBalanceEnergLIVDetGnaFisc()
        {
            var generalData = await _registroRepositorio.ObtenerMedicionesGasAsync();
            List<ResBalanceEnergLIVDetGnaFiscDto> ResBalanceEnergLIVDetGnaFisc = new List<ResBalanceEnergLIVDetGnaFiscDto>();

            int year = DateTime.Now.Year;
            //int month = DateTime.Now.Month;
            int month = 4;


            var allDaysInMonth = Enumerable.Range(1, DateTime.DaysInMonth(year, month)).ToList();

            foreach (var day in allDaysInMonth)
            {
                var item = generalData.FirstOrDefault(d => d.Dia == day);

                if (item == null)
                {
                    ResBalanceEnergLIVDetGnaFisc.Add(new ResBalanceEnergLIVDetGnaFiscDto
                    {
                        Dia = day,
                        GnaFiscVtaRefVolumen = 0,
                        GnaFiscVtaRefPoderCal = 0,
                        GnaFiscVtaRefEnergia = 0,

                        GnaFiscVtaLimaGasVolumen = 0,
                        GnaFiscVtaLimaGasPoderCal = 0,
                        GnaFiscVtaLimaGasEnergia = 0,

                        GnaFiscGasNorpVolumen = 0,
                        GnaFiscGasNorpPoderCal = 0,
                        GnaFiscGasNorpEnergia = 0,

                        GnaFiscVtaEnelVolumen = 0,
                        GnaFiscVtaEnelPoderCal = 0,
                        GnaFiscVtaEnelEnergia = 0,

                        GnaFiscGcyLgnVolumen = 0,
                        GnaFiscGcyLgnPoderCal = 0,
                        GnaFiscGcyLgnEnergia = 0,

                        GnaFiscGnafVolumen = 0,
                        GnaFiscGnafPoderCal = 0,
                        GnaFiscGnafEnergia = 0,

                        GnaFiscTotalVolumen = 0,
                        GnaFiscTotalEnergia = 0
                    });
                }
                else
                {
                    // Cálculos de energías individuales
                    double? GnaFiscVtaRefEnergia = item.MedGasVolGasSaviaVolumen.HasValue && item.MedGasGasNatAsocMedPoderCal.HasValue
                        ? (double?)Math.Round((item.MedGasVolGasSaviaVolumen.Value * item.MedGasGasNatAsocMedPoderCal.Value) / 1000.0, 4)
                        : null;

                    double? GnaFiscVtaLimaGasEnergia = item.MedGasVolGasLimaGasVolumen.HasValue && item.MedGasGasNatAsocMedPoderCal.HasValue
                        ? (double?)Math.Round((item.MedGasVolGasLimaGasVolumen.Value * item.MedGasGasNatAsocMedPoderCal.Value) / 1000.0, 4)
                        : null;

                    double? GnaFiscGasNorpEnergia = item.MedGasVolGasGasNorpVolumen.HasValue && item.MedGasGasNatAsocMedPoderCal.HasValue
                        ? (double?)Math.Round((item.MedGasVolGasGasNorpVolumen.Value * item.MedGasGasNatAsocMedPoderCal.Value) / 1000.0, 4)
                        : null;

                    double? GnaFiscVtaEnelEnergia = item.MedGasVolGasClienteVolumen.HasValue && item.MedGasGasNatAsocMedPoderCal.HasValue
                        ? (double?)Math.Round((item.MedGasVolGasClienteVolumen.Value * item.MedGasGasNatAsocMedPoderCal.Value) / 1000.0, 4)
                        : null;

                    double? GnaFiscGcyLgnEnergia = (item.MedGasGasCombSecoMedVolumen.HasValue && item.MedGasVolGasEquivLgnVolumen.HasValue && item.MedGasGasNatAsocMedPoderCal.HasValue)
                        ? (double?)Math.Round(((item.MedGasGasCombSecoMedVolumen.Value + item.MedGasVolGasEquivLgnVolumen.Value) * item.MedGasGasNatAsocMedPoderCal.Value) / 1000.0, 4)
                        : null;

                    // Calcular la suma de las energías
                    double? GnaFiscGnafEnergia = new double?[] { GnaFiscVtaRefEnergia, GnaFiscVtaLimaGasEnergia, GnaFiscGasNorpEnergia, GnaFiscVtaEnelEnergia, GnaFiscGcyLgnEnergia }
                        .Where(e => e.HasValue)
                        .Sum(e => e.Value);

                    // Calcular la suma de los volúmenes
                    double GnaFiscGnafVolumen = (item.MedGasVolGasSaviaVolumen.HasValue ? item.MedGasVolGasSaviaVolumen.Value : 0) +
                                                (item.MedGasVolGasLimaGasVolumen.HasValue ? item.MedGasVolGasLimaGasVolumen.Value : 0) +
                                                (item.MedGasVolGasGasNorpVolumen.HasValue ? item.MedGasVolGasGasNorpVolumen.Value : 0) +
                                                (item.MedGasVolGasClienteVolumen.HasValue ? item.MedGasVolGasClienteVolumen.Value : 0) +
                                                ((item.MedGasGasCombSecoMedVolumen.HasValue ? item.MedGasGasCombSecoMedVolumen.Value : 0) + (item.MedGasVolGasEquivLgnVolumen.HasValue ? item.MedGasVolGasEquivLgnVolumen.Value : 0));

                    ResBalanceEnergLIVDetGnaFisc.Add(new ResBalanceEnergLIVDetGnaFiscDto
                    {
                        Dia = item.Dia,
                        GnaFiscVtaRefVolumen = item.MedGasVolGasSaviaVolumen,
                        GnaFiscVtaRefPoderCal = item.MedGasGasNatAsocMedPoderCal,
                        GnaFiscVtaRefEnergia = GnaFiscVtaRefEnergia,

                        GnaFiscVtaLimaGasVolumen = item.MedGasVolGasLimaGasVolumen,
                        GnaFiscVtaLimaGasPoderCal = item.MedGasGasNatAsocMedPoderCal,
                        GnaFiscVtaLimaGasEnergia = GnaFiscVtaLimaGasEnergia,

                        GnaFiscGasNorpVolumen = item.MedGasVolGasGasNorpVolumen,
                        GnaFiscGasNorpPoderCal = item.MedGasGasNatAsocMedPoderCal,
                        GnaFiscGasNorpEnergia = GnaFiscGasNorpEnergia,

                        GnaFiscVtaEnelVolumen = item.MedGasVolGasClienteVolumen,
                        GnaFiscVtaEnelPoderCal = item.MedGasGasNatAsocMedPoderCal,
                        GnaFiscVtaEnelEnergia = GnaFiscVtaEnelEnergia,

                        GnaFiscGcyLgnVolumen = (item.MedGasGasCombSecoMedVolumen.HasValue ? item.MedGasGasCombSecoMedVolumen.Value : 0)
                                               + (item.MedGasVolGasEquivLgnVolumen.HasValue ? item.MedGasVolGasEquivLgnVolumen.Value : 0),
                        GnaFiscGcyLgnPoderCal = item.MedGasGasNatAsocMedPoderCal,
                        GnaFiscGcyLgnEnergia = GnaFiscGcyLgnEnergia,

                        GnaFiscGnafVolumen = GnaFiscGnafVolumen,
                        GnaFiscGnafPoderCal = item.MedGasGasNatAsocMedPoderCal,
                        GnaFiscGnafEnergia = GnaFiscGnafEnergia,

                        GnaFiscTotalVolumen = GnaFiscGnafVolumen,
                        GnaFiscTotalEnergia = GnaFiscGnafEnergia
                    });
                }
            }

            return ResBalanceEnergLIVDetGnaFisc;
        }

        // 3 LGN
        // MEDICIÓN DE GAS NATURAL DEL LOTE IV - BALANCE ENERGETICO DE PLANTA DE UNNA ENERGIA LGN (GLP y CGN)
        private async Task<List<ResBalanceEnergLgnLIV_2DetLgnDto>> ResBalanceEnergLIVDetMedGasLGN()
        {
            List<ResBalanceEnergLgnLIV_2DetLgnDto> ResBalanceEnergLgnLIV_2DetLgn = new List<ResBalanceEnergLgnLIV_2DetLgnDto>();

            var generalData = await _registroRepositorio.ObtenerMedicionesGasAsync();
            var parametrosLGNQ = await _registroRepositorio.ObtenerResumenBalanceEnergiaLGNParametrosAsync();

            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;

            var allDaysInMonth = Enumerable.Range(1, DateTime.DaysInMonth(year, month)).ToList();

            var parametroQ1 = parametrosLGNQ?.FirstOrDefault(p => p.Quincena == 1);
            var parametroQ2 = parametrosLGNQ?.FirstOrDefault(p => p.Quincena == 2);

            foreach (var day in allDaysInMonth)
            {
                var item = generalData.FirstOrDefault(d => d.Dia == day);
                var parametro = day <= 15 ? parametroQ1 : parametroQ2;

                if (item == null)
                {
                    ResBalanceEnergLgnLIV_2DetLgn.Add(new ResBalanceEnergLgnLIV_2DetLgnDto
                    {
                        Dia = day,
                        MedGasGlpVolumen = 0,
                        MedGasGlpPoderCal = parametro != null ? Math.Round(Convert.ToDouble(parametro.PCGLPMMBtuBl60F), 4) : (double?)null,
                        MedGasGlpEnergia = 0,
                        MedGasGlpDensidad = parametro != null ? Math.Round(Convert.ToDouble(parametro.DensidadGLPKgBl), 4) : (double?)null,

                        MedGasCgnVolumen = 0,
                        MedGasCgnPoderCal = parametro != null ? Math.Round(Convert.ToDouble(parametro.PCCGNMMBtuBl60F), 4) : (double?)null,
                        MedGasCgnEnergia = 0,

                        MedGasLgnVolumen = 0,
                        MedGasLgnPoderCal = parametro != null ? Math.Round(Convert.ToDouble(parametro.PCLGNMMBtuBl60F), 4) : (double?)null,
                        MedGasLgnEnergia = 0,
                    });
                }
                else
                {
                    ResBalanceEnergLgnLIV_2DetLgn.Add(new ResBalanceEnergLgnLIV_2DetLgnDto
                    {
                        Dia = item.Dia,
                        MedGasGlpVolumen = Math.Round(0.0, 4),
                        MedGasGlpPoderCal = parametro != null ? Math.Round(Convert.ToDouble(parametro.PCGLPMMBtuBl60F), 4) : (double?)null,
                        MedGasGlpEnergia = Math.Round(0.0 * (parametro != null ? Convert.ToDouble(parametro.PCGLPMMBtuBl60F) : 0), 4), // Calcula y redondea
                        MedGasGlpDensidad = parametro != null ? Math.Round(Convert.ToDouble(parametro.DensidadGLPKgBl), 4) : (double?)null,

                        MedGasCgnVolumen = Math.Round(0.0, 4),
                        MedGasCgnPoderCal = parametro != null ? Math.Round(Convert.ToDouble(parametro.PCCGNMMBtuBl60F), 4) : (double?)null,
                        MedGasCgnEnergia = Math.Round(0.0 * (parametro != null ? Convert.ToDouble(parametro.PCCGNMMBtuBl60F) : 0), 4), // Calcula y redondea

                        MedGasLgnVolumen = Math.Round(0.0, 4),
                        MedGasLgnPoderCal = parametro != null ? Math.Round(Convert.ToDouble(parametro.PCLGNMMBtuBl60F), 4) : (double?)null,
                        MedGasLgnEnergia = Math.Round(0.0 * (parametro != null ? Convert.ToDouble(parametro.PCLGNMMBtuBl60F) : 0), 4), // Calcula y redondea
                    });
                }
            }

            return ResBalanceEnergLgnLIV_2DetLgn;
        }

        public async Task<OperacionDto<RespuestaSimpleDto<string>>> GuardarAsync(ResBalanceEnergLIVPost peticion)
        {
            var dto = new ImpresionDto()
            {
                Id = "",
                IdConfiguracion = RijndaelUtilitario.EncryptRijndaelToUrl((int)TiposReportes.ResumenBalanceEnergiaLIVQuincenal),
                Fecha = DateTime.Now,
                IdUsuario = peticion.IdUsuario,
                Datos = JsonConvert.SerializeObject(peticion),
                Comentario = "TEst"
            };

            return await _impresionServicio.GuardarAsync(dto);
        }
    }
}
