using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Auth.Entidades;

namespace Unna.OperationalReport.Data.Mensual.Entidades.Mapeo
{
    class ServicioCompresionGnaLimaGasMapeo : IEntityTypeConfiguration<ServicioCompresionGnaLimaGas>
    {
        public void Configure(EntityTypeBuilder<ServicioCompresionGnaLimaGas> builder)
        {
            builder.ToTable("ServicioCompresionGnaLimaGas", "Mensual");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).HasColumnName("Id").IsRequired();
            builder.Property(e => e.Fecha).HasColumnName("Fecha").IsUnicode(false).HasMaxLength(100).IsRequired();
            builder.Property(e => e.Periodo).HasColumnName("Periodo");
            builder.Property(e => e.EnergiaVolumenProcesado).HasColumnName("EnergiaVolumenProcesado");
            builder.Property(e => e.SubTotalFacturar).HasColumnName("SubTotalFacturar");
            builder.Property(e => e.IgvCentaje).HasColumnName("IgvCentaje");
            builder.Property(e => e.Igv).HasColumnName("Igv");
            builder.Property(e => e.TotalFacturar).HasColumnName("TotalFacturar");
            builder.Property(e => e.Comentario).HasColumnName("Comentario");

            builder.Property(e => e.Creado).HasColumnName("Creado").IsRequired();
            builder.Property(e => e.Actualizado).HasColumnName("Actualizado");
            builder.Property(e => e.IdUsuario).HasColumnName("IdUsuario");
        }
    }
}
