using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuimicaTetu
{
    public class CAJA
    {
        private const string FilePath = "CajaData.txt";
        public int DineroActual { get; private set; }
        public CAJA()
        {
            // Cargar el valor desde el archivo  de texto
            if (File.Exists(FilePath))
            {
                string contenido = File.ReadAllText(FilePath);
                if (int.TryParse(contenido, out int dinero))
                {
                    DineroActual = dinero;
                }
                else
                {
                    DineroActual = 11100;
                }
            }
            else
            {
                DineroActual = 11100;
                GuardarEstado();
            }
        }
        public void GuardarEstado()
        {
            File.WriteAllText(FilePath,DineroActual.ToString());
        }
        public void ActualizarCaja(int monto)
        {
            DineroActual += monto;
        }
        public void EstablecerDineroActual(int nuevoMonto)
        {
            DineroActual = nuevoMonto;
            GuardarEstado(); // Guarda el estado después de establecer el nuevo valor  
        }
    }
}
