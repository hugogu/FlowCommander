using System;

namespace FlowCommander.Commands
{
    public class CommandArgumentDescription
    {
        public string Name { get; internal protected set; }

        public string Description { get; internal protected set; }

        public Type Type { get; internal protected set; }

        public bool IsOptional { get; internal protected set; }

        public object DefaultValue { get; internal protected set; }
    }
}
