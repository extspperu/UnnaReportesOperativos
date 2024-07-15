using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Data.Carta.Entidades.Mapeo
{

    class RegistroCromatografiaMapeo : IEntityTypeConfiguration<RegistroCromatografia>
    {
        public void Configure(EntityTypeBuilder<RegistroCromatografia> builder)
        {
            builder.ToTable("RegistroCromatografia", "Carta");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).HasColumnName("IdRegistroCromatografia").IsRequired().ValueGeneratedOnAdd();
            builder.Property(e => e.Periodo).HasColumnName("Periodo").IsUnicode(false).HasMaxLength(100).IsRequired();
            builder.Property(e => e.HoraMuestreo).HasColumnName("HoraMuestreo");
            builder.Property(e => e.Tipo).HasColumnName("Tipo");
            builder.Property(e => e.IdLote).HasColumnName("IdLote");
            builder.Property(e => e.Tanque).HasColumnName("Tanque");
            builder.Property(e => e.Creado).HasColumnName("Creado");
            builder.Property(e => e.Actualizado).HasColumnName("Actualizado");
            builder.Property(e => e.EstaBorrado).HasColumnName("EstaBorrado");
            builder.Property(e => e.IdUsuario).HasColumnName("IdUsuario");
        }
    }

}
