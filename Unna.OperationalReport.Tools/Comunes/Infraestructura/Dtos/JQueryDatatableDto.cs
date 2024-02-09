using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos
{
    public class JQueryDatatableDto<TE>
        where TE : class
    {

        public JQueryDatatableDto()
        {
            Data = new List<TE>();
        }

        public JQueryDatatableDto(List<TE> datos)
        {
            Data = datos;
        }

        [JsonIgnore]
        public int Draw { get; set; }

        [JsonProperty(PropertyName = "data")]
        public List<TE> Data { get; set; }

        [JsonProperty(PropertyName = "recordsTotal")]
        public long RecordsTotal { get; set; }

        [JsonProperty(PropertyName = "recordsFiltered")]
        public long RecordsFiltered { get; set; }
    }
}
