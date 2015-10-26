using FlowCommander.Collections;
using FlowCommander.Model;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using FolderData = FlowCommander.ViewModel.MapItemsViewModel<string, FlowCommander.Model.DirectoryNode>;

namespace FlowCommander.ViewModel
{
    public class SelectDirectoryViewModel : ReactiveObject
    {
        private ObservableCollection<FolderData> _leves = new ObservableCollection<FolderData>();
        private static IEqualityComparer<DirectoryNode> directoryNodeDisplayComparer = Utils.GetMemberEqualityComparer((DirectoryNode d) => d.Name);

        public SelectDirectoryViewModel()
        {
            var rootLevel = CreateFolderData(@"C:\");
            rootLevel.IsSourceItem = true;
            _leves.Add(rootLevel);
        }

        public IEnumerable<FolderData> Levels
        {
            get { return _leves; }
        }

        protected virtual FolderData CreateFolderData(string root)
        {
            FolderData data = new FolderData()
            {
                DisplayValueComparer = directoryNodeDisplayComparer,
                GenerateItems = GetSubDirectories,
                OnException = HandleException,
                Root = root,
            };
            data.ObservableForProperty(_ => _.CurrentItem)
                .Where(change => change.GetValue() == null || change.Sender.Root == change.GetValue().Root)
                .Subscribe(currentItem => OnSelectingCurrentItem(currentItem.Sender));

            return data;
        }

        private void OnSelectingCurrentItem(FolderData vm)
        {
            int index = _leves.IndexOf(vm);
            int nextIndex = index + 1;
            if (vm.CurrentItem != null)
            {
                var newRoot = Path.Combine(vm.Root, vm.CurrentItem.Name);
                if (nextIndex < _leves.Count)
                    _leves[nextIndex].Root = newRoot;
                else
                    _leves.Add(CreateFolderData(newRoot));
            }
            else
            {
                RemoveFoldersAfter(index);
            }
        }

        private void RemoveFoldersAfter(int index)
        {
            int end = _leves.Count;
            while (--end > index)
                _leves.RemoveAt(end);
        }

        private static IEnumerable<DirectoryNode> HandleException(FolderData vm, Exception exception)
        {
            return new[] { new DirectoryNode(vm.Root, exception.Message) { IsValid = false } };
        }

        private static IEnumerable<DirectoryNode> GetSubDirectories(string path)
        {
            return from directory in Directory.EnumerateDirectories(path)
                   select new DirectoryNode(path, Path.GetFileName(directory));
        }
    }
}
