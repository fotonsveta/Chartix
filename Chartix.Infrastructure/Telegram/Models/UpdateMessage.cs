using System;
using System.Threading.Tasks;
using Chartix.Infrastructure.Telegram.MessageVisitors;

namespace Chartix.Infrastructure.Telegram.Models
{
    public abstract record UpdateMessage
    {
        public DateTime Created { get; } = DateTime.UtcNow;

        public UpdateMessageType Type { get; init; }

        public int UpdateId { get; init; }

        public int MessageId { get; init; }

        public long ChatId { get; init; }

        public string Username { get; init; }

        public LangCode LanguageCode { get; init; }

        public DateTime UpdateDatetime { get; init; }

        public string Content { get; init; }

        public abstract Task Accept(IMessageVisitor messageVisitor);
    }
}
