using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using Unna.OperationalReport.Service.Reportes.Generales.Dtos;
using Unna.OperationalReport.Service.Reportes.Generales.Servicios.Abstracciones;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace Unna.OperationalReport.WebSite.Pages.Admin.EnvioCorreo
{
    public class IndexModel : PageModel
    {
        private readonly IReporteServicio _reporteServicio;
        public IndexModel(IReporteServicio reporteServicio)
        {
            _reporteServicio = reporteServicio;
        }


        public List<ReporteDto> reportes { get; set; }

        public async Task OnGet()
        {
            var operacion = await _reporteServicio.ListarAsync();
            
            reportes = operacion.Completado ? operacion.Resultado: new List<ReporteDto>();



            
        }

      


        public static void SendEmailFromAccount(Outlook.Application application, string subject, string body, string to, string smtpAddress)
        {

            // Create a new MailItem and set the To, Subject, and Body properties.
            Outlook.MailItem newMail = (Outlook.MailItem)application.CreateItem(Outlook.OlItemType.olMailItem);
            newMail.To = to;
            newMail.Subject = subject;
            newMail.Body = body;

            // Retrieve the account that has the specific SMTP address.
            Outlook.Account account = GetAccountForEmailAddress(application, smtpAddress);
            // Use this account to send the email.
            newMail.SendUsingAccount = account;
            newMail.Send();
        }


        public static Outlook.Account GetAccountForEmailAddress(Outlook.Application application, string smtpAddress)
        {

            // Loop over the Accounts collection of the current Outlook session.
            Outlook.Accounts accounts = application.Session.Accounts;
            foreach (Outlook.Account account in accounts)
            {
                // When the email address matches, return the account.
                if (account.SmtpAddress == smtpAddress)
                {
                    return account;
                }
            }
            throw new System.Exception(string.Format("No Account with SmtpAddress: {0} exists!", smtpAddress));
        }



        public static Boolean SendEmailWithOutlook(string mailDirection, string mailSubject)
        {
            try
            {
                var oApp = new Outlook.Application();
                Microsoft.Office.Interop.Outlook.NameSpace ns = oApp.GetNamespace("MAPI");
                var f = ns.GetDefaultFolder(Outlook.OlDefaultFolders.olFolderInbox);
                System.Threading.Thread.Sleep(1000);
                var mailItem = (Outlook.MailItem)oApp.CreateItem(Outlook.OlItemType.olMailItem);
                mailItem.Subject = mailSubject;
                mailItem.HTMLBody = "<p>hola</p>";
                mailItem.To = mailDirection;
                mailItem.Send();
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
            }
            return true;
        }

    }
}
