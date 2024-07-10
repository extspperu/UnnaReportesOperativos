using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Service.Seguimiento.BalanceDiario.Dtos;
using Unna.OperationalReport.Service.Seguimiento.BalanceDiario.Servicios.Abstracciones;

namespace Unna.OperationalReport.Service.Seguimiento.BalanceDiario.Servicios.Implementaciones
{
    public class SeguimientoBalanceDiarioServicio : ISeguimientoBalanceDiarioServicio
    {
        public List<ColumnaDto> ObtenerAsync()
        {
            return new List<ColumnaDto>
            {
                new ColumnaDto("Validar Datos", new List<SeguimientoBoxDto>
                {
                    new SeguimientoBoxDto("", "bg-secondary", "black", true, 1),
                    new SeguimientoBoxDto("Datos de Fiscalizador Lote I", "bg-success", "black", true, 2),
                    new SeguimientoBoxDto("Datos de Fiscalizador Lote IV", "bg-warning", "black", true, 3),
                    new SeguimientoBoxDto("Datos de Fiscalizador ENEL", "bg-danger", "black", true, 4),
                    new SeguimientoBoxDto("Datos de Supervisor PGT", "bg-warning", "black", true, 5)
                }),
                new ColumnaDto("Emitir Reportes", new List<SeguimientoBoxDto>
                {
                    new SeguimientoBoxDto("", "bg-secondary", "black", true, 1),
                    new SeguimientoBoxDto("Reporte CNPC", "bg-success", "black", true, 2),
                    new SeguimientoBoxDto("Reporte Balance Energía", "bg-success", "black", true, 3),
                    new SeguimientoBoxDto("Reporte Reporte Diario", "bg-danger", "black", true, 4)
                }),
                new ColumnaDto("Enviar Correos", new List<SeguimientoBoxDto>
                {
                    new SeguimientoBoxDto("", "bg-secondary", "black", true, 1)
                }),
                new ColumnaDto("Datos de OSINERGMIN", new List<SeguimientoBoxDto>
                {
                    new SeguimientoBoxDto("", "bg-secondary", "black", true, 1),
                    new SeguimientoBoxDto("Evidencia", "bg-success", "black", true, 2)
                }),
                new ColumnaDto("Almacenamiento de Reportes validados", new List<SeguimientoBoxDto>
                {
                    new SeguimientoBoxDto("", "bg-secondary", "black", true, 1),
                    new SeguimientoBoxDto("Reporte CNPC", "bg-success", "black", true, 2),
                    new SeguimientoBoxDto("Reporte Balance de Energía", "bg-danger", "black", true, 3)
                })
            };
        }
    }
}
