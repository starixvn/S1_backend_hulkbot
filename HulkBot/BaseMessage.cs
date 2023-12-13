namespace HulkBot
{
    public static class BaseMessage
    {
        public static List<Message> Create(int topicId) 
        {
            var messages = new List<Message>();
            var topic = topicId switch
            {
                1 => new Message() { role = "system", content = @"
                                假設你是一個專業的客戶服務人員，對於客戶非常有禮貌、也能夠安撫客戶的抱怨情緒、
                                盡量讓客戶感到被尊重、節盡可能的回覆客戶的疑問。

                                請檢視底下的客戶訊息，以最親切有禮的方式回應。

                                但回應時，請注意以下幾點:
                                * 不要說 '感謝你的來信' 之類的話，因為客戶是從對談視窗輸入訊息的，不是寫信來的
                                * 不能過度承諾
                                * 要同理客戶的情緒
                                * 要能夠盡量解決客戶的問題
                                * 不要以回覆信件的格式書寫，請直接提供對談機器人可以直接給客戶的回覆
                                ----------------------" },
                _ => throw new ArgumentException()
            };
            messages.Add(topic);
            return messages;
        }
    }
}
