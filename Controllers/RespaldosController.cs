using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using _2019_Respaldos.Model;
using System.Data;
using _2019_Respaldos.Data;

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

        [HttpGet("{action}/{ip}")]
        public async Task<List<Respaldo>> GetRespaldos(string ip)
        {
            return await respaldosRepository.GetAllRespaldos(ip);
        }

        [HttpGet("{action}")]
        public async Task<List<Sucursal>> GetSucursales()
        {
            return await respaldosRepository.GetSucursales();
        }

        [HttpGet("{action}/{sucursal}")]
        public async Task<List<Sucursal>> GetRespaldoSucursales(List<Sucursal> sucursal)
        {
            string _sucursal = "";
            return await respaldosRepository.GetRespaldoSucursales(_sucursal);
        }

        [HttpGet("{action}")]
        public async Task<string> GetJobsRespaldos([FromQuery] string sucursales, [FromQuery] string fecha, [FromQuery] string tipoConsulta)
        {
            return await respaldosRepository.GetJobsRespaldos(fecha, sucursales, tipoConsulta);
        }

        //[HttpGet("{action}")]
        //public async Task<string> GetFalloResp([FromQuery] string sucursales, [FromQuery] string fecha)
        //{
        //    return await respaldosRepository.GetFalloResp(fecha, sucursales);
        //}
    }
}
