using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Usuarios.Models
{
    public class Migracion
    {
        public int IdMigracion { get; set; }
        public string TipoMigracion { get; set; }

        public DateTime Fecha { get; set; }
        public int NroMigracion { get; set; }
        public int NroIntento { get; set; }
        public int Emplid { get; set; }
        public string Estado { get; set; }
        public string Periodo { get; set; }


    }
}
