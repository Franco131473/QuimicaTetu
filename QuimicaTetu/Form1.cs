using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using static QuimicaTetu.Productos;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.IO;
using System.Globalization;
using Newtonsoft.Json.Linq;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;

namespace QuimicaTetu
{
    public partial class Form1 : Form
    {
        private Datos_Ventas dv;
        private ElementosPDF efe;
        private Configurar_Productos ro;
        private Químicos q;
        private AccesoDatos datobase;
        private Ventas v;
        private VentaManual vm;
        // private Productos n;
        private Productos productos;
        private string rutaArchivo = @"C:\Users\Netbook\Desktop\ArchivosQuimica.json";
        //private string rutaArchivo = @"C:\Users\Netbook\ArchivosQuimica.json"; // Ruta donde se guardarán los datos
        public Form1()
        {
            InitializeComponent();
            this.KeyPreview = true;
            //datobase = new Productos();
            productos = new Productos();
            datobase = new AccesoDatos();
            datobase.CargarDatos(datobase.ruta); // Carga los datos al iniciar
            dataGridView1.Visible = false;
            ro = new Configurar_Productos();
            v = new Ventas();
            vm = new VentaManual();
            q = new Químicos();
            efe = new ElementosPDF();
            dv = new Datos_Ventas();
            label5.Visible = false;
            label5.Visible = false;
            button7.Visible = false;
        }        
        public void RenameProperty(JObject jsonObj, string oldName, string newName)
        {
            foreach (var token in jsonObj.Descendants().Where(t => t.Type == JTokenType.Property && ((JProperty)t).Name == oldName).ToList())
            {
                JProperty property = (JProperty)token;
                property.Replace(new JProperty(newName, property.Value));
            }
        }

