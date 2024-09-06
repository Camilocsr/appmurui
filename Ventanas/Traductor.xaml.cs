using RestSharp;
using System;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace MauiApp1.Ventanas
{
    public partial class Traductor : ContentPage
    {
        public Traductor()
        {
            InitializeComponent();
        }

        private void limitecaracter(object sender, TextChangedEventArgs e)
        {
            int count = e.NewTextValue.Length;
            contadorcaracter.Text = $"Caracteres {count}/250";
        }

        private async void TraducirTexto(object sender, EventArgs e)
        {
            string texto = espanol.Text;

            if (string.IsNullOrEmpty(texto))
            {
                await DisplayAlert("Error", "Por favor ingrese un texto para traducir", "OK");
                return;
            }

            try
            {
                var client = new RestClient("http://localhost:5000");

                var request = new RestRequest("translate", Method.Post);

                request.AddJsonBody(new { sentence = texto });

                var response = await client.ExecuteAsync(request);

                if (response.IsSuccessful)
                {
                    var result = JsonConvert.DeserializeObject<Dictionary<string, string>>(response.Content);

                    if (result != null && result.ContainsKey("translated_text"))
                    {
                        traduccion.Text = result["translated_text"];
                    }
                    else
                    {
                        await DisplayAlert("Error", "No se pudo obtener la traducción.", "OK");
                    }
                }
                else
                {
                    await DisplayAlert("Error", $"Error en la traducción: {response.StatusCode}", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Excepción: {ex.Message}", "OK");
            }
        }
    }
}