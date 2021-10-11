using API.Usuarios.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Usuarios.Services
{
    public interface IPeriodoService
    {
        public List<Periodo> AllPeriodo();
        public Periodo SavePeriodo(Periodo periodo);
    }
}
