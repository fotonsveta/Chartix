using System.Threading.Tasks;
using Chartix.Infrastructure.Telegram.MessageVisitors;

namespace Chartix.Infrastructure.Telegram.Models
{
    public record TextUpdateMessage : UpdateMessage
    {
        public TextUpdateMessage()
        {
            Type = UpdateMessageType.Text;
        }

        public override async Task Accept(IMessageVisitor messageVisitor)
        {
            await messageVisitor.Visit(this);
        }
    }
}
