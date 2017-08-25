using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplicationOlimpiadas.Models;

namespace WebApplicationOlimpiadas.Controllers
{
    public class HomeController : Controller
    {

        private TablaPruebaDBContext db = new TablaPruebaDBContext();

        public ActionResult Index(string courseList, string levelList)
        {
            Session["courseList"] = null;
            Session["levelList"] = null;
            List<string> courseL = db.Database.SqlQuery<string>("select Name from CourseList").ToList<string>();
            List<string> levelL = db.Database.SqlQuery<string>("select valueL from LevelList").ToList<string>();
            ViewBag.courseList = new SelectList(courseL);
            ViewBag.levelList = new SelectList(levelL);
            if (!String.IsNullOrEmpty(courseList) && !String.IsNullOrEmpty(levelList))
            {
                if (!courseList.Equals("All") && !levelList.Equals("All"))
                {
                    if (courseL.IndexOf(courseList) == -1 || levelL.IndexOf(levelList) == -1)
                    {
                        return View();
                    }
                    Session["courseList"] = courseL.IndexOf(courseList) + 1;
                    Session["levelList"] = levelL.IndexOf(levelList) + 1;
                    Session["ans"] = levelList;
                    Session["materia"] = courseList;
                    return RedirectToAction("Create", "TablaPruebas");
                }
            }
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}