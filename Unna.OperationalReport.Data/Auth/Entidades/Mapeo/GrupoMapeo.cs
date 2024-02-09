using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Data.Auth.Entidades.Mapeo
{
    class GrupoMapeo : IEntityTypeConfiguration<Grupo>
    {
        public void Configure(EntityTypeBuilder<Grupo> builder)
        {
            builder.ToTable("Grupo", "Auth");
            builder.HasKey(e => e.IdGrupo);
            builder.Property(e => e.IdGrupo).HasColumnName("IdGrupo").IsRequired();
            builder.Property(e => e.Nombre).HasColumnName("Nombre").IsUnicode(false).HasMaxLength(100).IsRequired();
            builder.Property(e => e.EstaHabilitado).HasColumnName("EstaHabilitado");
            builder.Property(e => e.UrlDefecto).HasColumnName("UrlDefecto");
            
            builder.Property(e => e.Creado).HasColumnName("Creado").IsRequired();
            builder.Property(e => e.Actualizado).HasColumnName("Actualizado").IsRequired();
            builder.Property(e => e.EstaBorrado).HasColumnName("EstaBorrado").IsRequired();
            builder.Property(e => e.Borrado).HasColumnName("Borrado");            
        }
    }
}
