using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebAppNomina.Controllers
{
    public class AsignacionController : Controller
    {
        // GET: Asignacion
        public ActionResult Index()
        {
            return View();
        }

        // GET: Asignacion/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Asignacion/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Asignacion/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Asignacion/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Asignacion/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Asignacion/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Asignacion/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
