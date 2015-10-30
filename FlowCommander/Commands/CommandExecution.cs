namespace FlowCommander.Commands
{
    public class CommandExecution
    {
        public IFlowCommand Command { get; internal set; }

        public object[] Arguments { get; internal set; }
    }
}
