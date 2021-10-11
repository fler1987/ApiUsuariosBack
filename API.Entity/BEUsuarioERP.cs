using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace API.Entity
{
    [DataContract]
    public class BEUsuarioERP
    { 
            [DataMember(Name = "Id")]
            public int Id { get; set; }

            [DataMember(Name = "Usuario")]
            public string Usuario { get; set; }
         
    }
}
