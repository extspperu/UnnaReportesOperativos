using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Tools.WebComunes.Infraestructura.Errores
{
    public class ApiError : Exception
    {
        public HttpStatusCode HttpCode { get; private set; }
        public int CodigoError { get; private set; }

        public List<string>? Errores { get; private set; }

        public ApiError(HttpStatusCode httpCode, int codigoError, List<string>? errores)
        {
            HttpCode = httpCode;
            CodigoError = codigoError;
            Errores = errores;
        }
    }
}
