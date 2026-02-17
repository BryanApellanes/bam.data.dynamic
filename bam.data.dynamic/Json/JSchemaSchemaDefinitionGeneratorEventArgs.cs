using Bam.Data.Schema;

namespace Bam.Schema.Json
{
    public class JSchemaSchemaDefinitionGeneratorEventArgs: EventArgs
    {
        public JSchemaSchemaDefinitionGeneratorEventArgs(JSchemaSchemaDefinitionGenerator generator)
        {
            Generator = generator;
        }
        
        public Exception Exception { get; set; } = null!;
        public JSchemaSchemaDefinitionGenerator Generator { get; set; } = null!;
        public IDaoSchemaDefinition SchemaDefinition { get; set; } = null!;
        public string TableName { get; set; } = null!;
        public string ColumnName { get; set; } = null!;
    }
}