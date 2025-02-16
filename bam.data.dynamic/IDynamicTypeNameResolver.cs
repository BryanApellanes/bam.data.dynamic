using Newtonsoft.Json.Linq;

namespace Bam.Data.Dynamic
{
    public interface IDynamicTypeNameResolver
    {
        string ResolveJsonTypeName(string json);
        string ResolveYamlTypeName(string yaml);
        string ResolveTypeName(JObject? jObject);
        string ResolveTypeName(Dictionary<object, object>? value);
    }
}
