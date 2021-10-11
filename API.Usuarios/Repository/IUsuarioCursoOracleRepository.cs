using API.Usuarios.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Usuarios.Repository
{    
    public interface IUsuarioCursoOracleRepository : IRepository<UsuarioCursoOracle>
    {       
        Task<List<UsuarioCursoOracle>> GetAllUsuarioCursoOracleAsync();        
    }
}
