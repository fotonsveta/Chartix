using System.Threading.Tasks;
using Chartix.Infrastructure.Telegram.Models;

namespace Chartix.Infrastructure.Telegram.Commands
{
    public interface ICommand
    {
        string Name { get; }

        Task HandleAsync(UpdateMessage message);
    }
}
