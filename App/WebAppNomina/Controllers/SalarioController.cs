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

        // --- RF-10: Búsqueda y Filtros con Corrección de Tipos ---
        public ActionResult Index(string busqueda, string dept_no, DateTime? fechaInicio, DateTime? fechaFin)
        {
            // Cargamos los departamentos para el filtro desplegable
            ViewBag.dept_no = new SelectList(db.Departamentos, "dept_no", "dept_name");

            var query = db.Salarios.Include(s => s.Empleado).AsQueryable();

            // 1. Filtro por texto (Nombre o Apellido)
            if (!string.IsNullOrEmpty(busqueda))
            {
                query = query.Where(s => s.Empleado.first_name.Contains(busqueda) ||
                                         s.Empleado.last_name.Contains(busqueda));
            }

            // 2. Filtro por Departamento (Corrección error CS0019)
            if (!string.IsNullOrEmpty(dept_no))
            {
                // Convertimos el string a int para que la comparación sea válida
                // Cambié 'AsignacionesDepto' por 'Asignaciones' según tu NominaContext
                query = query.Where(s => db.Asignaciones
                             .Where(a => a.dept_no.ToString() == dept_no)
                             .Select(a => a.emp_no)
                             .Contains(s.emp_no));
            }

            // 3. Filtro por Rango de Fechas
            if (fechaInicio.HasValue)
            {
                query = query.Where(s => s.from_date >= fechaInicio.Value);
            }

            if (fechaFin.HasValue)
            {
                query = query.Where(s => s.to_date <= fechaFin.Value || s.to_date == null);
            }

            return View(query.ToList());
        }

        // ... Los demás métodos se mantienen igual ...
        public ActionResult Auditoria() => View(db.Auditorias.Include(a => a.Empleado).OrderByDescending(a => a.fechaActualizacion).ToList());
        public ActionResult ReporteNominaDepto() => View(db.Departamentos.ToList());
        public ActionResult EstructuraOrg() => View(db.Departamentos.ToList());
        public ActionResult Create() { CargarEmpleados(); return View(); }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Salario model)
        {
            if (ModelState.IsValid) { db.Salarios.Add(model); db.SaveChanges(); return RedirectToAction("Index"); }
            CargarEmpleados(); return View(model);
        }

        private void CargarEmpleados() { ViewBag.emp_no = new SelectList(db.Empleados, "emp_no", "first_name"); }
        protected override void Dispose(bool disposing) { if (disposing) db.Dispose(); base.Dispose(disposing); }
    }
}