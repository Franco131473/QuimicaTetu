using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Windows.Forms;
using System.IO;

namespace QuimicaTetu
{
    internal class AccesoDatos
    {
        public string ruta = @"C:\Users\Netbook\Desktop\ArchivosQuimica.json";
        public string ruta2 = @"C:\Users\Netbook\Desktop\ArchivosQuimicaVentas.json";
        public string ruta3 = @"C:\Users\Netbook\Desktop\ArchivosQuimicaVentasTotal.json";
        public string ruta4 = @"C:\Users\Netbook\Desktop\PrecioQuímicos.json";
        // Restar porducto del Stock Luego de la Venta
        public List<string> Nombre = new List<string>();
        public List<string> Tipo = new List<String>();
        public List<string> CantidadList = new List<string>();
        public List<string> Fecha = new List<string>();
        public List<string> Hora = new List<string>();
        public List<string> PC = new List<String>();
        public List<string> PV = new List<String>();
        public List<string> Ganancia = new List<string>();


        public void RestarCantidad()
        {
            //List<ProductoArchivo> listaProductos;

            // Paso 1: Cargar los productos desde el archivo JSON  
            try
            {
             // MessageBox.Show($"{Nombre.Count}, {CantidadList.Count}");
                string json = File.ReadAllText(ruta);
                listaProductos = JsonConvert.DeserializeObject<List<ProductoArchivo>>(json);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al leer el archivo: {ex.Message}");
                return; // Salir de la función en caso de error  
            }

            bool cambiosRealizados = false; // Marca de cambios  

            for (int i = 0; i < Nombre.Count; i++)
            {
               // MessageBox.Show($"{Nombre[i]},{CantidadList[i]}");
                // Paso 2: Buscar el producto por nombre  
                var producto = listaProductos.FirstOrDefault(p => p.Nombre.Trim().Equals(Nombre[i].Trim(), StringComparison.OrdinalIgnoreCase));

                if (producto == null)
                {
                    MessageBox.Show($"Producto '{Nombre[i]}' no encontrado.");
                    continue; // Continúa al siguiente producto  
                }

                // Paso 3: Restar la cantidad  
                if (int.TryParse(CantidadList[i], out int cantidadARestar))
                {
                    if (producto.Cantidad >= cantidadARestar)
                    {
                        producto.Cantidad -= cantidadARestar;
                        cambiosRealizados = true; // Indicamos que se realizaron cambios  

               //         MessageBox.Show($"Se ha restado {cantidadARestar} de {producto.Nombre}. Nueva cantidad: {producto.Cantidad}");
                    }
                    else
                    {
                        MessageBox.Show($"No hay suficiente cantidad para restar de '{producto.Nombre}'. Cantidad disponible: {producto.Cantidad}");
                    }
                }
                else
                {
                    MessageBox.Show($"Cantidad no válida: {CantidadList[i]}");
                }
            }

            // Paso 4: Guardar los productos de vuelta en el archivo JSON  
            if (cambiosRealizados)
            {
                try
                {
                    string updatedJson = JsonConvert.SerializeObject(listaProductos, Formatting.Indented);
                    File.WriteAllText(ruta, updatedJson);
                    MessageBox.Show("Stock actualizado exitosamente.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al guardar el archivo: {ex.Message}");
                }
            }
        }
        //
        public List<ProductoArchivo> listaProductos { get; set; }
        public List<ArchivoIngresos> listaIngresos { get; set; }
        public List<ArchivosVentas> listaVentas { get; set; }
        public List<ArchivoLíquidoSuelto> listaLiquidos { get; set; }

