using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAppNomina.Models;
using WebAppNomina.DAL;

namespace WebAppNomina.Controllers
{
    public class EmpleadoController : Controller
    {
        private NominaContext db = new NominaContext();

        // GET: Empleado
        // RF-10: Implementa la consulta/filtro de empleados activos
        public ActionResult Index(string buscar)
        {
            // Solo traemos los que tienen activo = 1 en SQL
            var empleados = db.Empleados.Where(e => e.activo == true);

            if (!string.IsNullOrEmpty(buscar))
            {
                // Buscamos por nombre, apellido o CI
                empleados = empleados.Where(e => e.first_name.Contains(buscar) ||
                                                 e.last_name.Contains(buscar) ||
                                                 e.ci.Contains(buscar));
            }

            return View(empleados.ToList());
        }

        // GET: Empleado/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Empleado/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Empleado oEmpleado)
        {
            if (ModelState.IsValid)
            {
                // 1. Validación RF-11: No permitir fechas futuras
                if (oEmpleado.birth_date > DateTime.Now)
                {
                    ModelState.AddModelError("birth_date", "La fecha de nacimiento no puede ser futura.");
                    return View(oEmpleado);
                }

                // 2. Blindaje para SQL Server: Si la fecha es inválida o muy antigua
                if (oEmpleado.birth_date < new DateTime(1753, 1, 1))
                {
                    oEmpleado.birth_date = new DateTime(2000, 1, 1);
                }

                // 3. Garantizamos datos para campos obligatorios en SQL
                oEmpleado.activo = true;
                oEmpleado.hire_date = DateTime.Now;

                try
                {
                    db.Empleados.Add(oEmpleado);
                    db.SaveChanges(); // Guarda clave y genero correctamente ahora
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

        // POST: Empleado/Edit/5
        // CAMBIO: Lógica corregida para que el botón de Actualizar guarde los cambios
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Empleado oEmpleado)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // 1. Buscamos el registro original para evitar conflictos de rastreo
                    var empleadoBD = db.Empleados.Find(oEmpleado.emp_no);

                    if (empleadoBD != null)
                    {
                        // 2. Mapeamos los datos de la vista al objeto de la BD
                        empleadoBD.ci = oEmpleado.ci;
                        empleadoBD.first_name = oEmpleado.first_name;
                        empleadoBD.last_name = oEmpleado.last_name;
                        empleadoBD.correo = oEmpleado.correo;
                        empleadoBD.genero = oEmpleado.genero; // Ya no será NULL
                        empleadoBD.clave = oEmpleado.clave;   // Ya no será NULL

                        // Blindaje de fecha en edición
                        empleadoBD.birth_date = oEmpleado.birth_date < new DateTime(1753, 1, 1)
                                                ? new DateTime(2000, 1, 1)
                                                : oEmpleado.birth_date;

                        // 3. Notificamos el cambio y guardamos
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

        // POST: Empleado/Delete/5
        // REQUERIMIENTO CRÍTICO RF-02: Borrado Lógico
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Empleado oEmpleado = db.Empleados.Find(id);
            if (oEmpleado != null)
            {
                // Cambiamos a false (0 en SQL) para borrado lógico
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