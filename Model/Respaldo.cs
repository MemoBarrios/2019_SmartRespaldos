using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _2019_Respaldos.Model
{
    public class Respaldo
    {
        public string Tipo { get; set; }
        public string DB { get; set; }
        public byte NumeroSemana { get; set; }
        public string RutaActual { get; set; }
        public string RutaAnterior { get; set; }
    }
}
