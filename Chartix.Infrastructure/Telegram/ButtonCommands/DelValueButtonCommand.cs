using System;
using System.Threading.Tasks;
using Chartix.Core.Interfaces;
using Chartix.Infrastructure.Telegram.Interfaces;
using Chartix.Infrastructure.Telegram.Models;

namespace Chartix.Infrastructure.Telegram.ButtonCommands
{
    public class DelValueButtonCommand : ButtonCommand
    {
        private readonly IBotClient _botClient;
        private readonly IValueRepository _valueRepository;

        public DelValueButtonCommand(IBotClient botClient, IValueRepository valueRepository)
        {
            _botClient = botClient;
            _valueRepository = valueRepository;
        }

        public override ButtonCode Button => ButtonCode.DelValue;

        public override async Task HandleAsync(CallbackQueryUpdateMessage message)
        {
            if (!long.TryParse(Parameter, out long valueId))
            {
                throw new ArgumentException($"Can't perfom DelValue command, id parameter is not a number - {Parameter}");
            }

            var value = _valueRepository.GetById(valueId)
                ?? throw new ArgumentException($"Can't perfom DelValue command, there is no value with id {valueId}");

            value.SetDeleted();
            _valueRepository.Update(value);

            await _botClient.SendTextMessageAsync(message, MessageCode.Done);
        }
    }
}
