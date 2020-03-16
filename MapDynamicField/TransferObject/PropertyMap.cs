namespace MapDynamicField.TransferObject
{
    public class PropertyMap : IPropertyMap
    {
        public PropertyMap(string source, string destination)
        {
            Source = source;
            Destination = destination;
            IsSourceNavegationProperty = source.IndexOf('.') >= 0;
        }

        public string Source { get; }
        public string Destination { get; }
        public bool IsSourceNavegationProperty { get; }
    }
}
