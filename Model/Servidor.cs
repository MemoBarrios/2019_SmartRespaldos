using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _2019_Respaldos.Model
{
    public class Servidor
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string IP { get; set; }
        public Sucursal Sucursal { get; set; }
        public List<RutaRespaldo> RutasRespaldos { get; set; }
    }
}
