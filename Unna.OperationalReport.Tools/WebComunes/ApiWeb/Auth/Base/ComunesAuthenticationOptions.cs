using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Tools.WebComunes.ApiWeb.Auth.Base
{
    public class ComunesAuthenticationOptions : AuthenticationSchemeOptions
    {
        public const string DefaultScemeName = "ComunesAuthenticationOptions";
        public string TokenHeaderName { get; set; } = "token-acceso";
    }
}
