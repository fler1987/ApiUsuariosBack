using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Usuarios.Models
{
    public class UsuarioCursoOracle
    {
        public int IdUsuarioCursoOracle { get; set; }
        public string OpridSemilla { get; set; }
        public string Unidneg { get; set; }
        public string ShortName { get; set; } 
        public string UserName { get; set; }
        public int Emplid { get; set; }
        public string AcadCareer { get; set; }
        public string ClassSection { get; set; }
        public string Strm { get; set; }
        public int ClassNbr { get; set; } 
        public int Roleid { get; set; }
        public int Suspend { get; set; }
        public DateTime Sysdate { get; set; }
    }
}
