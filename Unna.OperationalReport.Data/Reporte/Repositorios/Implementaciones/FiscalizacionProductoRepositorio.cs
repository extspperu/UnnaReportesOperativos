using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Infraestructura.Configuraciones.Abstracciones;
using Unna.OperationalReport.Data.Infraestructura.Contextos.Abstracciones;
using Unna.OperationalReport.Data.Infraestructura.Repositorios.Implementaciones;
using Unna.OperationalReport.Data.Reporte.Entidades;
using Unna.OperationalReport.Data.Reporte.Repositorios.Abstracciones;

namespace Unna.OperationalReport.Data.Reporte.Repositorios.Implementaciones
{
    public class FiscalizacionProductoRepositorio : OperacionalRepositorio<FiscalizacionProducto, object>, IFiscalizacionProductoRepositorio
    {
        public FiscalizacionProductoRepositorio(IOperacionalUnidadDeTrabajo unidadDeTrabajo, IOperacionalConfiguracion configuracion) : base(unidadDeTrabajo, configuracion) { }


       


    }
}
