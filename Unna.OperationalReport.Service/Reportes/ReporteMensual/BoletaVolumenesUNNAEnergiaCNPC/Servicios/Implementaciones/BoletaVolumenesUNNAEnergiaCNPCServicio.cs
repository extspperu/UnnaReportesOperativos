using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletaVolumenesUNNAEnergiaCNPC.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletaVolumenesUNNAEnergiaCNPC.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;

namespace Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletaVolumenesUNNAEnergiaCNPC.Servicios.Implementaciones
{
    public class BoletaVolumenesUNNAEnergiaCNPCServicio : IBoletaVolumenesUNNAEnergiaCNPCServicio
    {
        public async Task<OperacionDto<BoletaVolumenesUNNAEnergiaCNPCDto>> ObtenerAsync(long idUsuario)
        {
            var dto = new BoletaVolumenesUNNAEnergiaCNPCDto
            {
                Año = "2023",//FechasUtilitario.ObtenerDiaOperativo().ToString("dd-MMMM-yyyy").Substring(3),
                Mes = "Noviembre",
                TotalGNAMPCS = 12341.00,
                TotalLGNGLPBLS = 283.63,
                TotalLGNCGNBLS = 86.10,
                TotalGNSMPCS = 9801.88,
                TotalGCMPCS = 476.08,

                TotalGravedadEspacificoGLP = 0.5388


            };
            dto.BoletaVolumenesUNNAEnergiaCNPCDet = await BoletaVolumenesUNNAEnergiaCNPCDet();

            return new OperacionDto<BoletaVolumenesUNNAEnergiaCNPCDto>(dto);
        }

