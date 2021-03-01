using System.Threading.Tasks;
using Chartix.Infrastructure.Telegram.Models;

namespace Chartix.Infrastructure.Telegram.MessageVisitors
{
    public interface IMessageVisitor
    {
        Task Visit(TextUpdateMessage message);

        Task Visit(CommandUpdateMessage message);

        Task Visit(DocumentUpdateMessage message);

        Task Visit(CallbackQueryUpdateMessage message);
    }
}
