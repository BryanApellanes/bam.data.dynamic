using System.Reflection;
using Bam.Data.Dynamic;
using Bam.Net.Data.Dynamic;
using Bam.Net.Data.Schema;
using Google.Protobuf.WellKnownTypes;

namespace Bam.Net.Schema.Json
{
    public class JSchemaDaoAssemblyGenerator : DaoAssemblyGenerator
    {
        private DaoAssemblyGenerator _daoGenerator;

        public JSchemaDaoAssemblyGenerator(IDaoSchemaExtractor schemaExtractor, IDaoGenerator daoGenerator, IDynamicDataWorkspacePath workspacePath = null) : base(schemaExtractor, daoGenerator, workspacePath)
        {
        }

        /*        public JSchemaDaoAssemblyGenerator(DaoAssemblyGenerator daoAssemlbyGenerator, JSchemaSchemaDefinitionGenerator schemaDefinitionGenerator = null, JSchemaEnumGenerator enumGenerator = null)
                {
                    JSchemaSchemaDefinitionGenerator = schemaDefinitionGenerator ?? new JSchemaSchemaDefinitionGenerator(new SchemaManager(), new JSchemaClassManager());
                    EnumGenerator = enumGenerator ?? new JSchemaEnumGenerator();
                    _daoGenerator = daoAssemlbyGenerator;// new DaoAssemblyGenerator();
                    Namespace = $"{nameof(JSchemaDaoAssemblyGenerator)}.Generated";
                }*/

        public JSchemaSchemaDefinitionGenerator JSchemaSchemaDefinitionGenerator { get; set; }
        public JSchemaEnumGenerator EnumGenerator { get; set; }
        public new string Workspace { get; set; }
        public string JsonSchemaRootPath { get; set; }
        public new string Namespace { get; set; }

        public void GenerateSource(string fromJsonSchemaDirectory, string writeSourceToPath)
        {
            JsonSchemaRootPath = fromJsonSchemaDirectory;
            GenerateSource(writeSourceToPath);
        }

        public override void GenerateSource(string workspace)
        {
            Args.ThrowIfNullOrEmpty(JsonSchemaRootPath, $"{nameof(JSchemaDaoAssemblyGenerator)}.{nameof(JsonSchemaRootPath)}");
            // Set the schema before calling base.GenerateSource so the Schema is set before generation
            JSchemaSchemaDefinition schemaDefinition = JSchemaSchemaDefinitionGenerator.GenerateSchemaDefinition(JsonSchemaRootPath);
            Schema = schemaDefinition;
            
            base.GenerateSource(workspace);

            EnumGenerator.Workspace = workspace;
            EnumGenerator.GenerateEnums(schemaDefinition, Namespace);
        }
    }
}