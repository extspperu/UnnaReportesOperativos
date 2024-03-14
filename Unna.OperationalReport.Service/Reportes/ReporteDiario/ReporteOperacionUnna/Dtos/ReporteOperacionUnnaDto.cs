using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Unna.OperationalReport.Service.Reportes.Generales.Dtos;

namespace Unna.OperationalReport.Service.Reportes.ReporteDiario.ReporteOperacionUnna.Dtos
{
    public class ReporteOperacionUnnaDto
    {
        public ReporteDto? General { get; set; }
        public string? ReporteNro { get; set; }
        public string? FechaEmision { get; set; }
        public string? DiaOperativo { get; set; }
        public double? CapacidadDisenio { get; set; }
        public string? Empresa { get; set; }

        public ReporteOperacionUnnaPlantaSepGasNatDto? PlantaSepGasNat { get; set; }
        public ReporteOperacionUnnaPlantaFracLiqGasNatDto? PlantaFracLiqGasNat { get; set; }

        //PROCESAMIENTO DE GAS NATURAL
        public ProcesamientoVolumenDto? ProcesamientoGasNatural { get; set; }

        //PROCESAMIENTO DE GAS NATURAL
        public List<ProcesamientoVolumenDto>? ProcesamientoGasNaturalSeco { get; set; }



        //PRODUCCIÓN DE LGN EN PLANTA UNNA
        public ProcesamientoVolumenDto? ProduccionLgn { get; set; }


        //PROCESAMIENTO DE LÍQUIDOS DE GAS NATURAL(LGN) - UNNA

        public ProcesamientoVolumenDto? ProcesamientoLiquidos { get; set; }

        //PRODUCTOS OBTENIDOS EN PLANTA - UNNA
        public List<ProcesamientoVolumenDto>? ProductosObtenido { get; set; }


        // ALMACENAMIENTO DE PRODUCTOS LÍQUIDOS - STOCK
        public List<ProcesamientoVolumenDto>? Almacenamiento { get; set; }
        public string? EventoOperativo { get; set; }


        [JsonIgnore]
        public long? IdUsuario { get; set; }
    }
}
