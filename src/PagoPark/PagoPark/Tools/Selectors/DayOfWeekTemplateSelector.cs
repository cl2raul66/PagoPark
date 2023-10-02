namespace PagoPark.Tools;

public class DayOfWeekTemplateSelector : DataTemplateSelector
{
    public DataTemplate StartItemTemplate { get; set; }
    public DataTemplate EndItemTemplate { get; set; }
    public DataTemplate DefaultItemTemplate { get; set; }

    protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
    {
        var daysOfWeek = (container as CollectionView).ItemsSource.Cast<string>().ToList();
        if (item.Equals(daysOfWeek.First()))
        {
            return StartItemTemplate;
        }
        else if (item.Equals(daysOfWeek.Last()))
        {
            return EndItemTemplate;
        }
        else
        {
            return DefaultItemTemplate;
        }
    }
}

