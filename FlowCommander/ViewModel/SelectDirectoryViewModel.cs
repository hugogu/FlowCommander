using ReactiveUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Linq;

namespace FlowCommander.ViewModel
{
    public class SelectDirectoryViewModel : ReactiveObject
    {
        public SelectDirectoryViewModel()
        {
            Level1Directory = new MapItemsViewModel<string, string>();
            Level2Directory = new MapItemsViewModel<string, string>();
            Level1Directory.GenerateItems = GetSubDirectories;
            Level2Directory.GenerateItems = GetSubDirectories;
            Level1Directory.OnException = HandleException;
            Level2Directory.OnException = HandleException;

            Level1Directory.ObservableForProperty(_ => _.CurrentItem)
                           .Where(currentChanged => currentChanged.GetValue() != null)
                           .Subscribe(currentItem => Level2Directory.Root = currentItem.GetValue());
        }

        public MapItemsViewModel<string, string> Level1Directory { get; set; }

        public MapItemsViewModel<string, string> Level2Directory { get; set; }

        private IEnumerable<string> HandleException(MapItemsViewModel<string, string> vm, Exception exception)
        {
            return new[] { exception.Message };
        }

        private IEnumerable<string> GetSubDirectories(string path)
        {
            return Directory.EnumerateDirectories(path);
        }
    }
}
