using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using API.Usuarios.Models;
using API.Usuarios.Services;
using System.Threading.Tasks;
using System;

namespace API.Usuarios.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioCursoOracleService service;
        private MyDBContext myDbContext; 
        public UsuariosController(MyDBContext context, IUsuarioCursoOracleService service)
        {
            this.myDbContext = context;
            this.service = service;
        }

        [HttpGet]
        public IEnumerable<UsuarioCursoOracle> Get()
        {
            return (this.myDbContext.UsuarioCursoOracles.ToList());
        }

        //[HttpGet("GetPrueba", Name = "GetPrueba")]
        //public IEnumerable<UsuarioCursoOracle> GetPrueba()
        //{
            

        //    var configuracion = myDbContext.Configuraciones.Single(p=> p.Habilitar.Equals(true));
        //    var periodo = "";
        //    if (configuracion != null) {
        //        periodo = configuracion.Periodo;
        //    }

        //    Console.Write("Periodo: " +periodo);

        //    if (!periodo.Equals("")) {

        //        //Buscar si existe periodo en la migracion
        //        List<Migracion> listaMigracion = new List<Migracion>();
        //        var migraciones = myDbContext.Migraciones.Where(m => m.Periodo.Contains(periodo)).ToList();
        //        if (migraciones.Count > 0)
        //        {
        //            //Existen Migraciones - Debe filtrar por fecha y periodo
        //            var dateAndTime = DateTime.Now;
        //            var fechaActual = dateAndTime.Date.ToString("dd-MM-yyyy");
        //            var alumnosOracle = myDbContext.UsuarioCursoOracles.Where(o => o.Strm == periodo && o.Sysdate.ToString("dd-MM-yyyy") == fechaActual).ToList();
        //            var alumnosMoodle = myDbContext.UsuarioCursoMoodles.Where(o => o.Strm == periodo && o.Sysdate.ToString("dd-MM-yyyy") == fechaActual).ToList();

        //            foreach (UsuarioCursoOracle usuarioOracle in alumnosOracle) {
        //                var oMigracion = new Migracion();
        //                var item = alumnosMoodle.Find(x => x.Emplid == usuarioOracle.Emplid);
        //                if (item != null) {
        //                    oMigracion.TipoMigracion = "Agregar";
        //                    oMigracion.Fecha = DateTime.Now;
        //                    oMigracion.NroMigracion = 1;
        //                    oMigracion.NroIntento = 1;
        //                    oMigracion.Emplid = usuarioOracle.Emplid;
        //                    oMigracion.Periodo = usuarioOracle.Strm;
        //                    listaMigracion.Add(oMigracion);
        //                }
        //            }

        //            foreach (UsuarioCursoMoodle usuarioMoodle in alumnosMoodle)
        //            {
        //                var oMigracion = new Migracion();
        //                var item = alumnosOracle.Find(x => x.Emplid == usuarioMoodle.Emplid);
        //                if (item != null)
        //                {
        //                    oMigracion.TipoMigracion = "Quitar";
        //                    oMigracion.Fecha = DateTime.Now;
        //                    oMigracion.NroMigracion = 1;
        //                    oMigracion.NroIntento = 1;
        //                    oMigracion.Emplid = usuarioMoodle.Emplid;
        //                    oMigracion.Periodo = usuarioMoodle.Strm;
        //                    listaMigracion.Add(oMigracion);
        //                }
        //            }
        //        }
        //        else {
        //            //No Existen Migraciones - Debe filtrar por periodo
        //            var alumnosOracle = myDbContext.UsuarioCursoOracles.Where(o => o.Strm == periodo).ToList();
        //            var alumnosMoodle = myDbContext.UsuarioCursoMoodles.Where(o => o.Strm == periodo).ToList();                    

        //            foreach (UsuarioCursoOracle usuarioOracle in alumnosOracle)
        //            {
        //                var oMigracion = new Migracion();
        //                var item = alumnosMoodle.Find(x => x.Emplid == usuarioOracle.Emplid);
        //                if (item != null)
        //                {
        //                    oMigracion.TipoMigracion = "Agregar";
        //                    oMigracion.Fecha = DateTime.Now;
        //                    oMigracion.NroMigracion = 1;
        //                    oMigracion.NroIntento = 0;
        //                    oMigracion.Emplid = usuarioOracle.Emplid;
        //                    oMigracion.Periodo = usuarioOracle.Strm;
        //                    listaMigracion.Add(oMigracion);
        //                }
        //            }

        //            foreach (UsuarioCursoMoodle usuarioMoodle in alumnosMoodle)
        //            {
        //                var oMigracion = new Migracion();
        //                var item = alumnosOracle.Find(x => x.Emplid == usuarioMoodle.Emplid);
        //                if (item != null)
        //                {
        //                    oMigracion.TipoMigracion = "Quitar";
        //                    oMigracion.Fecha = DateTime.Now;
        //                    oMigracion.NroMigracion = 1;
        //                    oMigracion.NroIntento = 0;
        //                    oMigracion.Emplid = usuarioMoodle.Emplid;
        //                    oMigracion.Periodo = usuarioMoodle.Strm;
        //                    listaMigracion.Add(oMigracion);
        //                }
        //            }

        //        }


        //        //Insertar en la Tabla Migracion
        //        if (listaMigracion.Count > 0) {
        //            listaMigracion.ForEach(x => {
        //                myDbContext.Migraciones.Update(x);
        //            });
        //        }

        //    }

        //    return (this.myDbContext.UsuarioCursoOracles.ToList());
        //}

        public class RequestMigracion {
            public int Emplid { get; set; }
            public string nombre { get; set; }
            public string apellidoPaterno { get; set; }
            public string apellidoMaterno { get; set; }
            public int nro_intento { get; set; }
        }

        [HttpGet("GetAllUsuarioCursoOracleAsync", Name = "GetAllUsuarioCursoOracleAsync")]
        public async Task<ActionResult<List<UsuarioCursoOracle>>> GetAllUsuarioCursoOracleAsync()
        {
            return await service.GetAllUsuarioCursoOracleAsync();
        }

        //private readonly Conexion _dbContext;
        //public UsuariosController(Conexion dbContext)
        //{
        //    _dbContext = dbContext;
        //}

        //[HttpGet]
        //public IActionResult GetAll()
        //{
        //    using (_dbContext)
        //    {
        //        var list = _dbContext.UsuarioInfos.ToList();
        //        return Ok(list);
        //    }
        //}

        //private readonly AppDb _dbContext;
        //public UsuariosController(AppDb db)
        //{
        //    _dbContext = db;
        //}



        //public UsuariosController(Conexion dbContext)
        //{
        //    _dbContext = dbContext;
        //}

        //[HttpGet]
        //public IActionResult GetAll()
        //{
        //    using (_dbContext)
        //    {
        //        // var list2 = "ok";
        //        return Ok(_dbContext.UsuarioInfos.ToArray());
        //        //var list = _dbContext.UsuarioInfos.ToList();
        //        //return Ok(list2);
        //    }
        //}


        //[HttpGet]
        //public ActionResult ListarUsuarios()
        //{
        //    return Ok(dbConexion.UsuarioInfos.ToArray());
        //}
        //public List<BEUsuarioERP> Listar()
        //{
        //    BLUsuarios oUsuarioLogic = new BLUsuarios();
        //    return oUsuarioLogic.ListarUsuariosERP();
        //}

        //[HttpGet]
        //public IActionResult GetAll()
        //{
        //    using (dbConexion)
        //    {
        //        var list = dbConexion.UsuarioInfos.ToList();
        //        return Ok(list);
        //    }
        //}
    }
}
