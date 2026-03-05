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
    /// <summary>
    /// Controlador encargado de gestionar el acceso al sistema, autenticación y registro de nuevos usuarios.
    /// Implementa RNF-03 para garantizar la mantenibilidad mediante documentación técnica.
    /// </summary>
    public class AccesoController : Controller
    {
        /// <summary>
        /// Muestra la vista principal del acceso.
        /// </summary>
        /// <returns>Vista de inicio del controlador.</returns>
        public ActionResult Index() => View();

        /// <summary>
        /// Muestra el formulario de inicio de sesión (Login).
        /// </summary>
        /// <returns>Vista de autenticación.</returns>
        public ActionResult Autenticar() => View();

        /// <summary>
        /// Muestra el formulario de registro de nuevos usuarios.
        /// </summary>
        /// <returns>Vista de registro.</returns>
        public ActionResult Registro() => View();

        /// <summary>
        /// Procesa las credenciales de un empleado para validar su ingreso al sistema.
        /// </summary>
        /// <param name="oempleado">Objeto que contiene el correo y la clave ingresados por el usuario.</param>
        /// <returns>Redirección al panel de opciones si es válido, o la misma vista con mensaje de error si falla.</returns>
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

        /// <summary>
        /// Registra un nuevo empleado en la base de datos a través de un procedimiento almacenado.
        /// </summary>
        /// <param name="cedula">Cédula de identidad del nuevo usuario.</param>
        /// <param name="nombre">Nombre del nuevo usuario.</param>
        /// <param name="apellido">Apellido del nuevo usuario.</param>
        /// <param name="correo">Correo electrónico único.</param>
        /// <param name="FechaNacimiento">Fecha de nacimiento del usuario.</param>
        /// <param name="clave">Contraseña de acceso.</param>
        /// <param name="genero">Género del usuario.</param>
        /// <returns>Redirección a la vista de Autenticar si tiene éxito, o vista de registro con error.</returns>
        [HttpPost]
        public ActionResult Registro(string cedula, string nombre, string apellido, string correo, DateTime FechaNacimiento, string clave, string genero)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["conexion"].ConnectionString))
                {
                    SqlCommand comando = new SqlCommand("spRegistrarUsuario", cn);
                    comando.CommandType = CommandType.StoredProcedure;

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
                ViewData["Mensaje"] = "Error en Registro: " + ex.Message;
                return View();
            }
        }
    }
}