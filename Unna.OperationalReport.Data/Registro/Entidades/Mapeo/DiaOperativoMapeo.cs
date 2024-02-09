using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Data.Registro.Entidades.Mapeo
{
    class DiaOperativoMapeo : IEntityTypeConfiguration<DiaOperativo>
    {
        public void Configure(EntityTypeBuilder<DiaOperativo> builder)
        {
            builder.ToTable("DiaOperativo", "Registro");
            builder.HasKey(e => e.IdDiaOperativo);
            builder.Property(e => e.IdDiaOperativo).HasColumnName("IdDiaOperativo").IsRequired().ValueGeneratedOnAdd();
            builder.Property(e => e.Fecha).HasColumnName("Fecha").IsUnicode(false).IsRequired();
            builder.Property(e => e.NumeroRegistro).HasColumnName("NumeroRegistro");
            builder.Property(e => e.Adjuntos).HasColumnName("Adjuntos").IsUnicode(false);
            builder.Property(e => e.Comentario).HasColumnName("Comentario").IsUnicode(false);
            builder.Property(e => e.Creado).HasColumnName("Creado").IsUnicode(false).IsRequired();
            builder.Property(e => e.Actualizado).HasColumnName("Actualizado").IsUnicode(false);
            builder.Property(e => e.Borrado).HasColumnName("Borrado").IsUnicode(false);
            builder.Property(e => e.EstaBorrado).HasColumnName("EstaBorrado").IsUnicode(false);
            builder.Property(e => e.IdUsuario).HasColumnName("IdUsuario").IsUnicode(false);
            builder.Property(e => e.DatoCulminado).HasColumnName("DatoCulminado").IsUnicode(false);
            builder.Property(e => e.DatoValidado).HasColumnName("DatoValidado").IsUnicode(false);
            builder.Property(e => e.IdLote).HasColumnName("IdLote").IsUnicode(false);
            builder.Property(e => e.EsObservado).HasColumnName("EsObservado");
            builder.Property(e => e.IdUsuarioObservado).HasColumnName("IdUsuarioObservado");
            builder.Property(e => e.FechaObservado).HasColumnName("FechaObservado");
            builder.Property(e => e.FechaValidado).HasColumnName("FechaValidado");
            builder.Property(e => e.IdUsuarioValidado).HasColumnName("IdUsuarioValidado");
            builder.Property(e => e.IdGrupo).HasColumnName("IdGrupo");
            builder.HasOne(e => e.Lote).WithMany(b => b.DiaOperativos).HasForeignKey(c => c.IdLote);

        }
    }
}
