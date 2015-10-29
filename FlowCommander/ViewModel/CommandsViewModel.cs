using FlowCommander.Commands;
using ReactiveUI;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FlowCommander.ViewModel
{
    public class CommandsViewModel : ReactiveObject
    {
        private readonly ObservableCollection<IFlowCommand> _commands;

        public CommandsViewModel(IEnumerable<IFlowCommand> commands)
        {
            _commands = new ObservableCollection<IFlowCommand>(commands);
        }

        public IEnumerable<IFlowCommand> Commands
        {
            get { return _commands; }
        }
    }
}
