using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Tools.WebComunes.WebSite.Base;

namespace Unna.OperationalReport.Tools.WebComunes.WebSite.Auth
{
    public class ControladorAuth: ControladorBase
    {
        private readonly CookieAuthenticationOptions _cookieAuthenticationOptions;

        public ControladorAuth(IOptionsMonitor<CookieAuthenticationOptions> optionsMonitor)
        {
            _cookieAuthenticationOptions = optionsMonitor.Get(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        [HttpGet("Logout")]
        public async Task<ActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToPage(_cookieAuthenticationOptions.LoginPath);
        }

    }
}
