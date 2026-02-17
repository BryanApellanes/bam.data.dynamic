using Bam.Data.Dynamic;
using Bam.Data.Schema;

namespace Bam.Schema.Json
{
    public class JSchemaDaoAssemblyGenerator : DaoAssemblyGenerator
    {
        #pragma warning disable CS0169, CS0414
        private DaoAssemblyGenerator _daoGenerator = null!;
        #pragma warning restore CS0169, CS0414

        public JSchemaDaoAssemblyGenerator(IDaoSchemaExtractor schemaExtractor, IDaoGenerator daoGenerator, IDynamicDataWorkspacePath workspacePath = null!) : base(schemaExtractor, daoGenerator, workspacePath)
        {
        }

        /*        public JSchemaDaoAssemblyGenerator(DaoAssemblyGenerator daoAssemlbyGenerator, JSchemaSchemaDefinitionGenerator schemaDefinitionGenerator = null, JSchemaEnumGenerator enumGenerator = null)
                {
                    JSchemaSchemaDefinitionGenerator = schemaDefinitionGenerator ?? new JSchemaSchemaDefinitionGenerator(new SchemaManager(), new JSchemaClassManager());
                    EnumGenerator = enumGenerator ?? new JSchemaEnumGenerator();
                    _daoGenerator = daoAssemlbyGenerator;// new DaoAssemblyGenerator();
                    Namespace = $"{nameof(JSchemaDaoAssemblyGenerator)}.Generated";
                }*/

        public JSchemaSchemaDefinitionGenerator JSchemaSchemaDefinitionGenerator { get; set; } = null!;
        public JSchemaEnumGenerator EnumGenerator { get; set; } = null!;
        public new string Workspace { get; set; } = null!;
        public string JsonSchemaRootPath { get; set; } = null!;
        public new string Namespace { get; set; } = null!;

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