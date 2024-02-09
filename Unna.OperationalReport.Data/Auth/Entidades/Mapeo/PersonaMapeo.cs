using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Data.Auth.Entidades.Mapeo
{
    public class PersonaMapeo : IEntityTypeConfiguration<Persona>
    {
        public void Configure(EntityTypeBuilder<Persona> builder)
        {
            builder.ToTable("Persona", "Auth");
            builder.HasKey(e => e.IdPersona);
            builder.Property(e => e.IdPersona).HasColumnName("IdPersona").IsRequired();
            builder.Property(e => e.Documento).HasColumnName("Documento").IsUnicode(false).HasMaxLength(20).IsRequired();
            builder.Property(e => e.Nombres).HasColumnName("Nombres").IsUnicode(false).HasMaxLength(100);
            builder.Property(e => e.Paterno).HasColumnName("Paterno").IsUnicode(false);
            builder.Property(e => e.Materno).HasColumnName("Materno").IsUnicode(false);
            builder.Property(e => e.Sexo).HasColumnName("Sexo");
            builder.Property(e => e.FechaNacimiento).HasColumnName("FechaNacimiento");
            builder.Property(e => e.Direccion).HasColumnName("Direccion");
            builder.Property(e => e.Distrito).HasColumnName("Distrito");
            builder.Property(e => e.Provincia).HasColumnName("Provincia");
            builder.Property(e => e.Departamento).HasColumnName("Departamento");
            builder.Property(e => e.IdTipoPersona).HasColumnName("IdTipoPersona");
            builder.Property(e => e.Telefono).HasColumnName("Telefono");
            builder.Property(e => e.Correo).HasColumnName("Correo");
        }
    }
}
