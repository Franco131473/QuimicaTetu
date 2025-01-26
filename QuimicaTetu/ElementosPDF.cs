using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuimicaTetu
{
    public class ElementosPDF
    {
        public List<string> listaProducto { get; set; } = new List<string>();
        public List<string> listaCantidad { get; set; } = new List<string>();
        public List<string> listaPrecio { get; set; } = new List<string>();
        //
        public List<string> listaProducto_HPyP { get; set; } = new List<string>();
        public List<string> listaCantidad_HPyP { get; set; } = new List<string>();
        public List<string> listaPrecio_HPyP { get; set; } = new List<string>();
        //
        public List<string> listaProducto_ArtLimpieza { get; set; } = new List<string>();
        public List<string> listaCantidad_ArtLimpieza { get; set; } = new List<string>();
        public List<string> listaPrecio_ArtLimpieza { get; set; } = new List<string>();
        //
        public List<string> listaProducto_Ropa { get; set; } = new List<string>();
        public List<string> listaCantidad_Ropa { get; set; } = new List<string>();
        public List<string> listaPrecio_Ropa { get; set; } = new List<string>();
        public string name { get; set; }
        public void CambiarString()
        {
            CultureInfo cultura = new CultureInfo("es-AR");
            for (int i = 0; i < listaPrecio.Count; i++)
            {
                if (decimal.TryParse(listaPrecio[i], out decimal valor))
                { listaPrecio[i] = valor.ToString("C", cultura); }
            }

        }
    } 

}
