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
    public class PeriodoController : ControllerBase
    {        
        private readonly IPeriodoService service;        
        public PeriodoController(IPeriodoService services)
        {
            service = services;          
        }

        [HttpGet]
        public ActionResult<List<Periodo>> GetAllPeriodo()
        {
            return service.AllPeriodo();
        }

        [HttpPost]
        public ActionResult<Periodo> CreatePeriodo(Periodo periodo)
        {
            return service.SavePeriodo(periodo);
        }
    }
}
