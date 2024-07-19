using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Service.Historico.Descarga.Servicios.Abstracciones;
namespace Unna.OperationalReport.Service.Historico.Descarga.Servicios.Implementaciones
{
    public class HistoricoServicios : IHistorico
    {
        public async Task<MemoryStream> ExistenciasExcel()
        {

            //var complexData = new
            //{
            //    Company = "Nombre de la Empresa",
                
            //};

            //var tempFilePath = Path.Combine(Path.GetTempPath(), "TemporaryName.xlsx");
            //var templatePath = @"./wwwroot/plantillas/reporte/Consultas/Existencias.xlsx";

            //using (var template = new XLTemplate(templatePath))
            //{
            //    template.AddVariable(complexData);
            //    template.Generate();
            //    template.SaveAs(tempFilePath);
            //}

            //using (var workbook = new XLWorkbook(tempFilePath))
            //{
            //    var worksheet = workbook.Worksheet("EXISTENCIAS");
            //    workbook.Save();
            //}

            //var bytes = File.ReadAllBytes(tempFilePath);
            //File.Delete(tempFilePath);

            MemoryStream stream = new MemoryStream();
            return stream;
        }
    }
}
