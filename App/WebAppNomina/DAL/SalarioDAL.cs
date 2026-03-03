using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebAppNomina.DAL
{
    public class SalarioDAL : Controller
    {
        // GET: SalarioDAL
        public ActionResult Index()
        {
            return View();
        }

        // GET: SalarioDAL/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: SalarioDAL/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SalarioDAL/Create
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

        // GET: SalarioDAL/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: SalarioDAL/Edit/5
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

        // GET: SalarioDAL/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: SalarioDAL/Delete/5
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
