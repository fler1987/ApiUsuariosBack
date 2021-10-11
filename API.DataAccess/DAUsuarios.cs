using API.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.DataAccess
{
    public class DAUsuarios
    {
         
        public static List<BEUsuarioERP> ListarUsuariosERP()
        {
            var lista = new List<BEUsuarioERP>();
            string cadenaConexion = "Data Source=CHECO;DataBase=BDApi;Integrated Security=true";
            //string cadenaConexion = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection cn = new SqlConnection(cadenaConexion);
            cn.Open();
            SqlCommand cmd = new SqlCommand("SpListarUsuariosERP", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataReader drlector = cmd.ExecuteReader();

            while (drlector.Read())
            {
                BEUsuarioERP oUsuarioERP = new BEUsuarioERP();
                oUsuarioERP.Id = Convert.ToInt32(drlector["Id"]);
                oUsuarioERP.Usuario = drlector["Usuario"].ToString().Trim(); 
                lista.Add(oUsuarioERP);
            }
            return lista;
        }



        public static List<BEUsuarioMoodle> ListarUsuariosMoodle()
        {
            var lista = new List<BEUsuarioMoodle>();
            string cadenaConexion = "Data Source=CHECO;DataBase=BDApi;Integrated Security=true";
            //string cadenaConexion = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection cn = new SqlConnection(cadenaConexion);
            cn.Open();
            SqlCommand cmd = new SqlCommand("SpListarUsuariosMoodle", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataReader drlector = cmd.ExecuteReader();

            while (drlector.Read())
            {
                var oUsuarioMoodle = new BEUsuarioMoodle();
                oUsuarioMoodle.Id = Convert.ToInt32(drlector["Id"]);
                oUsuarioMoodle.Usuario = drlector["Usuario"].ToString().Trim();
                lista.Add(oUsuarioMoodle);
            }
            return lista;
        }
    }
}
