using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Usuarios.Models
{
    public class UsuarioCursoMoodle
    {
        public int IdUsuarioCursoMoodle{ get; set; }
        public string UserName { get; set; }
        public int Emplid { get; set; }
        public string Strm { get; set; }
        public DateTime Sysdate { get; set; }

    }
}
