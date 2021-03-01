using System.Linq;
using System.Threading.Tasks;
using Chartix.Core.Interfaces;
using Chartix.Infrastructure.Telegram.Interfaces;
using Chartix.Infrastructure.Telegram.Models;

namespace Chartix.Infrastructure.Telegram.ButtonCommands
{
    public class ChooseDelValueButtonCommand : ButtonCommand
    {
        private readonly IMetricRepository _metricRepository;
        private readonly IValueRepository _valueRepository;
        private readonly IBotClient _botClient;

        public ChooseDelValueButtonCommand(IBotClient botClient, IMetricRepository metricRepository, IValueRepository valueRepository)
        {
            _metricRepository = metricRepository;
            _valueRepository = valueRepository;
            _botClient = botClient;
        }

        public override ButtonCode Button => ButtonCode.ChooseDelValue;

        protected override ButtonCode NextButton => ButtonCode.DelValue;

        public override async Task HandleAsync(CallbackQueryUpdateMessage message)
        {
            if (!int.TryParse(Parameter, out int currentPageNumber))
            {
                currentPageNumber = 1;
            }

            var mainMetric = _metricRepository.GetMainByExternalId(message.ChatId);
            if (mainMetric == null)
            {
                await _botClient.SendTextMessageAsync(message, MessageCode.NoMainMetric);
                return;
            }

            var valueNames = _valueRepository.GetByMetricId(mainMetric.Id, currentPageNumber, PagingMenuData.DefaultPageSize)
                            .ToDictionary(m => FormatKey(m.Id), m => $"{m}");

            var count = _valueRepository.CountByMetric(mainMetric.Id);
            var pageCount = (count / PagingMenuData.DefaultPageSize) + 1;

            var menuData = new PagingMenuData
            {
                ButtonTexts = valueNames,
                CurrentPage = currentPageNumber,
                PageCount = pageCount,
                Previous = ButtonCode.Menu,
            };

            message = message with { MenuMessageId = MenuMessageId };
            await _botClient.EditTextButtonsMenuWithPagingAsync(message, Button, menuData);
        }
    }
}
