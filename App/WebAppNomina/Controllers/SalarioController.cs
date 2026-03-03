using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebAppNomina.Models;
using WebAppNomina.DAL;
using System.Data.Entity;

namespace WebAppNomina.Controllers
{
    public class SalarioController : Controller
    {
        private NominaContext db = new NominaContext();

        public ActionResult Index(DateTime? fechaVigencia)
        {
            var query = db.Salarios.Include(s => s.Empleado).AsQueryable();
            if (fechaVigencia.HasValue)
            {
                query = query.Where(s => fechaVigencia >= s.from_date &&
                                   (s.to_date == null || fechaVigencia <= s.to_date));
            }
            return View(query.ToList());
        }

        public ActionResult Auditoria()
        {
            var historial = db.Auditorias
                              .Include(a => a.Empleado)
                              .OrderByDescending(a => a.fechaActualizacion)
                              .ToList();
            return View(historial);
        }

        // --- REPORTES CORREGIDOS (RF-09) ---

        public ActionResult ReporteNominaDepto()
        {
            // Se quitó .Include("Asignaciones") para evitar InvalidOperationException
            var departamentos = db.Departamentos.ToList();
            return View(departamentos);
        }

        public ActionResult EstructuraOrg()
        {
            // Se quitó .Include() para evitar errores de ruta no válida
            var estructura = db.Departamentos.ToList();
            return View(estructura);
        }

        // Métodos adicionales (Create, Delete, CargarEmpleados, Dispose) se mantienen igual
        public ActionResult Create() { CargarEmpleados(); return View(); }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Salario model)
        {
            if (ModelState.IsValid) { db.Salarios.Add(model); db.SaveChanges(); return RedirectToAction("Index"); }
            CargarEmpleados(); return View(model);
        }
        private void CargarEmpleados() { ViewBag.emp_no = new SelectList(db.Empleados, "emp_no", "first_name"); }
        protected override void Dispose(bool disposing) { if (disposing) { db.Dispose(); } base.Dispose(disposing); }
    }
}
