using FlowCommander.IO;
using Microsoft.Practices.Unity.Utility;
using ReactiveUI;
using System;
using System.IO;

namespace FlowCommander.Model
{
    public class DirectoryNode : ReactiveObject, IComparable<DirectoryNode>, IComparable
    {
        private bool? _isSymbolicLink = null;
        private string _target;

        public DirectoryNode(string root, string name)
        {
            Guard.ArgumentNotNullOrEmpty(root, "root");
            Guard.ArgumentNotNullOrEmpty(name, "name");

            Root = root;
            Name = name;
            IsValid = true;
        }

        public string Root { get; protected set; }

        public string Name { get; protected set; }

        public string FullPath
        {
            get { return Path.Combine(Root, Name); }
        }

        public bool IsValid
        {
            get; set;
        }

        public bool IsSymbolicLink
        {
            get { return IsValid && (_isSymbolicLink ?? (_isSymbolicLink = IsSymbolic(FullPath)).Value); }
        }

        public string Target
        {
            get
            {
                try
                {
                    if (IsSymbolicLink)
                        return _target ?? (_target = SymbolicLink.GetTarget(FullPath));
                    else
                        return null;
                }
                catch (UnauthorizedAccessException ex)
                {
                    return ex.Message;
                }
            }
        }

        public override bool Equals(object obj)
        {
            return CompareTo(obj) == 0;
        }

        public override int GetHashCode()
        {
            return Root.GetHashCode() ^ Name.GetHashCode();
        }

        public int CompareTo(DirectoryNode other)
        {
            if (other == null)
                return -1;

            if (other == this)
                return 0;

            int value = String.Compare(Root, other.Root);
            if (value != 0)
            {
                return value;
            }
            return String.Compare(Name, other.Name);
        }

        public int CompareTo(object obj)
        {
            return CompareTo(obj as DirectoryNode);
        }

        private bool IsSymbolic(string path)
        {
            return new FileInfo(path).Attributes.HasFlag(FileAttributes.ReparsePoint);
        }
    }
}
