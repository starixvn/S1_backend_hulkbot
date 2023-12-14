using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using System.Text;
using System.Text.Json;

namespace HulkBot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController(IOptions<OpenAIOptions> options, ChatService chatService = null) : ControllerBase
    {
        readonly OpenAIOptions _openai = options.Value;
        readonly ChatService _chatService = chatService;

        [HttpGet]
        [Route("Start")]
        public async Task<string> Start()
        {
            var id = ObjectId.GenerateNewId();
            var chat = new Chat() { Id = id };
            await _chatService.Create(chat);
            return id.ToString();
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
