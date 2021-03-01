using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Chartix.Infrastructure.Telegram.Interfaces
{
    public interface IBotUpdateService
    {
        Task HandleAsync(Update update);
    }
}
