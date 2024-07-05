using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Data.Reporte.Entidades.Mapeo
{

    class ConfiguracionMapeo : IEntityTypeConfiguration<Configuracion>
    {
        public void Configure(EntityTypeBuilder<Configuracion> builder)
        {
            builder.ToTable("Configuracion", "Reporte");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).HasColumnName("IdConfiguracion").IsRequired().ValueGeneratedOnAdd();
            builder.Property(e => e.Nombre).HasColumnName("Nombre").IsUnicode(false).HasMaxLength(500);
            builder.Property(e => e.Version).HasColumnName("Version").IsUnicode(false).HasMaxLength(200);
            builder.Property(e => e.PreparadoPor).HasColumnName("PreparadoPör").IsUnicode(false).HasMaxLength(200);
            builder.Property(e => e.AprobadoPor).HasColumnName("AprobadoPor").IsUnicode(false).HasMaxLength(200);
            builder.Property(e => e.Detalle).HasColumnName("Detalle").IsUnicode(false).HasMaxLength(200);
            builder.Property(e => e.Fecha).HasColumnName("Fecha").IsUnicode(false).HasMaxLength(200);
            builder.Property(e => e.Grupo).HasColumnName("Grupo").IsUnicode(false).HasMaxLength(200);
            builder.Property(e => e.Creado).HasColumnName("Creado").IsUnicode(false).IsRequired();
            builder.Property(e => e.Actualizado).HasColumnName("Actualizado").IsUnicode(false).IsRequired();
            builder.Property(e => e.Borrado).HasColumnName("Borrado").IsUnicode(false).HasMaxLength(200);
            builder.Property(e => e.EstaBorrado).HasColumnName("EstaBorrado").IsUnicode(false).IsRequired();
            builder.Property(e => e.NombreReporte).HasColumnName("NombreReporte").IsUnicode(false);
            builder.Property(e => e.CorreoDestinatario).HasColumnName("CorreoDestinatario").IsUnicode(false);
            builder.Property(e => e.CorreoCc).HasColumnName("CorreoCc").IsUnicode(false);
            builder.Property(e => e.CorreoAsunto).HasColumnName("CorreoAsunto").IsUnicode(false);
            builder.Property(e => e.CorreoCuerpo).HasColumnName("CorreoCuerpo").IsUnicode(false);
            builder.Property(e => e.EnviarAdjunto).HasColumnName("EnviarAdjunto").IsRequired().IsUnicode(false);
        }
    }
}
