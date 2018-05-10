namespace MyTestWeb2017.Service
{
    public interface ISerializer
    {
        string Serialize<T>(T t);

        T Deserialize<T>(string serializedString);

        byte[] SerializeToBytes<T>(T t);

        T Deserialize<T>(byte[] bytes);
    }
}