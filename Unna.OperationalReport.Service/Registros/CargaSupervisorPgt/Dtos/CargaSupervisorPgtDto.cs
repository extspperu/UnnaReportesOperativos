﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Registro;
using Unna.OperationalReport.Data.Registro.Entidades;

namespace Unna.OperationalReport.Service.Registros.CargaSupervisorPgt.Dtos
{
    public class CargaSupervisorPgtDto
    {

        public List<VolumenDeltaVDto>? VolumenDeltaV { get; set; }
        public List<GnsVolumeMsYPcBrutoDto>? VolumenMsPcBrutoGns { get; set; }
        public List<GnsVolumeMsYPcBrutoDto>? VolumenMsPcBrutoVol { get; set; }
        public List<DatoDeltaV>? DatoDeltaV { get; set; }
        public List<ProduccionDiariaMsDto>? ProduccionDiariaMs { get; set; }
        public List<DatoCgnDto>? DatoCgn { get; set; }
        public List<VolumenDespachoDto>? VolumenDespachoGlp { get; set; }
        public List<VolumenDespachoDto>? VolumenDespachoCgn { get; set; }
        public List<DespachoGlpEnvasadoDto>? DespachoGlpEnvasado { get; set; }
        
        public double? AlmacenamientoLimaGasBbl { get; set; }


    }
}
