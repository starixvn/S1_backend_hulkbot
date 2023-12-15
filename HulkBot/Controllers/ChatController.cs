using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
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
            var chat = new Chat();
            await _chatService.Create(chat);
            return chat.Id.ToString();
        }
        [HttpPost]
        [Route("Completions")]
        public async Task<string> Completions(CreateChatCompletions completions)
        {
            var chat = await _chatService.GetAsync(completions.ChatId);
            if (chat == null) return "chat not fount";

            if (chat.Messages == null) 
            {
                chat.InitialMessage(1);
            }
            chat.Append(new Message
            {
                role = "user",
                content = completions.Message
            });
            var body = new
            {
                model = "gpt-3.5-turbo",
                max_tokens = 1000,
                temperature = 0.5,
                messages = chat.Messages
            };
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_openai.Key}");

            var content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(_openai.Url + "/v1/chat/completions", content);
            var reponseContent = await response.Content.ReadAsStringAsync();
            var contentObj = JsonSerializer.Deserialize<ChatCompletionsResponse>(reponseContent);
            var message = contentObj.choices.FirstOrDefault()?.message;
            chat.Messages.Add(message);
            await _chatService.UpdateAsync(completions.ChatId, chat);
            return message.content;
        }
    }
}
