using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppNomina.Models
{
    public class Empleado
    {
        public string cedula { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public char genero { get; set; }
        public string FechaIngreso { get; set; }
        public string correo { get; set; }
        public string usuario { get; set; }
        public string clave { get; set; }
        public int Id { get; set; }

        public string mensaje { get; set; }

        public int retorno { get; set; }

    }
}