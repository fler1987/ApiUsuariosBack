using API.Usuarios.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Usuarios.Services
{
    public interface IMigracionService
    {
        List<Migracion> GetByPeriodo(string periodo);
        Task<Migracion> AddAsync(Migracion entity);

        Task<ResponseMigracion> ProcesarMigracionAsync();
    }
}