using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Data.Configuracion.Entidades.Mapeo
{
    class TipoArchivoMapeo : IEntityTypeConfiguration<TipoArchivo>
    {
        public void Configure(EntityTypeBuilder<TipoArchivo> builder)
        {
            builder.ToTable("TipoArchivo", "Configuracion");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).HasColumnName("IdTipoArchivo").IsRequired();
            builder.Property(e => e.Tipo).HasColumnName("Tipo").IsUnicode(false).HasMaxLength(50);
            builder.Property(e => e.Extension).HasColumnName("Extension").IsUnicode(false).HasMaxLength(20);
            builder.Property(e => e.TypeMime).HasColumnName("TypeMime").IsUnicode(false).HasMaxLength(100);

        }
    }
}
