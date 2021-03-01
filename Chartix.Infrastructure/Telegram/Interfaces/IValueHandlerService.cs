using System.Threading.Tasks;
using Chartix.Infrastructure.Telegram.Models;

namespace Chartix.Core.Interfaces
{
    public interface IValueHandlerService
    {
        Task HandleAsync(UpdateMessage message);
    }
}