using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml;

using DocumentFormat.OpenXml.Wordprocessing;

namespace WindowsFormsApp1
{
    public partial class Form2: Form
    {
        public Form2()
        {
            InitializeComponent();
           

        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnlimpiar_Click(object sender, EventArgs e)
        {
            txtNombre.Clear();
            txtDireccion.Clear();
            txtMotivo.Clear();

        }

        private void btncontinuar_Click(object sender, EventArgs e)
        {
            Form3 nuevoFormulario = new Form3();
            nuevoFormulario.Show();
            this.Hide();
        }

        private void btnConsultar_Click(object sender, EventArgs e)

        {
            string texto = "Hola, este es el contenido del documento.";
            string nombre = txtNombre.Text;
            string direccion = txtDireccion.Text;
            string motivo = txtMotivo.Text;

            // Validación simple
            if (string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(direccion) || string.IsNullOrWhiteSpace(motivo))
            {
                MessageBox.Show("Por favor completa todos los campos.");
                return;
            }

            // Guardar en SQL Server
            string conexion = "Data Source=ASUSMUÑOZ\\SQLEXPRESS;Initial Catalog=proyecto2;Integrated Security=True";
            string query = "INSERT INTO Solicitudes (Nombre, Direccion, Motivo) VALUES (@nombre, @direccion, @motivo)";

            using (SqlConnection conn = new SqlConnection(conexion))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@nombre", nombre);
                cmd.Parameters.AddWithValue("@direccion", direccion);
                cmd.Parameters.AddWithValue("@motivo", motivo);
                conn.Open();
                cmd.ExecuteNonQuery();
            }

            string ruta = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), $"Solicitud_{nombre}.docx");

            using (WordprocessingDocument doc = WordprocessingDocument.Create(ruta, WordprocessingDocumentType.Document))
            {
                string carpeta = @"C:\Users\munoz\OneDrive\Documents\proyecto final2\WindowsFormsApp1";
                
                            // Crear la estructura básica del documento
                            MainDocumentPart mainPart = doc.AddMainDocumentPart();
                mainPart.Document = new Document();
                Body cuerpo = new Body();

                // Agregar un párrafo
                Paragraph parrafo = new Paragraph();
                Run run = new Run();
                Text textoContenido = new Text(texto);

                run.Append(textoContenido);
                parrafo.Append(run);
                cuerpo.Append(parrafo);

                mainPart.Document.Append(cuerpo);
                mainPart.Document.Save();
            }
            MessageBox.Show("Tus  datos han sido guardados correctamente, Tu solicitud a sido tomada con Exito.", "Confirmación", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
    
}
