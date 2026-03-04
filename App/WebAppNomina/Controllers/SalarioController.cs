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

        // --- RF-10: Búsqueda y Filtros del Historial Principal ---
        public ActionResult Index(string busqueda, string dept_no, DateTime? fechaInicio, DateTime? fechaFin)
        {
            ViewBag.dept_no = new SelectList(db.Departamentos, "dept_no", "dept_name");
            var query = db.Salarios.Include(s => s.Empleado).AsQueryable();

            if (!string.IsNullOrEmpty(busqueda))
            {
                query = query.Where(s => s.Empleado.first_name.Contains(busqueda) ||
                                         s.Empleado.last_name.Contains(busqueda));
            }

            if (!string.IsNullOrEmpty(dept_no))
            {
                query = query.Where(s => db.Asignaciones
                             .Any(a => a.dept_no.ToString() == dept_no && a.emp_no == s.emp_no));
            }

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
                if (model.to_date.HasValue && model.to_date < model.from_date)
                {
                    ModelState.AddModelError("to_date", "La fecha final no puede ser anterior a la inicial.");
                    CargarEmpleados();
                    return View(model);
                }

                bool existeSolapamiento = db.Salarios.Any(s => s.emp_no == model.emp_no &&
                    ((model.from_date >= s.from_date && (s.to_date == null || model.from_date <= s.to_date)) ||
                     (model.to_date.HasValue && model.to_date >= s.from_date && (s.to_date == null || model.to_date <= s.to_date))));

                if (existeSolapamiento)
                {
                    ModelState.AddModelError("", "Error: El empleado ya tiene un salario registrado en este rango de fechas.");
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

        // --- RF-08 y RF-09: Auditoría con Filtros de Fecha ---
        // Se agregaron los parámetros 'inicio' y 'fin' para coincidir con tu Vista
        public ActionResult Auditoria(DateTime? inicio, DateTime? fin)
        {
            // Iniciamos la consulta de la tabla Auditorias definida en NominaContext
            var query = db.Auditorias.Include(a => a.Empleado).AsQueryable();

            // Filtro por fecha de inicio (Desde)
            if (inicio.HasValue)
            {
                query = query.Where(a => a.fechaActualizacion >= inicio.Value);
            }

            // Filtro por fecha de fin (Hasta)
            if (fin.HasValue)
            {
                // Agregamos un día para incluir todos los registros del último día seleccionado
                DateTime proximoDia = fin.Value.AddDays(1);
                query = query.Where(a => a.fechaActualizacion < proximoDia);
            }

            // Enviamos la lista filtrada y ordenada por fecha descendente
            return View(query.OrderByDescending(a => a.fechaActualizacion).ToList());
        }

        public ActionResult ReporteNominaDepto()
        {
            return View(db.Departamentos.ToList());
        }

        public ActionResult EstructuraOrg()
        {
            return View(db.Departamentos.ToList());
        }

        private void CargarEmpleados()
        {
            var lista = db.Empleados.Where(e => e.activo)
                        .Select(e => new {
                            e.emp_no,
                            NombreCompleto = e.first_name + " " + e.last_name
                        }).ToList();

            ViewBag.emp_no = new SelectList(lista, "emp_no", "NombreCompleto");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}