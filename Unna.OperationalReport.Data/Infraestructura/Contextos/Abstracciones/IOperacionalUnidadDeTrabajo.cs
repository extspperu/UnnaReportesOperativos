using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Auth.Entidades;
using Unna.OperationalReport.Data.Carta.Entidades;
using Unna.OperationalReport.Data.Configuracion.Entidades;
using Unna.OperationalReport.Data.Mensual.Entidades;
using Unna.OperationalReport.Data.Registro.Entidades;
using Unna.OperationalReport.Data.Reporte.Entidades;
using Unna.OperationalReport.Tools.DatosEF.Infraestructura.Contexto.Abstracciones;

namespace Unna.OperationalReport.Data.Infraestructura.Contextos.Abstracciones
{
    public interface IOperacionalUnidadDeTrabajo: IEFUnidadDeTrabajo
    {
        public DbSet<Usuario> AuthUsuarios { get; set; }
        public DbSet<UsuarioLote> AuthUsuarioLotes { get; set; }
        public DbSet<Grupo> AuthGrupos { get; set; }
        public DbSet<Persona> AuthPersonas { get; set; }

        public DbSet<Dato> RegistroDatos { get; set; }
        public DbSet<DiaOperativo> RegistroDiaOperativos { get; set; }
        public DbSet<Registro.Entidades.Registro> RegistroRegistros { get; set; }
        
        public DbSet<Lote> RegistroLotes { get; set; }



        public DbSet<Adjunto> ReporteAdjuntos { get; set; }
        public DbSet<AdjuntoSupervisor> ReporteAdjuntoSupervisores { get; set; }
        public DbSet<RegistroSupervisor> ReporteRegistroSupervisores { get; set; }
        public DbSet<Imprimir> ReporteImpresiones { get; set; }
        public DbSet<Data.Reporte.Entidades.Configuracion> ReporteConfiguraciones { get; set; }



        public DbSet<Archivo> ConfiguracionArchivos { get; set; }
        public DbSet<TipoArchivo> ConfiguracionTipoArchivos { get; set; }
        public DbSet<Empresa> ConfiguracionEmpresas { get; set; }


        public DbSet<ServicioCompresionGnaLimaGas> MensualServicioCompresionGnaLimaGas { get; set; }
        public DbSet<ServicioCompresionGnaLimaGasVentas> MensualServicioCompresionGnaLimaGasVentas { get; set; }

        public DbSet<RegistroCromatografia> CartaRegistroCromatografias { get; set; }
    }
}
