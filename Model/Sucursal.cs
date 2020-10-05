using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _2019_Respaldos.Model
{
    public class Sucursal
    {
        public int Clave { get; set; }
        public string Nombre { get; set; }
        public string Ip { get; set; }
        public string Db { get; set; }
        public bool Seleccionado { get; set; }
        public List<Respaldo> Respaldos { get; set; }
    }
}
