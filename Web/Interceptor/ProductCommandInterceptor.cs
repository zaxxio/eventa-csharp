using System.Diagnostics;
using Core.Command;
using Core.Interceptor;
using Web.Command;

namespace Web.Interceptor;

public class ProductCommandInterceptor : ICommandInterceptor
{
    public void PreHandle<T>(T baseCommand) where T : BaseCommand
    {
        if (baseCommand is CreateProductCommand)
        {
            Debug.WriteLine($"Product Pre Intercepted {baseCommand}");
        }
    }

    public void PostHandle<T>(T baseCommand) where T : BaseCommand
    {
        if (baseCommand is CreateProductCommand)
        {
            Debug.WriteLine($"Product Post Intercepted {baseCommand}");
        }
    }
}