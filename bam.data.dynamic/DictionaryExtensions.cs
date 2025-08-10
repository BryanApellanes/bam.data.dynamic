using Microsoft.CodeAnalysis;

namespace Bam.Data.Dynamic
{
    public static class DictionaryExtensions
    {
        public static Dictionary<object, object>? GetValue(this Dictionary<object, object> dictionary, string key)
        {
            if(dictionary.TryGetValue(key, out object? value))
            {
                if (value is Dictionary<object, object> innerDictionary)
                {
                    return innerDictionary;
                }

                return new Dictionary<object, object>()
                {
                    { key, dictionary[key] }
                };
            }

            return null;
        }
        public static dynamic? ToDynamic(this Dictionary<object, object> dictionary, string typeName, string nameSpace = null)
        {
            return ToDynamic(dictionary, typeName, () => new MetadataReference[] { }, nameSpace);
        }

        public static dynamic? ToDynamic(this Dictionary<object, object> dictionary, string typeName, Func<MetadataReference[]> getMetadataReferences, string nameSpace = null)
        {
            nameSpace = nameSpace ?? DynamicObject.DefaultNamespace;
            return DynamicObject.For(nameSpace, typeName, dictionary);
        }
    }
}
