using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using _2019_Respaldos.Model;
using System.Data;
using _2019_Respaldos.Data;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace _2019_Respaldos.Controllers
{
    [Route("{controller}/")]
    public class RespaldosController : Controller
    {
        private readonly RespaldosRepository respaldosRepository;
        public RespaldosController(RespaldosRepository _respaldosRepository)
        {
            this.respaldosRepository = _respaldosRepository ?? throw new ArgumentNullException(nameof(_respaldosRepository));
        }

        [HttpGet("{action}")]
        public async Task<List<Sucursal>> GetSucursales()
        {
            List<Sucursal> lstSucursales = await respaldosRepository.GetSucursales();
            HttpContext.Session.SetString("Sucursales", JsonConvert.SerializeObject(lstSucursales));
            return lstSucursales;
        }

        [HttpGet("{action}")]
        public async Task<string> GetJobsRespaldos([FromQuery] string sucursales, [FromQuery] string fecha, [FromQuery] string tipoConsulta)
        {
            List<Sucursal> _catalogoSucs = JsonConvert.DeserializeObject<List<Sucursal>>(HttpContext.Session.GetString("Sucursales"));
            return await respaldosRepository.GetJobsRespaldos(fecha, sucursales, tipoConsulta, _catalogoSucs);
        }

        [HttpGet("{action}")]
        public async Task<IEnumerable<Servidor>> GetRutasRespaldos([FromQuery] string sucursales)
        {
            List<Sucursal> _catalogoSucs = JsonConvert.DeserializeObject<List<Sucursal>>(HttpContext.Session.GetString("Sucursales"));
            return await respaldosRepository.GetRutasRespaldos(sucursales, _catalogoSucs);
        }
    }
}
