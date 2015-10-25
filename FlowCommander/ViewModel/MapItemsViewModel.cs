﻿using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Data;

namespace FlowCommander.ViewModel
{
    public class MapItemsViewModel<TS, TT> : ReactiveObject, IDisposable
    {
        private TS _root;
        private ObservableCollection<TT> _items = new ObservableCollection<TT>();
        private ICollectionView _itemsView;
        private IDisposable _rootSubscriber;

        public MapItemsViewModel()
        {
            _rootSubscriber = this
                .ObservableForProperty(_ => _.Root)
                .SelectMany(async rootChanged => await SelectItemsFrom(rootChanged.GetValue()))
                .Catch((Exception e) => Observable.Return(OnException(this, e)))
                .Repeat()
                .ObserveOn(DispatcherScheduler.Current)
                .Subscribe(
                    newItems => Items = newItems,
                    exception => Items = OnException(this, exception)
                );
            _itemsView = CollectionViewSource.GetDefaultView(_items);
            if (_itemsView.CanSort)
                _itemsView.SortDescriptions.Add(new SortDescription(".", ListSortDirection.Ascending));
            _itemsView.CurrentChanged += (s, e) =>
                this.RaisePropertyChanged("CurrentItem");
        }

        public Func<TS, IEnumerable<TT>> GenerateItems { get; set; }

        public Func<MapItemsViewModel<TS, TT>, Exception, IEnumerable<TT>> OnException { get; set; }

        public TS Root
        {
            get { return _root; }
            set { this.RaiseAndSetIfChanged(ref _root, value); }
        }

        public IEnumerable<TT> Items
        {
            get { return _items; }
            protected set
            {
                _items.Clear();
                _items.AddRange(value);
            }
        }

        public TT CurrentItem
        {
            get { return (TT)_itemsView.CurrentItem;  }
        }

        public void Dispose()
        {
            _rootSubscriber.Dispose();
        }

        private async Task<IEnumerable<TT>> SelectItemsFrom(TS root)
        {
            return await Task.Run(() =>
            {
                try
                {
                    return GenerateItems(root);
                }
                catch(Exception exception)
                {
                    return OnException(this, exception);
                }
            });
        }
    }
}