using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;


namespace ConexionSQL
{
    class CLSConexion
    {
        public SqlConnection DB_CONN;

        public CLSConexion()
        {
            try
            {
                DB_CONN = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                DB_CONN.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo conectar a la base de datos" + ex.ToString());

            }
        }
    }
}