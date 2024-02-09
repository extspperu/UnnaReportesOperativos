using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Data.Registro.Entidades.Mapeo
{
    class DatoMapeo : IEntityTypeConfiguration<Dato>
    {
        public void Configure(EntityTypeBuilder<Dato> builder)
        {
            builder.ToTable("Dato", "Registro");
            builder.HasKey(e => e.IdDato);
            builder.Property(e => e.IdDato).HasColumnName("IdDato").IsRequired();
            builder.Property(e => e.Nombre).HasColumnName("Nombre").IsUnicode(false).HasMaxLength(200).IsRequired();
            builder.Property(e => e.UnidadMedida).HasColumnName("UnidadMedida").IsUnicode(false).HasMaxLength(200);
            builder.Property(e => e.Tipo).HasColumnName("Tipo").IsUnicode(false);
        }
    }
}
