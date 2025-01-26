using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuimicaTetu
{
    public partial class Datos_Ventas : Form
    {
        private AccesoDatos ac;
        public string ruta3 = @"C:\Users\Netbook\Desktop\ArchivosQuimicaVentasTotal.json";
        private DataTable dataTable;
        private System.Drawing.Point posicioninicialbuton3;
        private System.Drawing.Point posicioninicialbuton4;
        private System.Drawing.Point posicioninicialPanel;
        public Datos_Ventas()
        {
            InitializeComponent();
            ac = new AccesoDatos();
            CargarYMostrarProductos();
            SumarTotal();
            SumarTotalIngresos();
            label1.Visible = false;
            dataGridView2.Visible = false;
            panel1.Visible = false;
            label8.Visible = false;
            posicioninicialbuton3 = button3.Location;
            posicioninicialbuton4 = button4.Location;
            posicioninicialPanel = panel1.Location;            
        }
        private void CargarYMostrarProductos()
        {
            //productos = new Productos();
            ac.CargarDatosVentas(ruta3);
            ac.CargarDatosIngresos(ac.ruta2);
            // Vincular la lista de productos al DataGridView  
            dataGridView1.DataSource = ac.listaVentas;
            dataGridView2.DataSource = ac.listaIngresos;
            SumarColumnaGanancia();
            label10.Text = dataGridView1.RowCount.ToString();
            // dataGridView1.Columns[4] 
            // Hacer scroll a la última fila  
            //   dataGridView1.FirstDisplayedScrollingRowIndex = dataGridView1.Rows.Count - 1;
        }

        private void dataGridView1_CellFormatting_1(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Verificar si estamos formateando la columna deseada (por ejemplo, la primera columna)
            if (e.ColumnIndex == 3 && e.Value != null)
            {
                if (int.TryParse(e.Value.ToString(), out int value))
                {
                    // Formatear el valor como "1.000,00"
                    e.Value = value.ToString("C", new CultureInfo("es-AR")); e.FormattingApplied = true;
                }
            }
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
                    string gananciaTexto = fila.Cells[5].Value?.ToString();

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

            label1.Text = acumuladorTotal.ToString("C");
            label2.Text = acumuladorGanancia.ToString("C");
        }
        private void SumarTotalIngresos()
        {
            int acumuladorTotal = 0;
            int acumuladorGanancia = 0;
            foreach (DataGridViewRow fila in dataGridView2.Rows)
            {
                if (!fila.IsNewRow)
                {
                    // Extraer y limpiar el valor de la celda (8 y 9)  
                    string precioTexto = fila.Cells[2].Value?.ToString();
                    string gananciaTexto = fila.Cells[3].Value?.ToString();

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

            label5.Text = acumuladorTotal.ToString("C");
            label7.Text = acumuladorGanancia.ToString("C");
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text == "Ver Modos De Pago")
            {
                dataGridView1.Visible = false;
                dataGridView2.Visible = true;
                button1.Text = "Ver Datos De Pago";
            }
            else
            {
                button1.Text = "Ver Modos De Pago";
                dataGridView1.Visible = true;
                dataGridView2.Visible = false;
            }

        }

        private void dataGridView2_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Verificar si estamos formateando la columna deseada (por ejemplo, la primera columna)
            if (e.ColumnIndex == 1 && e.Value != null)
            {
                if (int.TryParse(e.Value.ToString(), out int value))
                {
                    // Formatear el valor como "1.000,00"
                    e.Value = value.ToString("C", new CultureInfo("es-AR")); e.FormattingApplied = true;
                }
            }
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
        }
        private void MostrarUltimaFila()
        {
            dataGridView1.CurrentCell = dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[0];
            dataGridView1.FirstDisplayedScrollingRowIndex = dataGridView1.Rows.Count - 1;
        }
        private void Datos_Ventas_Load(object sender, EventArgs e)
        {

            // Mover el cursor a la última fila  
            dataGridView1.CurrentCell = dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[0];
            dataGridView1.FirstDisplayedScrollingRowIndex = dataGridView1.Rows.Count - 1; // También desplaza el scroll a la última fila  

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label8.Text = DateTime.Now.ToShortDateString();
        }
        bool bandera1 = false;
        private void button2_Click(object sender, EventArgs e)
        {
            if (bandera1 == false)
            {
                button4.Location = new System.Drawing.Point(788, 240);
                button3.Location = new System.Drawing.Point(788, 203);
                panel1.Location = new System.Drawing.Point(788, 109);
                panel1.Visible = true;

                var listaArchivo3 = ac.CargarArchivo3(ac.ruta3);
                string fechaRecibida = label8.Text;
                List<ResultadoFiltrado> resultadosFiltrados;
                var resultadoFecha = listaArchivo3
                        .Where(a => a.Fecha.Equals(fechaRecibida, StringComparison.OrdinalIgnoreCase))
                        .Select(a => new ResultadoFiltrado
                        {
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
                resultadosFiltrados = resultadoFecha;
                dataGridView1.DataSource = resultadosFiltrados;
                SumarColumnaGanancia();
                //FiltrarPorFecha(label8.Text);
                label10.Text = dataGridView1.RowCount.ToString();
                bandera1 = true;
                MostrarUltimaFila();
                button3.Enabled = false;
                button4.Enabled = false;
            }
            else
            {
                button3.Location = posicioninicialbuton3;
                button4.Location = posicioninicialbuton4;
                bandera1 = false;
                panel1.Visible = false;
                button3.Enabled = true;
                button4.Enabled = true;
            }
        }
        
        public class ResultadoFiltrado
        {
            public string NombreProducto { get; set; }
            public string Tipo { get; set; }
            public int Cantidad { get; set; }
            public int PrecioVenta { get; set; }
            public int PrecioCompra { get; set; }
            public int Ganancia { get; set; }
            public string Fecha { get; set; }
            public string Hora { get; set; }
        }
        private void SumarColumnaGanancia()
        {
            
            int acumuladorGanancia = 0;
            foreach (DataGridViewRow fila in dataGridView1.Rows)
            {
                if (!fila.IsNewRow)
                {
                    // Extraer y limpiar el valor de la celda (8 y 9)  
                 //   string precioTexto = fila.Cells[4].Value?.ToString();
            string gananciaTexto = fila.Cells[5].Value?.ToString();

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
            label9.Text = acumuladorGanancia.ToString("C");
        }
        bool bandera2 = false;
private void button3_Click(object sender, EventArgs e)
        {
            if (bandera2 == false)
            {
                button4.Location = new System.Drawing.Point(788, 285);
                panel1.Location = posicioninicialbuton4;
                panel1.Visible = true;
                string fechaRecibida = label8.Text; // Fecha recibida como string
                var listaArchivo3 = ac.CargarArchivo3(ac.ruta3);
                DateTime fecha;
                string fechaAyerString;
                DateTime.TryParseExact(fechaRecibida, "d/M/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out fecha);
                // Restar un día  
                DateTime fechaAyer = fecha.AddDays(-1);

                // Mostrar la fecha de ayer como string  
                fechaAyerString = fechaAyer.ToString("d/M/yyyy");


                List<ResultadoFiltrado> resultadosFiltrados;
                var resultadoFecha = listaArchivo3
                        .Where(a => a.Fecha.Equals(fechaAyerString, StringComparison.OrdinalIgnoreCase))
                        .Select(a => new ResultadoFiltrado
                        {
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
                resultadosFiltrados = resultadoFecha;
                dataGridView1.DataSource = resultadosFiltrados;
                SumarColumnaGanancia();
                label10.Text = dataGridView1.RowCount.ToString();
                bandera2 = true;
                MostrarUltimaFila();
                button2.Enabled = false;
                button4.Enabled = false;
            }
            else
            {
                bandera2 = false;
                button4.Location = posicioninicialbuton4;
                panel1.Visible = false;
                button2.Enabled = true;
                button4.Enabled = true;
            }
        }
        bool bandera3 = false;
        private void button4_Click(object sender, EventArgs e)
        {
            if (bandera3 == false)
            {
                CargarYMostrarProductos();
                SumarColumnaGanancia();
                MostrarUltimaFila();
                panel1.Location = posicioninicialPanel;
                panel1.Visible = true;
                bandera3 = true;
                button2.Enabled = false;
                button3.Enabled = false;
            }
            else
            {
                panel1.Visible = false;
                bandera3 = false;
                button2.Enabled = true;
                button3.Enabled = true;
            }
            //dataGridView1.DataSource = ac.listaVentas;
        }
        private void label9_Click(object sender, EventArgs e)
        {

        }
    }
}
