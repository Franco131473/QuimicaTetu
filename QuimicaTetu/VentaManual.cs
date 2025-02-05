using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static QuimicaTetu.VentaManual;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace QuimicaTetu
{
    public partial class VentaManual : Form
    {
        private Image[] images;
        private string[] nombreImagen;
        private int currentImageIndex = 0;
        private AccesoDatos datosbase;
        private CAJA caja;
        public VentaManual()
        {
          // dataGridView1.DefaultCellStyle.Font = new Font("Arial", 12, FontStyle.Regular); // Cambia "Arial" y 12 al tamaño de fuente deseado  
          //  dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 14, FontStyle.Bold); // Cambia el tamaño de la fuente del encabezado
            this.KeyPreview = true;
            InitializeComponent();
            datosbase = new AccesoDatos();
            caja = new CAJA();
            label7.Visible = false;
            label6.Text = $"{caja.DineroActual.ToString("C")}";
            lblTotal.Visible = true;
            label15.Visible = false;
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
            panel1.Visible = false;
            panel3.Visible = false;
            label16.Visible = false;
            button3.Visible = false;
           //caja.EstablecerDineroActual(68100);
        }
        //Clase local para filtrar los nombres del texto escrito en una texbox
        public class ResultadoFiltrado
        {
            public string Nombre { get; set; }
            public decimal Precio { get; set; }
            public double Cantidad { get; set; }
            public string Tipo { get; set; }
            public int Ganancia { get; set; }
            public decimal PrecioCosto { get; set; }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            this.KeyPreview = false;
            var textoBusqueda = textBox1.Text;

            // Cargar los datos de los archivos (puedes hacerlo en el constructor o en un método inicial)  
            var listaArchivo1 = datosbase.CargarArchivo1(@"C:\Users\Netbook\Desktop\ArchivosQuimica.json");
            var listaArchivo2 = datosbase.CargarArchivo2(@"C:\Users\Netbook\Desktop\PrecioQuímicos.json");

            List<ResultadoFiltrado> resultadosFiltrados;

            // Comprobar si el texto ingresado es un posible código de barras  
            if (EsCodigoDeBarras(textoBusqueda))
            {
                // Buscar por código de barras en archivo1  
                var resultadoPorCodigo = listaArchivo1
                    .Where(a => a.Codigo.Equals(textoBusqueda, StringComparison.OrdinalIgnoreCase))
                    .Select(a => new ResultadoFiltrado
                    {
                        Nombre = a.Nombre,
                        PrecioCosto = a.PreC,
                        Precio = a.PreV,
                        Cantidad = a.Cantidad,
                        Tipo = a.Tipo,
                        Ganancia = a.GananciaUnitaria
                    })
                    .ToList();
                //label1.Text = Nombre.ToString();
                //label2.Text = Cantidad.ToString();
                //label14.Text = Precio.ToString();
                resultadosFiltrados = resultadoPorCodigo;
               // textBox1.Clear();
            }
            else
            {
                // Filtrar normalmente por nombre  
                resultadosFiltrados = FiltrarNombres(textoBusqueda, listaArchivo1, listaArchivo2);
            }

            // Mostrar en DataGridView
            // dataGridView1.Dock = DockStyle.Fill; // Para que ocupe todo el formulario
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
           // dataGridView1.Columns[1].Width = 200;
            dataGridView1.DataSource = resultadosFiltrados;
            dataGridView1.Columns["Ganancia"].Visible = false; // ajuste con el nombre de la columna  
            dataGridView1.Columns["PrecioCosto"].Visible = false; // ajuste con el nombre de la columna
            this.KeyPreview = true;
            // Color de fondo del DataGridView  
            dataGridView1.BackgroundColor = Color.Black;
            dataGridView1.Columns[0].Width = 200;
            dataGridView1.DefaultCellStyle.BackColor = Color.Black; // Color de fondo de las celdas  
            dataGridView1.DefaultCellStyle.ForeColor = Color.White; // Color de texto de las celdas
            // Cambiar el estilo del encabezado  
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.White; // Fondo del encabezado  
            dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black; // Color del texto del encabezado  
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 10, FontStyle.Bold); // Fuente del encabezado
            //textBox1.Clear();
        }
        // Método para determinar si el texto es un código de barras  
        private bool EsCodigoDeBarras(string texto)
        {
            // Aquí puedes definir las condiciones que cumpla un código de barras  
            // Por ejemplo: longitud del código, que contenga solo números, etc.  
            // En este caso, consideramos códigos de barras numéricos  
            // Ajusta la longitud según el formato que manejes  
            return texto.All(char.IsDigit) && (texto.Length == 8 || texto.Length == 12 || texto.Length == 13 || texto.Length == 14);
        }
        public List<ResultadoFiltrado> FiltrarNombres(string texto, List<ProductoArchivo> archivo1, List<ArchivoLíquidoSuelto> archivo2)
        {
            var resultados = new List<ResultadoFiltrado>();

            // Filtrar en Archivo1  
            var resultadosArchivo1 = archivo1
                .Where(a => a.Nombre.IndexOf(texto, StringComparison.OrdinalIgnoreCase) >= 0)
                .Select(a => new ResultadoFiltrado
                {
                    Nombre = a.Nombre,
                    PrecioCosto = a.PreC,
                    Precio = a.PreV,
                    Cantidad = a.Cantidad,
                    Tipo = a.Tipo,
                   Ganancia = a.GananciaUnitaria
                });

            // Filtrar en Archivo2  
            var resultadosArchivo2 = archivo2
                .Where(a => a.Nombre.IndexOf(texto, StringComparison.OrdinalIgnoreCase) >= 0)
                .Select(a => new ResultadoFiltrado
                {
                    Nombre = a.Nombre,
                    PrecioCosto = a.PrecioCompra,
                    Precio = a.Precio,
                    Cantidad = a.Cantidad,
                    Tipo = a.Tipo,
                    Ganancia = a.GananciaXLitro
                });

            resultados.AddRange(resultadosArchivo1);
            resultados.AddRange(resultadosArchivo2);

            return resultados.Distinct().ToList(); // Evitar duplicados
        }
        int precio;
        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down && dataGridView1.Rows.Count > 0)
            {
                dataGridView1.Focus();
                //selecciona la primera fila;
                dataGridView1.CurrentCell = dataGridView1.Rows[0].Cells[0];
            }
        }
        bool bandera = false;
        bool bandera2 = true;
        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && dataGridView1 != null)
            {
                // Cada que elijo una oferta se agregan directamente ala listview
                string tipo = dataGridView1.CurrentRow.Cells["Tipo"].Value.ToString();
                string name = dataGridView1.CurrentRow.Cells["Nombre"].Value.ToString();
                string disponible = dataGridView1.CurrentRow.Cells["Cantidad"].Value.ToString();
                int precio = Convert.ToInt32(dataGridView1.CurrentRow.Cells["Precio"].Value.ToString());
                label14.Text = precio.ToString();
                label17.Text = tipo;
                label1.Text = name;
                label2.Text = disponible;
                int pC = Convert.ToInt32(dataGridView1.CurrentRow.Cells["PrecioCosto"].Value.ToString());
                label15.Text = pC.ToString();
                int ganancia = Convert.ToInt32(dataGridView1.CurrentRow.Cells["Ganancia"].Value.ToString());
                if (tipo == "Oferta")
                {
                    bandera = true;
                    ListViewItem item = new ListViewItem(name);
                    item.SubItems.Add(precio.ToString());
                    item.SubItems.Add($"{disponible}ml");
                    item.SubItems.Add(precio.ToString());
                    listView1.Items.Add(item);
                    //Agregar los mismos datos para las listas que conformaran los datos de venta
                    //y tambien para restar su cantidad
                    datosbase.Nombre.Add(name);
                    datosbase.Tipo.Add("Químico");
                    datosbase.CantidadList.Add(disponible);
                    datosbase.Fecha.Add(label12.Text);
                    datosbase.Hora.Add(label11.Text);
                    // datosbase.Hora.Add(label11.Text);
                    datosbase.PC.Add(pC.ToString());
                    datosbase.PV.Add(precio.ToString());
                    datosbase.Ganancia.Add(ganancia.ToString());
                    //datosbase.PC.Add();
                    textBox1.Clear();
                    textBox1.Focus();
                    SumaColumna();
                }
                else
                {
                    timer2.Start();
                    label1.Text = name;
                    label2.Text = disponible;
                    textBox2.Focus();
                    textBox1.Clear();
                    if (label17.Text == "Químico")
                    {
                        label16.Visible = true;
                        button3.Visible = true;
                    }
                }
                
            }
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
            label7.Text = total.ToString();
            //ca.ActualizandoCaja(total);           
        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
       
        

        }
        private int CantidadEnPesos(int monto, int precio) {
          return Convert.ToInt32(monto * 1.000) / precio;
           
        }
        int monto;
        int gananciaQ;
        bool bandera3 = false;
        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && textBox2 != null)
            {

                ListViewItem item = new ListViewItem(label1.Text);
                item.SubItems.Add(label14.Text);
                item.SubItems.Add(textBox2.Text);
                if (label17.Text == "Químico")
                {
                    bandera2 = false;
                    int precioTotal = Convert.ToInt32(textBox2.Text) * Convert.ToInt32(label14.Text) / 1000;
                    item.SubItems.Add(precioTotal.ToString());
                    listView1.Items.Add(item);

                    monto = Convert.ToInt32(textBox2.Text) * Convert.ToInt32(label15.Text) / 1000;
                    gananciaQ = precioTotal - monto;

                }
                else
                {
                    int precioTotal = Convert.ToInt32(textBox2.Text) * Convert.ToInt32(label14.Text);
                    item.SubItems.Add(precioTotal.ToString());
                    listView1.Items.Add(item);
                    bandera3 = true;
                }
                

                //listView1.Items.Add(item);
                // Añadir nombre y cantidad para restar stock y para hacer el registro de Ventas
                datosbase.Nombre.Add(label1.Text);
                datosbase.CantidadList.Add(textBox2.Text);
                datosbase.Tipo.Add(label17.Text);
                datosbase.PV.Add(label14.Text);
                datosbase.PC.Add(label15.Text);
                int ganancia = (Convert.ToInt32(label14.Text) - Convert.ToInt32(label15.Text)) * Convert.ToInt32(textBox2.Text);
                if (bandera2 == false && label17.Text == "Químico")
                {
                    datosbase.Ganancia.Add(gananciaQ.ToString());
                }
                else 
                {
                    datosbase.Ganancia.Add(ganancia.ToString());
                }
                datosbase.Hora.Add(label11.Text);
                datosbase.Fecha.Add(label12.Text);
                textBox1.Focus();
                SumaColumna();
                label1.Text = "";
                label2.Text = "";
                label14.Text = "";
                label17.Text = "";
                label15.Text = "";
                textBox2.Clear();
                timer2.Stop();
                label16.Visible = false;
            }
        
            if (e.KeyCode == Keys.Right)
            {
                button3.Focus();
            }
        

        }

        private void button1_Click(object sender, EventArgs e)
        {
           
            if (bandera == true)
            {
             //   MessageBox.Show($"Se Cumple la condición con:{datosbase.Nombre[0]}");
                //VentasQuimicos q = new VentasQuimicos();
                //q.CambiarNombreOfertas();
                for (int i = 0; i < datosbase.Nombre.Count; i++)
                {
                    switch (datosbase.Nombre[i])
                    {
                        // jabon x3 & x5
                        case ("Oferta Jabon(A)X3"):
                            datosbase.Nombre[i] = "Jabon Liquido(A)";
                            break;
                        case ("Oferta Jabon(A)X5"):
                            datosbase.Nombre[i] = "Jabon Liquido(A)";
                            break;
                        case ("Oferta Jabon(B)X3"):
                            datosbase.Nombre[i] = "Jabon Liquido(B)";
                            break;
                        case ("Oferta Jabon(B)X5"):
                            datosbase.Nombre[i] = "Jabon Liquido(B)";
                            break;
                        // suavizante x3 & x5 
                        case ("Oferta Suavizante(C)X3"):
                            datosbase.Nombre[i] = "Suavizante(C)";
                            break;
                        case ("Oferta Suavizante(C)X5"):
                            datosbase.Nombre[i] = "Suavizante(C)";
                            break;
                        case ("Oferta Suavizante(R)X3"):
                            datosbase.Nombre[i] = "Suavizante(R)";
                            break;
                        case ("Oferta Suavizante(R)X5"):
                            datosbase.Nombre[i] = "Suavizante(R)";
                            break;
                        // Detergente común x3 & x5
                        case ("Oferta Detergente X3"):
                            datosbase.Nombre[i] = "Detergente Comun";
                            break;
                        case ("Oferta Detergente X5"):
                            datosbase.Nombre[i] = "Detergente Comun";
                            break;
                        // Detergente mag x3 & x5
                        case ("Oferta Detergente Mag X3"):
                            datosbase.Nombre[i] = "Detergente Tipo Magistral";
                            break;
                        case ("Oferta Detergente Mag X5"):
                            datosbase.Nombre[i] = "Detergente Tipo Magistral";
                            break;
                        case ("Oferta Cera Blanca X3"):
                            datosbase.Nombre[i] = "Cera Blanca";
                            break;
                        case ("Oferta Cera Roja X3"):
                            datosbase.Nombre[i] = "Cera Roja";
                            break;
                        case ("Oferta Acido X3"):
                            datosbase.Nombre[i] = "Acido";
                            break;
                        case ("Oferta Quita Mancha X3"):
                            datosbase.Nombre[i] = "Quita Manchas";
                            break;
                        case ("Oferta Desengrasante X3"):
                            datosbase.Nombre[i] = "Desengrasante";
                            break;
                        case ("Oferta Desengrasante X5"):
                            datosbase.Nombre[i] = "Desengrasante";
                            break;
                        case ("Oferta Shampo X3"):
                            datosbase.Nombre[i] = "Shampo";
                            break;
                        case ("Oferta Shampo X5"):
                            datosbase.Nombre[i] = "Shampo";
                            break;
                        case ("Oferta Crema X3"):
                            datosbase.Nombre[i] = "Crema";
                            break;
                        case ("Oferta Crema X5"):
                            datosbase.Nombre[i] = "Crema";
                            break;
                        case ("Oferta Lavandina X3"):
                            datosbase.Nombre[i] = "Lavandina";
                            break;
                        case ("Oferta Lavandina X5"):
                            datosbase.Nombre[i] = "Lavandina";
                            break;
                        case ("Oferta Insecticida X3"):
                            datosbase.Nombre[i] = "Insecticida";
                            break;

                    }
                }
               // MessageBox.Show($"Sale como: {datosbase.Nombre[0]}");
                bandera = false;
                bandera2 = true;
            }
            if (bandera2 == true)
            {
                //MessageBox.Show("Se restan productos sueltos");
                datosbase.RestarQuimico();
            }
            if(bandera3 == true)
            {
                //MessageBox.Show("Se restan productos no sueltos");
                datosbase.RestarCantidad();
                bandera3 = false;
            }
            datosbase.DatosDeVenta();
            //label6.Text = caja.DineroActual.ToString("C");
            //lblTotal.Text = "";
            //label7.Text = "";
            //listView1.Items.Clear();
            if (pictureBox1.Image == images[0])
            {
                DialogResult result = MessageBox.Show("Estás seguro de que deseas realizar el Pago en EFECTIVO?", "Confirmación", MessageBoxButtons.YesNo,MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
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
                        if (!int.TryParse(label7.Text, out monto))
                        {
                            MessageBox.Show("El monto debe ser un número válido.");
                            return;
                        }

                        var nuevoProducto = new ArchivoIngresos
                        {
                            ModoDePago = "Efectivo",
                            Fecha = label11.Text, // Asegúrate de que tiene un formato válido  
                            Hora = label12.Text, // Asegúrate de que tiene un formato válido  
                            Efectivo = monto.ToString(),
                            Monto = monto,
                        };
                        datosbase.listaIngresos.Add(nuevoProducto);
                        datosbase.GuardarListaIngresos(datosbase.ruta2);
                        //caja.ActualizarCaja(monto);
                      caja.ActualizarCaja(Convert.ToInt32(label7.Text));
                      caja.GuardarEstado();
                        label6.Text = caja.DineroActual.ToString("C");
                        lblTotal.Text = "";
                        label7.Text = "";
                        listView1.Items.Clear();
                        // CargarYMostrarProductos();  
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error al guardar el archivo: {ex.Message}\n{ex.StackTrace}");
                    }
                }
                //MessageBox.Show("Pago realizado con Efectivo");
                datosbase.EliminarListas();
            }
            else
            {
                
            if (pictureBox1.Image == images[1])
                    {
                    DialogResult result = MessageBox.Show("Estás seguro de que deseas realizar el Pago con TRANSFERENCIA?", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
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
                            if (!int.TryParse(label7.Text, out monto))
                            {
                                MessageBox.Show("El monto debe ser un número válido.");
                                return;
                            }
                            var nuevoProducto = new ArchivoIngresos
                            {
                                ModoDePago = "Transferencia",
                                Fecha = label11.Text, //Convert.ToInt32(textBoxCantidad.Text),
                                Hora = label12.Text,
                                Transferencia = monto.ToString(),
                                //Efectivo = "-",
                                Monto = monto,
                            };
                            datosbase.listaIngresos.Add(nuevoProducto);
                            datosbase.GuardarListaIngresos(datosbase.ruta2);// Guardar los datos después de agregar
                            label6.Text = caja.DineroActual.ToString("C");
                            lblTotal.Text = "";
                            label7.Text = "";
                            listView1.Items.Clear();
                            //caja.ActualizarCaja(int.Parse(lblTotal.Text));
                            // CargarYMostrarProductos();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error al guardar el archivo: {ex.Message}");
                        }
                    }
                    MessageBox.Show("Pago realizado con Transferencia");
                }
            else
             {
             panel1.Visible = true;
             textBox3.Focus();
             }
             datosbase.EliminarListas();
            }
        }

        private void VentaManual_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                button1.Focus();
            }
            if (e.KeyCode == Keys.F7)
            {
                textBox1.Clear();
                textBox1.Focus();
            }
            if (e.KeyCode == Keys.Enter && button3.Focused)
            {
                textBox4.Focus(); e.SuppressKeyPress = true; // Previene el comportamiento predeterminado del Enter }
            }
        }
        private void VentaManual_Load(object sender, EventArgs e)
        {
            //label6.Text = caja.DineroActual.ToString("C");
            timer2.Interval = 500; // Intervalo de 500 ms  
           // timer2.Start(); // Inicia el Timer  
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
           // timer1.Interval = 500; // Intervalo de 500 ms 
            label11.Text = DateTime.Now.ToShortTimeString();
            label12.Text = DateTime.Now.ToShortDateString();
           
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            currentImageIndex = (currentImageIndex + 1) % images.Length;
            pictureBox1.Image = images[currentImageIndex];
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (label10.Text == "Ingrese monto nuevo de la Caja")
            {
                caja.EstablecerDineroActual(Convert.ToInt32(textBox3.Text));
                label6.Text = caja.DineroActual.ToString("C");
                MessageBox.Show($"La caja se ha actualizado, su nuevo valor es {label6.Text}");
                panel1.Visible = false;
            }
            else
            {
                DialogResult result = MessageBox.Show("Estás seguro de que deseas realizar el Pago con División?", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
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
                        if (!int.TryParse(label7.Text, out monto))
                        {
                            MessageBox.Show("El monto debe ser un número válido.");
                            return;
                        }
                        var nuevoProducto = new ArchivoIngresos
                        {
                            ModoDePago = "División",
                            Fecha = label12.Text, //Convert.ToInt32(textBoxCantidad.Text),
                            Hora = label11.Text,
                            //Transferencia = monto.ToString(),
                            Efectivo = textBox3.Text,
                            Monto = monto,
                            Transferencia = (monto - Convert.ToInt32(textBox3.Text)).ToString()
                        };
                        datosbase.listaIngresos.Add(nuevoProducto);
                        datosbase.GuardarListaIngresos(datosbase.ruta2);// Guardar los datos después de agregar
                        caja.ActualizarCaja(Convert.ToInt32(textBox3.Text));
                        listView1.Items.Clear();
                        //textBox3.Clear();
                        panel1.Visible = false;
                        // CargarYMostrarProductos();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error al guardar el archivo: {ex.Message}");
                    }
                }
                MessageBox.Show("Pago realizado con efectivo y transferencia");
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            label20.Visible = !label20.Visible;
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && textBox4 != null) {

                bandera2 = true;
                ListViewItem item = new ListViewItem(label1.Text); // nombre
                item.SubItems.Add(label14.Text); // PRecio

                textBox2.Text = (Convert.ToInt32(textBox4.Text) * 1000 / Convert.ToInt32(label14.Text)).ToString();
                item.SubItems.Add(textBox2.Text);
              //  MessageBox.Show($"{textBox2.Text}");
                int precioTotal = Convert.ToInt32(textBox2.Text) * Convert.ToInt32(label14.Text) / 1000;
               // MessageBox.Show($"{precioTotal}");
                item.SubItems.Add(precioTotal.ToString());
                int monto = Convert.ToInt32(label15.Text) * Convert.ToInt32(textBox2.Text) / 1000;
                listView1.Items.Add(item);
                //listView1.Items.Add(item);
                // Añadir nombre y cantidad para restar stock y para hacer el registro de Ventas
                datosbase.Nombre.Add(label1.Text);
                datosbase.CantidadList.Add(textBox2.Text);
                datosbase.Tipo.Add(label17.Text);
                datosbase.PV.Add(textBox4.Text);
                datosbase.PC.Add((Convert.ToInt32(textBox2.Text) * Convert.ToInt32(label15.Text)/ 1000).ToString());
                int ganancia = precioTotal - monto;
                datosbase.Ganancia.Add(ganancia.ToString());
                datosbase.Hora.Add(label11.Text);
                datosbase.Fecha.Add(label12.Text);
                textBox1.Focus();
                SumaColumna();
                label1.Text = "";
                label2.Text = "";
                label14.Text = "";
                label17.Text = "";
                label15.Text = "";
                textBox2.Clear();
                timer2.Stop();
                textBox4.Clear();
                panel3.Visible = false;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox4.Visible = true;
            textBox4.Focus();
            label16.Visible = false;
            panel3.Visible = true;
            button3.Visible = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            panel1.Visible = true;
            label10.Text = "Ingrese monto nuevo de la Caja";
        }

        private void button3_TextChanged(object sender, EventArgs e)
        {
        
            
        

    }

        private void button3_KeyDown(object sender, KeyEventArgs e)
        {
            //this.KeyPreview = false;
            if (e.KeyCode == Keys.Enter)
            {
                textBox4.Focus();
            }
        }
}
}
