using System;
using System.Linq;
using System.Threading.Tasks;
using Chartix.Core.Interfaces;
using Chartix.Infrastructure.Telegram.Interfaces;
using Chartix.Infrastructure.Telegram.Models;

namespace Chartix.Infrastructure.Telegram.ButtonCommands
{
    public class ChooseDelMetricButtonCommand : ButtonCommand
    {
        private readonly ISourceRepository _sourceRepository;
        private readonly IMetricRepository _metricRepository;
        private readonly IBotClient _botClient;

        public ChooseDelMetricButtonCommand(IBotClient botClient, ISourceRepository sourceRepository, IMetricRepository metricRepository)
        {
            _sourceRepository = sourceRepository;
            _metricRepository = metricRepository;
            _botClient = botClient;
        }

        public override ButtonCode Button => ButtonCode.ChooseDelMetric;

        protected override ButtonCode NextButton => ButtonCode.DelMetric;

        public override async Task HandleAsync(CallbackQueryUpdateMessage message)
        {
            if (!int.TryParse(Parameter, out int currentPageNumber))
            {
                currentPageNumber = 1;
            }

            var source = _sourceRepository.GetByExternalId(message.ChatId)
                ?? throw new ArgumentException($"Can't perfom {Button} command, there is no source with id {message.ChatId}");

            var metricNames = _metricRepository.GetBySourceId(source.Id, currentPageNumber, PagingMenuData.DefaultPageSize)
                                    .ToDictionary(m => FormatKey(m.Id), m => $"{m}");

            var count = _metricRepository.CountBySourceId(source.Id);
            var pageCount = (count / PagingMenuData.DefaultPageSize) + 1;

            var menuData = new PagingMenuData
            {
                ButtonTexts = metricNames,
                CurrentPage = currentPageNumber,
                PageCount = pageCount,
                Previous = ButtonCode.MetricMenu,
            };

            message = message with { MenuMessageId = MenuMessageId };
            await _botClient.EditTextButtonsMenuWithPagingAsync(message, Button, menuData);
        }
    }
}
