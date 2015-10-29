using FlowCommander.Commands;
using System;
using System.Linq;
using System.Reflection;

namespace FlowCommander.ViewModel
{
    public class MainWindowViewModel
    {
        public MainWindowViewModel()
        {
            DirectoryVM = new SelectDirectoryViewModel();
            CommandsVM = new CommandsViewModel(from type in Assembly.GetExecutingAssembly().GetTypes()
                                               where !type.IsInterface && !type.IsAbstract
                                               where typeof(IFlowCommand).IsAssignableFrom(type)
                                               select Activator.CreateInstance(type) as IFlowCommand);
        }

        public SelectDirectoryViewModel DirectoryVM { get; private set; }

        public CommandsViewModel CommandsVM { get; private set; }
    }
}