        private void CargarYInicializarProductos()
        {
            // Verificar si la carpeta existe y crearla si no es así  
            string folderPath = Path.GetDirectoryName(datobase.ruta);
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            //productos = new Productos();
            datobase.CargarDatos(rutaArchivo);

            // Agregar un producto inicial si la lista está vacía  
            if (datobase.listaProductos.Count == 0)
            {
                datobase.listaProductos.Add(new ProductoArchivo
                {
                    Nombre = "Producto Ejemplo",
                    Vencimiento = DateTime.Now.AddDays(30)
                });
                datobase.GuardarDatos(rutaArchivo);
            }
        }
        //Productos n = new Productos();
        // GUARDA LOS DATOS DEL PRODUCTO EN UN ARCHIVO:
        private void SumarTotal()
        {
            int acumuladorTotal = 0;
            int acumuladorGanancia = 0;
            foreach (DataGridViewRow fila in dataGridView1.Rows)
            {
                if (!fila.IsNewRow)
                {
                    // Extraer y limpiar el valor de la celda (8 y 9)  
                    string precioTexto = fila.Cells[8].Value?.ToString();
                    string gananciaTexto = fila.Cells[9].Value?.ToString();

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
            label6.Text = acumuladorGanancia.ToString("C");
        }
        private void button1_Click(object sender, EventArgs e)
        {
            //// Ruta del archivo JSON
            //string filePath = rutaArchivo;
            //// Leer el contenido del archivo JSON
            //string json = File.ReadAllText(filePath);
            //// Parsear el JSON a un objeto
            //JArray jsonArray = JArray.Parse(json);
            //// Renombrar el atributo "PrecioV" a "Precio" en cada objeto del array
            //foreach (JObject jsonObj in jsonArray.OfType<JObject>())
            //{
            //    RenameProperty(jsonObj, "PrecioV", "Precio");
            //}
            //// Guardar el JSON modificado de vuelta al archivo
            //File.WriteAllText(filePath, jsonArray.ToString());
            //MessageBox.Show("Atributo renombrado exitosamente.");
            //Json
            try
            {
                 var nuevoProducto = new ProductoArchivo
                    {
                        Codigo = textBoxCodigo.Text,
                        Nombre = textBoxNombre.Text,
                        Tipo = textBoxTipo.Text,
                        Cantidad = Convert.ToInt32(textBoxCantidad.Text),
                        PreC = Convert.ToInt32(textBoxPrecioC.Text),
                        PreV = Convert.ToInt32(textBoxPrecioV.Text),
                        Vencimiento = DateTime.Parse(dateTimePicker1.Text),// Asegúrate de validar que sea una fecha correcta
                        GananciaUnitaria = Convert.ToInt32(textBoxPrecioV.Text) - Convert.ToInt32(textBoxPrecioC.Text),
                        IngresoPotencial = Convert.ToInt32(textBoxPrecioV.Text) * Convert.ToInt32(textBoxCantidad.Text),
                        GananciaPotencial = (Convert.ToInt32(textBoxPrecioV.Text) - Convert.ToInt32(textBoxPrecioC.Text)) * Convert.ToInt32(textBoxCantidad.Text)
                    };
                    datobase.listaProductos.Add(nuevoProducto);
                    datobase.GuardarDatos(rutaArchivo);// Guardar los datos después de agregar
                    CargarYMostrarProductos();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar el archivo: {ex.Message}");
            }

            eliminarTexboxs();
        }
        private void CargarYMostrarProductos()
        {
            //productos = new Productos();
            datobase.CargarDatos(rutaArchivo);
            // Ordenar la lista antes de asignarla al DataGridView  
            var listaOrdenada = datobase.listaProductos.OrderBy(item => item.Cantidad).ToList(); // Asegúrate que Cantidad es accesible  
            dataGridView1.DataSource = listaOrdenada; 
        }
        public void eliminarTexboxs()
        {
            textBoxCodigo.Clear();
            textBoxNombre.Clear();
            textBoxTipo.Clear();
            textBoxCantidad.Clear();
            textBoxPrecioC.Clear();
            textBoxPrecioV.Clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            panel1.Visible = false;
            dataGridView1.Visible = true;
            label5.Visible = true;
            label5.Visible = true;
            SumarTotal();
            double num1 = 1.000;
            double num2 = 1.300;
            //MessageBox.Show($"{(num2 - num1) * 1000}");
            CargarYMostrarProductos();
            dataGridView1.Location = new Point(238, 10);
            // n.MostrarStock();
        }

        private void textBoxCodigo_Enter(object sender, EventArgs e)
        {
            if (textBoxCodigo.Text == "Código de Barra")
            {
                textBoxCodigo.Text = "";
                textBoxCodigo.ForeColor = Color.Black;
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            Configurar_Productos ro = new Configurar_Productos();
            ro.Show();
        }
        private void button5_Click(object sender, EventArgs e)
        {
            VentaManual vm = new VentaManual();
            vm.Show();
            //Ventas v = new Ventas();
            //v.Show();
        }
        private void button4_Click(object sender, EventArgs e)
        {
            panel1.Visible = true;
            dataGridView1.Visible = false;
            CargarYMostrarProductos();
            //dataGridView1.Location = new Point(238, 10);
            // n.MostrarStock();
        }

        // EL SIGUIENTE FRAGMENTO DE CODIGO RESTANTE ES PARA EL DISEÑO DE LA INTERFAZ 

        private void textBoxCodigo_Leave(object sender, EventArgs e)
        {
            if (textBoxCodigo.Text == "")
            {
                textBoxCodigo.Text = "Código de Barra";
                textBoxCodigo.ForeColor = Color.Silver;
            }
        }



        private void textBoxPrecioC_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBoxNombre_Enter(object sender, EventArgs e)
        {
            if (textBoxNombre.Text == "Nombre del producto")
            {
                textBoxNombre.Text = "";
                textBoxNombre.ForeColor = Color.Black;
            }
        }

        private void textBoxNombre_Leave(object sender, EventArgs e)
        {
            if (textBoxNombre.Text == "")
            {
                textBoxNombre.Text = "Nombre del producto";
                textBoxNombre.ForeColor = Color.Silver;
            }
        }

        private void textBoxTipo_Enter(object sender, EventArgs e)
        {
            if (textBoxTipo.Text == "Clase")
            {
                textBoxTipo.Text = "";
                textBoxTipo.ForeColor = Color.Black;
            }
        }

        private void textBoxTipo_Leave(object sender, EventArgs e)
        {
            if (textBoxTipo.Text == "")
            {
                textBoxTipo.Text = "Clase";
                textBoxTipo.ForeColor = Color.Silver;
            }
        }

        private void textBoxCantidad_Enter(object sender, EventArgs e)
        {
            if (textBoxCantidad.Text == "Cantidad")
            {
                textBoxCantidad.Text = "";
                textBoxCantidad.ForeColor = Color.Black;
            }
        }

        private void textBoxCantidad_Leave(object sender, EventArgs e)
        {
            if (textBoxCantidad.Text == "")
            {
                textBoxCantidad.Text = "Cantidad";
                textBoxCantidad.ForeColor = Color.Silver;
            }
        }

        private void textBoxPrecioC_Enter(object sender, EventArgs e)
        {
            if (textBoxPrecioC.Text == "Precio de Compra")
            {
                textBoxPrecioC.Text = "";
                textBoxPrecioC.ForeColor = Color.Black;
            }
        }

        private void textBoxPrecioC_Leave(object sender, EventArgs e)
        {
            if (textBoxPrecioC.Text == "")
            {
                textBoxPrecioC.Text = "Precio de Compra";
                textBoxPrecioC.ForeColor = Color.Silver;
            }
        }

        private void textBoxPrecioV_Enter(object sender, EventArgs e)
        {
            if (textBoxPrecioV.Text == "Precio de Venta")
            {
                textBoxPrecioV.Text = "";
                textBoxPrecioV.ForeColor = Color.Black;
            }
        }

        private void textBoxPrecioV_Leave(object sender, EventArgs e)
        {
            if (textBoxPrecioV.Text == "")
            {
                textBoxPrecioV.Text = "Precio de Venta";
                textBoxPrecioV.ForeColor = Color.Silver;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        // Le da formato a las columnas númericas de una datagridview
        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Verificar si estamos formateando la columna deseada (por ejemplo, la primera columna)
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
            if (e.ColumnIndex == 8 && e.Value != null)
            {
                if (int.TryParse(e.Value.ToString(), out int value))
                {
                    // Formatear el valor como "1.000,00"
                    e.Value = value.ToString("C", new CultureInfo("es-AR")); e.FormattingApplied = true;
                }
            }
            if (e.ColumnIndex == 9 && e.Value != null)
            {
                if (int.TryParse(e.Value.ToString(), out int value))
                {
                    // Formatear el valor como "1.000,00"
                    e.Value = value.ToString("C", new CultureInfo("es-AR")); e.FormattingApplied = true;
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Añadir la nueva columna al final para el resultado de la multiplicación
            
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1) {
                textBoxCodigo.Focus();
            }
            if (e.KeyCode == Keys.F2)
            {
                button1.Focus();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Químicos q = new Químicos();
            q.Show();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
        private void GuardarDatosEnLista2()
        {
            // Verificar si hay filas en el DataGridView  
            if (dataGridView1.Rows.Count == 0)
            {
                MessageBox.Show("No hay filas en el DataGridView.");
                return;
            }

            // Recorrer las filas del DataGridView  
            foreach (DataGridViewRow fila in dataGridView1.Rows)
            {
                // Asegurarse de que la fila no sea una fila de nuevo (si la habilitaste)  
                if (!fila.IsNewRow)
                {
                    string tipo = fila.Cells[2].Value?.ToString();
                    string producto = fila.Cells[1].Value?.ToString();
                    string cantidad = fila.Cells[3].Value?.ToString();
                    string precio = fila.Cells[5].Value?.ToString();
                    if (tipo == "Perfume" && tipo == "Peluqueria" && tipo == "Shampo & Acondicionador" && tipo == "Higiene Personal")
                    {
                        // Comprobar que product, cantidad y precio no sean null  
                        if (!string.IsNullOrEmpty(producto) && cantidad != null && precio != null)
                        {
                            efe.listaProducto_HPyP.Add(producto);
                            efe.listaCantidad_HPyP.Add(cantidad);
                            efe.listaPrecio_HPyP.Add(precio);

                            // Mostrar el producto para depuración  
                            //MessageBox.Show($"Producto: {producto}, Cantidad: {cantidad}, Precio: {precio}");
                        }
                        else
                        {
                            MessageBox.Show("Una de las celdas es nula o vacía.");
                        }
                    }
                }
            }

        }
        private void GuardarDatosEnLista()
        {
            // Verificar si hay filas en el DataGridView  
            if (dataGridView1.Rows.Count == 0)
            {
                MessageBox.Show("No hay filas en el DataGridView.");
                return;
            }

            // Recorrer las filas del DataGridView  
            foreach (DataGridViewRow fila in dataGridView1.Rows)
            {
                // Asegurarse de que la fila no sea una fila de nuevo (si la habilitaste)  
                if (!fila.IsNewRow)
                {
                    string producto = fila.Cells[1].Value?.ToString();
                    string cantidad = fila.Cells[3].Value?.ToString();
                    string precio = fila.Cells[5].Value?.ToString();

                    // Comprobar que product, cantidad y precio no sean null  
                    if (!string.IsNullOrEmpty(producto) && cantidad != null && precio != null)
                    {
                        efe.listaProducto.Add(producto);
                        efe.listaCantidad.Add(cantidad);
                        efe.listaPrecio.Add(precio);

                        // Mostrar el producto para depuración  
                        //MessageBox.Show($"Producto: {producto}, Cantidad: {cantidad}, Precio: {precio}");
                    }
                    else
                    {
                        MessageBox.Show("Una de las celdas es nula o vacía.");
                    }
                }
            }

        }
        public void CreatePdf()
        {
            // Crear un nuevo documento PDF  
            PdfDocument document = new PdfDocument();
            document.Info.Title = "Lista de Precios";

            // Crear una nueva página  
            PdfPage page = document.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);

            // Crear una fuente  
            XFont font = new XFont("Helvetica", 12, XFontStyle.Regular);
            XFont titleFont = new XFont("Helvetica", 16, XFontStyle.Bold); // Fuente para el título
            XFont subFont = new XFont("Helvetica", 12, XFontStyle.Regular);
            double margin = 40;
            double yPoint = margin;
            //XImage image = XImage.FromFile("C:\\Users\\Usuario\\Downloads\\free Delivery (1).png");
            //double imageX = 20; // Posición X de la imagen  
            //double imageY = 40; // Posición Y de la imagen  
            //double imageWidth = 70; // Ancho de la imagen  
            //double imageHeight = 50; // Alto de la imagen  
           // gfx.DrawImage(image, imageX, imageY, imageWidth, imageHeight);
            //Titulo
            string title = "Química Rosita";
            double titleWidth = gfx.MeasureString(title, titleFont).Width;
            gfx.DrawString(title, titleFont, XBrushes.Black, new XRect((page.Width - titleWidth) / 6, margin, page.Width, 50), XStringFormats.TopLeft);

            string additionalText = "Fecha: " + DateTime.Now.ToShortDateString(); // Ejemplo de texto adicional  
            gfx.DrawString(additionalText, font, XBrushes.Black, new XRect(page.Width - margin - 200, margin, 200, 50), XStringFormats.TopRight);
            yPoint += 70; // Ajustamos yPoint para que los siguientes elementos no se superpongan con la imagen, el título o el texto

            string leftText = $"Nombre y Precio de los Productos";
            double leftTextX = margin; // Ajusta la posición X a la derecha de la imagen  
            double leftTextY = margin + 50;
            double leftTextFontSize = 14; // Tamaño de fuente deseado  
            XFont leftTextFont = new XFont("Helvetica", leftTextFontSize, XFontStyle.Regular);
            // Dibujar el texto del cliente  
            gfx.DrawString(leftText, leftTextFont, XBrushes.Black, new XRect(leftTextX, leftTextY, page.Width - leftTextX - margin, 50), XStringFormats.TopLeft);
            // Medir el ancho del texto para posicionar la línea debajo  
            double leftTextWidth = gfx.MeasureString(leftText, leftTextFont).Width;
            // Dibujar la línea para subrayar  
            double lineYPos = leftTextY + leftTextFontSize + 2; // Posición de la línea un poco debajo del texto  
            gfx.DrawLine(XPens.Black, leftTextX, lineYPos, leftTextX + leftTextWidth, lineYPos);
            // Dibujar el encabezado de las columnas
            gfx.DrawString("Producto", font, XBrushes.Black, new XRect(margin, yPoint, page.Width - 2 * margin, page.Height), XStringFormats.TopLeft);
            //gfx.DrawString("Cantidad", font, XBrushes.Black, new XRect(margin + 200, yPoint, page.Width - 2 * margin, page.Height), XStringFormats.TopLeft);
            gfx.DrawString("Precio", font, XBrushes.Black, new XRect(margin + 300, yPoint, page.Width - 2 * margin, page.Height), XStringFormats.TopLeft);

            yPoint += 20;
            //// Dibujar los datos en el PDF  
            for (int i = 0; i < efe.listaProducto.Count; i++)
            {
                gfx.DrawString(efe.listaProducto[i], font, XBrushes.Black, new XRect(margin, yPoint, page.Width - 2 * margin, page.Height), XStringFormats.TopLeft);
               // gfx.DrawString(efe.listaCantidad[i], font, XBrushes.Black, new XRect(margin + 200, yPoint, page.Width - 2 * margin, page.Height), XStringFormats.TopLeft);
                gfx.DrawString(efe.listaPrecio[i].ToString(), font, XBrushes.Black, new XRect(margin + 300, yPoint, page.Width - 2 * margin, page.Height), XStringFormats.TopLeft);

                yPoint += 20; // Aumentar la posición Y para la siguiente línea  

                // Comprobar si se necesita una nueva página  
                if (yPoint > page.Height - 50) // Dejar un margen inferior  
                {
                    page = document.AddPage(); // Nueva página  
                    gfx = XGraphics.FromPdfPage(page);
                    yPoint = 20; // Reiniciar posición en Y  
                }
            }
            // Agregar un string al final después de los datos  
            //string footerText = $"TOTAL A PAGAR:{Total}"; // Texto que deseas agregar  
            //gfx.DrawString(footerText, subFont, XBrushes.Black, new XRect(100, yPoint + 50, page.Width - 2 * margin, 50), XStringFormats.TopLeft);

            // Guardar el documento  
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string filePath = Path.Combine(desktopPath, "PreciosQuímicaRosita.pdf");
            document.Save(filePath);

            MessageBox.Show("PDF creado y guardado en el escritorio: " + filePath);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            GuardarDatosEnLista();
            efe.CambiarString();
            CreatePdf();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Datos_Ventas dv = new Datos_Ventas();
            dv.Show();
        }
    }
}