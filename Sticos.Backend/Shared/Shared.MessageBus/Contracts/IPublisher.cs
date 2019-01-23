using System.Threading.Tasks;

namespace Shared.MessageBus.Contracts
{
    public interface IPublisher<T> where T : class, IContract
    {
        Task Publish(object message) ;
    }
}
