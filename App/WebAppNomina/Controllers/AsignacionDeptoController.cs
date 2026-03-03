using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebAppNomina.Models;
using WebAppNomina.DAL;
using System.Data.Entity;

namespace WebAppNomina.Controllers
{
    public class AsignacionDeptoController : Controller
    {
        private NominaContext db = new NominaContext();

        // 1. LISTADO (Index)
        public ActionResult Index()
        {
            // Incluimos Empleado y Departamento para ver nombres en lugar de IDs
            var asignaciones = db.Asignaciones.Include(a => a.Empleado).Include(a => a.Departamento).ToList();
            return View(asignaciones);
        }

        // 2. CREAR (GET)
        public ActionResult Create()
        {
            CargarListas();
            return View();
        }

        // 3. CREAR (POST) - Aquí aplicamos la validación RF-04
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AsignacionDepto model)
        {
            if (ModelState.IsValid)
            {
                // REGLA DE NEGOCIO: Evitar solapamientos
                bool solapado = db.Asignaciones.Any(a =>
                    a.emp_no == model.emp_no &&
                    (
                        (model.from_date >= a.from_date && model.from_date <= a.to_date) ||
                        (model.to_date >= a.from_date && model.to_date <= a.to_date) ||
                        (model.from_date <= a.from_date && model.to_date >= a.to_date)
                    )
                );

                if (solapado)
                {
                    ModelState.AddModelError("", "Error: El empleado ya tiene una asignación de departamento que se cruza con estas fechas.");
                    CargarListas();
                    return View(model);
                }

                db.Asignaciones.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            CargarListas();
            return View(model);
        }

        // 4. ELIMINAR (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            var asignacion = db.Asignaciones.Find(id);
            if (asignacion != null)
            {
                db.Asignaciones.Remove(asignacion);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        // Método auxiliar para llenar los DropDownList
        private void CargarListas()
        {
            // Solo empleados activos
            ViewBag.emp_no = new SelectList(db.Empleados.Where(e => e.activo == true), "emp_no", "first_name");
            // Solo departamentos activos
            ViewBag.dept_no = new SelectList(db.Departamentos.Where(d => d.activo == true), "dept_no", "dept_name");
        }
    }
}