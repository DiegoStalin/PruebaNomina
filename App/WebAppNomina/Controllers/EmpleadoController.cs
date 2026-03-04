
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using WebAppNomina.Models;
using WebAppNomina.DAL;

namespace WebAppNomina.Controllers
{
    public class EmpleadoController : Controller
    {
        private NominaContext db = new NominaContext();

        // --- RF-10: Búsqueda y Filtros de Empleados ---
        
        public ActionResult Index(string buscar, bool? verInactivos, int? pagina)
        {
            // 1. Configuración de Paginación (RNF-01)
            int registrosPorPagina = 20;
            int numeroPagina = (pagina ?? 1);

            // 2. Consulta base
            var empleados = db.Empleados.AsQueryable();

            // 3. Filtros (RF-10)
            if (verInactivos == true)
                empleados = empleados.Where(e => e.activo == false);
            else
                empleados = empleados.Where(e => e.activo == true);

            if (!string.IsNullOrEmpty(buscar))
            {
                empleados = empleados.Where(e => e.first_name.Contains(buscar) ||
                                                 e.last_name.Contains(buscar) ||
                                                 e.ci.Contains(buscar));
            }

            // 4. Lógica de Paginación y Envío de datos
            int totalRegistros = empleados.Count();

            var listaPaginada = empleados
                .OrderBy(e => e.last_name)
                .Skip((numeroPagina - 1) * registrosPorPagina)
                .Take(registrosPorPagina)
                .ToList();

            // 5. Datos para la navegación en la Vista
            ViewBag.CurrentFilter = buscar;
            ViewBag.MostrandoInactivos = verInactivos;
            ViewBag.PaginaActual = numeroPagina;
            ViewBag.TotalPaginas = (int)Math.Ceiling((double)totalRegistros / registrosPorPagina);

            return View(listaPaginada);
        }

        // GET: Empleado/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Empleado oEmpleado = db.Empleados.Find(id);
            if (oEmpleado == null) return HttpNotFound();

            return View(oEmpleado);
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
                // --- CAMBIO PARA RNF-02: SEGURIDAD (HASH) ---
                // Encriptamos la clave antes de cualquier otra validación o guardado
                if (!string.IsNullOrEmpty(oEmpleado.clave))
                {
                    oEmpleado.clave = BCrypt.Net.BCrypt.HashPassword(oEmpleado.clave);
                }
                // --------------------------------------------

                // RF-11: Validación de emp_no único
                if (db.Empleados.Any(e => e.emp_no == oEmpleado.emp_no))
                {
                    ModelState.AddModelError("emp_no", "Este número de empleado ya existe.");
                    return View(oEmpleado);
                }

                // RF-11: Validación de correo único
                if (db.Empleados.Any(e => e.correo == oEmpleado.correo))
                {
                    ModelState.AddModelError("correo", "Este correo ya está registrado.");
                    return View(oEmpleado);
                }

                oEmpleado.activo = true;
                if (oEmpleado.hire_date == DateTime.MinValue) oEmpleado.hire_date = DateTime.Now;

                db.Empleados.Add(oEmpleado);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(oEmpleado);
        }

        // GET: Empleado/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Empleado oEmpleado = db.Empleados.Find(id);
            if (oEmpleado == null) return HttpNotFound();

            return View(oEmpleado);
        }

        // POST: Empleado/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Empleado oEmpleado)
        {
            if (ModelState.IsValid)
            {
                // RF-11: Validar correo único en edición (excluyendo al empleado actual)
                if (db.Empleados.Any(e => e.correo == oEmpleado.correo && e.emp_no != oEmpleado.emp_no))
                {
                    ModelState.AddModelError("correo", "Este correo ya está en uso por otro empleado.");
                    return View(oEmpleado);
                }

                // Usamos Entry para actualizar solo los campos necesarios
                db.Entry(oEmpleado).State = EntityState.Modified;

                // --- INICIO MODIFICACIÓN RNF-02 ---
                // 2. IMPORTANTE: Evitamos que la clave (hash) se borre o cambie en la edición normal
                db.Entry(oEmpleado).Property(x => x.clave).IsModified = false;
                // --- FIN MODIFICACIÓN RNF-02 ---


                // Evitamos que la fecha de contratación se pierda si no está en el formulario de edición
                db.Entry(oEmpleado).Property(x => x.hire_date).IsModified = false;

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(oEmpleado);
        }

        // GET: Empleado/Delete/5 (Vista de confirmación de desactivación)
        public ActionResult Delete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Empleado oEmpleado = db.Empleados.Find(id);
            if (oEmpleado == null) return HttpNotFound();

            return View(oEmpleado);
        }

        // POST: Empleado/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            // RF-02: Se realiza desactivación (borrado lógico), no borrado físico
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