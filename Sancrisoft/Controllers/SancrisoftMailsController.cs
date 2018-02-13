using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Net.Mail;
using System.IO;
using Sancrisoft.Models;

namespace Sancrisoft.Controllers
{
    public class SancrisoftMailsController : Controller
    {

		private BDSContext db = new BDSContext();

		//// GET: SancrisoftMails
		//public ActionResult SendMail()
		//{
		//	return View();
		//}

		// POST: SancrisoftMails
		//[HttpPost]
		public ActionResult SendMail([Bind(Include = "IdAplicante,Nombres,Apellidos,FechaNacimiento,Email,IdCargo,UrlFoto,FotoSubida")] Aplicantes aplicantes, HttpPostedFileBase fileUploader)
		{
		  //	if (ModelState.IsValid)
		  //	{
				string from = "likecomtic@gmail.com"; //any valid GMail ID  
				using (MailMessage mail = new MailMessage(from, aplicantes.Email))
				{
				    mail.Subject = "Hemos recibido tu aplicación";
					mail.Body = "Estimado" + ' ' + aplicantes.Nombres + ' ' + aplicantes.Apellidos;
					if (fileUploader != null)
					{
						string fileName = Path.GetFileName(fileUploader.FileName);
						mail.Attachments.Add(new Attachment(fileUploader.InputStream, fileName));
					}
					mail.IsBodyHtml = false;
					SmtpClient smtp = new SmtpClient();
					smtp.Host = "smtp.gmail.com";
					smtp.EnableSsl = true;
					NetworkCredential networkCredential = new NetworkCredential(from, "ARQ6102le7");
					smtp.UseDefaultCredentials = true;
					smtp.Credentials = networkCredential;
					smtp.Port = 587;
					smtp.Host = "localhost";
					smtp.Send(mail);
					ViewBag.Message = "Sent";
					return View("Index", aplicantes);
				}
			//}
			//else
			//{
			//	return View();
			//}
		}
    }
}
