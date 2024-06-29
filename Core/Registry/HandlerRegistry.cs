using System.Reflection;

namespace Core.Registry;

public interface HandlerRegistry
{
    void RegisterHandler(Type type, MethodInfo handlerMethod);
    void RegisterConstructorHandler(Type type, ConstructorInfo constructorHandler);
    MethodInfo GetHandler(Type type);
    ConstructorInfo? GetConstructorHandler(Type type);
    List<MethodInfo> GetHandlers();
    List<ConstructorInfo> GetConstructorHandlers();
}