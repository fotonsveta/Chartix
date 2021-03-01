using System.Threading.Tasks;
using Chartix.Infrastructure.Telegram.MessageVisitors;

namespace Chartix.Infrastructure.Telegram.Models
{
    public record CallbackQueryUpdateMessage : UpdateMessage
    {
        public CallbackQueryUpdateMessage()
        {
            Type = UpdateMessageType.CallbackQuery;
        }

        public override async Task Accept(IMessageVisitor messageVisitor)
        {
            await messageVisitor.Visit(this);
        }

        public int MenuMessageId { get; init; }
    }
}
