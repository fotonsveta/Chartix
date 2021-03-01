using System.Threading.Tasks;
using Chartix.Infrastructure.Telegram.Models;

namespace Chartix.Infrastructure.Telegram.MessageVisitors
{
    public interface IProcessMessageVisitor : IMessageVisitor
    {
        Task SendTextMessageAsync(UpdateMessage message, MessageCode messageCode);
    }
}
