using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace SaveFiles.Controllers
{
    public class DownloadController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Download()
        {
            string path = Server.MapPath("~/UploadedFile/");
            DirectoryInfo dirinf = new DirectoryInfo(path);
            FileInfo[] files = dirinf.GetFiles("*.*");
            List<string> lst = new List<string>(files.Length);
            
            foreach(var item in files)
            {
                lst.Add(item.Name);
            }

            return View(lst);
        }

        public ActionResult DownloadFile(string filename)
        {
            if (Path.GetExtension(filename) == ".jpg" || Path.GetExtension(filename) == ".png")
            {
                string fullpath = Path.Combine(Server.MapPath("~/UploadedFile/"), filename);
                return File(fullpath, "UploadedFile/jpg");

            }
            else
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.Forbidden);
            }
        }
    }

}
