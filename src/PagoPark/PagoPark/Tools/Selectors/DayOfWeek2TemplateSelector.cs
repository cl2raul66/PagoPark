namespace PagoPark.Tools;

public class DayOfWeek2TemplateSelector : DataTemplateSelector
{
    public DataTemplate StartItemTemplate { get; set; }
    public DataTemplate EndItemTemplate { get; set; }
    public DataTemplate DefaultItemTemplate { get; set; }

    protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
    {
        var daysOfWeek = (container as CollectionView).ItemsSource.Cast<DateTime>();

        if (item is DateTime dateItem)
        {
            if (dateItem.ToString("yyyyMMdd") == daysOfWeek.First().ToString("yyyyMMdd"))
            {
                return StartItemTemplate;
            }
            else if (dateItem.ToString("yyyyMMdd") == daysOfWeek.Last().ToString("yyyyMMdd"))
            {
                return EndItemTemplate;
            }
        }
        return DefaultItemTemplate;
    }
}

