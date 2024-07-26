using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Registro.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Reporte.Enums;
using Unna.OperationalReport.Data.Reporte.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Reporte.Repositorios.Implementaciones;
using Unna.OperationalReport.Service.Reportes.Impresiones.Dtos;
using Unna.OperationalReport.Service.Reportes.Impresiones.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ComposicionGnaLIV.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ResBalanceEnergLIV.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ValorizacionVtaGns.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ValorizacionVtaGns.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;
using static Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ResBalanceEnergLIV.Servicios.Implementaciones.ResBalanceEnergLIVServicio;

namespace Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ValorizacionVtaGns.Servicios.Implementaciones
{
    public class ValorizacionVtaGnsServicio: IValorizacionVtaGnsServicio
    {
        private readonly IImpresionServicio _impresionServicio;
        private readonly IImprimirRepositorio _imprimirRepositorio;

        public ValorizacionVtaGnsServicio(IRegistroRepositorio registroRepositorio, IImpresionServicio impresionServicio, IImprimirRepositorio imprimirRepositorio)
        {
            //_registroRepositorio = registroRepositorio;
            _impresionServicio = impresionServicio;
            _imprimirRepositorio = imprimirRepositorio;
        }
        public async Task<OperacionDto<ValorizacionVtaGnsDto>> ObtenerAsync(long idUsuario, string someSetting)
        {
            var imprimir = await _imprimirRepositorio.BuscarPorIdConfiguracionYFechaAsync(12, DateTime.UtcNow.Date);
            string jsonData = string.Empty;
            string comentario = string.Empty;

            double enerVolTransM = 0;
            double subTotalFact = 0;
            double igv = 0;
            double totalFact = 0;
            RootObjectVal rootObject = null;

            if (imprimir is not null)
            {
                jsonData = imprimir.Datos.Replace("\\", "");
                rootObject = JsonConvert.DeserializeObject<RootObjectVal>(jsonData);
                comentario = rootObject?.Comentario?.ToString() ?? string.Empty;

                enerVolTransM = rootObject?.EnerVolTransM ?? 0;
                subTotalFact = rootObject?.SubTotalFact ?? 0;
                igv = rootObject?.Igv ?? 0;
                totalFact = rootObject?.TotalFact ?? 0;
            }

            // Usar someSetting para simular la fecha actual
            DateTime fechaActual;
            if (!DateTime.TryParse(someSetting, out fechaActual))
            {
                fechaActual = DateTime.Now; // Manejar error de parseo
            }

            string periodo;

            if (fechaActual.Day == 16)
            {
                int mesAnterior = fechaActual.Month == 1 ? 12 : fechaActual.Month - 1;
                int anioAnterior = fechaActual.Month == 1 ? fechaActual.Year - 1 : fechaActual.Year;
                int diasEnMesAnterior = DateTime.DaysInMonth(anioAnterior, mesAnterior);
                periodo = $"Del 16 al {diasEnMesAnterior} de {new DateTime(anioAnterior, mesAnterior, 1).ToString("MMMM yyyy")}";
            }
            else if (fechaActual.Day > 16)
            {
                periodo = $"Del 1 al 15 de {fechaActual.ToString("MMMM yyyy")}";
            }
            else if (fechaActual.Day == 1)
            {
                int mesAnterior = fechaActual.Month == 1 ? 12 : fechaActual.Month - 1;
                int anioAnterior = fechaActual.Month == 1 ? fechaActual.Year - 1 : fechaActual.Year;
                periodo = $"Del 1 al 15 de {new DateTime(anioAnterior, mesAnterior, 1).ToString("MMMM yyyy")}";
            }
            else
            {
                int mesAnterior = fechaActual.Month == 1 ? 12 : fechaActual.Month - 1;
                int anioAnterior = fechaActual.Month == 1 ? fechaActual.Year - 1 : fechaActual.Year;
                int diasEnMesAnterior = DateTime.DaysInMonth(anioAnterior, mesAnterior);
                periodo = $"Del 16 al {diasEnMesAnterior} de {new DateTime(anioAnterior, mesAnterior, 1).ToString("MMMM yyyy")}";
            }

            var dto = new ValorizacionVtaGnsDto
            {
                Periodo = periodo,
                PuntoFiscal = "MS-9225",
                TotalVolumen = 0,
                TotalPoderCal = 0,
                TotalEnergia = 0,
                PromPrecio = 0,
                TotalCosto = 0,
                EnerVolTransM = enerVolTransM,
                SubTotalFact = subTotalFact,
                Igv = igv,
                TotalFact = totalFact,
                Comentario = comentario,
            };

            dto.ValorizacionVtaGnsDet = await ValorizacionVtaGnsDet(fechaActual);

            dto.TotalVolumen = Math.Round(dto.ValorizacionVtaGnsDet.Sum(d => d.Volumen) ?? 0.0, 2);
            dto.TotalPoderCal = Math.Round(dto.ValorizacionVtaGnsDet.Average(d => d.PoderCal) ?? 0.0, 2);
            dto.TotalEnergia = Math.Round(dto.ValorizacionVtaGnsDet.Sum(d => d.Energia) ?? 0.0, 2);
            dto.TotalCosto = Math.Round(dto.ValorizacionVtaGnsDet.Sum(d => d.Costo) ?? 0.0, 2);

            return new OperacionDto<ValorizacionVtaGnsDto>(dto);
        }


