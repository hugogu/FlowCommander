using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reactive.Linq;
using FolderData = FlowCommander.ViewModel.MapItemsViewModel<string, string>;

namespace FlowCommander.ViewModel
{
    public class SelectDirectoryViewModel : ReactiveObject
    {
        private ObservableCollection<FolderData> _leves = new ObservableCollection<FolderData>();

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
            FolderData data = new FolderData();
            data.GenerateItems = GetSubDirectories;
            data.OnException = HandleException;
            data.ObservableForProperty(_ => _.CurrentItem)
                .Subscribe(currentItem => OnSelectingCurrentItem(currentItem.Sender));
            data.Root = root;

            return data;
        }

        private void OnSelectingCurrentItem(FolderData vm)
        {
            int index = _leves.IndexOf(vm);
            int nextIndex = index + 1;
            if (vm.CurrentItem != null)
            {
                if (nextIndex < _leves.Count)
                    _leves[nextIndex].Root = vm.CurrentItem;
                else
                    _leves.Add(CreateFolderData(vm.CurrentItem));
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

        private IEnumerable<string> HandleException(FolderData vm, Exception exception)
        {
            return new[] { exception.Message };
        }

        private IEnumerable<string> GetSubDirectories(string path)
        {
            return Directory.EnumerateDirectories(path);
        }
    }
}
