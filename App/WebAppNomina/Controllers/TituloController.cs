using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebAppNomina.Models;
using WebAppNomina.DAL;
using System.Data.Entity;

namespace WebAppNomina.Controllers
{
    public class TituloController : Controller
    {
        private NominaContext db = new NominaContext();

        // 1. Listado Histórico de Títulos
        public ActionResult Index()
        {
            var titulos = db.Titulos.Include(t => t.Empleado).ToList();
            return View(titulos);
        }

        // 2. Vista para Crear (GET)
        public ActionResult Create()
        {
            // Cargamos solo empleados activos para asignarles cargo
            ViewBag.emp_no = new SelectList(db.Empleados.Where(e => e.activo == true), "emp_no", "first_name");
            return View();
        }

        // 3. Lógica de Guardado con Validación de Solapamiento (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Titulo model)
        {
            if (ModelState.IsValid)
            {
                // VALIDACIÓN: ¿El empleado ya tiene un título que se cruza con estas fechas?
                // Comparamos el rango (from_date a to_date) contra los registros existentes en la tabla 'titles'
                bool existeSolapamiento = db.Titulos.Any(t =>
                    t.emp_no == model.emp_no &&
                    ((model.from_date >= t.from_date && (t.to_date == null || model.from_date <= t.to_date)) ||
                     (model.to_date != null && model.to_date >= t.from_date && (t.to_date == null || model.to_date <= t.to_date))));

                if (existeSolapamiento)
                {
                    ModelState.AddModelError("", "REGLA DE NEGOCIO: El empleado ya posee un cargo registrado que se solapa con las fechas ingresadas.");
                    ViewBag.emp_no = new SelectList(db.Empleados, "emp_no", "first_name", model.emp_no);
                    return View(model);
                }

                db.Titulos.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.emp_no = new SelectList(db.Empleados, "emp_no", "first_name", model.emp_no);
            return View(model);
        }

        // 4. Eliminar Registro
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            var titulo = db.Titulos.Find(id);
            if (titulo != null)
            {
                db.Titulos.Remove(titulo);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}