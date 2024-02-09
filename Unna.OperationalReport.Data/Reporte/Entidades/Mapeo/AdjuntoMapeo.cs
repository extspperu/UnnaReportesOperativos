using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Data.Reporte.Entidades.Mapeo
{
    class AdjuntoMapeo : IEntityTypeConfiguration<Adjunto>
    {
        public void Configure(EntityTypeBuilder<Adjunto> builder)
        {
            builder.ToTable("Adjunto", "Reporte");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).HasColumnName("IdAdjunto").IsRequired().ValueGeneratedOnAdd();            
            builder.Property(e => e.Grupo).HasColumnName("Grupo").IsUnicode(false).HasMaxLength(200);
            builder.Property(e => e.Nomenclatura).HasColumnName("Nomenclatura").IsUnicode(false).HasMaxLength(200);
            builder.Property(e => e.Extension).HasColumnName("Extension").IsUnicode(false).HasMaxLength(200);
        }
    }
}
