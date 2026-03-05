using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using WebAppNomina.DAL;

namespace WebAppNomina.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index() // son los constructores de inicializacion de la PARTE WEB, este es de pagina de index
        {
            return View();
        }

        public ActionResult About() // son los constructores de inicializacion de la pagina de acerca de
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact() // son los constructores de inicializacion de la pagina de contacto
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Autenticar() // son los constructores de inicializacion de la pagina de contacto
        {
            ViewBag.Message = "Página de Autenticación.";

            return View();
        }

        public ActionResult ProbarConexion()
        {
            Conexion cn = new Conexion();

            using (SqlConnection conn = cn.ObtenerConexion())
            {
                conn.Open();
            }

            return Content("Conexión exitosa");
        }

    }
}