        private async Task<List<BoletaVolumenesUNNAEnergiaCNPCDetDto>> BoletaVolumenesUNNAEnergiaCNPCDet()
        {
            List<BoletaVolumenesUNNAEnergiaCNPCDetDto> BoletaVolumenesUNNAEnergiaCNPCDet = new List<BoletaVolumenesUNNAEnergiaCNPCDetDto>();
            BoletaVolumenesUNNAEnergiaCNPCDet.Add(new BoletaVolumenesUNNAEnergiaCNPCDetDto
            {
                Fecha = 1,
                GNAMPCS = 0.00,
                LGNGLPBLS = 0.00,
                LGNCGNBLS = 0.00,
                GNSMPCS= 0.00,
                GCMPCS= 0.00



            });

            BoletaVolumenesUNNAEnergiaCNPCDet.Add(new BoletaVolumenesUNNAEnergiaCNPCDetDto
            {
                Fecha = 2,
                GNAMPCS = 0.00,
                LGNGLPBLS = 0.00,
                LGNCGNBLS = 0.00,
                GNSMPCS = 0.00,
                GCMPCS = 0.00



            });

            BoletaVolumenesUNNAEnergiaCNPCDet.Add(new BoletaVolumenesUNNAEnergiaCNPCDetDto
            {
                Fecha = 3,
                GNAMPCS = 0.00,
                LGNGLPBLS = 0.00,
                LGNCGNBLS = 0.00,
                GNSMPCS = 0.00,
                GCMPCS = 0.00



            });

            BoletaVolumenesUNNAEnergiaCNPCDet.Add(new BoletaVolumenesUNNAEnergiaCNPCDetDto
            {
                Fecha = 4,
                GNAMPCS = 0.00,
                LGNGLPBLS = 0.00,
                LGNCGNBLS = 0.00,
                GNSMPCS = 0.00,
                GCMPCS = 0.00



            });

            BoletaVolumenesUNNAEnergiaCNPCDet.Add(new BoletaVolumenesUNNAEnergiaCNPCDetDto
            {
                Fecha = 5,
                GNAMPCS = 0.00,
                LGNGLPBLS = 0.00,
                LGNCGNBLS = 0.00,
                GNSMPCS = 0.00,
                GCMPCS = 0.00



            });

            BoletaVolumenesUNNAEnergiaCNPCDet.Add(new BoletaVolumenesUNNAEnergiaCNPCDetDto
            {
                Fecha = 6,
                GNAMPCS = 0.00,
                LGNGLPBLS = 0.00,
                LGNCGNBLS = 0.00,
                GNSMPCS = 0.00,
                GCMPCS = 0.00



            });

            BoletaVolumenesUNNAEnergiaCNPCDet.Add(new BoletaVolumenesUNNAEnergiaCNPCDetDto
            {
                Fecha = 7,
                GNAMPCS = 0.00,
                LGNGLPBLS = 0.00,
                LGNCGNBLS = 0.00,
                GNSMPCS = 0.00,
                GCMPCS = 0.00



            });

            BoletaVolumenesUNNAEnergiaCNPCDet.Add(new BoletaVolumenesUNNAEnergiaCNPCDetDto
            {
                Fecha = 8,
                GNAMPCS = 0.00,
                LGNGLPBLS = 0.00,
                LGNCGNBLS = 0.00,
                GNSMPCS = 0.00,
                GCMPCS = 0.00



            });

            BoletaVolumenesUNNAEnergiaCNPCDet.Add(new BoletaVolumenesUNNAEnergiaCNPCDetDto
            {
                Fecha = 9,
                GNAMPCS = 0.00,
                LGNGLPBLS = 0.00,
                LGNCGNBLS = 0.00,
                GNSMPCS = 0.00,
                GCMPCS = 0.00



            });

            BoletaVolumenesUNNAEnergiaCNPCDet.Add(new BoletaVolumenesUNNAEnergiaCNPCDetDto
            {
                Fecha = 10,
                GNAMPCS = 0.00,
                LGNGLPBLS = 0.00,
                LGNCGNBLS = 0.00,
                GNSMPCS = 0.00,
                GCMPCS = 0.00



            });

            BoletaVolumenesUNNAEnergiaCNPCDet.Add(new BoletaVolumenesUNNAEnergiaCNPCDetDto
            {
                Fecha = 11,
                GNAMPCS = 0.00,
                LGNGLPBLS = 0.00,
                LGNCGNBLS = 0.00,
                GNSMPCS = 0.00,
                GCMPCS = 0.00



            });

            BoletaVolumenesUNNAEnergiaCNPCDet.Add(new BoletaVolumenesUNNAEnergiaCNPCDetDto
            {
                Fecha = 12,
                GNAMPCS = 0.00,
                LGNGLPBLS = 0.00,
                LGNCGNBLS = 0.00,
                GNSMPCS = 0.00,
                GCMPCS = 0.00



            });

            BoletaVolumenesUNNAEnergiaCNPCDet.Add(new BoletaVolumenesUNNAEnergiaCNPCDetDto
            {
                Fecha = 13,
                GNAMPCS = 0.00,
                LGNGLPBLS = 0.00,
                LGNCGNBLS = 0.00,
                GNSMPCS = 0.00,
                GCMPCS = 0.00



            });

            BoletaVolumenesUNNAEnergiaCNPCDet.Add(new BoletaVolumenesUNNAEnergiaCNPCDetDto
            {
                Fecha = 14,
                GNAMPCS = 0.00,
                LGNGLPBLS = 0.00,
                LGNCGNBLS = 0.00,
                GNSMPCS = 0.00,
                GCMPCS = 0.00



            });

            BoletaVolumenesUNNAEnergiaCNPCDet.Add(new BoletaVolumenesUNNAEnergiaCNPCDetDto
            {
                Fecha = 15,
                GNAMPCS = 0.00,
                LGNGLPBLS = 0.00,
                LGNCGNBLS = 0.00,
                GNSMPCS = 0.00,
                GCMPCS = 0.00



            });

            BoletaVolumenesUNNAEnergiaCNPCDet.Add(new BoletaVolumenesUNNAEnergiaCNPCDetDto
            {
                Fecha = 16,
                GNAMPCS = 0.00,
                LGNGLPBLS = 0.00,
                LGNCGNBLS = 0.00,
                GNSMPCS = 0.00,
                GCMPCS = 0.00



            });

            BoletaVolumenesUNNAEnergiaCNPCDet.Add(new BoletaVolumenesUNNAEnergiaCNPCDetDto
            {
                Fecha = 17,
                GNAMPCS = 0.00,
                LGNGLPBLS = 0.00,
                LGNCGNBLS = 0.00,
                GNSMPCS = 0.00,
                GCMPCS = 0.00



            });

            BoletaVolumenesUNNAEnergiaCNPCDet.Add(new BoletaVolumenesUNNAEnergiaCNPCDetDto
            {
                Fecha = 18,
                GNAMPCS = 0.00,
                LGNGLPBLS = 0.00,
                LGNCGNBLS = 0.00,
                GNSMPCS = 0.00,
                GCMPCS = 0.00



            });

            BoletaVolumenesUNNAEnergiaCNPCDet.Add(new BoletaVolumenesUNNAEnergiaCNPCDetDto
            {
                Fecha = 19,
                GNAMPCS = 0.00,
                LGNGLPBLS = 0.00,
                LGNCGNBLS = 0.00,
                GNSMPCS = 0.00,
                GCMPCS = 0.00



            });

            BoletaVolumenesUNNAEnergiaCNPCDet.Add(new BoletaVolumenesUNNAEnergiaCNPCDetDto
            {
                Fecha = 20,
                GNAMPCS = 0.00,
                LGNGLPBLS = 0.00,
                LGNCGNBLS = 0.00,
                GNSMPCS = 0.00,
                GCMPCS = 0.00



            });

            BoletaVolumenesUNNAEnergiaCNPCDet.Add(new BoletaVolumenesUNNAEnergiaCNPCDetDto
            {
                Fecha = 21,
                GNAMPCS = 0.00,
                LGNGLPBLS = 0.00,
                LGNCGNBLS = 0.00,
                GNSMPCS = 0.00,
                GCMPCS = 0.00



            });

            BoletaVolumenesUNNAEnergiaCNPCDet.Add(new BoletaVolumenesUNNAEnergiaCNPCDetDto
            {
                Fecha = 22,
                GNAMPCS = 0.00,
                LGNGLPBLS = 0.00,
                LGNCGNBLS = 0.00,
                GNSMPCS = 0.00,
                GCMPCS = 0.00



            });

            BoletaVolumenesUNNAEnergiaCNPCDet.Add(new BoletaVolumenesUNNAEnergiaCNPCDetDto
            {
                Fecha = 23,
                GNAMPCS = 0.00,
                LGNGLPBLS = 0.00,
                LGNCGNBLS = 0.00,
                GNSMPCS = 0.00,
                GCMPCS = 0.00



            });

            BoletaVolumenesUNNAEnergiaCNPCDet.Add(new BoletaVolumenesUNNAEnergiaCNPCDetDto
            {
                Fecha = 24,
                GNAMPCS = 0.00,
                LGNGLPBLS = 0.00,
                LGNCGNBLS = 0.00,
                GNSMPCS = 0.00,
                GCMPCS = 0.00



            });

            BoletaVolumenesUNNAEnergiaCNPCDet.Add(new BoletaVolumenesUNNAEnergiaCNPCDetDto
            {
                Fecha = 25,
                GNAMPCS = 0.00,
                LGNGLPBLS = 0.00,
                LGNCGNBLS = 0.00,
                GNSMPCS = 0.00,
                GCMPCS = 0.00



            });

            BoletaVolumenesUNNAEnergiaCNPCDet.Add(new BoletaVolumenesUNNAEnergiaCNPCDetDto
            {
                Fecha = 26,
                GNAMPCS = 0.00,
                LGNGLPBLS = 0.00,
                LGNCGNBLS = 0.00,
                GNSMPCS = 0.00,
                GCMPCS = 0.00



            });

            BoletaVolumenesUNNAEnergiaCNPCDet.Add(new BoletaVolumenesUNNAEnergiaCNPCDetDto
            {
                Fecha = 27,
                GNAMPCS = 0.00,
                LGNGLPBLS = 0.00,
                LGNCGNBLS = 0.00,
                GNSMPCS = 0.00,
                GCMPCS = 0.00



            });

            BoletaVolumenesUNNAEnergiaCNPCDet.Add(new BoletaVolumenesUNNAEnergiaCNPCDetDto
            {
                Fecha = 28,
                GNAMPCS = 0.00,
                LGNGLPBLS = 0.00,
                LGNCGNBLS = 0.00,
                GNSMPCS = 0.00,
                GCMPCS = 0.00



            });

            BoletaVolumenesUNNAEnergiaCNPCDet.Add(new BoletaVolumenesUNNAEnergiaCNPCDetDto
            {
                Fecha = 29,
                GNAMPCS = 0.00,
                LGNGLPBLS = 0.00,
                LGNCGNBLS = 0.00,
                GNSMPCS = 0.00,
                GCMPCS = 0.00



            });

            BoletaVolumenesUNNAEnergiaCNPCDet.Add(new BoletaVolumenesUNNAEnergiaCNPCDetDto
            {
                Fecha = 30,
                GNAMPCS = 0.00,
                LGNGLPBLS = 0.00,
                LGNCGNBLS = 0.00,
                GNSMPCS = 0.00,
                GCMPCS = 0.00



            });
            return BoletaVolumenesUNNAEnergiaCNPCDet;
        }

    }
    }
