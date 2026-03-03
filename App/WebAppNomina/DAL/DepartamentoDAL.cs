using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebAppNomina.DAL
{
    public class DepartamentoDAL : Controller
    {
        // GET: DepartamentoDAL
        public ActionResult Index()
        {
            return View();
        }

        // GET: DepartamentoDAL/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: DepartamentoDAL/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DepartamentoDAL/Create
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

        // GET: DepartamentoDAL/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: DepartamentoDAL/Edit/5
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

        // GET: DepartamentoDAL/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: DepartamentoDAL/Delete/5
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
