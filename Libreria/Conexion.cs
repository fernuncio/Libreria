using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;


namespace Libreria
{
    internal class Conexion
    {
        //connectionString="Data Source=.\SQLEXPRESS;integrated security=true;initial catalog=libreria;encrypt=false"
        //String cadenaConexion = "Data Source=LAPTOP-RDSR4SS0;" +
        //           "integrated security=true;initial catalog=libreria;encrypt=false";
        //string cadenaConexion = ConfigurationManager.ConnectionStrings["MiConexion"].ConnectionString;

        //uso
        string cadenaConexion = ConfigurationManager.ConnectionStrings["StrCad"].ConnectionString;
        SqlConnection conexion;

        private SqlConnection abrirConexion()
        {
            try
            {
                conexion = new SqlConnection(cadenaConexion);
                conexion.Open(); //Abrir conexion a BD
                return conexion;
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR EN LA CONEXION: " + ex.Message);
                return null;
            }

        }

        //nuevo
        //public Conexion()
        //{
        //    var config = ConfigurationManager.ConnectionStrings["MiConexion"];
        //    if (config == null)
        //        throw new Exception("Cadena de conexión 'MiConexion' no encontrada en App.config");

        //    cadenaConexion = config.ConnectionString;
        //}
        ////nuevo
        //private SqlConnection abrirConexion()
        //{
        //    try
        //    {
        //        conexion = new SqlConnection(cadenaConexion);
        //        conexion.Open(); // Abrir conexión a BD
        //        return conexion;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("ERROR EN LA CONEXIÓN: " + ex.Message);
        //        return null;
        //    }
        //}
        public bool prueba()
        {
            try
            {
                abrirConexion();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        //llamar en el frame
        public bool consultaB(string cs)
        {
            try
            {
                SqlCommand cmd = new SqlCommand(cs, abrirConexion());
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        public DataSet consulta(string cs)
        {
            try
            {
                DataSet ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(cs, abrirConexion());
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }

        }

    }
}
