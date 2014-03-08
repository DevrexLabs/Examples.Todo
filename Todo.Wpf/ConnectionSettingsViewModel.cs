using OrigoDB.Core;
using Todo.Core;


namespace Todo.Wpf
{
    public class ConnectionSettingsViewModel
    {
        public string Host { get; set; }
        public ushort Port { get; set; }
        public bool? IsEmbedded { get; set; }

        public bool IsRemote { get; set; }

        public ConnectionSettingsViewModel()
        {
            Host = "localhost";
            Port = 3001;
            IsEmbedded = true;
        }

        public IEngine<TodoModel> GetTransactionHandler()
        {
            if(IsEmbedded.Value) return Engine.LoadOrCreate<TodoModel>();
            
            RemoteClientConfiguration config = new RemoteClientConfiguration();
            return config.GetClient<TodoModel>();
        }
    }
}
