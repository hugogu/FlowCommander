using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
            DisplayValueComparer = EqualityComparer<TT>.Default;
            Refresh = ReactiveCommand.CreateAsyncTask(async _ => Items = await SelectItemsFrom(Root));
            Refresh.ThrownExceptions.Subscribe(exception => Items = OnException(this, exception));

            _rootSubscriber = this.ObservableForProperty(_ => _.Root).InvokeCommand(Refresh);

            _itemsView = CollectionViewSource.GetDefaultView(_items);
            if (_itemsView.CanSort)
                _itemsView.SortDescriptions.Add(new SortDescription(".", ListSortDirection.Ascending));
            _itemsView.CurrentChanged += OnCurrentItemChanged;
        }

        public Func<TS, IEnumerable<TT>> GenerateItems { get; set; }

        public Func<MapItemsViewModel<TS, TT>, Exception, IEnumerable<TT>> OnException { get; set; }

        public IEqualityComparer<TT> DisplayValueComparer { get; set; }

        public IReactiveCommand Refresh { get; protected set; }

        public TS Root
        {
            get { return _root; }
            set { this.RaiseAndSetIfChanged(ref _root, value); }
        }

        public bool IsSourceItem { get; set; }

        public IEnumerable<TT> Items
        {
            get { return _items; }
            protected set
            {
                TT selection = CurrentItem;
                _items.Clear();
                _items.AddRange(value);
                SetCurrentItemToSimilar(selection);
            }
        }

        public TT CurrentItem
        {
            get { return (TT)_itemsView.CurrentItem; }
        }

        public void Dispose()
        {
            _itemsView.CurrentChanged -= OnCurrentItemChanged;
            _rootSubscriber.Dispose();
        }

        private void OnCurrentItemChanged(object sender, EventArgs args)
        {
            this.RaisePropertyChanged("CurrentItem");
        }

        private void SetCurrentItemToSimilar(TT itemTarget)
        {
            foreach (var item in _items)
            {
                if (DisplayValueComparer.Equals(item, itemTarget))
                {
                    _itemsView.MoveCurrentTo(item);
                    break;
                }
            }
        }

        private async Task<IEnumerable<TT>> SelectItemsFrom(TS root)
        {
            return await Task.Run(() => GenerateItems(root));
        }
    }
}
