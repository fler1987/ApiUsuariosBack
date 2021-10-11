using API.Usuarios.Models;
using API.Usuarios.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Usuarios.Services
{
    public class UsuarioCursoOracleService : IUsuarioCursoOracleService
    {
        private readonly IUsuarioCursoOracleRepository repo;

        public UsuarioCursoOracleService(IUsuarioCursoOracleRepository repo)
        {
            this.repo = repo;
        }

        public async Task<List<UsuarioCursoOracle>> GetAllUsuarioCursoOracleAsync()
        {
            return await repo.GetAllUsuarioCursoOracleAsync();
        }
    }
}
