using RestSharp;
using System;
using System.Threading.Tasks;

namespace MauiApp1.Services
{
    public class ElevenLabsService
    {
        private const string BaseUrl = "https://api.elevenlabs.io/v1/";
        private const string VoiceId = "";
        private const string ApiKey = "";

        public async Task<string> ConvertTextToSpeechAsync(string text)
        {
            var client = new RestClient(BaseUrl);
            var request = new RestRequest($"text-to-speech/{VoiceId}", Method.Post);

            request.AddHeader("xi-api-key", ApiKey);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "audio/mpeg");

            var body = new
            {
                text = text,
                model_id = "eleven_multilingual_v2",
                voice_settings = new
                {
                    stability = 1,
                    similarity_boost = 0.5
                }
            };

            request.AddJsonBody(body);

            var response = await client.ExecuteAsync(request);

            if (response.IsSuccessful)
            {
                var filePath = "audio.mp3";
                await System.IO.File.WriteAllBytesAsync(filePath, response.RawBytes);
                return filePath;
            }
            else
            {
                throw new Exception($"Error al convertir texto a voz: {response.Content}");
            }
        }
    }
}