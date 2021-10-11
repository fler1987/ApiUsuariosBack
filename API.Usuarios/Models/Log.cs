using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Usuarios.Models
{
    public class Log
    {
        public int IdLog { get; set; }
        public int IdMigracion { get; set; }
        public string Request { get; set; }
        public string Response { get; set; }
        public DateTime Fecha { get; set; }
        public string Tipo { get; set; }

    }
}
