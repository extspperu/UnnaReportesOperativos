using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Data.Mensual.Entidades.Mapeo
{

    class ServicioCompresionGnaLimaGasVentasMapeo : IEntityTypeConfiguration<ServicioCompresionGnaLimaGasVentas>
    {
        public void Configure(EntityTypeBuilder<ServicioCompresionGnaLimaGasVentas> builder)
        {
            builder.ToTable("ServicioCompresionGnaLimaGasVentas", "Mensual");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).HasColumnName("Id").IsRequired();
            builder.Property(e => e.FechaDespacho).HasColumnName("FechaDespacho").IsUnicode(false).HasMaxLength(100).IsRequired();
            builder.Property(e => e.Placa).HasColumnName("Placa");
            builder.Property(e => e.NroConstanciaDespacho).HasColumnName("NroConstanciaDespacho");
            builder.Property(e => e.VolumenSm3).HasColumnName("VolumenSm3");
            builder.Property(e => e.VolumenMmpcs).HasColumnName("VolumenMmpcs");
            builder.Property(e => e.PoderCalorifico).HasColumnName("PoderCalorifico");
            builder.Property(e => e.Energia).HasColumnName("Energia");
            builder.Property(e => e.Precio).HasColumnName("Precio");
            builder.Property(e => e.SubTotal).HasColumnName("SubTotal");
            builder.Property(e => e.IdServicioCompresionGnaLimaGas).HasColumnName("IdServicioCompresionGnaLimaGas");

            builder.Property(e => e.Creado).HasColumnName("Creado").IsRequired();
            builder.Property(e => e.Actualizado).HasColumnName("Actualizado");
            builder.Property(e => e.IdUsuario).HasColumnName("IdUsuario");
            builder.Property(e => e.InicioCarga).HasColumnName("InicioCarga");
            builder.Property(e => e.FinCarga).HasColumnName("FinCarga");

            builder.HasOne(e => e.ServicioCompresionGnaLimaGas).WithMany(b => b.ServicioCompresionGnaLimaGasVentas).HasForeignKey(c => c.IdServicioCompresionGnaLimaGas);

        }
    }

}
