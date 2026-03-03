using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebAppNomina.DAL
{
    public class EmpleadoDAL : Controller
    {
        // GET: EmpleadoDAL
        public ActionResult Index()
        {
            return View();
        }

        // GET: EmpleadoDAL/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: EmpleadoDAL/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EmpleadoDAL/Create
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

        // GET: EmpleadoDAL/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: EmpleadoDAL/Edit/5
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

        // GET: EmpleadoDAL/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: EmpleadoDAL/Delete/5
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
