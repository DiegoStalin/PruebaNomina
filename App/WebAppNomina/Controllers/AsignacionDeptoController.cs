using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebAppNomina.Models;
using WebAppNomina.DAL;
using System.Data.Entity;

namespace WebAppNomina.Controllers
{
    /// <summary>
    /// Controlador encargado de gestionar la vinculación histórica entre Empleados y Departamentos.
    /// Implementa reglas de integridad cronológica para evitar duplicidad de asignaciones.
    /// </summary>
    public class AsignacionDeptoController : Controller
    {
        private NominaContext db = new NominaContext();

        /// <summary>
        /// Obtiene el listado completo de asignaciones. 
        /// Utiliza Eager Loading (.Include) para optimizar la consulta de datos relacionados.
        /// </summary>
        /// <returns>Vista con la lista de asignaciones departamento-empleado.</returns>
        public ActionResult Index()
        {
            // Incluimos Empleado y Departamento para ver nombres en lugar de IDs
            var asignaciones = db.Asignaciones.Include(a => a.Empleado).Include(a => a.Departamento).ToList();
            return View(asignaciones);
        }

        /// <summary>
        /// Presenta el formulario para crear una nueva asignación.
        /// </summary>
        /// <returns>Vista de creación con listas desplegables cargadas.</returns>
        public ActionResult Create()
        {
            CargarListas();
            return View();
        }

        /// <summary>
        /// Procesa la nueva asignación de un empleado a un departamento.
        /// Implementa la regla RF-04 para validar que no existan solapamientos de fechas.
        /// </summary>
        /// <param name="model">Objeto con los datos de la asignación y periodo de tiempo.</param>
        /// <returns>Redirección al Index si es válido, o la misma vista con el error de validación.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AsignacionDepto model)
        {
            if (ModelState.IsValid)
            {
                // REGLA DE NEGOCIO: Evitar solapamientos (Validación de integridad temporal)
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


        /// <summary>
        /// Elimina un registro de asignación específico.
        /// </summary>
        /// <param name="id">ID de la asignación a remover.</param>
        /// <returns>Redirección al listado principal.</returns>
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

        /// <summary>
        /// Método auxiliar para poblar los ViewBags de selección.
        /// Filtra registros según el estado de actividad para garantizar el borrado lógico.
        /// </summary>
        private void CargarListas()
        {
            // Solo empleados activos
            ViewBag.emp_no = new SelectList(db.Empleados.Where(e => e.activo == true), "emp_no", "first_name");
            // Solo departamentos activos
            ViewBag.dept_no = new SelectList(db.Departamentos.Where(d => d.activo == true), "dept_no", "dept_name");
        }
    }
}