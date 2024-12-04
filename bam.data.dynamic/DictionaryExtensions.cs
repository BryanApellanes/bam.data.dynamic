using Bam.Data.Repositories;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public static dynamic? ToDto(this Dictionary<object, object> dictionary, string typeName, string nameSpace = null)
        {
            return ToDto(dictionary, typeName, () => new MetadataReference[] { }, nameSpace);
        }

        public static dynamic? ToDto(this Dictionary<object, object> dictionary, string typeName, Func<MetadataReference[]> getMetadataReferences, string nameSpace = null)
        {
            nameSpace = nameSpace ?? Dto.DefaultNamespace;
            return Dto.InstanceFor(nameSpace, typeName, dictionary);
        }
    }
}
