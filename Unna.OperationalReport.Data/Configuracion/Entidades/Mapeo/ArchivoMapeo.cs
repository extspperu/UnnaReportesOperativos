using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Data.Configuracion.Entidades.Mapeo
{
    class ArchivoMapeo : IEntityTypeConfiguration<Archivo>
    {
        public void Configure(EntityTypeBuilder<Archivo> builder)
        {
            builder.ToTable("Archivo", "Configuracion");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).HasColumnName("IdArchivo").IsRequired();
            builder.Property(e => e.NombreArchivoOriginal).HasColumnName("NombreArchivoOriginal").IsUnicode(false).HasMaxLength(1000);
            builder.Property(e => e.NombreArchivo).HasColumnName("NombreArchivo").IsUnicode(false).HasMaxLength(50);
            builder.Property(e => e.RutaArchivo).HasColumnName("RutaArchivo").IsUnicode(false).HasMaxLength(1000);
            builder.Property(e => e.IdTipoArchivo).HasColumnName("IdTipoArchivo");
            builder.Property(e => e.Creado).HasColumnName("Creado").IsRequired();
            builder.Property(e => e.Actualizado).HasColumnName("Actualizado").IsRequired();
            builder.HasOne(e => e.TipoArchivo).WithMany(b => b.Archivos).HasForeignKey(c => c.IdTipoArchivo);

        }
    }
}
