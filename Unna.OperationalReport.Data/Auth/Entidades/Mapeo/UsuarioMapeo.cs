using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Data.Auth.Entidades.Mapeo
{
    public class UsuarioMapeo : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("Usuario", "Auth");
            builder.HasKey(e => e.IdUsuario);
            builder.Property(e => e.IdUsuario).HasColumnName("IdUsuario").IsRequired().ValueGeneratedOnAdd();
            builder.Property(e => e.Username).HasColumnName("Username").IsUnicode(false).HasMaxLength(100).IsRequired();
            builder.Property(e => e.Password).HasColumnName("Password").IsUnicode(false).HasMaxLength(100).IsRequired();
            builder.Property(e => e.PasswordSalt).HasColumnName("PasswordSalt").IsUnicode(false).HasMaxLength(100).IsRequired();
            builder.Property(e => e.IdGrupo).HasColumnName("IdGrupo").IsUnicode(false).HasMaxLength(100);
            builder.Property(e => e.EstaHabilitado).HasColumnName("EstaHabilitado").IsUnicode(false).HasMaxLength(100).IsRequired();
            builder.Property(e => e.EstaBorrado).HasColumnName("EstaBorrado").IsUnicode(false);
            builder.Property(e => e.Creado).HasColumnName("Creado").IsRequired();
            builder.Property(e => e.Actualizado).HasColumnName("Actualizado").IsRequired();
            builder.Property(e => e.EstaBorrado).HasColumnName("EstaBorrado").IsRequired();
            builder.Property(e => e.Borrado).HasColumnName("Borrado");
            builder.Property(e => e.IdPersona).HasColumnName("IdPersona").IsUnicode(false).HasMaxLength(1000);
            builder.Property(e => e.UltimoLogin).HasColumnName("UltimoLogin").IsUnicode(false).HasMaxLength(1000);
            builder.Property(e => e.EsAdministrador).HasColumnName("EsAdministrador").IsUnicode(false);
            builder.Property(e => e.UrlFirma).HasColumnName("UrlFirma").IsUnicode(false);

            builder.HasOne(e => e.Persona).WithMany(b => b.Usuarios).HasForeignKey(c => c.IdPersona);

            //builder.HasOne(e => e.Tipo).WithMany(b => b.Usuarios).HasForeignKey(c => c.IdTipo);
        }
    }
}
