using Newtonsoft.Json.Schema;

namespace Bam.Schema.Json
{
    public class JSchemaClassNameExtraction
    {
        public static implicit operator string(JSchemaClassNameExtraction extraction)
        {
            return extraction.ClassName;
        }

        public JSchemaClassNameExtraction()
        {
        }
        
        public JSchemaClassNameExtraction(string className)
        {
            ClassName = className;
        }
        
        public JSchema JSchema { get; set; } = null!;
        public JSchemaClass JSchemaClass { get; set; } = null!;
        public JSchemaClassManager JSchemaClassManager { get; set; } = null!;

        /// <summary>
        /// The list of properties that the JSchemaClassManager checked.
        /// </summary>
        public string[] ClassNameProperties { get; set; } = null!;
        public string ClassName { get; set; } = null!;
    }
}