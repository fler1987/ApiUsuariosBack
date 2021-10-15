using API.Usuarios.Models;
using API.Usuarios.Repository;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace API.Usuarios.Services
{
    public class MigracionService : IMigracionService
    {
        private readonly IMigracionRepository repo;
        private MyDBContext myDbContext;
        static readonly HttpClient client = new HttpClient();

        public MigracionService(IMigracionRepository repo, MyDBContext context)
        {
            this.repo = repo;
            this.myDbContext = context;
        }

        public List<Migracion> GetByPeriodo(string periodo)
        {
            return repo.GetByPeriodoAsync(periodo);
        }

        public async Task<Migracion> AddAsync(Migracion entity)
        {
            return await repo.AddAsync(entity);
        }



        public async Task<ResponseMigracion> ProcesarMigracionAsync()
        {

            ResponseMigracion respuesta = new ResponseMigracion();
            try
            {

                int MigracionesCorrectas = 0;
                int MigracionesIncorrectas = 0;

                //Insertar Schedule
                Schedule calendario = new Schedule();
                calendario.Fecha = DateTime.Now;
                myDbContext.Schedules.Add(calendario);
                myDbContext.SaveChanges();
                int IdSchedule = calendario.IdSchedule;

                var listaConfiguracion = myDbContext.Configuraciones.Where(p => p.Habilitar.Equals(true)).ToList();
                if (listaConfiguracion.Count > 0)
                {

                    var periodo = "";
                    foreach (var configuracion in listaConfiguracion)
                    {

                        periodo = configuracion.Periodo;


                        List<Migracion> listaMigracion = new List<Migracion>();

                        if (!periodo.Equals(""))
                        {
                            //Buscar si existe periodo en la migracion
                            var migraciones = myDbContext.Migraciones.Where(m => m.Periodo.Contains(periodo)).ToList();
                            if (migraciones.Count > 0)
                            {
                                //Existen Migraciones - Debe filtrar por fecha y periodo
                                var dateAndTime = DateTime.Now;
                                var fechaActual = dateAndTime.Date.ToString("dd-MM-yy");
                                var alumnosOracle = myDbContext.UsuarioCursoOracles.Where(o => o.Strm.Equals(periodo) && o.Sysdate.Equals(Convert.ToDateTime(fechaActual))).ToList();
                                var alumnosMoodle = myDbContext.UsuarioCursoMoodles.Where(o => o.Strm.Equals(periodo) && o.Sysdate.Equals(Convert.ToDateTime(fechaActual))).ToList();

                                foreach (UsuarioCursoOracle usuarioOracle in alumnosOracle)
                                {
                                    var existeUsuarioEnMoodle = myDbContext.UsuarioCursoMoodles.Where(m => m.Emplid.Equals(usuarioOracle.Emplid)).SingleOrDefault();                                
                                    if (existeUsuarioEnMoodle == null)
                                    {
                                        var oMigracion = CrearModeloMigracion(IdSchedule, usuarioOracle.Emplid, usuarioOracle.Strm, "Agregar");
                                        var existeMigracion = listaMigracion.Find(x => x.Emplid.Equals(oMigracion.Emplid));
                                        if (existeMigracion == null)
                                        {
                                            listaMigracion.Add(oMigracion);
                                        }
                                    }
                                }

                                foreach (UsuarioCursoMoodle usuarioMoodle in alumnosMoodle)
                                {
                                    var existeUsuarioEnOracle = myDbContext.UsuarioCursoOracles.Where(o => o.Emplid.Equals(usuarioMoodle.Emplid)).SingleOrDefault(); ;
                                    if (existeUsuarioEnOracle == null)
                                    {
                                        var oMigracion = CrearModeloMigracion(IdSchedule, usuarioMoodle.Emplid, usuarioMoodle.Strm, "Quitar");
                                        var existeMigracion = listaMigracion.Find(x => x.Emplid.Equals(oMigracion.Emplid));
                                        if (existeMigracion == null)
                                        {
                                            listaMigracion.Add(oMigracion);
                                        }                                       
                                    }
                                }
                            }
                            else
                            {
                                //No Existen Migraciones - Debe filtrar por periodo
                                var alumnosOracle = myDbContext.UsuarioCursoOracles.Where(o => o.Strm.Equals(periodo)).ToList();
                                var alumnosMoodle = myDbContext.UsuarioCursoMoodles.Where(o => o.Strm.Equals(periodo)).ToList();

                                foreach (UsuarioCursoOracle usuarioOracle in alumnosOracle)
                                {
                                    var item = alumnosMoodle.Find(x => x.Emplid.Equals(usuarioOracle.Emplid));
                                    if (item == null)
                                    {
                                        var oMigracion = CrearModeloMigracion(IdSchedule, usuarioOracle.Emplid, usuarioOracle.Strm, "Agregar");
                                        var existeMigracion = listaMigracion.Find(x => x.Emplid.Equals(oMigracion.Emplid));
                                        if (existeMigracion == null)
                                        {
                                            listaMigracion.Add(oMigracion);
                                        }

                                    }
                                }

                                foreach (UsuarioCursoMoodle usuarioMoodle in alumnosMoodle)
                                {
                                    var item = alumnosOracle.Find(x => x.Emplid.Equals(usuarioMoodle.Emplid));
                                    if (item == null)
                                    {
                                        var oMigracion = CrearModeloMigracion(IdSchedule, usuarioMoodle.Emplid, usuarioMoodle.Strm, "Quitar");
                                        var existeEnListaMigracion = listaMigracion.Find(x => x.Emplid.Equals(oMigracion.Emplid));
                                        if (existeEnListaMigracion == null)
                                        {
                                            listaMigracion.Add(oMigracion);
                                        }
                                    }
                                }
                            }

                            //Guardar Migracion
                            if (listaMigracion.Count > 0)
                            {
                                foreach (var entity in listaMigracion)
                                {
                                    await AddAsync(entity);
                                }
                            }

                            //Obtener Migraciones Pendientes
                            var listaEnvio = myDbContext.Migraciones.Where(m => m.Estado.Equals("Pendiente") && m.NroMigracion.Equals(IdSchedule)).ToList();
                            if (listaEnvio.Count > 0)
                            {
                                //Enviar Migracion
                                var rptaMigracion1 = enviarMigraciones(IdSchedule, listaEnvio);
                                MigracionesCorrectas = rptaMigracion1.correctas;
                                MigracionesIncorrectas = rptaMigracion1.incorrectas;

                                var listaEnvioError = myDbContext.Migraciones.Where(m => m.Estado.Equals("Rechazados") && m.NroMigracion.Equals(IdSchedule)).ToList();
                                if (listaEnvioError.Count > 0)
                                {
                                    int intento = 1;
                                    int intentoCorrecto = 0;
                                    do
                                    {
                                        var rptaMigracion2 = enviarMigraciones(IdSchedule, listaEnvio);
                                        intentoCorrecto += rptaMigracion2.correctas;
                                        intento++;
                                    } while (intento < 3);

                                    MigracionesCorrectas += intentoCorrecto;
                                    MigracionesIncorrectas -= intentoCorrecto;
                                }
                            }
                        }
                    }
                }

                respuesta.correctas = MigracionesCorrectas;
                respuesta.incorrectas = MigracionesIncorrectas;
                respuesta.respuesta = "Migraciones Correctas: " + respuesta.correctas + ", Migraciones Incorrectas: " + MigracionesIncorrectas;

                return respuesta;

            }
            catch (Exception ex)
            {
                
                GuardarLog(0, "", ex.Message.ToString(), "");
                respuesta.respuesta = ex.Message.ToString();
                return respuesta;
            }
        }

        public Migracion CrearModeloMigracion(int IdSchedule, int Emplid, string Periodo, String TipoMigracion)
        {
            var oMigracion = new Migracion();

            oMigracion.TipoMigracion = TipoMigracion;
            oMigracion.Fecha = DateTime.Now;
            oMigracion.NroMigracion = IdSchedule;
            oMigracion.NroIntento = 0;
            oMigracion.Emplid = Emplid;
            oMigracion.Periodo = Periodo;
            oMigracion.Estado = "Pendiente";


            return oMigracion;
        }

        public Migracion MergeAgregarUsuarios(int IdSchedule, UsuarioCursoOracle usuarioOracle, List<UsuarioCursoMoodle> alumnosMoodle)
        {
            var oMigracion = new Migracion();
            var item = alumnosMoodle.Find(x => x.Emplid.Equals(usuarioOracle.Emplid));

            if (item == null)
            {
                oMigracion.TipoMigracion = "Agregar";
                oMigracion.Fecha = DateTime.Now;
                oMigracion.NroMigracion = IdSchedule;
                oMigracion.NroIntento = 0;
                oMigracion.Emplid = usuarioOracle.Emplid;
                oMigracion.Periodo = usuarioOracle.Strm;
                oMigracion.Estado = "Pendiente";
            }

            return oMigracion;
        }
        public Migracion MergeQuitarUsuarios(int IdSchedule, UsuarioCursoMoodle usuarioMoodle, List<UsuarioCursoOracle> alumnosOracle)
        {

            var oMigracion = new Migracion();
            var item = alumnosOracle.Find(x => x.Emplid.Equals(usuarioMoodle.Emplid));

            if (item == null)
            {
                oMigracion.TipoMigracion = "Quitar";
                oMigracion.Fecha = DateTime.Now;
                oMigracion.NroMigracion = IdSchedule;
                oMigracion.NroIntento = 0;
                oMigracion.Emplid = usuarioMoodle.Emplid;
                oMigracion.Periodo = usuarioMoodle.Strm;
                oMigracion.Estado = "Pendiente";
            }

            return oMigracion;
        }


        public ResponseMigracion enviarMigraciones(int IdSchedule, List<Migracion> listaEnvio)
        {
            ResponseMigracion respuesta = new ResponseMigracion();
            int MigracionesCorrectas = 0;
            int MigracionesIncorrectas = 0;

            if (listaEnvio.Count > 0)
            {
                foreach (var entity in listaEnvio)
                {
                    if (!entity.Estado.Equals("Aceptado"))
                    {
                        var alumno = myDbContext.Alumnos.Where(a => a.Emplid.Equals(entity.Emplid)).SingleOrDefault();
                        if (alumno != null)
                        {
                            

                            //Enviar a WS===============================================================
                            try
                            {

                                RequestUsuario req = new RequestUsuario();
                                req.Emplid = alumno.Emplid;
                                req.Nombre = alumno.Nombre;
                                req.ApellidoPaterno = alumno.ApellidoPaterno;
                                req.ApellidoMaterno = alumno.ApellidoMaterno;
                                req.TipoMigracion = entity.TipoMigracion;

                                var request = JsonConvert.SerializeObject(req);

                                var response = ConsumirWS(request);

                                if (response.Equals("OK"))
                                {

                                    var respuestaLog = GuardarLog(IdSchedule, request, response.ToString(), entity.TipoMigracion);
                                    MigracionesCorrectas++;
                                    entity.Estado = "Aceptado";
                                    var respuestaActualizarMigracion = ActualizarMigracion(entity);
                                   
                                }
                                else {
                                    var respuestaLog = GuardarLog(IdSchedule, request, response.ToString(), entity.TipoMigracion);
                                    MigracionesIncorrectas++;                                    
                                    entity.Estado = "Rechazado";
                                    var respuestaActualizarMigracion = ActualizarMigracion(entity);
                                }
                                
                            }
                            catch (HttpRequestException e)
                            {
                                MigracionesIncorrectas++;
                                entity.NroIntento++;
                                entity.Estado = "Rechazado";
                                myDbContext.Migraciones.Update(entity);
                                myDbContext.SaveChanges();

                                Console.WriteLine("\nException Caught!");
                                Console.WriteLine("Message :{0} ", e.Message);
                            }

                        }
                    }
                }
            }

            respuesta.correctas = MigracionesCorrectas;
            respuesta.incorrectas = MigracionesIncorrectas;
            return respuesta;
        }

        public string ConsumirWS(string request) {

            HttpClient client = new HttpClient();            
            
            var url = "http://ip.jsontest.com/";            
            //var data = new StringContent(request, Encoding.UTF8, "application/json");
            //var response = await client.PostAsync(url, data);
                  
            var webRequest = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new StringContent(request, Encoding.UTF8, "application/json")
            };

            var response = client.Send(webRequest);

            //using var reader = new StreamReader(response.Content.ReadAsStream());
            //return reader.ReadToEnd();
            if (response.IsSuccessStatusCode)
            {
                //result = response.Content.ReadAsStringAsync().Result;
                return response.StatusCode.ToString();
            }
            else {
                return "Error";
            }           
        }

        public string GuardarLog(int IdSchedule, string request, string response, string tipoMigracion) {

            Log log = new Log();
            log.IdMigracion = IdSchedule;
            log.Request =request;
            log.Response = response;
            log.Fecha = DateTime.Now;
            log.Tipo = tipoMigracion;

            myDbContext.Logs.Add(log);
            if (myDbContext.SaveChanges() > 0)
            {
                return "OK";
            }
            else {
                return "Error";
            }
        }

        public string ActualizarMigracion(Migracion entity) {

            entity.NroIntento++;            
            myDbContext.Migraciones.Update(entity);
            if (myDbContext.SaveChanges() > 0)
            {
                return "OK";
            }
            else {
                return "Error Actualizar Migracion";
            }
        }
    }
}
