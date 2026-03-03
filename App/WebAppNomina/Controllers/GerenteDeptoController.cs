using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebAppNomina.Models;
using WebAppNomina.DAL;
using System.Data.Entity;

namespace WebAppNomina.Controllers
{
    public class GerenteDeptoController : Controller
    {
        private NominaContext db = new NominaContext();

        // 1. Listado de Managers
        public ActionResult Index()
        {
            var gerentes = db.Gerentes.Include(g => g.Empleado).Include(g => g.Departamento).ToList();
            return View(gerentes);
        }

        // 2. Crear (GET)
        public ActionResult Create()
        {
            CargarListas();
            return View();
        }

        // 3. Crear (POST) - Aplicando Regla RF-05
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(GerenteDepto model)
        {
            if (ModelState.IsValid)
            {
                // VALIDACIÓN: ¿Ya hay un manager asignado a este depto en estas fechas?
                bool yaExisteManager = db.Gerentes.Any(g =>
                    g.dept_no == model.dept_no &&
                    ((model.from_date >= g.from_date && model.from_date <= g.to_date) ||
                     (model.to_date >= g.from_date && model.to_date <= g.to_date)));

                if (yaExisteManager)
                {
                    ModelState.AddModelError("", "REGLA DE NEGOCIO: Este departamento ya tiene un manager activo en el rango de fechas seleccionado.");
                    CargarListas();
                    return View(model);
                }

                db.Gerentes.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            CargarListas();
            return View(model);
        }

        // 4. Eliminar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            var gerente = db.Gerentes.Find(id);
            if (gerente != null)
            {
                db.Gerentes.Remove(gerente);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        private void CargarListas()
        {
            // Solo empleados activos para ser seleccionados como manager
            ViewBag.emp_no = new SelectList(db.Empleados.Where(e => e.activo == true), "emp_no", "first_name");
            ViewBag.dept_no = new SelectList(db.Departamentos.Where(d => d.activo == true), "dept_no", "dept_name");
        }
    }
}