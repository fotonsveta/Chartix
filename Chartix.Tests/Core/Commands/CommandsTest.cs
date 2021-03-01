using System.Threading.Tasks;
using Chartix.Infrastructure.Telegram.Commands;
using Chartix.Infrastructure.Telegram.Interfaces;
using Chartix.Infrastructure.Telegram.Models;
using Chartix.Tests.Core.Bot;
using Xunit;
using Xunit.Abstractions;

namespace Chartix.Tests.Core.Commands
{
    public class CommandsTest
    {
        private readonly ICommandStrategyFactory<ICommand> _commandFactory;

        public CommandsTest(ITestOutputHelper outputHelper)
        {
            var botResponse = new FakeBotResponse(outputHelper);
            IBotClient botClient = new FakeBotClient(botResponse);

            _commandFactory = new ServiceHelper().CreateCSF(botClient);
        }

        [Theory]
        [InlineData("about", LangCode.Ru, "about")]
        [InlineData("Help", LangCode.Ru, "help")]
        [InlineData("mEnu", LangCode.Ru, "menu")]
        [InlineData("starT  ", LangCode.Ru, "start")]
        [InlineData("sfjlgjfd", LangCode.Ru, null)]
        [InlineData("", LangCode.En, null)]
        [InlineData(null, LangCode.Ru, null)]
        [InlineData("aBout", LangCode.En, "about")]
        [InlineData("help", LangCode.En, "help")]
        [InlineData("mENu", LangCode.En, "menu")]
        [InlineData("START  ", LangCode.En, "start")]
        public async Task GetCommand_Success(string commandName, LangCode langCode, string expectedName)
        {
            var message = new TextUpdateMessage
            {
                ChatId = 1,
                LanguageCode = langCode,
            };

            var command = _commandFactory.GetCommand(commandName);
            await command.HandleAsync(message);

            Assert.Equal(command.Name, expectedName);
        }
    }
}
