using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Data.Reporte.Entidades.Mapeo
{
    class ImprimirMapeo : IEntityTypeConfiguration<Imprimir>
    {
        public void Configure(EntityTypeBuilder<Imprimir> builder)
        {
            builder.ToTable("Imprimir", "Reporte");
            builder.HasKey(e => e.IdImprimir);
            builder.Property(e => e.IdImprimir).HasColumnName("IdImprimir").IsRequired().ValueGeneratedOnAdd();
            builder.Property(e => e.IdConfiguracion).HasColumnName("IdConfiguracion").IsUnicode(false).IsRequired();
            builder.Property(e => e.Fecha).HasColumnName("Fecha").IsUnicode(false).IsRequired();
            builder.Property(e => e.Datos).HasColumnName("Datos").IsUnicode(false);
            builder.Property(e => e.IdUsuario).HasColumnName("IdUsuario");
            builder.Property(e => e.Comentario).HasColumnName("Comentario");
            builder.Property(e => e.EsEditado).HasColumnName("EsEditado").IsUnicode(false);

            builder.Property(e => e.Creado).HasColumnName("Creado").IsUnicode(false).IsRequired();
            builder.Property(e => e.Actualizado).HasColumnName("Actualizado").IsUnicode(false);
            builder.Property(e => e.Borrado).HasColumnName("Borrado").IsUnicode(false);
            builder.Property(e => e.EstaBorrado).HasColumnName("EstaBorrado").IsUnicode(false);
            builder.Property(e => e.RutaArchivoPdf).HasColumnName("RutaArchivoPdf").IsUnicode(false);
            builder.Property(e => e.RutaArchivoExcel).HasColumnName("RutaArchivoExcel").IsUnicode(false);
            

            builder.HasOne(e => e.Configuracion).WithMany(b => b.Impresiones).HasForeignKey(c => c.IdConfiguracion);

        }
    }
}
