using API.Usuarios.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Usuarios.Repository
{    
    public interface IConfigurationRepository : IRepository<Configuracion>
    {       
        Task<List<Configuracion>> GetAllConfiguracionAsync();
        Task<Configuracion> AddAsync(Configuracion entity);
        Task<Configuracion> GetConfiguracionByPeriodoAsync(string periodo);
        Task<Configuracion> GetConfiguracionById(int id);
        Task<Configuracion> GetConfiguracionByHabilitar(bool habilitar);
    }
}
