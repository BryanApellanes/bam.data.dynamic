using Bam.Net.Data.Repositories;
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
        public static dynamic ToDynamic(this Dictionary<object, object> dictionary, string typeName, string nameSpace = null)
        {
            return ToDynamic(dictionary, typeName, () => new MetadataReference[] { }, nameSpace);
        }

        public static dynamic ToDynamic(this Dictionary<object, object> dictionary, string typeName, Func<MetadataReference[]> getMetadataReferences, string nameSpace = null)
        {
            nameSpace = nameSpace ?? Dto.DefaultNamespace;
            return Dto.InstanceFor(nameSpace, typeName, dictionary);
        }
    }
}