        public List<string> NombreQ = new List<string>();
        public List<double> CantidadQ = new List<double>();
        //public List<string> NombreQ = new List<string>();
        public void RestarQuimico()
        {
            // Paso 1: Cargar los productos desde el archivo JSON  
            try
            {
              //  MessageBox.Show($"{Nombre.Count}, {CantidadList.Count}");
                string json = File.ReadAllText(ruta4);
                listaLiquidos = JsonConvert.DeserializeObject<List<ArchivoLíquidoSuelto>>(json);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al leer el archivo: {ex.Message}");
                return; // Salir de la función en caso de error  
            }

            bool cambiosRealizados = false; // Marca de cambios  

            for (int i = 0; i < Nombre.Count; i++)
            {
                if (Tipo[i] == "Químico")
                {

                //    MessageBox.Show($"{Nombre[i]},{CantidadList[i]}");
                    // Paso 2: Buscar el producto por nombre  
                    var producto = listaLiquidos.FirstOrDefault(p => p.Nombre.Equals(Nombre[i], StringComparison.OrdinalIgnoreCase));

                    if (producto == null)
                    {
                        MessageBox.Show($"Producto '{Nombre[i]}' no encontrado.");
                        continue; // Continúa al siguiente producto  
                    }

                    // Paso 3: Restar la cantidad  
                    if (double.TryParse(CantidadList[i], out double cantidadARestar))
                    {
                        if (producto.Cantidad >= cantidadARestar)
                        {
                            producto.Cantidad -= cantidadARestar;
                            cambiosRealizados = true; // Indicamos que se realizaron cambios  

                           // MessageBox.Show($"Se ha restado {cantidadARestar} de {producto.Nombre}. Nueva cantidad: {producto.Cantidad}");
                        }
                        else
                        {
                            MessageBox.Show($"No hay suficiente cantidad para restar de '{producto.Nombre}'. Cantidad disponible: {producto.Cantidad}");
                        }
                    }
                    else
                    {
                        MessageBox.Show($"Cantidad no válida: {CantidadList[i]}");
                    }
                }

                // Paso 4: Guardar los productos de vuelta en el archivo JSON  
                if (cambiosRealizados)
                {
                    try
                    {
                        string updatedJson = JsonConvert.SerializeObject(listaLiquidos, Formatting.Indented);
                        File.WriteAllText(ruta4, updatedJson);
                        MessageBox.Show("Stock actualizado exitosamente.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error al guardar el archivo: {ex.Message}");
                    }
                }
            }
        }
        public AccesoDatos()
        {
            listaProductos = new List<ProductoArchivo>();
            listaIngresos = new List<ArchivoIngresos>();
            listaVentas = new List<ArchivosVentas>();
            listaLiquidos = new List<ArchivoLíquidoSuelto>();
        }
        public void GuardarDatos(string ruta)
        {
            try
            {
                // Asegúrate de que la lista no contenga productos con valores vacíos  
                string json = JsonConvert.SerializeObject(listaProductos, Formatting.Indented);
                File.WriteAllText(ruta, json);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar el archivo: {ex.Message}");
            }
            // Serializa la lista de productos a una cadena JSON  
            // string json = JsonConvert.SerializeObject(listaProductos, Formatting.Indented);
            //JsonConvert.SerializeObject: Este método convierte tu lista de objetos a una cadena de texto en formato JSON.
            //Formatting.Indented: Esto hace que la salida JSON sea más legible, añadiendo sangrías y saltos de línea.
            // Escribe la cadena JSON en el archivo especificado  
            // File.WriteAllText(ruta, json);
        }
        public void CargarDatosQuímicos(string ruta)
        {
            try
            {
                string json = File.ReadAllText(ruta);
                listaLiquidos = JsonConvert.DeserializeObject<List<ArchivoLíquidoSuelto>>(json);

                // Mostrar nombres de los productos cargados para depuración  
                foreach (var producto in listaLiquidos)
                {
                    // MessageBox.Show($"Producto cargado: {producto.Nombre}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los datos: {ex.Message}");
            }

        }
        public void CargarDatosIngresos(string ruta)
        {
            try
            {
                string json = File.ReadAllText(ruta);
                listaIngresos = JsonConvert.DeserializeObject<List<ArchivoIngresos>>(json);

                // Mostrar nombres de los productos cargados para depuración  
                foreach (var producto in listaIngresos)
                {
                    // MessageBox.Show($"Producto cargado: {producto.Nombre}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los datos: {ex.Message}");
            }

        }
        public void CargarDatosVentas(string ruta)
        {
            try
            {
                string json = File.ReadAllText(ruta);
                listaVentas = JsonConvert.DeserializeObject<List<ArchivosVentas>>(json);

                // Mostrar nombres de los productos cargados para depuración  
                foreach (var producto in listaVentas)
                {
                    // MessageBox.Show($"Producto cargado: {producto.Nombre}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los datos: {ex.Message}");
            }

        }
        public void CargarDatos(string ruta)
        {
            try
            {
                string json = File.ReadAllText(ruta);
                listaProductos = JsonConvert.DeserializeObject<List<ProductoArchivo>>(json);

                // Mostrar nombres de los productos cargados para depuración  
                foreach (var producto in listaProductos)
                {
                    // MessageBox.Show($"Producto cargado: {producto.Nombre}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los datos: {ex.Message}");
            }

        }
        public ProductoArchivo BuscarProducto(string code)
        {
            return listaProductos.FirstOrDefault(p => p.Codigo.Equals(code, StringComparison.OrdinalIgnoreCase));
        } 
        public ProductoArchivo BuscarCodigo(string code)
        {
            return listaProductos.FirstOrDefault(p => p.Codigo.Equals(code, StringComparison.OrdinalIgnoreCase));
        }
        public ProductoArchivo BuscarProductoWhitName(string name)
        {
            return listaProductos.FirstOrDefault(p => p.Nombre.Equals(name, StringComparison.OrdinalIgnoreCase));
        }
        public void CambiarProducto(ProductoArchivo productoExistente, string nuevoNombre)
        {
            if (productoExistente != null)
            {
                productoExistente.Nombre = nuevoNombre;
                // productoExistente.Vencimiento = nuevaFecha;
            }
        }

        public void CambiarPrecioCostoProducto(ProductoArchivo productoExistente, int nuevoPrecio)
        {
            if (productoExistente != null)
            {
                productoExistente.PreC = nuevoPrecio;
                // productoExistente.Vencimiento = nuevaFecha;
            }
        }
        public void CambiarPrecioProducto(ProductoArchivo productoExistente, int nuevoPrecio)
        {
            if (productoExistente != null)
            {
                productoExistente.PreV = nuevoPrecio;
                // productoExistente.Vencimiento = nuevaFecha;
            }
        }
        public void CambiarCantidadProducto(ProductoArchivo productoExistente, int nuevaCantidad)
        {
            if (productoExistente != null)
            {
                productoExistente.Cantidad = nuevaCantidad;
                // productoExistente.Vencimiento = nuevaFecha;
            }
        }
        public void CambiarTipoProducto(ProductoArchivo productoExistente, string nuevoTipo)
        {
            if (productoExistente != null)
            {
                productoExistente.Tipo = nuevoTipo;
                // productoExistente.Vencimiento = nuevaFecha;
            }
        }
        public void EliminarProducto(ProductoArchivo producto)
        {
            if (producto != null)
            {
                if (listaProductos.Remove(producto))
                {
                    // Confirmar que el producto fue eliminado  
                    MessageBox.Show($"Producto eliminado: {producto.Nombre}");
                    GuardarDatos(@"C:\Users\Netbook\Desktop\ArchivosQuimica.json");
                }
                else
                {
                    MessageBox.Show("El producto no se encontró en la lista.");
                }
            }
        }
        // Ingresos:
        public void GuardarDatos2(string ruta2)
        {
            try
            {
                // Asegúrate de que la lista no contenga productos con valores vacíos  
                string json = JsonConvert.SerializeObject(listaIngresos, Formatting.Indented);
                File.WriteAllText(ruta2, json);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar el archivo de modos de pago: {ex.Message}");
            }

        }
        public void GuardarDatos3(string ruta3, List<ArchivosVentas> listaVentas)
        {
            try
            {
                string json = JsonConvert.SerializeObject(listaVentas, Formatting.Indented);
                File.WriteAllText(ruta3, json);
                MessageBox.Show("Datos guardados exitosamente.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar el archivo a lista de ventas: {ex.Message}");
            }
        }
        public void GuardarListaQuímicos(string ruta4)
        {
            try
            {
                string json = JsonConvert.SerializeObject(listaLiquidos, Formatting.Indented);
                File.WriteAllText(ruta4, json);
              //  MessageBox.Show("Datos guardados exitosamente.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar el archivo: {ex.Message}");
            }
        }
        public void GuardarListaIngresos(string ruta2)
        {
            try
            {
                string json = JsonConvert.SerializeObject(listaIngresos, Formatting.Indented);
                File.WriteAllText(ruta2, json);
              // MessageBox.Show("Datos guardados exitosamente.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar el archivo: {ex.Message}");
            }
        }
        public void DatosDeVenta()
        {

            // Paso 1: Cargar los datos existentes desde el archivo JSON  
            List<ArchivosVentas> listaVentasExistentes = new List<ArchivosVentas>();
            if (File.Exists(ruta3))
            {
                try
                {
                    string jsonExistente = File.ReadAllText(ruta3);
                    listaVentasExistentes = JsonConvert.DeserializeObject<List<ArchivosVentas>>(jsonExistente) ?? new List<ArchivosVentas>();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al cargar el archivo existente: {ex.Message}");
                }
            }

            // Paso 2: Agregar nuevos registros  
            for (int i = 0; i < Nombre.Count; i++)
            {
                try
                {
                    var registro = new ArchivosVentas
                    {
                        NombreProducto = Nombre[i],
                        Tipo = Tipo[i],
                        Cantidad = int.Parse(CantidadList[i]),
                        PrecioVenta = int.Parse(PV[i]),
                        PrecioCompra = int.Parse(PC[i]),
                        Ganancia = int.Parse(Ganancia[i]),
                        Fecha = Fecha[i].ToString(),
                        Hora = Hora[i].ToString()
                    };
                    listaVentasExistentes.Add(registro); // Agregar a la lista existente  
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al crear el registro: {ex.Message}");
                }
            }

            // Paso 3: Guardar todos los datos de vuelta en el archivo JSON  
            GuardarDatos3(ruta3, listaVentasExistentes);
        //    MessageBox.Show($"{Nombre[0]}, {Tipo[0]}, {CantidadList[0]}, {PV[0]}, {PC[0]}, {Ganancia[0]}, {FechaYHora[0]}");
        }
        public void EliminarListas()
        {
            Nombre.Clear();
            Tipo.Clear();
            CantidadList.Clear();
            PV.Clear();
            PC.Clear();
            Ganancia.Clear();
            Fecha.Clear();
            Hora.Clear();
        }
        // Mètodos para filtrar nombres en datagridview
        public List<ProductoArchivo> CargarArchivo1(string ruta)
        {
            if (File.Exists(ruta))
            {
                var json = File.ReadAllText(ruta);
                return JsonConvert.DeserializeObject<List<ProductoArchivo>>(json) ?? new List<ProductoArchivo>();
            }
            return new List<ProductoArchivo>();
        }

        public List<ArchivoLíquidoSuelto> CargarArchivo2(string ruta)
        {
            if (File.Exists(ruta))
            {
                var json = File.ReadAllText(ruta);
                return JsonConvert.DeserializeObject<List<ArchivoLíquidoSuelto>>(json) ?? new List<ArchivoLíquidoSuelto>();
            }
            return new List<ArchivoLíquidoSuelto>();
        }
        public List<ArchivosVentas> CargarArchivo3(string ruta)
        {
            if (File.Exists(ruta))
            {
                var json = File.ReadAllText(ruta);
                return JsonConvert.DeserializeObject<List<ArchivosVentas>>(json) ?? new List<ArchivosVentas>();
            }
            return new List<ArchivosVentas>();
        }
    }
}
