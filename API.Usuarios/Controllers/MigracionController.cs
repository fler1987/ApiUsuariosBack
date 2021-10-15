using API.Usuarios.Models;
using API.Usuarios.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
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
        
        [HttpGet("ObtenerIPPublica", Name = "ObtenerIPPublica")]

        public ActionResult<String> ObtenerIPPublica()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://ip.jsontest.com/");
            var getTask = client.GetStringAsync("");
            getTask.Wait();
            var Res =  getTask.Result;

            return Res;
        }

        [HttpPost("EnviarJsonTest", Name = "EnviarJsonTest")]

        public async Task<ActionResult<String>> EnviarJsonTest()
        {
            HttpClient client = new HttpClient();
            //client.BaseAddress = new Uri("http://ip.jsontest.com/");

            Req req = new Req();
            req.property = "Sites";
            req.report_type = "ALL";

            var url = "http://ip.jsontest.com/";
            var json = JsonConvert.SerializeObject(req);
            var data = new System.Net.Http.StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(url, data);
            string result = "";
            if (response.IsSuccessStatusCode) {
                result = response.Content.ReadAsStringAsync().Result;
            }
      
            if (response.StatusCode == System.Net.HttpStatusCode.OK) {
                return result;
            }
            else
            {
                return "Error";
            }                                     
        }

        [HttpPost("PostValue", Name = "PostValue")]
        public ActionResult<string> PostValue()
        {
            var client = new HttpClient();
            var url = "http://ip.jsontest.com/";
            Req req = new Req();
            req.property = "Sites";
            req.report_type = "ALL";

            
            var json = JsonConvert.SerializeObject(req);
            

            var webRequest = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };

            var response = client.Send(webRequest);

            using var reader = new StreamReader(response.Content.ReadAsStream());

            return reader.ReadToEnd();
        }

        public class Req {
            public string property { get; set; }
            public string report_type { get; set; }
        }


    }
}
