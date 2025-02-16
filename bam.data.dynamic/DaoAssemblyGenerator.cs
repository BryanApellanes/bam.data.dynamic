/*
	Copyright © Bryan Apellanes 2015  
*/

using System.Reflection;
using Bam.Data.Repositories;
using Bam.Data.Schema;

namespace Bam.Data.Dynamic
{
    public class DaoAssemblyGenerator: IAssemblyGenerator
    {
        public DaoAssemblyGenerator(IDaoSchemaExtractor schemaExtractor, IDaoGenerator daoGenerator, IDynamicDataWorkspacePath workspacePath = null)
        {
            this.SchemaExtractor = schemaExtractor;
            this.DaoGenerator = daoGenerator;
            this.WorkspacePath = workspacePath;
            this.Subscribe(this.DaoGenerator);
        }

        public IDaoSchemaExtractor SchemaExtractor { get; private set; }

        public IDaoGenerator DaoGenerator { get; private set; }
        public IDynamicDataWorkspacePath WorkspacePath { get; private set; }


        DaoSchemaDefinition _schema;
        readonly object _schemaDefinitionLock = new object();
        public DaoSchemaDefinition Schema
        {
            get
            {
                return _schemaDefinitionLock.DoubleCheckLock(ref _schema, () => SchemaExtractor.Extract());
            }
            set => _schema = value;
        }
        
        public SchemaNameMap NameMap { get; set; }
        public string FileName { get; private set; }
        public string Workspace { get; set; }
        Assembly _assembly;
        readonly object _assemblyLock = new object();
        public Assembly Assembly => _assemblyLock.DoubleCheckLock(ref _assembly, GetAssembly);

        public TargetTableEventDelegate OnTableStarted
        {
            get;
            set;
        }

        public GeneratorEventDelegate OnGenerateStarted
        {
            get;
            set;
        }

        public GeneratorEventDelegate OnGenerateComplete
        {
            get;
            set;
        }

        public string Namespace { get; set; }

        protected string FilePath { get; set; }

        protected Assembly[] ReferenceAssemblies { get; set; }

        readonly List<string> _referenceAssemblyPaths;
        protected string[] ReferenceAssemblyPaths
        {
            get
            {
                var results = new List<string>(ReferenceAssemblies.Select(a => a.GetFilePath()).ToArray());
                results.AddRange(_referenceAssemblyPaths);
                return results.ToArray();
            }
        }

        protected virtual IDaoSchemaDefinition GetSchemaDefinition()
        {
            if (NameMap != null)
            {
                MappedSchemaDefinition mappedDefinition = new MappedSchemaDefinition(Schema, NameMap);
                return mappedDefinition.MapSchemaClassAndPropertyNames();
            }
            else
            {
                return Schema;
            }
        }

        protected Assembly GetAssembly()
        {
            return GetAssemblyInfo().GetAssembly();
        }

        public GeneratedAssemblyInfo GetAssemblyInfo()
        {
            FileName = $"{Schema.Name}Dao";
            string fileName = Path.GetFileNameWithoutExtension(FileName);
            FilePath = Path.Combine(WorkspacePath.Value, $"{fileName}.dll");
            return GeneratedAssemblyInfo.GetGeneratedAssembly(FilePath, this);
        }

        readonly object _generateLock = new object();
        public GeneratedAssemblyInfo GenerateAssembly()
        {
            lock (_generateLock)
            {
                string sourcePath = Path.Combine(WorkspacePath.Value, "src");
                GenerateSource(sourcePath);
                GeneratedAssemblyInfo result = Compile(sourcePath);

                return result;
            }
        }

        public void WriteSource(string writeSourceTo)
        {
            GenerateSource(writeSourceTo);
        }

        public virtual void GenerateSource(string sourcePath)
        {
            DirectoryInfo sourceDir = new DirectoryInfo(sourcePath);
            if (!sourceDir.Exists)
            {
                sourceDir.Create();
            }
            
            this.DaoGenerator.Generate(GetSchemaDefinition(), sourcePath);
        }

        public GeneratedAssemblyInfo Compile(string sourcePath, string fileName = null)
        {
            fileName = fileName ?? FileName;
            RoslynCompiler compiler = new RoslynCompiler();
            compiler.AddMetadataReferenceResolver(new AssemblyPathMetadataReferenceResolver(ReferenceAssemblyPaths));
            byte[] assemblyBytes = compiler.CompileDirectories(fileName, new DirectoryInfo(sourcePath));

            GeneratedDaoAssemblyInfo result = new GeneratedDaoAssemblyInfo(FilePath, Assembly.Load(assemblyBytes), assemblyBytes);
            result.Save();
            return result;
        }

        private void Subscribe(IDaoGenerator generator)
        {
            if (OnGenerateStarted != null)
            {
                generator.GenerateStarted += OnGenerateStarted;
            }
            if (OnGenerateComplete != null)
            {
                generator.GenerateComplete += OnGenerateComplete;
            }
        }
    }
}
