using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace QuimicaTetu
{
    public partial class Configurar_Productos : Form
    {
        //private Productos produc;
        AccesoDatos datosbase;
        public Configurar_Productos()
        {
            InitializeComponent();
            panel2.Visible = false;
            datosbase = new AccesoDatos();
            comboBox1.Visible = false;
            textBox1.Focus();
            this.KeyPreview = true;
        }

        private void Configurar_Productos_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
                //string rutaArchivo = @"C:\Users\Netbook\Desktop\ArchivosQuimica.json";
                //datosbase = new AccesoDatos();

                // Cargar productos  
                datosbase.CargarDatos(datosbase.ruta);

                // Mostrar cuántos productos se han cargado  
                // MessageBox.Show($"Total de productos cargados: {datosbase.listaProductos.Count}");

                string nombreBuscado = textBox1.Text;
                ProductoArchivo productoEncontrado = datosbase.BuscarProductoWhitName(nombreBuscado);

                if (productoEncontrado != null)
                {
                    //label10.Text = productoEncontrado.Codigo;
                    label2.Text = productoEncontrado.Nombre;
                    label3.Text = productoEncontrado.Tipo;
                    label4.Text = productoEncontrado.Cantidad.ToString();
                    label5.Text = productoEncontrado.PreC.ToString("C");
                    label6.Text = productoEncontrado.PreV.ToString("C");
                    label7.Text = productoEncontrado.Vencimiento.ToString("yyyy-MM-dd");
                }
                else
                {
                    MessageBox.Show("Producto no encontrado.");
                }           
        }

        private void button2_Click(object sender, EventArgs e)
        {
            panel2.Visible = true;
            label8.Text = "Nombre Actual";
            label9.Text = "Nombre Nuevo";
            textBox2.Text = label2.Text;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            panel2.Visible = true;
            label8.Text = "Cantidad Actual";
            label9.Text = "Cantidad Nuevo";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            panel2.Visible = true;
            textBox2.Text = label6.Text;
            label8.Text = "Precio Actual";
            label9.Text = "Precio Nuevo";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (label8.Text == "Nombre Actual")
            {
                datosbase.CargarDatos(datosbase.ruta);

                string nombreBuscado = label2.Text; // Asume que tienes un TextBox para la búsqueda  
                ProductoArchivo productoEncontrado = datosbase.BuscarProductoWhitName(nombreBuscado);

                if (productoEncontrado != null)
                {
                    string nuevoNombre = textBox3.Text; // Nuevo nombre a actualizar  

                    datosbase.CambiarProducto(productoEncontrado, nuevoNombre);
                    datosbase.GuardarDatos(datosbase.ruta); // Guarda los cambios en el archivo
                    MessageBox.Show($"El producto {nuevoNombre} fue actualizado correctamente.");
                }
                else
                {
                    MessageBox.Show("Producto no encontrado.");
                }
                panel2.Visible = false;
                textBox1.Focus();
                textBox3.Clear();
            }
            else if (label8.Text == "Precio Actual")
            {

                datosbase.CargarDatos(datosbase.ruta);

                string nombreBuscado = label2.Text; // Asume que tienes un TextBox para la búsqueda  
                MessageBox.Show($"{nombreBuscado}");
                ProductoArchivo productoEncontrado = datosbase.BuscarProductoWhitName(nombreBuscado);
                MessageBox.Show($"{productoEncontrado}");
                if (productoEncontrado != null)
                {
                    int nuevoPrecio = Convert.ToInt32(textBox3.Text); // Nuevo nombre a actualizar  

                    datosbase.CambiarPrecioProducto(productoEncontrado, nuevoPrecio);
                    datosbase.GuardarDatos(datosbase.ruta); // Guarda los cambios en el archivo
                    MessageBox.Show($"El precio {nuevoPrecio} fue actualizado correctamente.");
                }
                else
                {
                    MessageBox.Show("Producto no encontrado.");
                }
                panel2.Visible = false;
                textBox1.Focus();
                textBox3.Clear();
            }
            else if (label8.Text == "Cantidad Actual")
            {
                datosbase.CargarDatos(datosbase.ruta);

                string nombreBuscado = label2.Text; // Asume que tienes un TextBox para la búsqueda  
                MessageBox.Show($"{nombreBuscado}");
                ProductoArchivo productoEncontrado = datosbase.BuscarProductoWhitName(nombreBuscado);
                MessageBox.Show($"{productoEncontrado}");
                if (productoEncontrado != null)
                {
                    int nuevaCantidad = Convert.ToInt32(textBox3.Text); // Nuevo nombre a actualizar  

                    datosbase.CambiarCantidadProducto(productoEncontrado, nuevaCantidad);
                    datosbase.GuardarDatos(datosbase.ruta); // Guarda los cambios en el archivo
                    MessageBox.Show($"El precio {nuevaCantidad} fue actualizado correctamente.");
                }
                else
                {
                    MessageBox.Show("Producto no encontrado.");
                }
                panel2.Visible = false;
                textBox1.Focus();
                textBox3.Clear();
            }
            else if (label8.Text == "Precio Costo Actual")
            {
                datosbase.CargarDatos(datosbase.ruta);

                string nombreBuscado = label2.Text; // Asume que tienes un TextBox para la búsqueda  
                MessageBox.Show($"{nombreBuscado}");
                ProductoArchivo productoEncontrado = datosbase.BuscarProductoWhitName(nombreBuscado);
                MessageBox.Show($"{productoEncontrado}");
                if (productoEncontrado != null)
                {
                    int nuevoPrecio = Convert.ToInt32(textBox3.Text); // Nuevo nombre a actualizar  

                    datosbase.CambiarPrecioCostoProducto(productoEncontrado, nuevoPrecio);
                    datosbase.GuardarDatos(datosbase.ruta); // Guarda los cambios en el archivo
                    MessageBox.Show($"El precio {nuevoPrecio} fue actualizado correctamente.");
                }
                else
                {
                    MessageBox.Show("Producto no encontrado.");
                }
                panel2.Visible = false;
                textBox1.Focus();
                textBox3.Clear();
            }
            else if (label8.Text == "Tipo Actual")
            {
                datosbase.CargarDatos(datosbase.ruta);

                string nombreBuscado = label2.Text; // Asume que tienes un TextBox para la búsqueda  
               // MessageBox.Show($"{nombreBuscado}");
                ProductoArchivo productoEncontrado = datosbase.BuscarProductoWhitName(nombreBuscado);
               // MessageBox.Show($"{productoEncontrado}");
                if (productoEncontrado != null)
                {
                    string nuevoTipo = comboBox1.Text; // Nuevo nombre a actualizar  

                    datosbase.CambiarTipoProducto(productoEncontrado, nuevoTipo);
                    datosbase.GuardarDatos(datosbase.ruta); // Guarda los cambios en el archivo
                    MessageBox.Show($"El precio {nuevoTipo} fue actualizado correctamente.");
                }
                else
                {
                    MessageBox.Show("Producto no encontrado.");
                }
                panel2.Visible = false;
                textBox1.Focus();
                textBox1.Clear();
                textBox3.Clear();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            panel2.Visible = true;
            label8.Text = "Producto a Eliminar:";
            label9.Visible = false;
            textBox3.Visible = false;
            textBox2.Text = textBox1.Text;
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                //string rutaArchivo = @"C:\Users\Netbook\Desktop\ArchivosQuimica.json";
                datosbase = new AccesoDatos();

                // Cargar productos  
                datosbase.CargarDatos(datosbase.ruta);

                // Mostrar cuántos productos se han cargado  
             //   MessageBox.Show($"Total de productos cargados: {datosbase.listaProductos.Count}");

                string codigoBuscado = textBox1.Text;
                ProductoArchivo productoEncontrado = datosbase.BuscarProducto(codigoBuscado);

                if (productoEncontrado != null)
                {
                    // label10.Text = productoEncontrado.Codigo;
                    label2.Text = productoEncontrado.Nombre;
                    label3.Text = productoEncontrado.Tipo;
                    label4.Text = productoEncontrado.Cantidad.ToString();
                    label5.Text = productoEncontrado.PreC.ToString("C");
                    label6.Text = productoEncontrado.PreV.ToString("C");
                    label7.Text = productoEncontrado.Vencimiento.ToString("yyyy-MM-dd");
                }
                else
                {
                    MessageBox.Show("Producto no encontrado.");
                }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            panel2.Visible = true;
            comboBox1.Visible = true;
            comboBox1.Focus();
            textBox2.Text = label6.Text;
            textBox3.Visible = false;
            label8.Text = "Tipo Actual";
            label9.Text = "Tipo Nuevo";
        }

        private void button8_Click(object sender, EventArgs e)
        {
            panel2.Visible = true;
            textBox2.Text = label5.Text;
            label8.Text = "Precio Costo Actual";
            label9.Text = "Precio Nuevo";
            textBox3.Focus();
        }

        private void Configurar_Productos_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                textBox1.Focus();
                textBox1.Clear();
            }
            if (e.KeyCode == Keys.F2)
            {
                button7.Focus();
                //textBox1.Clear();
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            button5.Focus();
        }
    }
    }
