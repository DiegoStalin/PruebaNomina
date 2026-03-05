
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
    /// <summary>
    /// Controlador para la gestión administrativa de empleados.
    /// Implementa RNF-03 (Mantenibilidad) mediante documentación técnica y separación de lógica.
    /// </summary>
    public class EmpleadoController : Controller
    {
        private NominaContext db = new NominaContext();

        /// <summary>
        /// Lista los empleados con soporte para búsqueda (RF-10) y paginación (RNF-01).
        /// </summary>
        /// <param name="buscar">Criterio de búsqueda por nombre, apellido o CI.</param>
        /// <param name="verInactivos">Filtro para visualizar empleados con borrado lógico.</param>
        /// <param name="pagina">Número de página actual para la navegación.</param>
        /// <returns>Vista principal con lista de empleados filtrada y paginada.</returns>
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

            // 5. Datos para la navegación en la Vista (ViewBag para evitar pérdida de estado)
            ViewBag.CurrentFilter = buscar;
            ViewBag.MostrandoInactivos = verInactivos;
            ViewBag.PaginaActual = numeroPagina;
            ViewBag.TotalPaginas = (int)Math.Ceiling((double)totalRegistros / registrosPorPagina);

            return View(listaPaginada);
        }

        /// <summary>
        /// Muestra el detalle informativo de un empleado específico.
        /// </summary>
        /// <param name="id">Identificador único del empleado.</param>
        /// <returns>Vista con los datos del empleado o error si no existe.</returns>
        public ActionResult Details(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Empleado oEmpleado = db.Empleados.Find(id);
            if (oEmpleado == null) return HttpNotFound();

            return View(oEmpleado);
        }

        /// <summary>
        /// Muestra el formulario para registrar un nuevo empleado.
        /// </summary>
        /// <returns>Vista de creación.</returns>
        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Procesa el registro de un nuevo empleado aplicando Hash de seguridad.
        /// </summary>
        /// <param name="oEmpleado">Objeto que contiene los datos del nuevo empleado.</param>
        /// <returns>Redirección al listado o vista con errores de validación.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Empleado oEmpleado)
        {
            if (ModelState.IsValid)
            {
                // RNF-02: Encriptamos la clave mediante BCrypt antes de persistir
                if (!string.IsNullOrEmpty(oEmpleado.clave))
                {
                    oEmpleado.clave = BCrypt.Net.BCrypt.HashPassword(oEmpleado.clave);
                }

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

        /// <summary>
        /// Obtiene los datos del empleado para ser editados.
        /// </summary>
        /// <param name="id">ID del empleado.</param>
        /// <returns>Vista de edición con datos cargados.</returns>
        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Empleado oEmpleado = db.Empleados.Find(id);
            if (oEmpleado == null) return HttpNotFound();

            return View(oEmpleado);
        }

        /// <summary>
        /// Actualiza la información de un empleado protegiendo la integridad del Hash.
        /// </summary>
        /// <param name="oEmpleado">Objeto con los cambios del empleado.</param>
        /// <returns>Redirección al listado tras actualización exitosa.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Empleado oEmpleado)
        {
            if (ModelState.IsValid)
            {
                // RF-11: Validar correo único excluyendo al registro actual
                if (db.Empleados.Any(e => e.correo == oEmpleado.correo && e.emp_no != oEmpleado.emp_no))
                {
                    ModelState.AddModelError("correo", "Este correo ya está en uso por otro empleado.");
                    return View(oEmpleado);
                }

                db.Entry(oEmpleado).State = EntityState.Modified;

                // RNF-02: Se excluye la propiedad 'clave' de la actualización para no perder el Hash original
                db.Entry(oEmpleado).Property(x => x.clave).IsModified = false;
                db.Entry(oEmpleado).Property(x => x.hire_date).IsModified = false;

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(oEmpleado);
        }

        /// <summary>
        /// Muestra la confirmación de baja (borrado lógico) de un empleado.
        /// </summary>
        /// <param name="id">ID del empleado.</param>
        public ActionResult Delete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Empleado oEmpleado = db.Empleados.Find(id);
            if (oEmpleado == null) return HttpNotFound();

            return View(oEmpleado);
        }

        /// <summary>
        /// Ejecuta el borrado lógico del empleado (RF-02) cambiando su estado de actividad.
        /// </summary>
        /// <param name="id">ID del empleado a desactivar.</param>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Empleado oEmpleado = db.Empleados.Find(id);
            if (oEmpleado != null)
            {
                // RF-02: Desactivación, no borrado físico para mantener integridad histórica
                oEmpleado.activo = false;
                db.Entry(oEmpleado).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Libera los recursos de la conexión a la base de datos.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}