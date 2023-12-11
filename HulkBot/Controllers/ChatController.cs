using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace HulkBot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private const string ApiUrl = "https://api.openai.com/v1/chat/completions";
        private const string KEY = "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";
        [HttpPost]
        [Route("Completions")]
        public async Task<string> Completions(List<CreateChatCompletions> completions) 
        {
            var body = new
            {
                model = "gpt-3.5-turbo",
                max_tokens = 1000,
                temperature = 0.5,
                messages = completions
            };

            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {KEY}");

            var content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(ApiUrl, content);
            var reponseContent = await response.Content.ReadAsStringAsync();
            var contentObj = JsonSerializer.Deserialize<ChatCompletionsResponse>(reponseContent);
            return contentObj.choices.FirstOrDefault()?.message.content;
        }
    }
}
