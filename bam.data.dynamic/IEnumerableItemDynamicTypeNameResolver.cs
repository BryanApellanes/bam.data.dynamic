namespace Bam.Data.Dynamic;

public interface IEnumerableItemDynamicTypeNameResolver
{
    string ResolveItemTypeName(object[] array);
    string ResolveItemTypeName(IEnumerable<object> enumerable);
}