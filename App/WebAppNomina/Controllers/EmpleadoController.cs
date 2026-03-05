using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using WebAppNomina.Models;
using WebAppNomina.DAL;

namespace WebAppNomina.Controllers
{
    public class EmpleadoController : Controller
    {
        private NominaContext db = new NominaContext();

        // --- RF-10: Búsqueda y Filtros de Empleados ---
        public ActionResult Index(string buscar, bool? verInactivos)
        {
            // Iniciamos la consulta base
            var empleados = db.Empleados.AsQueryable();

            // 1. Filtro por Estado (Campo clave: Activo) - RF-10
            if (verInactivos == true)
            {
                // Si el usuario marca la casilla, vemos solo los inactivos
                empleados = empleados.Where(e => e.activo == false);
            }
            else if (verInactivos == false)
            {
                // Si la marca como "No", vemos solo activos
                empleados = empleados.Where(e => e.activo == true);
            }
            else
            {
                // Por defecto, solo mostramos los activos para no saturar la vista
                empleados = empleados.Where(e => e.activo == true);
            }

            // 2. Filtro por Texto (Nombre, Apellido o CI) - RF-10
            if (!string.IsNullOrEmpty(buscar))
            {
                empleados = empleados.Where(e => e.first_name.Contains(buscar) ||
                                                 e.last_name.Contains(buscar) ||
                                                 e.ci.Contains(buscar));
            }

            // Guardamos el estado del filtro para la vista
            ViewBag.CurrentFilter = buscar;
            ViewBag.MostrandoInactivos = verInactivos;

            return View(empleados.ToList());
        }

        // GET: Empleado/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Empleado oEmpleado)
        {
            if (ModelState.IsValid)
            {
                // RF-11: Validación de Negocio (No fechas futuras)
                if (oEmpleado.birth_date > DateTime.Now)
                {
                    ModelState.AddModelError("birth_date", "La fecha de nacimiento no puede ser futura.");
                    return View(oEmpleado);
                }

                // Blindaje SQL Server
                if (oEmpleado.birth_date < new DateTime(1753, 1, 1))
                {
                    oEmpleado.birth_date = new DateTime(2000, 1, 1);
                }

                oEmpleado.activo = true;
                oEmpleado.hire_date = DateTime.Now;

                try
                {
                    db.Empleados.Add(oEmpleado);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error al guardar: " + ex.Message);
                }
            }
            return View(oEmpleado);
        }

        // GET: Empleado/Edit/5
        public ActionResult Edit(int id)
        {
            var oEmpleado = db.Empleados.Find(id);
            if (oEmpleado == null) return HttpNotFound();
            return View(oEmpleado);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Empleado oEmpleado)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var empleadoBD = db.Empleados.Find(oEmpleado.emp_no);

                    if (empleadoBD != null)
                    {
                        empleadoBD.ci = oEmpleado.ci;
                        empleadoBD.first_name = oEmpleado.first_name;
                        empleadoBD.last_name = oEmpleado.last_name;
                        empleadoBD.correo = oEmpleado.correo;
                        empleadoBD.genero = oEmpleado.genero;
                        empleadoBD.clave = oEmpleado.clave;
                        empleadoBD.activo = oEmpleado.activo; // Permitir reactivar desde edición

                        empleadoBD.birth_date = oEmpleado.birth_date < new DateTime(1753, 1, 1)
                                                ? new DateTime(2000, 1, 1)
                                                : oEmpleado.birth_date;

                        db.Entry(empleadoBD).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error al actualizar: " + ex.Message);
            }
            return View(oEmpleado);
        }

        // RF-02: Borrado Lógico
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Empleado oEmpleado = db.Empleados.Find(id);
            if (oEmpleado != null)
            {
                oEmpleado.activo = false;
                db.Entry(oEmpleado).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}