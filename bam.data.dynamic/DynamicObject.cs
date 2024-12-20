/*
	Copyright © Bryan Apellanes 2015  
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.IO;
using System.Runtime.CompilerServices;
using Bam.Data.Repositories;
using Bam.Generators;
using Microsoft.CodeAnalysis;

namespace Bam.Data.Dynamic
{
    /// <summary>
    /// A Data Transfer Object.  Represents the properties
    /// of Dao types without the associated methods.  
    /// </summary>
    public class DynamicObject
    {
        public const string DefaultNamespace = "Bam.Data.Dynamic";
        public string ToJson()
        {
            return ObjectExtensions.ToJson(this);
        }

        public static dynamic? For(string typeName, Dictionary<object, object> dictionary)
        {
            return For(DefaultNamespace, typeName, dictionary);
        }
        
        public static dynamic? For(string nameSpace, string typeName, Dictionary<object, object> dictionary)
        {
            Type? type = CreateType(nameSpace, typeName, dictionary);
            if (type != null)
            {
                return dictionary.ToInstance(type);
            }

            return null;
        }

        public static Type? CreateType(string typeName, Dictionary<object, object> dictionary)
        {
            return CreateType(DefaultNamespace, typeName, dictionary);
        }
        
        public static Type? CreateType(string nameSpace, string typeName, Dictionary<object, object> dictionary)
        {
            return CreateAssembly(nameSpace, typeName, dictionary).GetTypes().FirstOrDefault(t => t.Name.Equals(DynamicObjectModel.CleanTypeName(typeName)));
        }

        public static Assembly CreateAssembly(string typeName, Dictionary<object, object> dictionary)
        {
            return CreateAssembly(DefaultNamespace, typeName, dictionary);
        }

        public static Assembly CreateAssembly(string nameSpace, string typeName, Dictionary<object, object> dictionary)
        {
            return CreateAssembly(nameSpace, nameSpace, typeName, dictionary);
        }
        
        static readonly Dictionary<string, Assembly> _dtoAssemblies = new Dictionary<string, Assembly>();
        static readonly object _dtoAssemblyLock = new object();
        public static Assembly CreateAssembly(string assemblyName, string nameSpace, string typeName, Dictionary<object, object> dictionary, Func<MetadataReference[]>? getMetaDataReferences = null)
        {
            nameSpace = nameSpace ?? DefaultNamespace;
            DynamicObjectModel dynamicObjectModel = new DynamicObjectModel(nameSpace, typeName, dictionary);
            Func<MetadataReference[]>? arg = getMetaDataReferences;

            MetadataReference[] MetaDataReferenceProvider() // local function
            {
                List<MetadataReference> results = new List<MetadataReference>();
                results.AddRange(dynamicObjectModel.MetadataReferenceResolver.GetMetaDataReferences().ToArray());
                if (arg != null)
                {
                    results.AddRange(arg());
                }

                return results.ToArray();
            }

            string dtoSrc = dynamicObjectModel.Render();
            string key = dtoSrc.Sha256();
            lock (_dtoAssemblyLock)
            {
                if (!_dtoAssemblies.ContainsKey(key))
                {
                    RoslynCompiler compiler = new RoslynCompiler();
                    _dtoAssemblies.Add(key, compiler.CompileAssembly(assemblyName, dtoSrc, MetaDataReferenceProvider));
                }
                return _dtoAssemblies[key];
            }
        }
        
        private static void WriteRenderedDto(string writeSourceTo, DynamicObjectModel dynamicObjectModel)
        {
            string csFile = "{0}.cs".Format(dynamicObjectModel.TypeName);
            FileInfo csFileInfo = new FileInfo(Path.Combine(writeSourceTo, csFile));
            if (csFileInfo.Directory != null && !csFileInfo.Directory.Exists)
            {
                csFileInfo.Directory.Create();
            }

            using StreamWriter sw = new StreamWriter(csFileInfo.FullName);
            sw.Write(dynamicObjectModel.Render());
        }
    }
}
