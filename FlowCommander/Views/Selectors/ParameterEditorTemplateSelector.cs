using FlowCommander.Commands;
using System.Windows;
using System.Windows.Controls;

namespace FlowCommander.Views.Selectors
{
    public class ParameterEditorTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var description = item as CommandArgumentDescription;
            if (description == null)
            {
                return base.SelectTemplate(item, container);
            }

            if (description.ValueCandidates == null)
            {
                return (container as FrameworkElement).FindResource("UserFillArgumentTemplate") as DataTemplate;
            }
            else
            {
                return (container as FrameworkElement).FindResource("CandidatesSelectorArgumentTemplate") as DataTemplate;
            }
        }
    }
}
