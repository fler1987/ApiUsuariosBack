using API.Usuarios.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Usuarios.Services
{
    public interface IUsuarioCursoOracleService
    {
        Task<List<UsuarioCursoOracle>> GetAllUsuarioCursoOracleAsync();
    }
}
