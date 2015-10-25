using FlowCommander.Model;
using FlowCommander.ViewModel;
using System.Windows;
using System.Windows.Controls;

namespace FlowCommander.Views.Selectors
{
    public class FirstItemSpecialTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var control = container as FrameworkElement;
            var data = item as MapItemsViewModel<string, DirectoryNode>;
            if (data.IsSourceItem)
                return control.FindResource("UserTypedRootMapItemsVMDataTemplate") as DataTemplate;
            else
                return control.FindResource("BoundRootMapItemsVMDataTemplate") as DataTemplate;
        }
    }
}
