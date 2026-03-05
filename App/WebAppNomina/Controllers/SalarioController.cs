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

        // --- RF-10: Búsqueda y Filtros ---
        public ActionResult Index(string busqueda, string dept_no, DateTime? fechaInicio, DateTime? fechaFin)
        {
            // Cargamos los departamentos para el filtro desplegable en la vista
            ViewBag.dept_no = new SelectList(db.Departamentos, "dept_no", "dept_name");

            // Consulta base incluyendo la relación con Empleado para evitar errores de referencia
            var query = db.Salarios.Include(s => s.Empleado).AsQueryable();

            // 1. Filtro por texto: Nombre o Apellido del empleado (RF-10)
            if (!string.IsNullOrEmpty(busqueda))
            {
                query = query.Where(s => s.Empleado.first_name.Contains(busqueda) ||
                                         s.Empleado.last_name.Contains(busqueda));
            }

            // 2. Filtro por Departamento (RF-10)
            if (!string.IsNullOrEmpty(dept_no))
            {
                // Filtramos buscando empleados asignados a ese departamento específico
                query = query.Where(s => db.Asignaciones
                             .Any(a => a.dept_no.ToString() == dept_no && a.emp_no == s.emp_no));
            }

            // 3. Filtro por Rango de Fechas (RF-10)
            if (fechaInicio.HasValue)
            {
                query = query.Where(s => s.from_date >= fechaInicio.Value);
            }

            if (fechaFin.HasValue)
            {
                // Manejo de to_date nulo para considerar salarios que aún están activos
                query = query.Where(s => s.to_date <= fechaFin.Value || s.to_date == null);
            }

            return View(query.ToList());
        }

        // --- RF-07: Creación de Salarios con RF-11 (Validaciones de Negocio) ---
        public ActionResult Create()
        {
            CargarEmpleados();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Salario model)
        {
            if (ModelState.IsValid)
            {
                // RF-11: No permitir fechas to_date anteriores a from_date
                if (model.to_date.HasValue && model.to_date < model.from_date)
                {
                    ModelState.AddModelError("", "La fecha final (to_date) no puede ser anterior a la inicial (from_date).");
                    CargarEmpleados();
                    return View(model);
                }

                // RF-11: Impedir salarios que se solapen en el mismo rango de fechas
                bool existeSolapamiento = db.Salarios.Any(s => s.emp_no == model.emp_no &&
                    ((model.from_date >= s.from_date && (s.to_date == null || model.from_date <= s.to_date)) ||
                     (model.to_date.HasValue && model.to_date >= s.from_date && (s.to_date == null || model.to_date <= s.to_date))));

                if (existeSolapamiento)
                {
                    ModelState.AddModelError("", "Error: Ya existe un registro de salario para este empleado que se solapa con las fechas ingresadas.");
                    CargarEmpleados();
                    return View(model);
                }

                db.Salarios.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            CargarEmpleados();
            return View(model);
        }

        // --- Reportes y Auditoría (RF-08, RF-12) ---
        public ActionResult Auditoria()
        {
            return View(db.Auditorias.Include(a => a.Empleado).OrderByDescending(a => a.fechaActualizacion).ToList());
        }

        public ActionResult ReporteNominaDepto()
        {
            return View(db.Departamentos.ToList());
        }

        public ActionResult EstructuraOrg()
        {
            return View(db.Departamentos.ToList());
        }

        // Métodos de apoyo
        private void CargarEmpleados()
        {
            ViewBag.emp_no = new SelectList(db.Empleados, "emp_no", "first_name");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}