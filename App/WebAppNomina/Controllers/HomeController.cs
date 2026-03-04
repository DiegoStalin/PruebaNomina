using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security; // Necesario para FormsAuthentication
using WebAppNomina.DAL;
using System.Data.Entity;

namespace WebAppNomina.Controllers
{
    public class HomeController : Controller
    {
        private NominaContext db = new NominaContext();

        public ActionResult Index()
        {
            // RNF-05: Dashboard informativo para mejorar la usabilidad
            ViewBag.TotalEmpleados = db.Empleados.Count(e => e.activo);

            // Filtramos los cambios de auditoría realizados hoy (RF-08)
            var fechaHoy = DateTime.Now.Date;
            ViewBag.UltimosCambios = db.Auditorias.Count(a => a.fechaActualizacion >= fechaHoy);

            return View();
        }

        // RF-01: Página de entrada
        public ActionResult Autenticar()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string usuario, string password)
        {
            // RF-01: Validamos contra la tabla de Empleados (usando correo como usuario)
            // Buscamos un empleado activo que coincida con el correo y la clave ingresada
            var user = db.Empleados.FirstOrDefault(e => e.correo == usuario && e.clave == password && e.activo);

            if (user != null)
            {
                // RF-12: Creamos la sesión de usuario
                FormsAuthentication.SetAuthCookie(user.correo, false);

                // Guardamos el nombre para mostrarlo en el Layout
                Session["UsuarioNombre"] = user.first_name + " " + user.last_name;

                return RedirectToAction("Index");
            }

            // Si falla, regresamos a la vista con un mensaje de error
            ViewBag.Error = "Usuario o contraseña incorrectos.";
            return View("Autenticar");
        }

        // Método para cerrar sesión
        public ActionResult Salir()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            return RedirectToAction("Autenticar");
        }
    }
}