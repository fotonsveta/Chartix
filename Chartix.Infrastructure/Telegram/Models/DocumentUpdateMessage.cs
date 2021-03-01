using System.Threading.Tasks;
using Chartix.Infrastructure.Telegram.MessageVisitors;

namespace Chartix.Infrastructure.Telegram.Models
{
    public record DocumentUpdateMessage : UpdateMessage
    {
        public DocumentUpdateMessage()
        {
            Type = UpdateMessageType.Document;
        }

        public string DocumentMimeType { get; init; }

        public string DocumentFileId { get; init; }

        public override async Task Accept(IMessageVisitor messageVisitor)
        {
            await messageVisitor.Visit(this);
        }
    }
}
