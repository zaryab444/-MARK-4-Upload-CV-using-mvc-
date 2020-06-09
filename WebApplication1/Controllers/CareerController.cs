using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class CareerController : Controller
    {
        //
        // GET: /Career/
      //  CareersEntities db = new CareersEntities();
        public ActionResult SubmitCV()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitCV(CV C, HttpPostedFileBase file)
        {
            //string fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            //  C.CV1 = (Path.Combine(Server.MapPath("~/UploadedCV"), fileName));
            string path = uploadfile(file);
            if (path.Equals("-1"))
            {
                ViewBag.error = "file could not be uploaded....";
            }
            else
            {

                CV u = new CV();
                u.CID = C.CID;
                u.Full_Name = C.Full_Name;
                u.Email_id = C.Email_id;
                u.CV1 = path;

                //db.CVs.Add(u);
                // db.SaveChanges();
                string fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
              //  C.CV1 = (Path.Combine(Server.MapPath("~/UploadedCV"), fileName));
                C.CV1 = Path.Combine(Server.MapPath("~/UploadedCV"), fileName);
                using (CareersEntities dc = new CareersEntities())
                {
                    C.CV1 = fileName;
                    dc.CVs.Add(C);
                //    dc.SaveChanges();







                }
              
            }
            return View();    
        }
        public string uploadfile(HttpPostedFileBase file)
        {
            Random r = new Random();
            string path = "-1";
            int random = r.Next();
            if (file != null && file.ContentLength > 0)
            {
                string extension = Path.GetExtension(file.FileName);
                if (extension.ToLower().Equals(".pdf") || extension.ToLower().Equals(".docs") )
                {
                    try
                    {

                        path = Path.Combine(Server.MapPath("~/UploadCV"), random + Path.GetFileName(file.FileName));
                        file.SaveAs(path);
                        path = "~/UploadCV" + random + Path.GetFileName(file.FileName);

                          ViewBag.Message = "File uploaded successfully";
                    }
                    catch (Exception ex)
                    {
                        path = "-1";
                    }
                }
                else
                {
                    Response.Write("<script>alert('Only pdf formats are acceptable....'); </script>");
                }
            }

            else
            {
                Response.Write("<script>alert('Please select a file'); </script>");
                path = "-1";
            }



            return path;
        }
    }
}