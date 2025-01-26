using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace QuimicaTetu
{
    internal class Productos
    {
        public List<string> codigo = new List<string>();
        public List<string> nombre = new List<string>();
        public List<string> tipo = new List<string>();
        public List<int> cantidad = new List<int>();
        public List<int> precioDeCompra = new List<int>();
        public List<int> precioDeVenta = new List<int>();
        public List<DateTime> Vencimiento = new List<DateTime>();
        

        public void MostrarStock()
        {
            for (int i = 0; i < codigo.Count; i++)
            {
                MessageBox.Show($"{nombre[i]}, {tipo[i]}, {cantidad[i]}, {precioDeCompra[i]},{precioDeVenta[i]},{Vencimiento[i]}.");
            }
        }
       

    }
}
