using System.Threading.Tasks;
using Chartix.Infrastructure.Telegram.MessageVisitors;

namespace Chartix.Infrastructure.Telegram.Models
{
    public record CommandUpdateMessage : UpdateMessage
    {
        public CommandUpdateMessage()
        {
            Type = UpdateMessageType.Command;
        }

        public override async Task Accept(IMessageVisitor messageVisitor)
        {
            await messageVisitor.Visit(this);
        }
    }
}
