namespace Chartix.Infrastructure.Telegram.Interfaces
{
    public interface ICommandStrategyFactory<out T>
    {
        T GetCommand(string data);
    }
}
