using API.Usuarios.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Usuarios.Services
{
    public interface IConfiguracionService
    {      
        Task<List<Configuracion>> GetAllConfiguracionAsync();

        Task<Configuracion> AddConfiguracionAsync(Configuracion configuracion);

        Task<Configuracion> GetConfiguracionByPeriodoAsync(string periodo);

        Task<Configuracion> UpdateConfiguracionAsync(Configuracion configuracion);
        
        Task<Configuracion> GetConfiguracionById(int id);
        Task<Configuracion> GetConfiguracionByHabilitar(bool habilitar);
    }
    
}
