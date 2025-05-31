using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.IO;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Text.Json;


namespace WindowsFormsApp1
{
    public partial class Form3: Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtApiKey.Clear();
            txtConsulta.Clear();
        }

        private async void btnEnviar_Click(object sender, EventArgs e)
        {

            string pregunta = txtApiKey.Text;
            string apiKey = "";

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

                var requestBody = new
                {
                    model = "gpt-3.5-turbo",
                    messages = new[]
                    {
                new { role = "system", content = "Eres un asistente útil que responde preguntas de manera clara." },
                new { role = "user", content = pregunta }
            }
                };

                var contenido = new StringContent(System.Text.Json.JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

                var respuesta = await client.PostAsync("https://api.openai.com/v1/chat/completions", contenido);
                respuesta.EnsureSuccessStatusCode();

                string json = await respuesta.Content.ReadAsStringAsync();
                using (JsonDocument doc = JsonDocument.Parse(json))
                {
                    string texto = doc.RootElement
                                      .GetProperty("choices")[0]
                                      .GetProperty("message")
                                      .GetProperty("content")
                                      .GetString();

                    txtConsulta.Text = texto;
                }
            }
        }
    }
}
