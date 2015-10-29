using System.Collections.Generic;

namespace FlowCommander.Commands
{
    public interface IFlowCommand
    {
        string Name { get; }

        IEnumerable<CommandArgumentDescription> Arguments { get; }

        CommandExecution Execute(params object[] arguments);
    }
}
