namespace Bam.Data.Dynamic;

public class EnumerableItemDynamicTypeNameResolver : IEnumerableItemDynamicTypeNameResolver
{
    public string ResolveItemTypeName(object[] array)
    {
        throw new NotImplementedException();
    }

    public string ResolveItemTypeName(IEnumerable<object> enumerable)
    {
        throw new NotImplementedException();
    }
}