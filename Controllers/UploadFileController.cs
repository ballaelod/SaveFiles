using SaveFiles.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;



namespace SaveFiles.Controllers
{
    public class UploadFileController : Controller
    {
        private DataSaveDBEntities db = new DataSaveDBEntities();

        [HttpGet]
        public ActionResult Upload()
        {

            return View();
        }
        
        [HttpPost]
       // public ActionResult Upload(Saveimg filemodel)
       //{
       //    try 
       //    {
       //        string fileName = Path.GetFileNameWithoutExtension(filemodel.UploadFile.FileName);
       //        filemodel.ImgPath = "~/UploadedFile/" + fileName + ".jpg";
       //        filemodel.ImgName = fileName;
       //        fileName = Path.Combine(Server.MapPath("~/UploadedFile/"), fileName);
       //        filemodel.UploadFile.SaveAs(fileName + ".jpg");
       //        db.Saveimgs.Add(filemodel);
       //        db.SaveChanges();
       //    }
       //    catch
       //    {
       //        ViewBag.msg = "Error while uploading the files";
       //    }
    
            
            
            
            
       //     ModelState.Clear();
       //     return View();  
       //}

        public ActionResult UploadFiles(HttpPostedFileBase[] files)
        {

            //Ensure model state is valid  
            if (ModelState.IsValid)
            {   //iterating through multiple file collection   
                foreach (HttpPostedFileBase file in files)
                {
                    //Checking file is available to save.  
                    if (file != null)
                    {
                        var InputFileName = Path.GetFileName(file.FileName);
                        var ServerSavePath = Path.Combine(Server.MapPath("~/UploadedFile/") + InputFileName);
                        //Save file to server folder  
                        file.SaveAs(ServerSavePath);
                        //assigning file uploaded status to ViewBag for showing message to user.  
                        ViewBag.UploadStatus = files.Count().ToString() + " files uploaded successfully.";
                    }

                }
            }
            return View();
        }  

        [HttpGet]
        public ActionResult View(int id)
        {
            Saveimg filemodel = new Saveimg();
            
            filemodel = db.Saveimgs.Where(f => f.ImgId == id).FirstOrDefault();
            
                return View(filemodel);
            

        }
        public ActionResult List(string search)
        {
            var file = from src in db.Saveimgs
                       select src;
            if(!String.IsNullOrEmpty(search))
            {
                file = file.Where(f => f.ImgName.Contains(search));
            }

            return View(file.ToList());

        }
        public ActionResult RenameAll([Bind(Include = "ImgId,ImgName,ImgPath")] Saveimg svim)
        {
            if (ModelState.IsValid)
            {
                db.Entry(svim).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("List");
            }
            
            return View(svim);
        }
     
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Saveimg img = db.Saveimgs.Find(id);
            if (img == null)
            {
                return HttpNotFound();
            }
            return View(img);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Saveimg img = db.Saveimgs.Find(id);
            db.Saveimgs.Remove(img);
            db.SaveChanges();
            return RedirectToAction("List");
        }

        public ActionResult Rename(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Saveimg svm = db.Saveimgs.Find(id);
            if (svm == null)
            {
                return HttpNotFound();
            }
            ViewBag.ImgId = new SelectList(db.Saveimgs, "ImgId", "ImgName", svm.ImgId);
            return View(svm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Rename([Bind(Include = "ImgId,ImgName,ImgPath")] Saveimg svm)
        {
            if (ModelState.IsValid)
            {
                db.Entry(svm).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("List");
            }
            ViewBag.ImgId = new SelectList(db.Saveimgs, "Imgid", "ImgName", svm.ImgId);
            return View(svm);
        }
    }


}