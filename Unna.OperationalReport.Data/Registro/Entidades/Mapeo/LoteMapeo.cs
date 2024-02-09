using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Data.Registro.Entidades.Mapeo
{
    class LoteMapeo : IEntityTypeConfiguration<Lote>
    {
        public void Configure(EntityTypeBuilder<Lote> builder)
        {
            builder.ToTable("Lote", "Registro");
            builder.HasKey(e => e.IdLote);
            builder.Property(e => e.IdLote).HasColumnName("IdLote").IsRequired().ValueGeneratedOnAdd();
            builder.Property(e => e.Nombre).HasColumnName("Nombre").IsUnicode(false).HasMaxLength(200);
        }
    }
}
