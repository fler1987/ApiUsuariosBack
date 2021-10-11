using API.Usuarios.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Usuarios.Repository.Implementation
{
    public class MigracionRepository : Repository<Migracion>, IMigracionRepository
    {
        protected readonly MyDBContext context;
        public MigracionRepository(MyDBContext myDBContext) : base(myDBContext)
        {
        }

        public async Task<List<Migracion>> GetAllAsync()
        {
            return await GetAll().ToListAsync();
        }

        public List<Migracion> GetByPeriodoAsync(string periodo)
        {
            return GetAll().Where(m => m.Periodo.Contains(periodo)).ToList();
            // FirstOrDefaultAsync(x => x.Periodo == periodo);
            //return await context.Migraciones.Where(m => m.Periodo.Contains(periodo)).ToList();
        }

        //public async Task<List<Migracion>> GetByPeriodoAndFechaAsync(string Periodo, DateTime Fecha)
        //{
        //    return await GetAll().FirstOrDefaultAsync(x => x.Periodo == Periodo && x.Fecha.ToString("dd/MM/yy") == Fecha.ToString("dd/MM/yy"));
        //}
    }
}
