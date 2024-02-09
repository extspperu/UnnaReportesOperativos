using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Data.Auth.Entidades.Mapeo
{
    class UsuarioLoteMapeo : IEntityTypeConfiguration<UsuarioLote>
    {
        public void Configure(EntityTypeBuilder<UsuarioLote> builder)
        {
            builder.ToTable("UsuarioLote", "Auth");
            builder.HasKey(e => e.IdUsuario);
            builder.HasKey(e => e.IdLote);
            builder.Property(e => e.IdUsuario).HasColumnName("IdUsuario").IsRequired();
            builder.Property(e => e.IdLote).HasColumnName("IdLote").IsUnicode(false).HasMaxLength(20).IsRequired();
            builder.Property(e => e.Creado).HasColumnName("Creado").IsUnicode(false).HasMaxLength(100);
            builder.Property(e => e.EstaActivo).HasColumnName("EstaActivo").IsUnicode(false);
            builder.Property(e => e.IdGrupo).HasColumnName("IdGrupo").IsUnicode(false);
            
        }
    }
}
