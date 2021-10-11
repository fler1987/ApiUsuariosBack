using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Usuarios.Models
{
    public class Alumno
    {
        public int IdAlumno { get; set; } 
        public int Emplid { get; set; } 
        public string Nombre { get; set; } 
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; } 
        public string Email { get; set; }
    }
}
