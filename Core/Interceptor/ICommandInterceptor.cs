using Core.Command;

namespace Core.Interceptor;

public interface ICommandInterceptor
{
    void PreHandle<T>(T baseCommand) where T : BaseCommand;
    void PostHandle<T>(T baseCommand) where T : BaseCommand;
}