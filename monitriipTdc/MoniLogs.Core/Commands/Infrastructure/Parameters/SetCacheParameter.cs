namespace MoniLogs.Core.Commands.Infrastructure.Parameters
{
    public class SetCacheParameter
    {
        public string Key { get; private set; }
        public string Value { get; private set; }
        
        public SetCacheParameter(string key, string value)
        {
            Key = key;
            Value = value;
        }
    }
}