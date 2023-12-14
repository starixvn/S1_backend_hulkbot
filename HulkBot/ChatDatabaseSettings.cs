namespace HulkBot
{
    public class ChatDatabaseSettings
    {
        public string ConnectionString { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;

        public string CompletionsCollectionName { get; set; } = null!;
    }
}
