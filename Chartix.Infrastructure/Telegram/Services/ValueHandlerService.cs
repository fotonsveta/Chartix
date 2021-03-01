using System.Threading.Tasks;
using Chartix.Core.Entities;
using Chartix.Core.Interfaces;
using Chartix.Infrastructure.Telegram.Interfaces;
using Chartix.Infrastructure.Telegram.Models;
using Microsoft.Extensions.Logging;

namespace Chartix.Infrastructure.Telegram.Services
{
    /// <summary>
    /// Service for processing the entered metric value.
    /// </summary>
    public class ValueHandlerService : IValueHandlerService
    {
        private readonly ILogger<ValueHandlerService> _logger;
        private readonly IBotClient _botClient;
        private readonly IMetricRepository _metricRepository;
        private readonly IValueRepository _valueRepository;

        public ValueHandlerService(
                                    ILogger<ValueHandlerService> logger,
                                    IBotClient botClient,
                                    IMetricRepository metricRepository,
                                    IValueRepository valueRepository)
        {
            _logger = logger;
            _botClient = botClient;
            _metricRepository = metricRepository;
            _valueRepository = valueRepository;
        }

        /// <summary>
        /// Parse the metric value. If the value is correct, it saves value to the db.
        /// </summary>
        /// <param name="message">User message containing value.</param>
        /// <returns>Task representing the asynchronous operation.</returns>
        public async Task HandleAsync(UpdateMessage message)
        {
            _logger.LogDebug($"Try parse value {message.Content} in update {message.UpdateId}");

            (Value value, string error) = new ValueParser().Parse(message);
            MessageCode code;
            if (!string.IsNullOrEmpty(error))
            {
                code = MessageCode.InvalidValue;
                _logger.LogError(error);
            }
            else
            {
                code = SaveValue(value, message);
            }

            await _botClient.SendTextMessageAsync(message, code);
        }

        private MessageCode SaveValue(Value value, UpdateMessage message)
        {
            var mainMetric = _metricRepository.GetMainByExternalId(message.ChatId);
            if (mainMetric == null)
            {
                return MessageCode.ValueWithoutMetric;
            }

            value.UpdateMetric(mainMetric);
            value = _valueRepository.Add(value);
            _logger.LogDebug($"Saved value {value.Content} {value.ValueDate} with id {value.Id}");
            return MessageCode.Done;
        }
    }
}
