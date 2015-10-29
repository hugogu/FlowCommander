using FlowCommander.IO;
using Microsoft.Practices.Unity.Utility;
using System;
using System.Collections.Generic;
using System.IO;

namespace FlowCommander.Commands
{
    public class CreateSoftlinkCommand : IFlowCommand
    {
        public string Name
        {
            get { return "Create SoftLink"; }
        }

        public CommandExecution Execute(params object[] arguments)
        {
            Guard.ArgumentNotNull(arguments, "arguments");
            if (arguments.Length != 2)
                throw new ArgumentException("Two arguments are exepcted.");
            var source = arguments[0] as string;
            var target = arguments[1] as string;
            Guard.ArgumentNotNull(source, "source");
            Guard.ArgumentNotNull(target, "target");

            if (File.Exists(source))
            {
                SymbolicLink.CreateFileLink(target, source);
            }
            else if (Directory.Exists(target))
            {
                SymbolicLink.CreateDirectoryLink(target, source);
            }
            else
            {
                throw new NotSupportedException(String.Format("Cannot create softlink from '{0}' to '{1}'", source, target));
            }
            return new CommandExecution { Arguments = arguments };
        }

        public IEnumerable<CommandArgumentDescription> Arguments
        {
            get
            {
                yield return new CommandArgumentDescription
                {
                    Name = "source",
                    Type = typeof(string),
                    Description = "The source file or directory the softlink is referring to."
                };
                yield return new CommandArgumentDescription
                {
                    Name = "target",
                    Type = typeof(string),
                    Description = "The target file or directory the softlink is referring to."
                };
            }
        }
    }
}
