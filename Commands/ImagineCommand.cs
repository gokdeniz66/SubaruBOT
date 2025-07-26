using Discord.WebSocket;
using System.Text;
using Newtonsoft.Json;

namespace SubaruBOT.Commands
{
    public class ImagineCommand
    {
        private readonly HttpClient _httpClient;

        public ImagineCommand()
        {
            _httpClient = new HttpClient
            {
                Timeout = TimeSpan.FromMinutes(5)
            };
        }

        public async Task ExecuteAsync(SocketMessage message)
        {
            try
            {
                string prompt = message.Content.Replace("!imagine", "").Trim();
                if (string.IsNullOrWhiteSpace(prompt))
                {
                    await message.Channel.SendMessageAsync("Usage: `!imagine <prompt>`");
                    return;
                }

                // Send loading message
                var loadingMessage = await message.Channel.SendMessageAsync(":paintbrush:  Generating image... (This may take 30-90 seconds)");

                var payload = new
                {
                    prompt = prompt,
                    negative_prompt = "blurry, low quality, ugly, deformed, bad anatomy, bad hands, extra limbs, watermark, text",
                    steps = 20,
                    sampler_index = "DPM++ 2M",
                    width = 768,
                    height = 768,
                    cfg_scale = 7.5,
                    seed = -1,
                };

                var json = JsonConvert.SerializeObject(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Running the request locally to the Stable Diffusion API
                var response = await _httpClient.PostAsync("http://127.0.0.1:7860/sdapi/v1/txt2img", content);

                if (!response.IsSuccessStatusCode)
                {
                    await loadingMessage.ModifyAsync(msg => msg.Content = ":warning:  Error generating image.");
                    return;
                }

                var resultJson = await response.Content.ReadAsStringAsync();
                dynamic result = JsonConvert.DeserializeObject(resultJson);

                string base64 = result.images[0];
                byte[] imageBytes = Convert.FromBase64String(base64);

                string tempFilePath = Path.Combine(Path.GetTempPath(), $"generated_{Guid.NewGuid()}.png");
                await File.WriteAllBytesAsync(tempFilePath, imageBytes);

                await message.Channel.SendFileAsync(tempFilePath, $":robot:  Prompt: `{prompt}`");

                // Delete file after sending
                File.Delete(tempFilePath);

                // Delete loading message after successful generation
                await loadingMessage.DeleteAsync();
            }
            catch (Exception ex)
            {
                await message.Channel.SendMessageAsync(":warning: An error occurred with !imagine.");
                Console.WriteLine($"Error in ImagineCommand: {ex.Message}");
            }
        }
    }
}