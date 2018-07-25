using Model;
using proyecto.App_Start;
using proyecto.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using Rotativa.MVC;

namespace proyecto.Controllers
{
    public class DefaultController : Controller
    {
        private Usuario usuario = new Usuario();

        public ActionResult Index()
        {
            return View(usuario.Obtener(FrontOfficeStartUp.UsuarioVisualizando(), true));
        }

        public JsonResult EnviarCorreo(ContactoViewModel model)
        {
            var rm = new ResponseModel();

            if (ModelState.IsValid)
            {
                try
                {
                    var _usuario = usuario.Obtener(FrontOfficeStartUp.UsuarioVisualizando());

                    var mail = new MailMessage();
                    mail.From = new MailAddress(model.Correo, model.Nombre);
                    mail.To.Add(_usuario.Email);
                    mail.Subject = "Correo desde contacto";
                    mail.IsBodyHtml = true;
                    mail.Body = model.Mensaje;

                    var SmtpServer = new SmtpClient("smtp.live.com"); // or "smtp.gmail.com"
                    SmtpServer.Port = 587;
                    SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
                    SmtpServer.UseDefaultCredentials = false;

                    // Agrega tu correo y tu contraseña, hemos usado el servidor de Outlook.
                    SmtpServer.Credentials = new System.Net.NetworkCredential("hitogoroshi@outlook.com", "PON_TU_CONTRASENA");
                    SmtpServer.EnableSsl = true;
                    SmtpServer.Send(mail);
                }
                catch (Exception e)
                {
                    rm.SetResponse(false, e.Message);
                    return Json(rm);
                    throw;
                }

                rm.SetResponse(true);
                rm.function = "CerrarContacto();";
            }

            return Json(rm);
        }

        public JsonResult Comentar(Testimonio testimonio)
        {
            var rm = new ResponseModel();

            if (ModelState.IsValid)
            {
                rm = testimonio.Guardar();

                if (rm.response) rm.message = "Gracias por comentar";
            }

            return Json(rm);
        }

        public ActionResult ExportaAPDF()
        {
            return new ActionAsPdf("PDF");
        }

        public ActionResult PDF()
        {
            return View(
                usuario.Obtener(FrontOfficeStartUp.UsuarioVisualizando(), true)
            );
        }

    }
}