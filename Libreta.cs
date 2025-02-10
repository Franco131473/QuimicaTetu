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
    public partial class _Libreta : Form
    {
        public bool banderaL = false;
        private AccesoDatos datosbase;
        private VentaManual vm;
        public _Libreta(AccesoDatos datosbase)
        {
            vm = new VentaManual();
            InitializeComponent();
            panel1.Visible = false;
            panel2.Visible = false;
            panel4.Visible = false;
            this.datosbase = datosbase;
            if (datosbase.Nombre.Count > 0)
            {
                panel1.Visible = true;
                //button2.Visible = true;
            }
            else
            {
                comboBox1.Visible = false;
                //button2.Visible = false;
            }
            CargarYMostrarProductos();
            SumarTotal();
           // Visivilidad();
        }

        private void Visivilidad()
        {
            if (banderaL == true)
            {
                comboBox1.Visible = true;
            }
            else
            {
                comboBox1.Visible = false;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            button1.Focus();
        }
        private void CargarYMostrarProductos()
        {
            //productos = new Productos();
            datosbase.CargarDatosLibreta(datosbase.ruta5);
            // Ordenar la lista antes de asignarla al DataGridView  
            //var listaOrdenada = datobase.listaP.OrderBy(item => item.Cantidad).ToList(); // Asegúrate que Cantidad es accesible  
            dataGridView1.DataSource = datosbase.listaLibreta;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            //datosbase = new AccesoDatos();
            MessageBox.Show(datosbase.Nombre.Count.ToString());
            datosbase.GuardarListaLibreta(comboBox1.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            EliminarElementos(comboBox1.Text, datosbase.ruta5);
        }

        public void EliminarElementos(string nombreElemento, string rutaArchivoJson)
        {
            // Leer el archivo JSON
            string json = File.ReadAllText(rutaArchivoJson);
            List<ArchivoLibreta> items = JsonConvert.DeserializeObject<List<ArchivoLibreta>>(json);

            // Filtrar los elementos que no cumplen con la condición
            List<ArchivoLibreta> itemsFiltrados = items.FindAll(item => item.Cliente != nombreElemento);

            // Guardar los cambios en el archivo JSON
            string nuevoJson = JsonConvert.SerializeObject(itemsFiltrados, Formatting.Indented);
            File.WriteAllText(rutaArchivoJson, nuevoJson);
        }

        private void _Libreta_Load(object sender, EventArgs e)
        {
           // Visivilidad();
        }
        bool activo = false;
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            activo = true;
            string nombreRecibido = comboBox2.Text;
            var listaArchivo4 = datosbase.CargarArchivo4(datosbase.ruta5);
            List<ResultadoFiltrado> resultadoFiltrados;
            var resultadoNombre = listaArchivo4
                .Where(a => a.Cliente.Equals(nombreRecibido, StringComparison.OrdinalIgnoreCase))
                        .Select(a => new ResultadoFiltrado
                        {
                            Cliente = a.Cliente,
                            NombreProducto = a.NombreProducto,
                            Tipo = a.Tipo,
                            Cantidad = a.Cantidad,
                            PrecioVenta = a.PrecioVenta,
                            PrecioCompra = a.PrecioCompra,
                            Ganancia = a.Ganancia,
                            Fecha = a.Fecha,
                            Hora = a.Hora
                        })
                        .ToList();
            resultadoFiltrados = resultadoNombre;
            dataGridView1.DataSource = resultadoFiltrados;
            SumarTotal();
            textBox3.Enabled = true;
            textBox3.ReadOnly = true;
        }

        public class ResultadoFiltrado
        {
            public string Cliente { get; set; }
            public string NombreProducto { get; set; }
            public string Tipo { get; set; }
            public int Cantidad { get; set; }
            public int PrecioVenta { get; set; }
            public int PrecioCompra { get; set; }
            public int Ganancia { get; set; }
            public string Fecha { get; set; }
            public string Hora { get; set; }
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 4 && e.Value != null)
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
                    string precioTexto = fila.Cells[4].Value?.ToString();
                    string gananciaTexto = fila.Cells[6].Value?.ToString();

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
            if (!activo)
            {
                label1.Text = acumuladorTotal.ToString("C");
                label2.Text = acumuladorGanancia.ToString("C");
            }
            else
            {
                textBox3.Text = acumuladorTotal.ToString("C");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            panel2.Visible = true;
            textBox1.Focus();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string newItem = textBox1.Text;

            // Verifica que el TextBox no esté vacío
            if (!string.IsNullOrWhiteSpace(newItem))
            {
                // Agrega el nuevo ítem al ComboBox
                comboBox1.Items.Add(newItem);
                comboBox2.Items.Add(newItem);
                // Limpia el TextBox
                textBox1.Clear();
                panel2.Visible = false;
                MessageBox.Show("Agregado Correctamente");
            }
            else
            {
                MessageBox.Show("Por favor, ingresa un ítem válido.");

            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                var listaLib = new ArchivoLibreta
                {
                    Cliente = comboBox3.Text,
                    NombreProducto = "Efectivo",
                    Tipo = "Prestamo",
                    Cantidad = int.Parse(textBox4.Text),
                    PrecioVenta = int.Parse(textBox4.Text),
                    PrecioCompra = int.Parse(textBox4.Text),
                    Ganancia = 0,
                    Fecha = label5.Text,
                    Hora = label4.Text
                };
                datosbase.listaLibreta.Add(listaLib);
                datosbase.GuardarListaLibreta(datosbase.ruta5);
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Error al guardar el archivo: {ex.Message}");
            }
            textBox4.Clear();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label4.Text = DateTime.Now.ToShortTimeString();
            label5.Text = DateTime.Now.ToShortDateString();
        }
    }
}
