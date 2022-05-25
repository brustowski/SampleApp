using Autofac;
using Framework.Domain.Commands;

namespace FilingPortal.Domain.Common
{
    class CommandDispatcher : ICommandDispatcher
    {
        private readonly IComponentContext _container;

        public CommandDispatcher(IComponentContext container)
        {
            _container = container;
        }

        public CommandResult Dispatch<TCommand>(TCommand command) where TCommand : ICommand
        {
            var commandHandler = _container.Resolve<ICommandHandler<TCommand>>();
            return commandHandler.Handle(command);
        }
    }
}
