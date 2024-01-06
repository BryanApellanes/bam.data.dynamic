using System.Collections.Generic;
using Bam.Net.Data.Schema;

namespace Bam.Net.Schema.Json
{
    public class JSchemaSchemaDefinition
    {
        public static implicit operator DaoSchemaDefinition(JSchemaSchemaDefinition jSchemaSchemaDefinition)
        {
            return (DaoSchemaDefinition)jSchemaSchemaDefinition.SchemaDefinition;
        }
        
        public JSchemaSchemaDefinition(IDaoSchemaDefinition schemaDefinition, HashSet<JSchemaClass> classes)
        {
            this.SchemaDefinition = schemaDefinition;
            this.Classes = classes;
        }
        
        public IDaoSchemaDefinition SchemaDefinition { get; set; }
        public HashSet<JSchemaClass> Classes { get; set; }
    }
}