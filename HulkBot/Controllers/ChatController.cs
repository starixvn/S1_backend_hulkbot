using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;

namespace HulkBot.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ChatController(IOptions<OpenAIOptions> options, IChatRepository chatRepository, IKonwledgeRepository konwledgeRepository) : ControllerBase
    {
        readonly OpenAIOptions _openai = options.Value;
        readonly IChatRepository _chatRepository = chatRepository;
        readonly IKonwledgeRepository _konwledgeRepository = konwledgeRepository;
        const string Model = "gpt-4";
        const int Max_Token = 1000;
        const double Temperature = 0.5;

        [HttpGet]
        [Route("Start")]
        public async Task<string> Start()
        {
            var chat = new Chat();
            await _chatRepository.Create(chat);
            return chat.Id.ToString();
        }

        [HttpPost]
        [Route("Completions")]
        public async Task<string> Completions(CreateChatCompletions completions)
        {
            var chat = await _chatRepository.GetAsync(completions.ChatId);
            if (chat == null) return "chat not found";

            if (chat.Messages == null)
            {
                var msg = _konwledgeRepository.Query("system");
                var sb = new StringBuilder();
                foreach (var m in msg)
                {
                    if (!string.IsNullOrEmpty(m.Message))
                    {
                        sb.AppendLine(m.Message);
                    }
                    else 
                    {
                        sb.AppendLine("-----------------------------------");
                        sb.Append("Issue:").AppendLine(m.Issue);
                        sb.Append("Solution:").AppendLine(m.Solution);
                    }
                }
                chat.InitialMessage(sb.ToString());
            }
            chat.Append(new Message { role = "user", content = completions.Message });
            var body = new
            {
                model = Model,
                max_tokens = Max_Token,
                temperature = Temperature,
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
            await _chatRepository.UpdateAsync(completions.ChatId, chat);
            return message.content;
        }
    }
}
