using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuimicaTetu
{
    
    public class VentasQuimicos
    {
        private AccesoDatos datos = new AccesoDatos();
        int ganancia;

        public void CambiarNombreOfertas()
        {
         for (int i = 0; i < datos.Nombre.Count; i++)
            {
            switch (datos.Nombre[i])
            {
                    // jabon x3 & x5
                case ("Oferta Jabón(A)X3"):
                datos.Nombre[i] = "Jabón Líquido(A)";
                break;
                case ("Oferta Jabón(A)X5"):
                 datos.Nombre[i] = "Jabón Líquido(A)";
                break;
                case ("Oferta Jabón(B)X3"):
                datos.Nombre[i] = "Jabón Líquido(B)";
                break;
                case ("Oferta Jabón(B)X5"):
                datos.Nombre[i] = "Jabón Líquido(B)";
                break;
                        // suavizante x3 & x5 
                case ("Oferta Suavizante(C)X3"):
                datos.Nombre[i] = "Suavizante(C)";
                break;
                case ("Oferta Suavizante(C)X5"):
                datos.Nombre[i] = "Suavizante(C)";
                break;
                case ("Oferta Suavizante(R)X3"):
                datos.Nombre[i] = "Suavizante(R)";
                break;
                case ("Oferta Suavizante(R)X5"):
                datos.Nombre[i] = "Suavizante(R)";
                break;
                    // Detergente común x3 & x5
                case ("Oferta Detergente X3"):
                datos.Nombre[i] = "Detergente Común";
                break;
                case ("Oferta Detergente X5"):
                datos.Nombre[i] = "Detergente Común";
                break;
                        // Detergente mag x3 & x5
                case ("Oferta Detergente Mag X3"):
                datos.Nombre[i] = "Detergente Tipo Magistral";
                break;
                case ("Oferta Detergente Mag X5"):
                datos.Nombre[i] = "Detergente Tipo Magistral";
                break;
                case ("Oferta Cera Blanca X3"):
                datos.Nombre[i] = "Cera Blanca";
                break;
                case ("Oferta Cera Roja X3"):
                datos.Nombre[i] = "Cera Roja";
                break;
                case ("Oferta Acido X3"):
                datos.Nombre[i] = "Acido";
                break;
                case ("Oferta Quita Mancha X3"):
                datos.Nombre[i] = "Quita Manchas";
                break;
                case ("Oferta Desengrasante X3"):
                datos.Nombre[i] = "Desengrasante";
                break;
                case ("Oferta Desengrasante X5"):
                datos.Nombre[i] = "Desengrasante";
                break;
                case ("Oferta Shampo X3"):
                datos.Nombre[i] = "Shampo";
                break;
                case ("Oferta Shampo X5"):
                datos.Nombre[i] = "Shampo";
                break;
                case ("Oferta Crema X3"):
                datos.Nombre[i] = "Crema";
                break;
                case ("Oferta Crema X5"):
                datos.Nombre[i] = "Crema";
                break;
                }
          }
        }
    }
}
