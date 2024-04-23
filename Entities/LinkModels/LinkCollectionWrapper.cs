namespace Entities.LinkModels
{
    public class LinkCollectionWrapper<T> : LinkResourceBase
    {
        public List<T> Values { get; set; } = new List<T>();

        public LinkCollectionWrapper(List<T> values)
        {
            Values = values;
        }

        public LinkCollectionWrapper()
        {

        }

    }
}