        private string GetDayFromID(string id)
        {
            var parts = id.Split('_');
            return parts.Length > 1 ? parts[1] : "01"; // Default to "01" if no day is found
        }
        public class RootObjectVal
        {
            public int IdUsuario { get; set; }
            public string Mes { get; set; }
            public string Anio { get; set; }
            public string Comentario { get; set; }
            public double EnerVolTransM { get; set; }
            public double SubTotalFact { get; set; }
            public double Igv { get; set; }
            public double TotalFact { get; set; }
            public List<MedicionVal> Mediciones { get; set; }
        }
        public class MedicionVal
        {
            public string ID { get; set; }
            public double Valor { get; set; }
        }
        private async Task<List<ValorizacionVtaGnsDetDto>> ValorizacionVtaGnsDet(DateTime fechaActual)
        {
            var imprimir = await _imprimirRepositorio.BuscarPorIdConfiguracionYFechaAsync(12, DateTime.UtcNow.Date);
            List<ValorizacionVtaGnsDetDto> valorizacionVtaGnsDet = new List<ValorizacionVtaGnsDetDto>();

            if (imprimir is null)
            {
                int mesActual = fechaActual.Month;
                int anioActual = fechaActual.Year;
                int diaActual = fechaActual.Day;

                if (diaActual == 16)
                {
                    // Mostrar del 16 al último día del mes anterior
                    if (mesActual == 1)
                    {
                        mesActual = 12;
                        anioActual -= 1;
                    }
                    else
                    {
                        mesActual -= 1;
                    }

                    int diasEnMesAnterior = DateTime.DaysInMonth(anioActual, mesActual);
                    for (int dia = 16; dia <= diasEnMesAnterior; dia++)
                    {
                        DateTime fecha = new DateTime(anioActual, mesActual, dia);
                        valorizacionVtaGnsDet.Add(new ValorizacionVtaGnsDetDto
                        {
                            Fecha = fecha.ToString("dd-MM-yyyy"),
                            Volumen = 0,
                            PoderCal = 0,
                            Energia = 0,
                            Precio = 0,
                            Costo = 0
                        });
                    }
                }
                else if (diaActual > 16)
                {
                    // Mostrar del 1 al 15 del mes actual
                    for (int dia = 1; dia <= 15; dia++)
                    {
                        DateTime fecha = new DateTime(anioActual, mesActual, dia);
                        valorizacionVtaGnsDet.Add(new ValorizacionVtaGnsDetDto
                        {
                            Fecha = fecha.ToString("dd-MM-yyyy"),
                            Volumen = 0,
                            PoderCal = 0,
                            Energia = 0,
                            Precio = 0,
                            Costo = 0
                        });
                    }
                }
                else if (diaActual == 1)
                {
                    // Si es el primer día del mes, mostrar del 1 al 15 del mes anterior
                    if (mesActual == 1)
                    {
                        mesActual = 12;
                        anioActual -= 1;
                    }
                    else
                    {
                        mesActual -= 1;
                    }

                    for (int dia = 1; dia <= 15; dia++)
                    {
                        DateTime fecha = new DateTime(anioActual, mesActual, dia);
                        valorizacionVtaGnsDet.Add(new ValorizacionVtaGnsDetDto
                        {
                            Fecha = fecha.ToString("dd-MM-yyyy"),
                            Volumen = 0,
                            PoderCal = 0,
                            Energia = 0,
                            Precio = 0,
                            Costo = 0
                        });
                    }
                }
                else
                {
                    // Mostrar del 16 al último día del mes anterior
                    if (mesActual == 1)
                    {
                        mesActual = 12;
                        anioActual -= 1;
                    }
                    else
                    {
                        mesActual -= 1;
                    }

                    int diasEnMesAnterior = DateTime.DaysInMonth(anioActual, mesActual);
                    for (int dia = 16; dia <= diasEnMesAnterior; dia++)
                    {
                        DateTime fecha = new DateTime(anioActual, mesActual, dia);
                        valorizacionVtaGnsDet.Add(new ValorizacionVtaGnsDetDto
                        {
                            Fecha = fecha.ToString("dd-MM-yyyy"),
                            Volumen = 0,
                            PoderCal = 0,
                            Energia = 0,
                            Precio = 0,
                            Costo = 0
                        });
                    }
                }
            }
            else
            {
                string jsonData = imprimir.Datos.Replace("\\", "");
                RootObjectVal rootObject = JsonConvert.DeserializeObject<RootObjectVal>(jsonData);
                if (rootObject != null)
                {
                    var medicionesAgrupadas = rootObject.Mediciones
                        .GroupBy(m => GetDayFromID(m.ID))
                        .ToList();
                    double totalVolumen = 0;
                    double totalPoderCal = 0;
                    double totalEnergia = 0;
                    double totalPrecio = 0;
                    double totalCosto = 0;
                    foreach (var grupo in medicionesAgrupadas)
                    {
                        var dia = grupo.Key;
                        var fecha = dia;

                        double volumen = Convert.ToDouble(grupo.FirstOrDefault(m => m.ID.Contains("Volumen"))?.Valor);
                        double poderCal = Convert.ToDouble(grupo.FirstOrDefault(m => m.ID.Contains("PoderCal"))?.Valor);
                        double energia = Convert.ToDouble(grupo.FirstOrDefault(m => m.ID.Contains("Energia"))?.Valor);
                        double precio = Convert.ToDouble(grupo.FirstOrDefault(m => m.ID.Contains("Precio"))?.Valor);
                        double costo = Convert.ToDouble(grupo.FirstOrDefault(m => m.ID.Contains("Costo"))?.Valor);

                        totalVolumen += volumen;
                        totalPoderCal += poderCal;
                        totalEnergia += energia;
                        totalPrecio += precio;
                        totalCosto += costo;

                        valorizacionVtaGnsDet.Add(new ValorizacionVtaGnsDetDto
                        {
                            Fecha = fecha,
                            Volumen = volumen,
                            PoderCal = poderCal,
                            Energia = energia,
                            Precio = precio,
                            Costo = costo
                        });
                    }
                }
            }
            return valorizacionVtaGnsDet;
        }


        public async Task<OperacionDto<RespuestaSimpleDto<string>>> GuardarAsync(ValorizacionVtaGnsPost peticion)
        {
            var dto = new ImpresionDto()
            {
                Id = "",
                IdConfiguracion = RijndaelUtilitario.EncryptRijndaelToUrl((int)TiposReportes.ValorizacionVentaGNSGasNORP),
                Fecha = DateTime.Now,
                IdUsuario = peticion.IdUsuario,
                Datos = JsonConvert.SerializeObject(peticion),
                Comentario = peticion.Comentario
            };

            return await _impresionServicio.GuardarAsync(dto);
        }
    }
}
