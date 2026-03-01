using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


// IMPORTAR LIBRERIAS PARA CONECTARSE A SQL
using System.Data;
using System.Data.SqlClient;

// IMPORTAR LIBRERIA PARA MANEJAR OTRAS FUNCIONALIDADES
using System.Security.Cryptography;
using System.Configuration;

// instancia de la capa del modelo xq necesito acceder dentro del modelo ami clase empleado xq ahi ya estan los atributos autilizar para caprturar datos del formulario
using WebAppNomina.Models;


namespace WebAppNomina.Controllers
{
    public class AccesoController : Controller
    {
        // GET: AccesoControlador comn este metodo obtengo de vista
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Autenticar()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Registro()
        {
            return View();
        }

        // POST
        [HttpPost]
        // dar funcionalidades

        // METODO EXTRATION RESULT DE AUTENTICAR
        public ActionResult Autenticar(Empleado oempleado)//recibe de parametro un objeto de tipo empleado para capturar los datos declarados
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["conexion"].ConnectionString))  //mando a llamar al nombre conexion en web.config
                {
                    //SqlCommand comando = new SqlCommand("Select * from employees", cn);//qui en las "" pongo el objeto el procedure o bjeto a ejecutar como aatributos.
                    //comando.CommandType=CommandType.Text; // aqui especifico el tipo de comando

                    //cn.Open();  //aqui abro la conexion
                    //comando.ExecuteNonQuery();  //para ejecutar el comando
                    //cn.Close();

                    //return RedirectToAction("About", "Home"); // si todo coincide le mando al index


                    // PARA VALIDAR INGRESO DE UN USUARIO
                    // debo indica procedure y los parametros de entraa y salida_

                    SqlCommand comando = new SqlCommand("spValidarUsuario", cn);
                    comando.Parameters.AddWithValue("usuario", oempleado.correo); // este comando tiene 2 atributos la referencia el nombre dle cambio en la DB y el otro es de donde voy a tomar el valor
                    comando.Parameters.AddWithValue("clave", oempleado.clave);
                    comando.Parameters.Add("id", SqlDbType.Int).Direction=ParameterDirection.Output;// este parametro de salida
                    comando.Parameters.Add("mensaje", SqlDbType.VarChar ,100).Direction = ParameterDirection.Output;
                    comando.CommandType = CommandType.StoredProcedure;
                    cn.Open();
                    comando.ExecuteNonQuery();

                    // antes de cerrar la conexion debo capturar los datos especificados en parametros de salida

                    oempleado.Id = Convert.ToInt32(comando.Parameters["id"].Value); //aqui capturo en este objeteo el valor de mi arg de salida del procedure
                    oempleado.mensaje = comando.Parameters["mensaje"].Value.ToString();


                    cn.Close();

                    //ahora esto le da navegavilidad
                    if (oempleado.Id==1)
                    {
                        return RedirectToAction("Index", "Home");
                    } else if (oempleado.Id==0)
                    {
                        return RedirectToAction("Registro", "Acceso");
                    } else
                    {
                        return View();
                    }
                }
            } catch (Exception e)
            {
                return View();
            }
        }

        // METODO ACTION RESULT DE REGISTRO
        [HttpPost]
        public ActionResult Registro(Empleado oempleado)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["conexion"].ConnectionString))  //mando a llamar al nombre conexion en web.config
                {
                    SqlCommand comando = new SqlCommand("spRegistrarUsuario", cn);
                    comando.Parameters.AddWithValue("cedula", oempleado.cedula);
                    comando.Parameters.AddWithValue("nombre", oempleado.nombre);
                    comando.Parameters.AddWithValue("apellido", oempleado.apellido);
                    comando.Parameters.AddWithValue("correo", oempleado.correo);
                    comando.Parameters.AddWithValue("genero", oempleado.genero);
                    comando.Parameters.AddWithValue("fecha", oempleado.FechaNacimiento);
                    comando.Parameters.AddWithValue("clave", oempleado.clave);

                    comando.Parameters.Add("retorno", SqlDbType.Int).Direction = ParameterDirection.Output;
                    comando.Parameters.Add("mensaje", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
                    comando.CommandType = CommandType.StoredProcedure;
                    cn.Open();
                    comando.ExecuteNonQuery();

                    oempleado.retorno = Convert.ToInt32(comando.Parameters["retorno"].Value);
                    oempleado.mensaje = comando.Parameters["mensaje"].Value.ToString();

                    cn.Close();

                    if (oempleado.retorno == 1)
                    {
                        return RedirectToAction("Autenticar", "Acceso");
                    }
                    else if (oempleado.retorno == 0)
                    {
                        return RedirectToAction("Autenticar", "Acceso");
                    }
                    else
                    {
                        return View();
                    }
                }
                          
            } catch (Exception e)
            { 
                return View();  
            }
        }

    }
}
