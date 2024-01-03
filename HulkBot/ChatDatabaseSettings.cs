namespace HulkBot
{
    public class ChatDatabaseSettings
    {
        public string ConnectionString { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;

        public string CompletionsCollectionName { get; set; } = null!;
    }
    public class Elastic 
    {
        public string ConnectionString { get; set; } = null!;

        public string Username { get; set; } = null!;

        public string Password { get; set; } = null!;
    }
}
