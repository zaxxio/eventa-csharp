using Core.Command;
using Core.Interceptor;

namespace Core.Registry;

public class CommandInterceptorRegisterer(IEnumerable<ICommandInterceptor> commandInterceptors)
{
    public void PreHandle<T>(T baseCommand) where T : BaseCommand
    {
        foreach (var interceptor in commandInterceptors)
        {
            interceptor.PreHandle(baseCommand);
        }
    }
    
    public void PostHandle<T>(T baseCommand) where T : BaseCommand
    {
        foreach (var interceptor in commandInterceptors)
        {
            interceptor.PostHandle(baseCommand);
        }
    }
}