using API.Usuarios.Models;
using API.Usuarios.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Usuarios.Services
{
    public class ConfiguracionService : IConfiguracionService
    {
        private readonly IConfigurationRepository repo;        

        public ConfiguracionService(IConfigurationRepository repo)
        {
            this.repo = repo;
        }

        public async Task<List<Configuracion>> GetAllConfiguracionAsync()
        {
            return await repo.GetAllConfiguracionAsync();
        }
        public async Task<Configuracion> GetConfiguracionByPeriodoAsync(string periodo)
        {
            return await repo.GetConfiguracionByPeriodoAsync(periodo);
        }

        public async Task<Configuracion> AddConfiguracionAsync(Configuracion configuracion)
        {          
            return await repo.AddAsync(configuracion);
        }
        
        public async Task<Configuracion> UpdateConfiguracionAsync(Configuracion configuracion)
        {
            return await repo.UpdateAsync(configuracion);
        }

        public async Task<Configuracion> GetConfiguracionById(int id) {
            return await repo.GetConfiguracionById(id);
        }
        
        public async Task<Configuracion> GetConfiguracionByHabilitar(bool habilitar) {
            return await repo.GetConfiguracionByHabilitar(habilitar);
        }


            //public IEnumerable<Configuracion> GetConfiguracion()
            //{
            //    return repo.GetConfiguracion();
            //}

        //public Configuracion GetByPeriodo(string Periodo)
        //{
        //    return repo.GetByPeriodo(Periodo);
        //}
    }
}

 

