using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using WebAppNomina.Models;
using WebAppNomina.DAL;

namespace WebAppNomina.Controllers
{
    public class DepartamentoController : Controller
    {
        private NominaContext db = new NominaContext();

        // GET: Departamento (Lista de departamentos activos)
        public ActionResult Index()
        {
            // Filtramos solo los que están activos
            var departamentos = db.Departamentos.Where(d => d.activo == true).ToList();
            return View(departamentos);
        }

        // GET: Departamento/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Departamento/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Departamento oDepartamento)
        {
            if (ModelState.IsValid)
            {
                oDepartamento.activo = true; // Se crea como activo por defecto
                db.Departamentos.Add(oDepartamento);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(oDepartamento);
        }

        // GET: Departamento/Edit/5
        public ActionResult Edit(int id)
        {
            var oDept = db.Departamentos.Find(id);
            if (oDept == null) return HttpNotFound();
            return View(oDept);
        }

        // POST: Departamento/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Departamento oDepartamento)
        {
            if (ModelState.IsValid)
            {
                db.Entry(oDepartamento).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(oDepartamento);
        }

        // POST: Departamento/Delete/5 (Borrado Lógico)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var oDept = db.Departamentos.Find(id);
            if (oDept != null)
            {
                oDept.activo = false; // Desactivar en lugar de borrar físicamente
                db.Entry(oDept).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
