using API.DataAccess;
using API.Entity;
using System.Collections.Generic;

namespace API.BussinessLogic
{
    public class BLUsuarios
    {
        public List<BEUsuarioERP> ListarUsuariosERP()
        {
            return DAUsuarios.ListarUsuariosERP();
        }

        public List<BEUsuarioERP> BuscarUsuarioRepetidos()
        {
            List<BEUsuarioERP> usuarioERP = new List<BEUsuarioERP>();
            List<BEUsuarioMoodle> usuarioMoodle = new List<BEUsuarioMoodle>();

            usuarioERP = DAUsuarios.ListarUsuariosERP();
            usuarioMoodle = DAUsuarios.ListarUsuariosMoodle();

            return DAUsuarios.ListarUsuariosERP();
        }
    }
}
