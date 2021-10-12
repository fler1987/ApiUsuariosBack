using API.Usuarios.Models;
using API.Usuarios.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
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
                                var alumnosMoodle = myDbContext.UsuarioCursoMoodles.Where(o => o.Strm.Equals(periodo)).ToList();

                                foreach (UsuarioCursoOracle usuarioOracle in alumnosOracle)
                                {
                                    var oMigracion = MergeAgregarUsuarios(IdSchedule, usuarioOracle, alumnosMoodle);
                                    if (oMigracion != null)
                                    {
                                        listaMigracion.Add(oMigracion);
                                    }
                                }

                                foreach (UsuarioCursoMoodle usuarioMoodle in alumnosMoodle)
                                {
                                    var oMigracion = MergeQuitarUsuarios(IdSchedule, usuarioMoodle, alumnosOracle);
                                    if (oMigracion != null)
                                    {
                                        listaMigracion.Add(oMigracion);
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
                                    var oMigracion = MergeAgregarUsuarios(IdSchedule, usuarioOracle, alumnosMoodle);
                                    if (!oMigracion.Emplid.Equals(0))
                                    {
                                        var existeMigracion = listaMigracion.Find(x => x.Emplid.Equals(oMigracion.Emplid));
                                        if (existeMigracion == null)
                                        {
                                            listaMigracion.Add(oMigracion);
                                        }
                                    }
                                }

                                foreach (UsuarioCursoMoodle usuarioMoodle in alumnosMoodle)
                                {
                                    var oMigracion = MergeQuitarUsuarios(IdSchedule, usuarioMoodle, alumnosOracle);
                                    if (!oMigracion.Emplid.Equals(0))
                                    {
                                        var existeMigracion = listaMigracion.Find(x => x.Emplid.Equals(oMigracion.Emplid));
                                        if (existeMigracion == null)
                                        {
                                            listaMigracion.Add(oMigracion);
                                        }
                                    }
                                }

                            }

                            //Insertar en la Tabla Migracion
                            if (listaMigracion.Count > 0)
                            {
                                foreach (var entity in listaMigracion)
                                {
                                    await AddAsync(entity);
                                }
                            }

                            var listaEnvio = myDbContext.Migraciones.Where(m => m.Estado.Equals("Pendiente") && m.NroMigracion.Equals(IdSchedule)).ToList();
                            if (listaEnvio.Count > 0)
                            {
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
                respuesta.respuesta = ex.Message.ToString();
                return respuesta;
            }
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
                        var alumno = myDbContext.Alumnos.First(a => a.Emplid.Equals(entity.Emplid));
                        if (alumno != null)
                        {
                            RequestUsuario req = new RequestUsuario();
                            req.Emplid = alumno.Emplid;
                            req.Nombre = alumno.Nombre;
                            req.ApellidoPaterno = alumno.ApellidoPaterno;
                            req.ApellidoMaterno = alumno.ApellidoMaterno;
                            req.TipoMigracion = entity.TipoMigracion;

                            //Enviar a WS===============================================================

                            try
                            {

                                client.BaseAddress = new Uri("https://reqbin.com/echo/post/json");


                                var postTask = client.PostAsJsonAsync<RequestUsuario>("migracion", req);
                                postTask.Wait();
                                var Res = postTask.Result;

                                Log log = new Log();
                                log.IdMigracion = IdSchedule;
                                log.Request = Convert.ToString(req);
                                log.Response = Res.ToString();
                                log.Fecha = DateTime.Now;
                                log.Tipo = entity.TipoMigracion;

                                myDbContext.Logs.Add(log);
                                myDbContext.SaveChanges();

                                if (Res.IsSuccessStatusCode)
                                {
                                    MigracionesCorrectas++;
                                    entity.NroIntento++;
                                    entity.Estado = "Aceptado";
                                    myDbContext.Migraciones.Update(entity);
                                    myDbContext.SaveChanges();
                                }
                                else
                                {
                                    MigracionesIncorrectas++;
                                    entity.NroIntento++;
                                    entity.Estado = "Rechazado";
                                    myDbContext.Migraciones.Update(entity);
                                    myDbContext.SaveChanges();
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


        //public WebResponse PostUsuario(int IdSchedule, string TipoMigracion, string data)
        //{
        //    var url = $"http://localhost:8080/usuario";
        //    var request = (HttpWebRequest)WebRequest.Create(url);
        //    string json = $"{{\"data\":\"{data}\"}}";
        //    request.Method = "POST";
        //    request.ContentType = "application/json";
        //    request.Accept = "application/json";
        //    using (var streamWriter = new StreamWriter(request.GetRequestStream()))
        //    {
        //        streamWriter.Write(json);
        //        streamWriter.Flush();
        //        streamWriter.Close();
        //    }
        //    try
        //    {
        //        using (WebResponse response = request.GetResponse())
        //        {
        //            using (Stream strReader = response.GetResponseStream())
        //            {
        //                if (strReader == null) return;
        //                using (StreamReader objReader = new StreamReader(strReader))
        //                {
        //                    return 
        //                    string responseBody = objReader.ReadToEnd();
        //                    return responseBody;
        //                }
        //            }
        //        }
        //    }
        //    catch (WebException ex)
        //    {
        //        // Handle error
        //    }
        //}
    }
}
