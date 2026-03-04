using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using WebAppNomina.Models;

namespace WebAppNomina.Controllers
{
    public class AccesoController : Controller
    {
        public ActionResult Index() => View();
        public ActionResult Autenticar() => View();
        public ActionResult Registro() => View();

        [HttpPost]
        public ActionResult Autenticar(Empleado oempleado)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["conexion"].ConnectionString))
                {
                    SqlCommand comando = new SqlCommand("spValidarUsuario", cn);
                    comando.CommandType = CommandType.StoredProcedure;
                    comando.Parameters.AddWithValue("usuario", oempleado.correo);
                    comando.Parameters.AddWithValue("clave", oempleado.clave);
                    comando.Parameters.Add("id", SqlDbType.Int).Direction = ParameterDirection.Output;
                    comando.Parameters.Add("mensaje", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;

                    cn.Open();
                    comando.ExecuteNonQuery();

                    int idResultado = Convert.ToInt32(comando.Parameters["id"].Value);
                    if (idResultado > 0) return RedirectToAction("Index", "Opciones");
                    else
                    {
                        ViewData["Mensaje"] = comando.Parameters["mensaje"].Value.ToString();
                        return View();
                    }
                }
            }
            catch (Exception ex)
            {
                ViewData["Mensaje"] = "Error Login: " + ex.Message;
                return View();
            }
        }

        [HttpPost]
        public ActionResult Registro(string cedula, string nombre, string apellido, string correo, DateTime FechaNacimiento, string clave, string genero)
        {
            // Nota: Los nombres de arriba (cedula, nombre, etc) ahora coinciden con tu HTML
            try
            {
                using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["conexion"].ConnectionString))
                {
                    SqlCommand comando = new SqlCommand("spRegistrarUsuario", cn);
                    comando.CommandType = CommandType.StoredProcedure;

                    // Mapeamos los datos del formulario a los parámetros de SQL
                    comando.Parameters.AddWithValue("cedula", cedula);
                    comando.Parameters.AddWithValue("nombre", nombre);
                    comando.Parameters.AddWithValue("apellido", apellido);
                    comando.Parameters.AddWithValue("correo", correo);
                    comando.Parameters.AddWithValue("genero", genero);
                    comando.Parameters.AddWithValue("fecha", FechaNacimiento);
                    comando.Parameters.AddWithValue("clave", clave);

                    comando.Parameters.Add("retorno", SqlDbType.Int).Direction = ParameterDirection.Output;
                    comando.Parameters.Add("mensaje", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;

                    cn.Open();
                    comando.ExecuteNonQuery();

                    int retorno = Convert.ToInt32(comando.Parameters["retorno"].Value);

                    if (retorno == 1)
                    {
                        // Si todo sale bien, vamos al Login
                        return RedirectToAction("Autenticar", "Acceso");
                    }
                    else
                    {
                        ViewData["Mensaje"] = comando.Parameters["mensaje"].Value.ToString();
                        return View();
                    }
                }
            }
            catch (Exception ex)
            {
                // Si hay error de SQL, lo veremos en pantalla
                ViewData["Mensaje"] = "Error en Registro: " + ex.Message;
                return View();
            }
        }
    }
}