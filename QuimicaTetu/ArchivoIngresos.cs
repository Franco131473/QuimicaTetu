using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuimicaTetu
{
    internal class ArchivoIngresos
    {
        public string ModoDePago { get; set; }
        public int Monto { get; set; }
        public string Efectivo { get; set; }
        public string Transferencia { get; set; }
        public string Fecha { get; set; }
        public string Hora { get; set; }
    }
}
