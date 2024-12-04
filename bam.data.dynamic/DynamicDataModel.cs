namespace Bam.Data.Dynamic;

public class DynamicDataModel
{
    private const string? DefaultNamespace = "DynamicDataTypes";
    private const string DefaultTypeName = "DynamicDataType";

    public DynamicDataModel()
    {
        this.Namespace = DefaultNamespace;
        this.TypeName = DefaultTypeName;
        this.TypeNameResolver = (dict) => DefaultTypeName;
        this.NamespaceResolver = (dict) => DefaultNamespace;
    }

    public DynamicDataModel(string typeName) : this()
    {
        this.TypeName = typeName;
    }

    public DynamicDataModel(string typeName, string? nameSpace)
    {
        this.Namespace = nameSpace;
        this.TypeName = typeName;
        this.TypeNameResolver = (dict) => DefaultTypeName;
        this.NamespaceResolver = (dict) => DefaultNamespace;
    }

    public DynamicDataModel(string? typeName, string nameSpace, Dictionary<object, object> data) : this(nameSpace,
        typeName)
    {
        this.Data = data;
        this.TypeNameResolver = (dict) => DefaultTypeName;
        this.NamespaceResolver = (dict) => DefaultNamespace;
    }

    public DynamicDataModel(Dictionary<object, object> data,
        Func<Dictionary<object, object>, string>? typeNameResolver = null)
    {
        this.Data = data;
        this.TypeName = typeNameResolver == null ? DefaultTypeName : typeNameResolver(data);
        this.TypeNameResolver = typeNameResolver ?? ((dict) => DefaultTypeName);
        this.NamespaceResolver = (dict) => DefaultNamespace;
    }
    
    public DynamicDataModel(Dictionary<object, object> data,
        Func<Dictionary<object, object>, string>? typeNameResolver = null,
        Func<Dictionary<object, object>, string?>? nameSpaceResolver = null)
    {
        this.Data = data;
        this.TypeName = typeNameResolver == null ? DefaultTypeName : typeNameResolver(data);
        this.Namespace = nameSpaceResolver == null ? DefaultNamespace : nameSpaceResolver(data);
        
        this.TypeNameResolver = typeNameResolver ?? ((dict) => DefaultTypeName);
        this.NamespaceResolver = nameSpaceResolver ?? ((dict) => DefaultNamespace);
    }

    private string? _namespace;
    public string? Namespace
    {
        get
        {
            if (string.IsNullOrEmpty(_namespace) && Data != null)
            {
                _namespace = NamespaceResolver(Data);
            }

            return _namespace;
        }
        set => _namespace = value;
    }

    private string? _typeName;

    public string? TypeName
    {
        get
        {
            if (string.IsNullOrEmpty(_typeName) && Data != null)
            {
                _typeName = TypeNameResolver(Data);
            }

            return _typeName;
        }
        set => _typeName = value;
    }

    public string[] Properties
    {
        get; 
        set;
    }

    private Dictionary<object, object>? _data;
    public Dictionary<object, object>? Data
    {
        get => _data;
        set
        {
            _data = value;
            if (_data != null)
            {
                HashSet<Type> referenceTypes = new HashSet<Type>();
                HashSet<string> propertyDefinitions = new HashSet<string>();
                foreach (object key in _data.Keys)
                {
                    object propertyValue = _data[key];
                    Type propertyType = propertyValue.GetType();
                    referenceTypes.Add(propertyType);
                    string? propertyName = key.ToString();
                    string propertyTypeName = propertyType.Name;

                    referenceTypes.Add(propertyType);
                    propertyDefinitions.Add($"\t\tpublic {propertyTypeName} {propertyName} {{get;set;}}\r\n");
                }

                this.ReferenceTypes = referenceTypes;
                this.Properties = propertyDefinitions.ToArray();
            }
        }
    }
    
    public Func<Dictionary<object, object>, string> TypeNameResolver { get; }
    public Func<Dictionary<object, object>, string?> NamespaceResolver { get; }

    public HashSet<Type> ReferenceTypes
    {
        get;
        set;
    }
}