using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplicationOlimpiadas.Models;

namespace WebApplicationOlimpiadas.Controllers
{
    public class TablaPruebasController : BaseController
    {
        private TablaPruebaDBContext db = new TablaPruebaDBContext();

        // GET: TablaPruebas
        public ActionResult Index(string courseList, string levelList)
        {
            List<string> courseL = db.Database.SqlQuery<string>("select Name from CourseList").ToList<string>();
            List<string> levelL = db.Database.SqlQuery<string>("select valueL from LevelList").ToList<string>();
            ViewBag.courseList = new SelectList(courseL);
            ViewBag.levelList = new SelectList(levelL);
            //Danger("select * from tableReportE(" + courseL.IndexOf(courseList) + "," + levelL.IndexOf(levelList) + ") :"+ courseList, true);
            if (!String.IsNullOrEmpty(courseList) && !String.IsNullOrEmpty(levelList))
            {
                if (courseL.IndexOf(courseList) == -1 || levelL.IndexOf(levelList) == -1)
                {
                    return View(db.Movies.ToList<TablaPrueba>());
                }
                List<TablaPrueba> listaO = db.Database.SqlQuery<TablaPrueba>("select * from tableReportE({0},{1})", courseL.IndexOf(courseList) + 1, levelL.IndexOf(levelList) + 1).ToList<TablaPrueba>();
                if ((listaO.Count-1)<0)
                {
                    List<TablaPrueba> laux = db.Database.SqlQuery<TablaPrueba>("SELECT * FROM TablaPruebas WHERE goodAnswer={0} AND wrongAnswer={1} AND blankAnswer={2} AND Correlative NOT LIKE '{3}' AND Course={4} AND Level={5}",listaO.ElementAt(listaO.Count -1).goodAnswer,listaO.ElementAt(listaO.Count -1).wrongAnswer,listaO.ElementAt(listaO.Count -1).blankAnswer,listaO.ElementAt(listaO.Count -1).Correlative, courseL.IndexOf(courseList) + 1, levelL.IndexOf(levelList) + 1).ToList<TablaPrueba>();
                //listaO.RemoveAt(listaO.Count - 1);
                foreach (TablaPrueba item in laux)
                {
                    bool enter = true;
                    foreach (TablaPrueba item1 in listaO)
                    {
                        if (item.Correlative.Equals(item1.Correlative))
                        {
                            enter = false;
                        }
                    }
                    if (enter)
                    {
                    listaO.Add(item);
                    }
                }
                }
                return View(listaO);
            }
            List<TablaPrueba> listaDB = db.Database.SqlQuery<TablaPrueba>("select * from TablaPruebas").ToList<TablaPrueba>();
            List<TablaDatos> listaDBL = db.Database.SqlQuery<TablaDatos>("TableDatos").ToList<TablaDatos>();
            
            return View(db.Movies.ToList<TablaPrueba>());
        }
        //return HttpUtility.HtmlEncode(

        public string IndexM(string course,string level)
        {
            return HttpUtility.HtmlEncode("Curso: "+course+" Nivel "+level);
        }

        // GET: TablaPruebas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TablaPrueba tablaPrueba = db.Movies.Find(id);
            if (tablaPrueba == null)
            {
                return HttpNotFound();
            }
            return View(tablaPrueba);
        }

        // GET: TablaPruebas/Create
        public ActionResult Create()
        {

            if (Session["courseList"] == null || Session["levelList"] == null)
            {
                return RedirectToAction("", "Home");
            }
            ViewBag.c = Session["courseList"].ToString();
            ViewBag.l = Session["levelList"].ToString();
            ViewBag.m = Session["materia"].ToString();
            return View();
        }

        // POST: TablaPruebas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Correlative,goodAnswer,wrongAnswer,blankAnswer")] TablaPrueba tablaPrueba)
        {
            if (Session["courseList"] == null || Session["levelList"] == null)
            {
                return RedirectToAction("", "Home");
            }
            tablaPrueba.Course = int.Parse(Session["courseList"].ToString());
            tablaPrueba.Level = int.Parse(Session["levelList"].ToString());
            //Warning(string.Format("<b>{0}</b> already exists in the database. select * from TablaPruebas where Correlative like '{1}'", db.Database.SqlQuery<TablaPrueba>("select * from TablaPruebas where Correlative like '{0}'", tablaPrueba.Correlative).ToList<TablaPrueba>().Count, tablaPrueba.Correlative), true);
            //if (db.Database.SqlQuery<TablaPrueba>("select * from TablaPruebas where Correlative like '{0}'", tablaPrueba.Correlative).ToList<TablaPrueba>().Count >0)
            //{
            //}
            foreach (TablaPrueba item in db.Movies.ToList<TablaPrueba>())
            {
                if (item.Correlative.Contains(tablaPrueba.Correlative))
                {
                    Warning(string.Format("<b>{0}</b> already exists in the database.", tablaPrueba.Correlative), true);
                    return View(tablaPrueba);
                }
            }
            if (ModelState.IsValid)// && (tablaPrueba.blankAnswer+tablaPrueba.goodAnswer + tablaPrueba.wrongAnswer)==10)
            {
                if ((tablaPrueba.blankAnswer + tablaPrueba.goodAnswer + tablaPrueba.wrongAnswer) != int.Parse(Session["ans"].ToString()))
                {
                    ViewBag.c = Session["courseList"].ToString();
                    ViewBag.l = Session["levelList"].ToString();
                    ViewBag.m = Session["materia"].ToString();
                    Danger("La cantidad de respuestas <b>no es valida</b>", true);
                    return View(tablaPrueba);
                }
                db.Movies.Add(tablaPrueba);
                db.SaveChanges();
                Success(string.Format("<b>{0}</b> was successfully added to the database.", tablaPrueba.Correlative), true);
                //return RedirectToAction("Index");
                return RedirectToAction("Create");
            }
            Danger("Looks like something went wrong. Please check your form.");
            return View(tablaPrueba);
        }

        // GET: TablaPruebas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TablaPrueba tablaPrueba = db.Movies.Find(id);
            if (tablaPrueba == null)
            {
                return HttpNotFound();
            }
            return View(tablaPrueba);
        }

        // POST: TablaPruebas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Correlative,goodAnswer,wrongAnswer,blankAnswer,Course,Level")] TablaPrueba tablaPrueba)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tablaPrueba).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tablaPrueba);
        }

        // GET: TablaPruebas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TablaPrueba tablaPrueba = db.Movies.Find(id);
            if (tablaPrueba == null)
            {
                return HttpNotFound();
            }
            return View(tablaPrueba);
        }

        // POST: TablaPruebas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TablaPrueba tablaPrueba = db.Movies.Find(id);
            db.Movies.Remove(tablaPrueba);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

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
