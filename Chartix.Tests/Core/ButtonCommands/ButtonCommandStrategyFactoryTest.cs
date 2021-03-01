using System;
using Chartix.Infrastructure.Telegram.ButtonCommands;
using Chartix.Infrastructure.Telegram.Interfaces;
using Chartix.Infrastructure.Telegram.Models;
using Xunit;

namespace Chartix.Tests.Core.ButtonCommands
{
    public class ButtonCommandStrategyFactoryTest : IClassFixture<ButtonCommandFactoryFixture>
    {
        private readonly ICommandStrategyFactory<IButtonCommand> _commandFactory;

        public ButtonCommandStrategyFactoryTest(ButtonCommandFactoryFixture fixture)
        {
            _commandFactory = fixture.ButtonCommandFactory;
        }

        [Theory]
        [InlineData("AddMetric_100", ButtonCode.AddMetric, 100)]
        [InlineData("ChooseDelMetric_100", ButtonCode.ChooseDelMetric, 100)]
        [InlineData("DelMetric_100_13", ButtonCode.DelMetric, 100, "13")]
        [InlineData("DelValue_100_13_23", ButtonCode.DelValue, 100, "13")]
        public void GetCommand_Success(string data, ButtonCode expectedCode, int expectedMenuMessageId, string expectedParameter = null)
        {
            var buttonCommand = _commandFactory.GetCommand(data);

            Assert.NotNull(buttonCommand);
            Assert.Equal(buttonCommand.Button, expectedCode);
            Assert.Equal(buttonCommand.MenuMessageId, expectedMenuMessageId);
            Assert.Equal(buttonCommand.Parameter, expectedParameter);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void GetCommand_Fail_ArgumentException(string data)
        {
            Assert.Throws<ArgumentException>(() => _commandFactory.GetCommand(data));
        }

        [Theory]
        [InlineData("AddMetricc", typeof(ArgumentException))]
        [InlineData("AddMetric|DelMetric", typeof(ArgumentException))]
        [InlineData("sdfjsdhfk", typeof(ArgumentException))]
        [InlineData("AddMetric_number", typeof(FormatException))]
        public void GetCommand_Fail_Exception(string data, Type expectedType)
        {
            var exception = Record.Exception(() => _commandFactory.GetCommand(data));
            Assert.IsType(expectedType, exception);
        }
    }
}
