using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Usuarios.Models
{
    public class Configuracion
    {
        public int IdConfiguracion { get; set; }
        public int IdCategoria { get; set; }
        public string Periodo { get; set; }
        public string Nombre { get; set; }
        public Boolean Habilitar { get; set; }
    }
}
