using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Data.Reporte.Entidades.Mapeo
{
    class AdjuntoSupervisorMapeo : IEntityTypeConfiguration<AdjuntoSupervisor>
    {
        public void Configure(EntityTypeBuilder<AdjuntoSupervisor> builder)
        {
            builder.ToTable("AdjuntoSupervisor", "Reporte");
            builder.HasKey(e => e.IdAdjuntoSupervisor);
            builder.Property(e => e.IdAdjuntoSupervisor).HasColumnName("IdAdjuntoSupervisor").IsRequired().ValueGeneratedOnAdd();
            builder.Property(e => e.EsConciliado).HasColumnName("EsConciliado").IsUnicode(false).HasMaxLength(200);
            builder.Property(e => e.IdAdjunto).HasColumnName("IdAdjunto").IsUnicode(false).HasMaxLength(200);
            builder.Property(e => e.IdArchivo).HasColumnName("IdArchivo");
            builder.Property(e => e.IdRegistroSupervisor).HasColumnName("IdRegistroSupervisor").IsUnicode(false).HasMaxLength(200);

            builder.Property(e => e.Creado).HasColumnName("Creado").IsUnicode(false).IsRequired();
            builder.Property(e => e.Actualizado).HasColumnName("Actualizado").IsUnicode(false);
            builder.Property(e => e.Borrado).HasColumnName("Borrado").IsUnicode(false);
            builder.Property(e => e.EstaBorrado).HasColumnName("EstaBorrado").IsUnicode(false);

            builder.HasOne(e => e.RegistroSupervisor).WithMany(b => b.AdjuntoSupervisores).HasForeignKey(c => c.IdRegistroSupervisor);
            builder.HasOne(e => e.Adjunto).WithMany(b => b.AdjuntoSupervisores).HasForeignKey(c => c.IdAdjunto);
            builder.HasOne(e => e.Archivo).WithMany(b => b.AdjuntoSupervisores).HasForeignKey(c => c.IdArchivo);

        }
    }
}
