using RestSharp;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.Maui.Controls;
using MauiApp1.Services;

namespace MauiApp1.Ventanas
{
    public partial class Traductor : ContentPage
    {
        private readonly ElevenLabsService _elevenLabsService;

        public Traductor()
        {
            InitializeComponent();
            _elevenLabsService = new ElevenLabsService();
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

                        string audioFilePath = await _elevenLabsService.ConvertTextToSpeechAsync(result["translated_text"]);
                        await DisplayAlert("Éxito", $"El audio se guardó en: {audioFilePath}", "OK");
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