using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Data.Configuracion.Entidades.Mapeo
{
    class EmpresaMapeo : IEntityTypeConfiguration<Empresa>
    {
        public void Configure(EntityTypeBuilder<Empresa> builder)
        {
            builder.ToTable("Empresa", "Configuracion");
            builder.HasKey(e => e.IdEmpresa);
            builder.Property(e => e.IdEmpresa).HasColumnName("IdEmpresa").IsRequired();
            builder.Property(e => e.RazonSocial).HasColumnName("RazonSocial").IsUnicode(false).HasMaxLength(500);
            builder.Property(e => e.Ruc).HasColumnName("Ruc").IsUnicode(false).HasMaxLength(20);
            builder.Property(e => e.Abreviatura).HasColumnName("Abreviatura").IsUnicode(false).HasMaxLength(100);
            builder.Property(e => e.NombreCorto).HasColumnName("NombreCorto").IsUnicode(false).HasMaxLength(100);
            builder.Property(e => e.Direccion).HasColumnName("Direccion").IsUnicode(false).HasMaxLength(500);
            builder.Property(e => e.CodigoOsinergmin).HasColumnName("CodigoOsinergmin").IsUnicode(false).HasMaxLength(100);
            builder.Property(e => e.NroRegistroHidrocarburo).HasColumnName("NroRegistroHidrocarburo").IsUnicode(false).HasMaxLength(100);

        }
    }
}
