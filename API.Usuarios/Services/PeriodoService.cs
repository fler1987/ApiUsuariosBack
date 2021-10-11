using API.Usuarios.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Usuarios.Services
{
    public class PeriodoService : IPeriodoService
    {
        private MyDBContext myDBContext;

        public PeriodoService(MyDBContext context) {
            myDBContext = context;
        }

        public List<Periodo> AllPeriodo() {
            return myDBContext.Periodos.ToList();
        }
        public Periodo SavePeriodo(Periodo periodo) {
            myDBContext.Periodos.Add(periodo);
            myDBContext.SaveChanges();
            return periodo;
        }

        
    }
}
