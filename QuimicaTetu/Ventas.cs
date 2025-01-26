using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuimicaTetu
{
    public partial class Ventas : Form
    {
        private VentaManual ventana;
        AccesoDatos datosbase;
        private CAJA caja;
        private Image[] images;
        private string[] nombreImagen;
        private int currentImageIndex = 0;
        public Ventas()
        {
            InitializeComponent();
            this.KeyPreview = true;
            ventana = new VentaManual();
            //Ver monto de la caja
            caja = new CAJA();
            label1.Text = $"{caja.DineroActual.ToString("C")}";
            label8.Visible = false;
            panel1.Visible = false;
            datosbase = new AccesoDatos();
            images = new Image[]
            {
                Properties.Resources.mil,
                Properties.Resources.mercadopago1,
                Properties.Resources.uniones2
            };
            nombreImagen = new string[]
            {
                "Efectivo",
                "Transferencia",
                "División"
            };
            pictureBox1.Image = images[currentImageIndex];
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            label3.Visible = false;
        }

        private void textBox1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {

            if (e.KeyCode == Keys.Enter) {

                //datosbase = new AccesoDatos();
                // Cargar productos  
                datosbase.CargarDatos(datosbase.ruta);
                // Mostrar cuántos productos se han cargado  
                // MessageBox.Show($"Total de productos cargados: {datosbase.listaProductos.Count}");

                string codigoBuscado = textBox1.Text;
                ProductoArchivo codigoEncontrado = datosbase.BuscarCodigo(codigoBuscado);
                if (codigoBuscado != null)
                {
                    //textBox1.Clear();
                    lblNombre.Text = codigoEncontrado.Nombre;
                    lblTipo.Text = codigoEncontrado.Tipo;
                    lblStock.Text = codigoEncontrado.Cantidad.ToString();
                    lblPrecio.Text = codigoEncontrado.PreV.ToString();
                    label8.Text = codigoEncontrado.PreC.ToString();
                    textBox5.Focus();
                    textBox1.Clear();
                }
                else
                {
                    MessageBox.Show("Producto no encontrado.");
                }
            }
        }

        private void textBox5_KeyDown(object sender, KeyEventArgs e)
        {
            //datosbase = new AccesoDatos();
            if (e.KeyCode == Keys.Enter) {
                ListViewItem item = new ListViewItem(lblNombre.Text);
                item.SubItems.Add(lblPrecio.Text);
                item.SubItems.Add(textBox5.Text);
                int precioTotal = Convert.ToInt32(textBox5.Text) * Convert.ToInt32(lblPrecio.Text);
                item.SubItems.Add(precioTotal.ToString());
                listView1.Items.Add(item);
                // Añadir nombre y cantidad para restar stock y para hacer el registro de Ventas
                datosbase.Nombre.Add(lblNombre.Text);
                datosbase.CantidadList.Add(textBox5.Text);
                datosbase.Tipo.Add(lblTipo.Text);
                datosbase.PV.Add(lblPrecio.Text);
                datosbase.PC.Add(label8.Text);
                int ganancia = (Convert.ToInt32(lblPrecio.Text) - Convert.ToInt32(label8.Text)) * Convert.ToInt32(textBox5.Text);
                datosbase.Ganancia.Add(ganancia.ToString());
               // datosbase.FechaYHora.Add($"{lblHora.Text} / {lblFecha.Text}");
                // MessageBox.Show(datosbase.FechaYHora[0]);
                // Añadir el elemento al ListView  


                // Opcional: Limpiar el TextBox después de agregar  
                textBox5.Clear();
                textBox1.Focus();
                //e.SuppressKeyPress = true; // Evita el sonido de la tecla Enter
            }
            SumaColumna();
        }
        private void SumaColumna()
        {
            int total = 0; // Variable para acumular la suma  

            // Iteramos a través de los items del ListView  
            foreach (ListViewItem item in listView1.Items)
            {
                // Intentamos convertir el valor de la columna a decimal y sumarlo  
                if (int.TryParse(item.SubItems[3].Text, out int value))
                {
                    total += value; // Acumulamos el valor  
                }
            }
            // Actualizamos el Label  
            lblTotal.Text = total.ToString("C");
            label3.Text = total.ToString();
            //ca.ActualizandoCaja(total);           
        }
        private void Ventas_Load(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }
        int efectivo;
        private void button1_Click(object sender, EventArgs e)
        {
            datosbase.RestarCantidad();
            datosbase.DatosDeVenta();

            //Jsons
            if (pictureBox1.Image == images[0])
            {
                try
                {
                    // Paso 1: Cargar la lista existente desde el archivo JSON  
                    if (File.Exists(datosbase.ruta2))
                    {
                        string jsonExistente = File.ReadAllText(datosbase.ruta2);
                        datosbase.listaIngresos = JsonConvert.DeserializeObject<List<ArchivoIngresos>>(jsonExistente) ?? new List<ArchivoIngresos>();
                    }
                    else
                    {
                        datosbase.listaIngresos = new List<ArchivoIngresos>(); // Iniciar una nueva lista si el archivo no existe  
                    }
                    int monto;
                    if (!int.TryParse(label3.Text, out monto))
                    {
                        MessageBox.Show("El monto debe ser un número válido.");
                        return;
                    }

                    var nuevoProducto = new ArchivoIngresos
                    {
                        ModoDePago = "Efectivo",
                        Fecha = lblFecha.Text, // Asegúrate de que tiene un formato válido  
                        Hora = lblHora.Text, // Asegúrate de que tiene un formato válido  
                        Efectivo = monto.ToString(),
                        Monto = monto,
                    };
                    datosbase.listaIngresos.Add(nuevoProducto);
                    datosbase.GuardarListaIngresos(datosbase.ruta2);
                    caja.ActualizarCaja(monto);
                    label1.Text = caja.DineroActual.ToString("C");
                    // CargarYMostrarProductos();  
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al guardar el archivo: {ex.Message}\n{ex.StackTrace}");
                }
                datosbase.EliminarListas();
            }
            else
            {
                if (pictureBox1.Image == images[1])
                {
                    try
                    {
                        // Paso 1: Cargar la lista existente desde el archivo JSON  
                        if (File.Exists(datosbase.ruta2))
                        {
                            string jsonExistente = File.ReadAllText(datosbase.ruta2);
                            datosbase.listaIngresos = JsonConvert.DeserializeObject<List<ArchivoIngresos>>(jsonExistente) ?? new List<ArchivoIngresos>();
                        }
                        else
                        {
                            datosbase.listaIngresos = new List<ArchivoIngresos>(); // Iniciar una nueva lista si el archivo no existe  
                        }
                        int monto;
                        if (!int.TryParse(label3.Text, out monto))
                        {
                            MessageBox.Show("El monto debe ser un número válido.");
                            return;
                        }
                        var nuevoProducto = new ArchivoIngresos
                        {
                            ModoDePago = "Transferencia",
                            Fecha = lblFecha.Text, //Convert.ToInt32(textBoxCantidad.Text),
                            Hora = lblHora.Text,
                            Transferencia = monto.ToString(),
                            //Efectivo = "-",
                            Monto = monto,
                        };
                        datosbase.listaIngresos.Add(nuevoProducto);
                        datosbase.GuardarListaQuímicos(datosbase.ruta2);// Guardar los datos después de agregar
                        //caja.ActualizarCaja(int.Parse(lblTotal.Text));
                        // CargarYMostrarProductos();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error al guardar el archivo: {ex.Message}");
                    }
                }
                else
                {
                    panel1.Visible = true;
                    textBox2.Focus();
                }
            }
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            currentImageIndex = (currentImageIndex + 1) % images.Length;
            pictureBox1.Image = images[currentImageIndex];
        }

        private void HoraFecha_Tick(object sender, EventArgs e)
        {
            lblHora.Text = DateTime.Now.ToShortTimeString();
            lblFecha.Text = DateTime.Now.ToShortDateString();
        }


        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            

        }
        private void button2_Click(object sender, EventArgs e)
        {
            VentaManual ventana = new VentaManual();
            ventana.Show();
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                // Paso 1: Cargar la lista existente desde el archivo JSON  
                if (File.Exists(datosbase.ruta2))
                {
                    string jsonExistente = File.ReadAllText(datosbase.ruta2);
                    datosbase.listaIngresos = JsonConvert.DeserializeObject<List<ArchivoIngresos>>(jsonExistente) ?? new List<ArchivoIngresos>();
                }
                else
                {
                    datosbase.listaIngresos = new List<ArchivoIngresos>(); // Iniciar una nueva lista si el archivo no existe  
                }
                int monto;
                if (!int.TryParse(label3.Text, out monto))
                {
                    MessageBox.Show("El monto debe ser un número válido.");
                    return;
                }
                var nuevoProducto = new ArchivoIngresos
                {
                    ModoDePago = "División",
                    Fecha = lblFecha.Text, //Convert.ToInt32(textBoxCantidad.Text),
                    Hora = lblHora.Text,
                    //Transferencia = monto.ToString(),
                    Efectivo = textBox2.Text,
                    Monto = monto,
                    Transferencia = (monto - Convert.ToInt32(textBox2.Text)).ToString()
                };
                datosbase.listaIngresos.Add(nuevoProducto);
                datosbase.GuardarListaIngresos(datosbase.ruta2);// Guardar los datos después de agregar
                caja.ActualizarCaja(Convert.ToInt32(textBox2.Text));
                textBox2.Clear();
                panel1.Visible = true;
                // CargarYMostrarProductos();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar el archivo: {ex.Message}");
            }
        
        lblTotal.Text = $"${0}";
            panel1.Visible = false;
            datosbase.EliminarListas();
            textBox1.Focus();
            listView1.Clear();
        }
        // Datos para ventas:
        // Nombre, Cantidad, ModoPago, Efectivo, 

    }
}
