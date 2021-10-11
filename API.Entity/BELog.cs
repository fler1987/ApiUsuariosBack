using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace API.Entity
{
    [DataContract]
    public class BELog
    {
        [DataMember(Name = "Id")]
        public int Id { get; set; }

        [DataMember(Name = "Descripcion")]
        public string Descripcion { get; set; }

        [DataMember(Name = "FechaRegistro")]
        public DateTime FechaRegistro { get; set; }
    }
}
