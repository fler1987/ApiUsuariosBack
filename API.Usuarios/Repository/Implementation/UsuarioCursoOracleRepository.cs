using API.Usuarios.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Usuarios.Repository.Implementation
{
    public class UsuarioCursoOracleRepository : Repository<UsuarioCursoOracle>, IUsuarioCursoOracleRepository
    {

        public UsuarioCursoOracleRepository(MyDBContext myDBContext) : base(myDBContext)
        {
        }

        public async Task<List<UsuarioCursoOracle>> GetAllUsuarioCursoOracleAsync()
        {
            return await GetAll().ToListAsync();
        }
    }
}
