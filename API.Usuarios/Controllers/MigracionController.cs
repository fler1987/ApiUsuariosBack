using API.Usuarios.Models;
using API.Usuarios.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Usuarios.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MigracionController : ControllerBase
    {
        private readonly IConfiguracionService configuracionService;
        private readonly IMigracionService migracionService;
        private MyDBContext myDbContext;
        public MigracionController(IConfiguracionService configuracion, IMigracionService migracion, MyDBContext contexts)
        {
            configuracionService = configuracion;
            migracionService = migracion;
            myDbContext = contexts;
        }

        [HttpPost]
        public async Task<ActionResult<ResponseMigracion>> IniciarMigracion()
        {
            return await migracionService.ProcesarMigracionAsync();
        }        
    }
}
