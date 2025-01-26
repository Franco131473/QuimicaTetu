using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuimicaTetu
{
    public partial class Químicos : Form
    {
        private AccesoDatos datosbase;
        public Químicos()
        {
            InitializeComponent();
            datosbase = new AccesoDatos();
            this.KeyPreview = true;
            label5.Visible = false;
            textBox5.Visible = false;
            CargarYMostrarProductos();
            dataGridView1.Visible = false;
            SumarTotal();
            panel2.Visible = false;
        }
        private void CargarYMostrarProductos()
        {
            //productos = new Productos();
            datosbase.CargarDatosQuímicos(datosbase.ruta4);
            //datosbase.CargarDatosIngresos(datosbase.ruta2);
            // Vincular la lista de productos al DataGridView  
            dataGridView1.DataSource = datosbase.listaLiquidos;
            //dataGridView2.DataSource = ac.listaIngresos;
            // dataGridView1.Columns[4] 
        }
        private void Químicos_Load(object sender, EventArgs e)
        {
            //if(textBox3.Text != null && textBox4.Text != null)
            //{
            //    textBox5.Text = (Convert.ToInt32(textBox4.Text) - Convert.ToInt32(textBox3.Text)).ToString();
            //}
        }

        private void button1_Click(object sender, EventArgs e)
        { 
            //Json
            try
            {
                // Paso 1: Cargar la lista existente desde el archivo JSON  
                if (File.Exists(datosbase.ruta4))
                {
                    string jsonExistente = File.ReadAllText(datosbase.ruta4);
                    datosbase.listaLiquidos = JsonConvert.DeserializeObject<List<ArchivoLíquidoSuelto>>(jsonExistente) ?? new List<ArchivoLíquidoSuelto>();
                }
                else
                {
                    datosbase.listaLiquidos = new List<ArchivoLíquidoSuelto>(); // Iniciar una nueva lista si el archivo no existe  
                }
                if (comboBox1.Text == "INGRESAR QUIMICOS")
                {
                    var nuevoProducto = new ArchivoLíquidoSuelto
                    {

                        //Codigo = textBoxCodigo.Text,
                        Nombre = textBox1.Text,
                        //Tipo = textBoxTipo.Text,
                        Cantidad = Convert.ToDouble(textBox2.Text),
                        PrecioCompra = Convert.ToInt32(textBox3.Text),
                        Precio = Convert.ToInt32(textBox4.Text),
                        GananciaXLitro = Convert.ToInt32(textBox4.Text) - Convert.ToInt32(textBox3.Text),
                        IngresoAproximado = Convert.ToInt32(textBox4.Text) * Convert.ToInt32(textBox2.Text) / 1000,
                        GananciaPotencialAproximada = (Convert.ToInt32(textBox4.Text) - Convert.ToInt32(textBox3.Text)) * Convert.ToInt32(textBox2.Text) / 1000,
                        Tipo = "Químico",

                    };
                    datosbase.listaLiquidos.Add(nuevoProducto);
                    datosbase.GuardarListaQuímicos(datosbase.ruta4);// Guardar los datos después de agregar
                }
                else
                {
                    var nuevoProducto = new ArchivoLíquidoSuelto
                    {

                        //Codigo = textBoxCodigo.Text,
                        Nombre = textBox1.Text,
                        //Tipo = textBoxTipo.Text,
                        Cantidad = Convert.ToDouble(textBox2.Text),
                        PrecioCompra = Convert.ToInt32(textBox3.Text),
                        Precio = Convert.ToInt32(textBox4.Text),
                        GananciaXLitro = Convert.ToInt32(textBox5.Text),
                        IngresoAproximado = 0,
                        GananciaPotencialAproximada = 0,
                        Tipo = "Oferta",

                    };
                    datosbase.listaLiquidos.Add(nuevoProducto);
                    datosbase.GuardarListaQuímicos(datosbase.ruta4);
                }
                
                                                                //CargarYMostrarProductos();
                textBox1.Clear();
                textBox2.Clear();
                textBox3.Clear();
                textBox4.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar el archivo: {ex.Message}");
            }
        }
        

        private void Químicos_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.C)
            {
                textBox1.Focus();
            }
            if (e.KeyCode == Keys.F)
            {
                button1.Focus();
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text == "INGRESAR OFERTAS")
            {
                label5.Visible = true;
                textBox5.Visible = true;
            }
            else
            {
                label5.Visible = false;
                textBox5.Visible = false;
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            this.KeyPreview = false;
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            this.KeyPreview = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            panel1.Visible = false;
            dataGridView1.Visible = true;
            panel2.Visible = true;
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Verificar si estamos formateando la columna deseada (por ejemplo, la primera columna)
            if (e.ColumnIndex == 2 && e.Value != null)
            {
                if (int.TryParse(e.Value.ToString(), out int value))
                {
                    // Formatear el valor como "1.000,00"
                    e.Value = value.ToString("C", new CultureInfo("es-AR")); e.FormattingApplied = true;
                }
            }
            if (e.ColumnIndex == 3 && e.Value != null)
            {
                if (int.TryParse(e.Value.ToString(), out int value))
                {
                    // Formatear el valor como "1.000,00"
                    e.Value = value.ToString("C", new CultureInfo("es-AR")); e.FormattingApplied = true;
                }
            }
            if (e.ColumnIndex == 5 && e.Value != null)
            {
                if (int.TryParse(e.Value.ToString(), out int value))
                {
                    // Formatear el valor como "1.000,00"
                    e.Value = value.ToString("C", new CultureInfo("es-AR")); e.FormattingApplied = true;
                }
            }
            if (e.ColumnIndex == 6 && e.Value != null)
            {
                if (int.TryParse(e.Value.ToString(), out int value))
                {
                    // Formatear el valor como "1.000,00"
                    e.Value = value.ToString("C", new CultureInfo("es-AR")); e.FormattingApplied = true;
                }
            }
            if (e.ColumnIndex == 7 && e.Value != null)
            {
                if (int.TryParse(e.Value.ToString(), out int value))
                {
                    // Formatear el valor como "1.000,00"
                    e.Value = value.ToString("C", new CultureInfo("es-AR")); e.FormattingApplied = true;
                }
            }
        }
        private void SumarTotal()
        {
            int acumuladorTotal = 0;
            int acumuladorGanancia = 0;
            foreach (DataGridViewRow fila in dataGridView1.Rows)
            {
                if (!fila.IsNewRow)
                {
                    // Extraer y limpiar el valor de la celda (8 y 9)  
                    string precioTexto = fila.Cells[6].Value?.ToString();
                    string gananciaTexto = fila.Cells[7].Value?.ToString();

                    // Limpiar el string para convertirlo a un número  
                    if (!string.IsNullOrEmpty(precioTexto))
                    {
                        precioTexto = precioTexto.Replace(".", "").Replace(",", "");
                        if (int.TryParse(precioTexto, out int precio))
                        {
                            acumuladorTotal += precio;
                        }
                    }

                    if (!string.IsNullOrEmpty(gananciaTexto))
                    {
                        gananciaTexto = gananciaTexto.Replace(".", "").Replace(",", "");
                        if (int.TryParse(gananciaTexto, out int ganancia))
                        {
                            acumuladorGanancia += ganancia;
                        }
                    }
                }
            }

            label7.Text = acumuladorTotal.ToString("C");
            label8.Text = acumuladorGanancia.ToString("C");
        }
    }
}
