using System.Configuration;
using System.Data.SqlClient;

namespace WebAppNomina.DAL
{
    public class Conexion
    {
        private string cadena = ConfigurationManager
            .ConnectionStrings["conexion"].ConnectionString;

        public SqlConnection ObtenerConexion()
        {
            return new SqlConnection(cadena);
        }
    }
}