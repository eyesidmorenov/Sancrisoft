using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using Sancrisoft.Helpers;
using Sancrisoft.Models;

namespace Sancrisoft.Controllers
{
    public class AplicantesController : Controller
    {
        private BDSContext db = new BDSContext();

        // GET: Aplicantes
        public ActionResult Index()
        {
            var aplicantes = db.Aplicantes.Include(a => a.Cargos);
            return View(aplicantes.ToList());
        }

        // GET: Aplicantes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Aplicantes aplicantes = db.Aplicantes.Find(id);
            if (aplicantes == null)
            {
                return HttpNotFound();
            }
            return View(aplicantes);
        }

        // GET: Aplicantes/Create
        public ActionResult Create()
        {
			var nuevoAplicante = new Aplicantes()
			{
				FechaNacimiento = DateTime.Now
			};

			ViewBag.IdCargo = new SelectList(db.Cargos, "IdCargo", "Descripcion");
			return View(nuevoAplicante);
        }

        // POST: Aplicantes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdAplicante,Nombres,Apellidos,FechaNacimiento,Email,IdCargo,UrlFoto,FotoSubida")] Aplicantes aplicantes)
        {
            if (ModelState.IsValid)
            {
				var guardarFoto = new GuardarFoto();

				string filename = Guid.NewGuid().ToString();

				aplicantes.UrlFoto = guardarFoto.ResizeAndSave(filename, aplicantes.FotoSubida.InputStream, Tamanos.Miniatura, false);

                db.Aplicantes.Add(aplicantes);
                db.SaveChanges();

				string from = "likecomtic@gmail.com"; //any valid GMail ID  
				using (MailMessage mail = new MailMessage(from, aplicantes.Email))
				{
					mail.Subject = "Hemos recibido tu aplicación";
					mail.Body = "Estimado" + ' ' + aplicantes.Nombres + ' ' + aplicantes.Apellidos;
					//if (fileUploader != null)
					//{
					//	string fileName = Path.GetFileName(fileUploader.FileName);
					//	mail.Attachments.Add(new Attachment(fileUploader.InputStream, fileName));
					//}
					mail.IsBodyHtml = false;
					SmtpClient smtp = new SmtpClient();
					smtp.Host = "smtp.gmail.com";
					smtp.EnableSsl = true;
					NetworkCredential networkCredential = new NetworkCredential(from, "ARQ6102le7");
					smtp.UseDefaultCredentials = true;
					smtp.Credentials = networkCredential;
					smtp.Port = 587;
					//smtp.Host = "localhost";
					smtp.Send(mail);

					mail.Dispose();
					smtp.Dispose();

					ViewBag.Message = "Sent";
					//return View("Index", aplicantes);
				}

				return RedirectToAction("Index");
            }

            ViewBag.IdCargo = new SelectList(db.Cargos, "IdCargo", "Descripcion", aplicantes.IdCargo);
            return View(aplicantes);
        }

		//// POST: SancrisoftMails
		////[HttpPost]
		//public ActionResult SendMail([Bind(Include = "IdAplicante,Nombres,Apellidos,FechaNacimiento,Email,IdCargo,UrlFoto,FotoSubida")] Aplicantes aplicantes, HttpPostedFileBase fileUploader)
		//{
		//	//	if (ModelState.IsValid)
		//	//	{
		//	string from = "likecomtic@gmail.com"; //any valid GMail ID  
		//	using (MailMessage mail = new MailMessage(from, aplicantes.Email))
		//	{
		//		mail.Subject = "Hemos recibido tu aplicación";
		//		mail.Body = "Estimado" + ' ' + aplicantes.Nombres + ' ' + aplicantes.Apellidos;
		//		if (fileUploader != null)
		//		{
		//			string fileName = Path.GetFileName(fileUploader.FileName);
		//			mail.Attachments.Add(new Attachment(fileUploader.InputStream, fileName));
		//		}
		//		mail.IsBodyHtml = false;
		//		SmtpClient smtp = new SmtpClient();
		//		smtp.Host = "smtp.gmail.com";
		//		smtp.EnableSsl = true;
		//		NetworkCredential networkCredential = new NetworkCredential(from, "ARQ6102le7");
		//		smtp.UseDefaultCredentials = true;
		//		smtp.Credentials = networkCredential;
		//		smtp.Port = 587;
		//		smtp.Host = "localhost";
		//		smtp.Send(mail);
		//		ViewBag.Message = "Sent";
		//		return View("Index", aplicantes);
		//	}
		//}

		// GET: Aplicantes/Edit/5
		public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Aplicantes aplicantes = db.Aplicantes.Find(id);
            if (aplicantes == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdCargo = new SelectList(db.Cargos, "IdCargo", "Descripcion", aplicantes.IdCargo);
            return View(aplicantes);
        }

        // POST: Aplicantes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdAplicante,Nombres,Apellidos,IdCargo,UrlFoto")] Aplicantes aplicantes)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aplicantes).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdCargo = new SelectList(db.Cargos, "IdCargo", "Descripcion", aplicantes.IdCargo);
            return View(aplicantes);
        }

        // GET: Aplicantes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Aplicantes aplicantes = db.Aplicantes.Find(id);
            if (aplicantes == null)
            {
                return HttpNotFound();
            }
            return View(aplicantes);
        }

        // POST: Aplicantes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Aplicantes aplicantes = db.Aplicantes.Find(id);
            db.Aplicantes.Remove(aplicantes);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

		// POST: Aplicantes/postulate
		//[HttpPost]
		//[ValidateAntiForgeryToken]
		//public ActionResult Postulate(SancrisoftMails objModelMail, HttpPostedFileBase fileUploader)
		//{
		//	if (ModelState.IsValid)
		//	{
		//		string from = "likecomtic@gmail.com"; //any valid GMail ID  
		//		using (MailMessage mail = new MailMessage(from, objModelMail.To))
		//		{
		//			mail.Subject = objModelMail.Subject;
		//			mail.Body = objModelMail.Body;
		//			if (fileUploader != null)
		//			{
		//				string fileName = Path.GetFileName(fileUploader.FileName);
		//				mail.Attachments.Add(new Attachment(fileUploader.InputStream, fileName));
		//			}
		//			mail.IsBodyHtml = false;
		//			SmtpClient smtp = new SmtpClient();
		//			smtp.Host = "smtp.gmail.com";
		//			smtp.EnableSsl = true;
		//			NetworkCredential networkCredential = new NetworkCredential(from, "Gmail Id Password");
		//			smtp.UseDefaultCredentials = true;
		//			smtp.Credentials = networkCredential;
		//			smtp.Port = 587;
		//			smtp.Host = "localhost";
		//			smtp.Send(mail);
		//			ViewBag.Message = "Sent";
		//			return View("Index", objModelMail);
		//		}
		//	}
		//}

		protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
