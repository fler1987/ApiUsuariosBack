using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Usuarios.Models
{
    public class RequestUsuario
    {
        public int Emplid { get; set; }

        [JsonProperty("Nombre", Required = Newtonsoft.Json.Required.Always)]
        public string Nombre { get; set; }
        [JsonProperty("ApellidoMaterno", Required = Newtonsoft.Json.Required.Always)]
        public string ApellidoMaterno { get; set; }

        [JsonProperty("ApellidoPaterno", Required = Newtonsoft.Json.Required.Always)]
        public string ApellidoPaterno { get; set; }

        [JsonProperty("TipoMigracion", Required = Newtonsoft.Json.Required.Always)]
        public string TipoMigracion { get; set; }
    }
}
