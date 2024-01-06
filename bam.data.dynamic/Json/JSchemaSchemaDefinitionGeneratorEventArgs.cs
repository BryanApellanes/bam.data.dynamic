using System;
using Bam.Net.Data.Schema;

namespace Bam.Net.Schema.Json
{
    public class JSchemaSchemaDefinitionGeneratorEventArgs: EventArgs
    {
        public JSchemaSchemaDefinitionGeneratorEventArgs(JSchemaSchemaDefinitionGenerator generator)
        {
            Generator = generator;
        }
        
        public Exception Exception { get; set; }
        public JSchemaSchemaDefinitionGenerator Generator { get; set; }
        public IDaoSchemaDefinition SchemaDefinition { get; set; }
        public string TableName { get; set; }
        public string ColumnName { get; set; }
    }
}