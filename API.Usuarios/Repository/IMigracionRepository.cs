using API.Usuarios.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Usuarios.Repository
{
    public interface IMigracionRepository : IRepository<Migracion>
    {
        Task<List<Migracion>> GetAllAsync();        
        List<Migracion> GetByPeriodoAsync(string periodo);
        //Task<List<Migracion>> GetByPeriodoAndByFechaAsync(string periodo);
    }
}
