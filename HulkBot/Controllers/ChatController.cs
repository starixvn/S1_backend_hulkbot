using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;

namespace HulkBot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        OpenAIOptions _openai;
        public ChatController(IOptions<OpenAIOptions> options)
        {
            _openai = options.Value;
        }
        [HttpGet]
        [Route("Start")]
        public string Start()
        {
            return Guid.NewGuid().ToString();
        }
        [HttpPost]
        [Route("Completions")]
        public async Task<string> Completions(CreateChatCompletions completions)
        {
            var messages = BaseMessage.Create(1);
            messages.Add(new Message
            {
                role = "user",
                content = completions.Message
            });
            var body = new
            {
                model = "gpt-3.5-turbo",
                max_tokens = 1000,
                temperature = 0.5,
                messages
            };
            
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_openai.Key}");

            var content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(_openai.Url + "/v1/chat/completions", content);
            var reponseContent = await response.Content.ReadAsStringAsync();
            var contentObj = JsonSerializer.Deserialize<ChatCompletionsResponse>(reponseContent);
            return contentObj.choices.FirstOrDefault()?.message.content;
        }
    }
}
