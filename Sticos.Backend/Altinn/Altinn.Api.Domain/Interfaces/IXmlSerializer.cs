namespace Altinn.Api.Domain.Interfaces
{
    public interface IXmlSerializer
    {
        string Serialize<T>(T message);
        T Deserialize<T>(string xml);
    }
}