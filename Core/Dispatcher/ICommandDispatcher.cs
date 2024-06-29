using Core.Command;

namespace Core.Dispatcher;

public interface ICommandDispatcher
{ 
    void Dispatch<TCommand>(TCommand command) where TCommand : BaseCommand;
}