using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Data.Registro.Entidades.Mapeo
{
    class RegistroMapeo : IEntityTypeConfiguration<Registro>
    {
        public void Configure(EntityTypeBuilder<Registro> builder)
        {
            builder.ToTable("Registro", "Registro");
            builder.HasKey(e => e.IdRegistro);
            builder.Property(e => e.IdRegistro).HasColumnName("IdRegistro").IsRequired().ValueGeneratedOnAdd();
            builder.Property(e => e.Valor).HasColumnName("Valor");
            builder.Property(e => e.EsConciliado).HasColumnName("EsConciliado").IsUnicode(false).HasMaxLength(200);
            builder.Property(e => e.IdDato).HasColumnName("IdDato").IsUnicode(false);
            builder.Property(e => e.IdDiaOperativo).HasColumnName("IdDiaOperativo").IsUnicode(false);
            builder.Property(e => e.IdUsuario).HasColumnName("IdUsuario").IsUnicode(false);
            builder.Property(e => e.EsValido).HasColumnName("EsValido").IsUnicode(false);
            builder.Property(e => e.FechaValido).HasColumnName("FechaValido").IsUnicode(false);
            builder.Property(e => e.IdUsuarioValidador).HasColumnName("IdUsuarioValidador").IsUnicode(false);
            builder.Property(e => e.EsEditadoPorProceso).HasColumnName("EsEditadoPorProceso").IsUnicode(false);
            builder.Property(e => e.EsDevuelto).HasColumnName("EsDevuelto").IsUnicode(false);

            builder.Property(e => e.Creado).HasColumnName("Creado").IsRequired().IsUnicode(false);
            builder.Property(e => e.Actualizado).HasColumnName("Actualizado").IsUnicode(false);
            builder.Property(e => e.Borrado).HasColumnName("Borrado").IsUnicode(false);
            builder.Property(e => e.EstaBorrado).HasColumnName("EstaBorrado").IsUnicode(false).IsRequired();

            builder.HasOne(e => e.DiaOpetarivo).WithMany(b => b.Registros).HasForeignKey(c => c.IdDiaOperativo);
            builder.HasOne(e => e.Dato).WithMany(b => b.Registros).HasForeignKey(c => c.IdDato);
        }
    }
}
