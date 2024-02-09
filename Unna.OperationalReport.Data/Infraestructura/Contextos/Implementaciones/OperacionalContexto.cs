using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Auth.Entidades;
using Unna.OperationalReport.Data.Auth.Entidades.Mapeo;
using Unna.OperationalReport.Data.Configuracion.Entidades;
using Unna.OperationalReport.Data.Configuracion.Entidades.Mapeo;
using Unna.OperationalReport.Data.Infraestructura.Contextos.Abstracciones;
using Unna.OperationalReport.Data.Registro.Entidades;
using Unna.OperationalReport.Data.Registro.Entidades.Mapeo;
using Unna.OperationalReport.Data.Reporte.Entidades;
using Unna.OperationalReport.Data.Reporte.Entidades.Mapeo;
using Unna.OperationalReport.Tools.DatosEF.Infraestructura.Contexto.Implementaciones;

namespace Unna.OperationalReport.Data.Infraestructura.Contextos.Implementaciones
{
    public class OperacionalContexto : EFDbContext, IOperacionalUnidadDeTrabajo
    {
        #pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
        public OperacionalContexto(DbContextOptions opciones) : base(opciones) { }
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.

        public DbSet<Usuario> AuthUsuarios { get; set; }
        public DbSet<UsuarioLote> AuthUsuarioLotes { get; set; }
        public DbSet<Grupo> AuthGrupos { get; set; }

        public DbSet<Dato> RegistroDatos { get; set; }
        public DbSet<DiaOperativo> RegistroDiaOperativos { get; set; }
        public DbSet<Registro.Entidades.Registro> RegistroRegistros { get; set; }
        public DbSet<Lote> RegistroLotes { get; set; }


        public DbSet<Adjunto> ReporteAdjuntos { get; set; }
        public DbSet<AdjuntoSupervisor> ReporteAdjuntoSupervisores { get; set; }
        public DbSet<RegistroSupervisor> ReporteRegistroSupervisores { get; set; }


        public DbSet<Archivo> ConfiguracionArchivos { get; set; }
        public DbSet<TipoArchivo> ConfiguracionTipoArchivos { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new UsuarioMapeo());
            modelBuilder.ApplyConfiguration(new UsuarioLoteMapeo());
            modelBuilder.ApplyConfiguration(new GrupoMapeo());

            modelBuilder.ApplyConfiguration(new DatoMapeo());
            modelBuilder.ApplyConfiguration(new DiaOperativoMapeo());
            modelBuilder.ApplyConfiguration(new RegistroMapeo());
            modelBuilder.ApplyConfiguration(new LoteMapeo());


            modelBuilder.ApplyConfiguration(new ArchivoMapeo());
            modelBuilder.ApplyConfiguration(new TipoArchivoMapeo());

            modelBuilder.ApplyConfiguration(new AdjuntoMapeo());
            modelBuilder.ApplyConfiguration(new AdjuntoSupervisorMapeo());
            modelBuilder.ApplyConfiguration(new RegistroSupervisorMapeo());


        }
    }
}
