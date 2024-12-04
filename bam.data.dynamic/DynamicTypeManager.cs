using Bam.Data.Dynamic.Data;

namespace Bam.Data.Dynamic;

public class DynamicTypeManager
{
    public DynamicTypeManager(IDynamicTypeNameResolver typeNameResolver, IEnumerableItemDynamicTypeNameResolver enumerableItemDynamicTypeNameResolver)
    {
        this.TypeNameResolver = typeNameResolver;
        this.EnumerableItemDynamicTypeNameResolver = enumerableItemDynamicTypeNameResolver;
    }
    
    public IDynamicTypeNameResolver TypeNameResolver { get; }
    public IEnumerableItemDynamicTypeNameResolver EnumerableItemDynamicTypeNameResolver { get; }

    public IEnumerable<DynamicTypeDescriptor> CreateTypeDescriptors(Dictionary<object, object> propertyValues)
    {
        yield return CreateTypeDescriptor(propertyValues);
        foreach (object key in propertyValues.Keys)
        {
            if (propertyValues[key] is Dictionary<object, object> dict)
            {
                yield return CreateTypeDescriptor(dict);
            }
        }
    }
    
    public DynamicTypeDescriptor CreateTypeDescriptor(Dictionary<object, object> propertyValues)
    {
        return CreateTypeDescriptor(TypeNameResolver.ResolveTypeName(propertyValues), propertyValues);
    }
    
    public DynamicTypeDescriptor CreateTypeDescriptor(string typeName, Dictionary<object, object> propertyValues)
    {
        DynamicTypeDescriptor result = new DynamicTypeDescriptor() { TypeName = typeName };
        foreach (object key in propertyValues.Keys)
        {
            AddProperty(result, (string)key, propertyValues[key]);
        }

        return result;
    }

    protected virtual void AddProperty(DynamicTypeDescriptor parentDescriptor, string propertyName, object? value)
    {
        if (value is Dictionary<object, object> data)
        {
            parentDescriptor.Properties.Add(CreatePropertyDescriptor(parentDescriptor, propertyName, data));
        }
        else
        {
            parentDescriptor.Properties.Add(CreatePropertyDescriptor(parentDescriptor, propertyName, value));
        }
    }

    protected virtual DynamicTypePropertyDescriptor CreatePropertyDescriptor(DynamicTypeDescriptor parentDescriptor, string propertyName, Dictionary<object, object>? propertyData)
    {
        return new DynamicTypePropertyDescriptor()
        {
            ParentType = parentDescriptor,
            ParentTypeName = parentDescriptor.TypeName,
            PropertyType = TypeNameResolver.ResolveTypeName(propertyData),
            PropertyName = propertyName
        };
    }
    
    protected virtual DynamicTypePropertyDescriptor CreatePropertyDescriptor(DynamicTypeDescriptor parentDescriptor, string propertyName, object? propertyValue)
    {
        return new DynamicTypePropertyDescriptor()
        {
            ParentType = parentDescriptor,
            ParentTypeName = parentDescriptor.TypeName,
            PropertyType = propertyValue?.GetType().Name,
            PropertyName = propertyName
        };
    }
}