using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Data.Reporte.Entidades.Mapeo
{
 
    class RegistroSupervisorMapeo : IEntityTypeConfiguration<RegistroSupervisor>
    {
        public void Configure(EntityTypeBuilder<RegistroSupervisor> builder)
        {
            builder.ToTable("RegistroSupervisor", "Reporte");
            builder.HasKey(e => e.IdRegistroSupervisor);
            builder.Property(e => e.IdRegistroSupervisor).HasColumnName("IdRegistroSupervisor").IsRequired().ValueGeneratedOnAdd();
            builder.Property(e => e.Fecha).HasColumnName("Fecha").IsUnicode(false).HasMaxLength(200).IsRequired();
            builder.Property(e => e.Comentario).HasColumnName("Comentario").IsUnicode(false);
            builder.Property(e => e.IdArchivo).HasColumnName("IdArchivo").IsUnicode(false);
            builder.Property(e => e.IdUsuario).HasColumnName("IdUsuario");

            builder.Property(e => e.EsValidado).HasColumnName("EsValidado");
            builder.Property(e => e.FechaValidado).HasColumnName("FechaValidado");
            builder.Property(e => e.IdUsuarioValidado).HasColumnName("IdUsuarioValidado");
            builder.Property(e => e.EsObservado).HasColumnName("EsObservado");
            builder.Property(e => e.FechaObservado).HasColumnName("FechaObservado");
            builder.Property(e => e.IdUsuarioObservado).HasColumnName("IdUsuarioObservado");

            builder.Property(e => e.Creado).HasColumnName("Creado").IsUnicode(false).IsRequired();
            builder.Property(e => e.Actualizado).HasColumnName("Actualizado").IsUnicode(false);
            builder.Property(e => e.Borrado).HasColumnName("Borrado").IsUnicode(false);
            builder.Property(e => e.EstaBorrado).HasColumnName("EstaBorrado").IsUnicode(false);

            builder.HasOne(e => e.Archivo).WithMany(b => b.RegistroSupervisores).HasForeignKey(c => c.IdArchivo);

        }
    }
}
