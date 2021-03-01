using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace Chartix.Tests.Core.Bot
{
    public class FakeBotResponse
    {
        private readonly ILogger<object> _logger;
        private readonly ITestOutputHelper _outputHelper;

        public FakeBotResponse()
        {
        }

        public FakeBotResponse(ITestOutputHelper outputHelper, ILogger<object> logger = null)
        {
            _logger = logger;
            _outputHelper = outputHelper;
        }

        public virtual void Response(string text)
        {
            _outputHelper?.WriteLine(text);
            _logger?.Log(LogLevel.Debug, text);
        }
    }
}
