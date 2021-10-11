using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Usuarios.Models
{
    public class ResponseMigracion
    {
        public int correctas { get; set; }
        public int incorrectas { get; set; }
        public string respuesta { get; set; }
    }
}
