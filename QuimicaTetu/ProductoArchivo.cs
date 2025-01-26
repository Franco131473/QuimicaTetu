using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuimicaTetu
{
    public class ProductoArchivo
    {
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Tipo { get; set; }
        public int Cantidad { get; set; }
        public int PreC  { get; set; }
        public int PreV { get; set; }
        public int GananciaUnitaria { get; set; }
        public DateTime Vencimiento { get; set; }
        public int IngresoPotencial { get; set; }
        public int GananciaPotencial { get; set; }

    }
}
